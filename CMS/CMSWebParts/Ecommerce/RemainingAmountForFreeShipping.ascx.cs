using System;

using CMS.Core;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;


public partial class CMSWebParts_Ecommerce_RemainingAmountForFreeShipping : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Text for remaining value.
    /// </summary>
    public string LabelText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LabelText"), "");
        }
        set
        {
            SetValue("LabelText", value);
        }
    }

    #endregion


    #region "Lifecycle"

    /// <summary>
    /// OnPreRender override
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!StopProcessing)
        {
            SetText();

            if (!lblText.Visible)
            {
                ContentBefore = string.Empty;
                ContentAfter = string.Empty;
                ContainerTitle = string.Empty;
                ContainerName = string.Empty;
            }
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Set text of remaining amount for free shipping
    /// </summary>
    private void SetText()
    {
        ShoppingCartInfo cart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart();

        // Hide text by default
        lblText.Visible = false;

        if (cart.IsEmpty || !cart.IsShippingNeeded)
        {
            return;
        }

        var remainingAmount = cart.CalculateRemainingAmountForFreeShipping();
        if (remainingAmount > 0)
        {
            try
            {
                var currency = cart.Currency;
                var formatedAmount = currency == null ? remainingAmount.ToString() : currency.FormatPrice(remainingAmount);

                // Display text of remaining amount
                lblText.Visible = true;
                lblText.Text = string.Format(LabelText, formatedAmount);
            }
            catch (Exception ex)
            {
                // Log exception
                EventLogProvider.LogException("Web part", "EXCEPTION", ex, cart.ShoppingCartSiteID);
            }
        }
    }

    #endregion
}