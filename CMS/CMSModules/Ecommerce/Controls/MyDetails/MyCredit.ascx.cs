using System;

using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.DataEngine;
using CMS.DataEngine.Query;


public partial class CMSModules_Ecommerce_Controls_MyDetails_MyCredit : CMSAdminControl
{
    private CurrencyInfo mCurrency = null;
    private decimal rate = 1m;
    
    
    /// <summary>
    /// Site default currency object.
    /// </summary>
    private CurrencyInfo Currency
    {
        get
        {
            if (mCurrency == null)
            {
                mCurrency = ECommerceContext.CurrentCurrency;
            }

            return mCurrency ?? (mCurrency = CurrencyInfoProvider.GetMainCurrency(SiteContext.CurrentSiteID));
        }
    }


    /// <summary>
    /// Customer ID.
    /// </summary>
    public int CustomerId
    {
        get;
        set;
    }


    /// <summary>
    /// If true, control does not process the data.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["StopProcessing"], false);
        }
        set
        {
            ViewState["StopProcessing"] = value;
        }
    }
    

    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!StopProcessing)
        {
            if (AuthenticationHelper.IsAuthenticated())
            {
                // Get site id of credits main currency
                var creditSiteId = ECommerceHelper.GetSiteID(SiteContext.CurrentSiteID, ECommerceSettings.USE_GLOBAL_CREDIT);

                gridCreditEvents.HideControlForZeroRows = true;
                gridCreditEvents.IsLiveSite = IsLiveSite;
                gridCreditEvents.OnExternalDataBound += gridCreditEvents_OnExternalDataBound;
                gridCreditEvents.OrderBy = "EventDate DESC, EventName ASC";
                gridCreditEvents.WhereCondition = new WhereCondition()
                                                    .WhereEquals("EventCustomerID", CustomerId)
                                                    .WhereEquals("EventSiteID".AsColumn().IsNull(0), creditSiteId)
                                                    .ToString(true);

                // Get total credit value
                var credit = CreditEventInfoProvider.GetTotalCredit(CustomerId, SiteContext.CurrentSiteID);

                if (Currency != null)
                {
                    // Convert credit to current currency when using one
                    CurrencyConverter.TryGetExchangeRate(creditSiteId == 0, Currency.CurrencyCode, SiteContext.CurrentSiteID, ref rate);
                    credit = CurrencyConverter.ApplyExchangeRate(credit, rate);
                }

                lblCreditValue.Text = CurrencyInfoProvider.GetFormattedPrice(credit, Currency);
            }
            else
            {
                // Hide if user is not authenticated
                Visible = false;
            }
        }
    }

    
    protected object gridCreditEvents_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        // Show only date part from date-time value
        switch (sourceName.ToLowerInvariant())
        {
            case "eventdate":
                var date = ValidationHelper.GetDateTime(parameter, DateTimeHelper.ZERO_TIME);
                if (date != DateTimeHelper.ZERO_TIME)
                {
                    return TimeZoneHelper.ConvertToUserTimeZone(date, true, MembershipContext.AuthenticatedUser, SiteContext.CurrentSite);
                }

                return String.Empty;

            case "eventcreditchange":
                var credit = CurrencyConverter.ApplyExchangeRate(ValidationHelper.GetDecimal(parameter, 0m), rate);
                return CurrencyInfoProvider.GetFormattedPrice(credit, Currency);
        }

        return parameter;
    }
    

    /// <summary>
    /// Overridden SetValue - because of MyAccount webpart.
    /// </summary>
    /// <param name="propertyName">Name of the property to set</param>
    /// <param name="value">Value to set</param>
    public override bool SetValue(string propertyName, object value)
    {
        base.SetValue(propertyName, value);

        if (String.Equals(propertyName, "customerid", StringComparison.OrdinalIgnoreCase))
        {
            CustomerId = ValidationHelper.GetInteger(value, 0);
        }

        return true;
    }
}