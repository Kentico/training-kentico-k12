using System;
using System.Web.UI;

using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;

/// <summary>
/// Shopping cart totals web part
/// </summary>
public partial class CMSWebParts_Ecommerce_Checkout_Viewers_ShoppingCartTotals : CMSCheckoutWebPart
{
    #region "Constructor"

    /// <summary>
    /// Initializes a new instance of the CMSWebParts_Ecommerce_Checkout_Viewers_ShoppingCartTotals" class.
    /// </summary>
    public CMSWebParts_Ecommerce_Checkout_Viewers_ShoppingCartTotals()
    {
        // Do not resolve Visible field in configuration
        base.NotResolveProperties = string.Format("{0};Visible;", base.NotResolveProperties);
    }

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets the visibility condition. If the condition is true web part is visible.
    /// </summary>
    public string VisibilityCondition
    {
        get
        {
            // return macro string without macro brackets
            return ValidationHelper.GetString(GetValue("Visible"), "").Replace("{%", "").Replace("%}", "");
        }
    }


    /// <summary>
    /// Gets or sets the name of the transformation which is used for MultiBuy discount summary.
    /// </summary>
    public string OrderDiscountSummaryTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderDiscountSummaryTransformationName"), "");
        }
        set
        {
            SetValue("OrderDiscountSummaryTransformationName", value);
        }
    }

    #endregion


    #region "Event handling"

    /// <summary>
    /// OnInit event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Subscribe to the wizard events
        SubscribeToWizardEvents();
    }


    /// <summary>
    /// Load event handler.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        SetupControl();

        if (UpdatePanel != null)
        {
            UpdatePanel.UpdateMode = UpdatePanelUpdateMode.Always;
        }
    }


    /// <summary>
    /// PreRender event handler.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!String.IsNullOrEmpty(VisibilityCondition))
        {
            // Check condition value, if it is false, hide web part and also envelope
            var res = ContextResolver.ResolveMacroExpression(VisibilityCondition, true);
            if ((res == null) || !ValidationHelper.GetBoolean(res.Result, false))
            {
                totalViewer.Visible = false;
                HideWebPartContent();
            }
        }
    }


    /// <summary>
    /// Updates web part.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The EventArgs instance containing the event data.</param>
    public void Update(object sender, EventArgs e)
    {
        SetupControl();
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Subscribes the web part to the wizard events.
    /// </summary>
    private void SubscribeToWizardEvents()
    {
        ComponentEvents.RequestEvents.RegisterForEvent(SHOPPING_CART_CHANGED, Update);
    }


    /// <summary>
    /// Setups the control.
    /// </summary>
    public void SetupControl()
    {
        string type = ValidationHelper.GetString(GetValue("TotalToDisplay"), "TotalPriceOfProducts");

        // Display the correct value according to the TotalToDisplay property of the web part
        switch (type)
        {
            case "TotalPriceOfOrder":
            {
                var value = ShoppingCart.GrandTotal;
                DisplayValue(GetFormattedPriceToDisplay(value));
                break;
            }

            case "TotalPriceOfProducts":
            {
                var value = ShoppingCart.TotalItemsPrice;
                DisplayValue(GetFormattedPriceToDisplay(value));
                break;
            }

            case "TotalShipping":
            {
                var value = (ShoppingCart.ShippingOption != null) ? ShoppingCart.TotalShipping : 0;
                DisplayValue(GetFormattedPriceToDisplay(value));
                break;
            }

            case "TotalWeight":
            {
                var value = ShoppingCart.TotalItemsWeight;
                var stringFormat = GetStringFormat();
                DisplayValue(stringFormat == "" ? ShippingOptionInfoProvider.GetFormattedWeight(value, CurrentSiteName) : String.Format(stringFormat, value));
                break;
            }

            case "TotalOrderTax":
            {
                var value = ShoppingCart.TotalTax;
                DisplayValue(GetFormattedPriceToDisplay(value));
                break;
            }

            case "TotalOrderDiscount":
            {
                var value = ShoppingCart.OrderDiscount;
                DisplayValue(GetFormattedPriceToDisplay(value));
                break;
            }

            case "OtherPaymentsTotal":
            {
                var value = ShoppingCart.OtherPayments;
                DisplayValue(GetFormattedPriceToDisplay(value));
                break;
            }
        }
    }


    private string GetStringFormat()
    {
        string stringFormat = ValidationHelper.GetString(GetValue("StringFormat"), "");

        // Try to use the stringFormat format, to check, if it's a valid one
        try
        {
            String.Format(stringFormat, 0.0m);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Checkout process", "ERROR", ex, CurrentSite.SiteID, "The StringFormat property of the web part isn't in a correct format: '" + stringFormat + "'");

            // Recovery
            return "";
        }

        return stringFormat;
    }


    private string GetFormattedPriceToDisplay(decimal value)
    {
        var stringFormat = GetStringFormat();
        if(!string.IsNullOrEmpty(stringFormat))
        {
            return String.Format(stringFormat, value);
        }

        return CurrencyInfoProvider.GetFormattedPrice(value, ShoppingCart.Currency);
    }


    /// <summary>
    /// Displays the passed value and manages the label visibility.
    /// </summary>
    /// <param name="value">Value to display</param>
    private void DisplayValue(string value)
    {
        totalViewer.Visible = true;

        string label = ValidationHelper.GetString(GetValue("Label"), "");
        label = HTMLHelper.HTMLEncode(ResHelper.LocalizeString(label));

        if (!string.IsNullOrEmpty(label))
        {
            lblLabel.Text = label;
            lblLabel.Visible = true;
        }

        lblValue.Text = value;
    }

    #endregion
}