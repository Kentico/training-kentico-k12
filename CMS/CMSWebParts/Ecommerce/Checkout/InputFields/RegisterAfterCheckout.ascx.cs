using System;

using CMS.Core;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.SiteProvider;


public partial class CMSWebParts_Ecommerce_Checkout_InputFields_RegisterAfterCheckout : CMSCheckoutWebPart
{
    private bool UseSettings => ECommerceSettings.AutomaticCustomerRegistration(SiteContext.CurrentSiteID);


    private bool UseWebPartSettings => CurrentUser != null && CurrentUser.IsPublic();


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        SetupControl();
    }


    /// <summary>
    /// Sets up the control.
    /// </summary>
    public void SetupControl()
    {
        if (StopProcessing || RequestHelper.IsPostBack())
        {
            return;
        }

        if (UseSettings)
        {
            // Show error message only on Page and Design tabs
            if ((PortalContext.ViewMode == ViewModeEnum.Design) || (PortalContext.ViewMode == ViewModeEnum.Edit))
            {
                pnlCheckBox.Visible = false;
                pnlError.Visible = true;
                lblError.Text = GetString("com.checkout.registeraftercheckouterror");
            }
            else
            {
                IsVisible = false;
            }
        }
        else if (UseWebPartSettings)
        {
            var isCustomerRegisteredAfterCheckout = Service.Resolve<ICustomerRegistrationRepositoryFactory>().GetRepository(SiteContext.CurrentSiteID).IsCustomerRegisteredAfterCheckout;

            // Show the control
            chkRegister.Text = GetStringValue("Text", "");
            chkRegister.Checked = ValidationHelper.GetBoolean(GetValue("Checked"), false) || isCustomerRegisteredAfterCheckout;
        }
        else
        {
            IsVisible = false;
        }
    }


    protected void CheckedChanged(object sender, EventArgs e)
    {
        if (!RequestHelper.IsPostBack() || !UseWebPartSettings)
        {
            return;
        }

        var repository = Service.Resolve<ICustomerRegistrationRepositoryFactory>().GetRepository(SiteContext.CurrentSiteID);
        repository.IsCustomerRegisteredAfterCheckout = chkRegister.Checked;
        repository.RegisteredAfterCheckoutTemplate = GetStringValue("EmailTemplate", "");

        ShoppingCart.Evaluate();
        ComponentEvents.RequestEvents.RaiseEvent(this, e, SHOPPING_CART_CHANGED);
    }
}