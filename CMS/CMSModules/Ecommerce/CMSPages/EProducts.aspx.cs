using System;
using System.Data;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;


[Title("com.downloadsdialog.title")]
public partial class CMSModules_Ecommerce_CMSPages_EProducts : CMSLiveModalPage
{
    #region "Variables"

    private int orderId = 0;

    #endregion


    #region "Page methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Get order ID from URL
        orderId = QueryHelper.GetInteger("orderid", 0);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Get order
        OrderInfo oi = OrderInfoProvider.GetOrderInfo(orderId);

        if (oi != null)
        {
            // Get customer for current user
            CustomerInfo customer = CustomerInfoProvider.GetCustomerInfoByUserID(MembershipContext.AuthenticatedUser.UserID);

            // If order does not belong to current user 
            if ((customer == null) || ((customer != null) && (oi.OrderCustomerID != customer.CustomerID)))
            {
                // Redirect to access denied page
                URLHelper.Redirect("~/CMSMessages/AccessDeniedToPage.aspx");
            }
        }
        else
        {
            // Redirect to error page
            URLHelper.Redirect(AdministrationUrlHelper.GetErrorPageUrl("com.downloadsdialog.ordernotfoundtitle", "com.downloadsdialog.ordernotfoundtext"));
        }

        // Initialize close button
        btnClose.Text = GetString("general.close");

        // Initialize unigrid
        downloadsGridElem.ZeroRowsText = GetString("com.downloadsdialog.nodownloadsfound");
        downloadsGridElem.OnDataReload += downloadsGridElem_OnDataReload;
        downloadsGridElem.OnExternalDataBound += downloadsGridElem_OnExternalDataBound;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (downloadsGridElem.IsEmpty)
        {
            pnlDownloads.CssClass = "PageContent";
        }
    }

    #endregion


    #region "Methods"

    private DataSet downloadsGridElem_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        return OrderItemSKUFileInfoProvider.GetOrderItemSKUFiles(orderId);
    }


    private object downloadsGridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        DateTime orderItemValidTo = DateTimeHelper.ZERO_TIME;

        switch (sourceName.ToLowerCSafe())
        {
            case "file":
                DataRowView row = (parameter as DataRowView);

                // Get values from parameter
                int orderSiteId = ValidationHelper.GetInteger(row["OrderSiteID"], 0);
                int fileId = ValidationHelper.GetInteger(row["OrderItemSKUFileID"], 0);
                string productName = HTMLHelper.HTMLEncode(ResHelper.LocalizeString(ValidationHelper.GetString(row["OrderItemSKUName"], null)));
                string fileName = ValidationHelper.GetString(row["FileName"], null);
                Guid token = ValidationHelper.GetGuid(row["Token"], Guid.Empty);
                string fileUrl = UrlResolver.ResolveUrl(OrderItemSKUFileInfoProvider.GetOrderItemSKUFileUrl(token, fileName, orderSiteId));
                orderItemValidTo = ValidationHelper.GetDateTime(row["OrderItemValidTo"], DateTimeHelper.ZERO_TIME);

                // If download is not expired
                if ((orderItemValidTo.CompareTo(DateTimeHelper.ZERO_TIME) == 0) || (orderItemValidTo.CompareTo(DateTime.Now) > 0))
                {
                    // Return download link
                    return String.Format("{0} (<a href=\"{1}\" target=\"_blank\">{2}</a>)", productName, fileUrl, HTMLHelper.HTMLEncode(fileName));
                }
                else
                {
                    // Return file name
                    return String.Format("{0} ({1})", productName, HTMLHelper.HTMLEncode(fileName));
                }

            case "expiration":
                orderItemValidTo = ValidationHelper.GetDateTime(parameter, DateTimeHelper.ZERO_TIME);

                // If download never expires
                if (orderItemValidTo.CompareTo(DateTimeHelper.ZERO_TIME) == 0)
                {
                    // Return dash
                    return "-";
                }
                else
                {
                    // Return expiration date and time according to current user or site time zones
                    return TimeZoneHelper.ConvertToUserTimeZone(orderItemValidTo, true, MembershipContext.AuthenticatedUser, SiteContext.CurrentSite);
                }
        }

        return parameter;
    }

    #endregion
}