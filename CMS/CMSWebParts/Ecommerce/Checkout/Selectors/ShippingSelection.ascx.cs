using System;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;

/// <summary>
/// Shipping selector web part
/// </summary>
public partial class CMSWebParts_Ecommerce_Checkout_Selectors_ShippingSelection : CMSCheckoutWebPart
{
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
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ControlsHelper.UpdateCurrentPanel(this);
    }


    /// <summary>
    /// Handles the shipping change event. Saving of the selected value into the shopping cart object.
    /// </summary>
    protected void SelectShipping_ShippingChange(object sender, EventArgs e)
    {
        // Only if selection is different
        if (ShoppingCart.ShoppingCartShippingOptionID != drpShipping.SelectedID)
        {
            // Set shipping option for the shopping cart according to the selected value
            ShoppingService.SetShippingOption(drpShipping.SelectedID);

            // Make sure that in-memory changes persist (unsaved address, etc.)
            Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(ShoppingCart);

            // Raise the change event for all subscribed web parts
            ComponentEvents.RequestEvents.RaiseEvent(this, e, SHOPPING_CART_CHANGED);
        }
    }


    /// <summary>
    /// Updates the web part according to the new shopping cart values.
    /// </summary>
    private void Update(object sender, EventArgs e)
    {
        if (sender != this)
        {
            LoadControlData();
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Validates the data.
    /// </summary>
    protected override void ValidateStepData(object sender, StepEventArgs e)
    {
        base.ValidateStepData(sender, e);

        lblError.Visible = false;

        // If shipping selector is visible (needed) check if something is selected
        if (pnlShipping.Visible && (drpShipping.SelectedID == 0))
        {
            e.CancelEvent = true;
            lblError.Text = ResHelper.GetString("com.checkoutprocess.shippingneeded");
            lblError.Visible = true;
        }
    }


    /// <summary>
    /// Saves the wizard step data.
    /// </summary>
    protected override void SaveStepData(object sender, StepEventArgs e)
    {
        base.SaveStepData(sender, e);

        // Clear shipping option if cart does not need shipping
        if (!ShoppingCart.IsShippingNeeded)
        {
            ShoppingService.SetShippingOption(0);
        }
    }


    /// <summary>
    /// Subscribes the web part to the wizard events.
    /// </summary>
    private void SubscribeToWizardEvents()
    {
        ComponentEvents.RequestEvents.RegisterForEvent(SHOPPING_CART_CHANGED, Update);
    }


    /// <summary>
    /// Setting up the control.
    /// </summary>
    private void SetupControl()
    {
        if (!StopProcessing)
        {
            // Set up empty record text. The macro ResourcePrefix + .empty represents empty record value.
            drpShipping.UniSelector.ResourcePrefix = "com.livesiteselector";

            // Set shopping cart. Selector will use it to compute shipping price.
            drpShipping.ShoppingCart = ShoppingCart;

            LoadControlData();
        }
    }


    private void LoadControlData()
    {
        // Initialize selector if needed
        if (!RequestHelper.IsPostBack() && drpShipping.HasData)
        {
            drpShipping.Reload();
            drpShipping.SelectedID = ShoppingService.GetShippingOption();
        }

        // Evaluate cart if shipping was pre-selected
        if (ShoppingCart.ItemChanged("ShoppingCartShippingOptionID"))
        {
            ShoppingCart.Evaluate();
            ShoppingCartInfoProvider.SetShoppingCartInfo(ShoppingCart);

            // Make sure that in-memory changes persist (unsaved address, etc.)
            Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(ShoppingCart);
        }
    }

    #endregion
}