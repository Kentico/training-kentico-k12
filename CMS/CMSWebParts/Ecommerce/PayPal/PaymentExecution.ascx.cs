using System;
using System.Linq;

using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;


public partial class CMSWebParts_Ecommerce_PayPal_PaymentExecution : CMSAbstractWebPart
{
    private Guid mOrderGuid;
    private string mPaymentId;
    private string mPayerId;

    public override void OnContentLoaded()
    {
        base.OnContentLoaded();

        LoadData();
        ExecutePayment();
    }


    private void LoadData()
    {
        if (StopProcessing || RequestHelper.IsCallback())
        {
            return;
        }

        mOrderGuid = QueryHelper.GetGuid("pporderid", Guid.Empty);
        mPaymentId = QueryHelper.GetString("paymentid", "");
        mPayerId = QueryHelper.GetString("payerid", "");
    }


    private void ExecutePayment()
    {
        if (mOrderGuid == Guid.Empty || StopProcessing || RequestHelper.IsCallback())
        {
            return;
        }

        if (String.IsNullOrEmpty(mPaymentId) || String.IsNullOrEmpty(mPayerId))
        {
            // Payment parameters not specified
            LogEvent(null, "Payment_Parameters_Not_Specified");

            return;
        }

        var order = OrderInfoProvider.GetOrders().WhereEquals("OrderGuid", mOrderGuid).FirstOrDefault();
        if (order != null)
        {
            var payPalProvider = CMSPaymentGatewayProvider.GetPaymentGatewayProvider<CMSPayPalProvider>(order.OrderPaymentOptionID);
            if (payPalProvider != null)
            {
                payPalProvider.OrderId = order.OrderID;
                payPalProvider.ExecutePayment(mPaymentId, mPayerId);
            }
            else
            {
                // Payment provider not found
                LogEvent(null, "Payment_Provider_Not_Found");
            }
        }
        else
        {
            // Order not found
            LogEvent(String.Format(ResHelper.GetString("PaymentGatewayProvider.ordernotfound"), mOrderGuid), "Order_Not_Found");
        }
    }


    private void LogEvent(string message, string eventCode)
    {
        try
        {
            // Add some additional information to the error message
            var info = $"ORDER GUID: {mOrderGuid}\r\nTRANSACTION ID: {mPaymentId}\r\nPAYMENT STATUS: {mPayerId}\r\n";

            EventLogProvider.LogEvent(EventType.ERROR, "PayPal Payment Execution", eventCode, info + message);
        }
        catch
        {
            // Unable to log the event
        }
    }
}