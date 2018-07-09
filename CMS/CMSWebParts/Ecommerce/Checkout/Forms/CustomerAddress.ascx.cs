using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Globalization;
using CMS.Helpers;
using CMS.PortalEngine;
using AddressTypeEnum = CMS.Ecommerce.AddressType;


public partial class CMSWebParts_Ecommerce_Checkout_Forms_CustomerAddress : CMSCheckoutWebPart
{
    #region "Constants"

    private const string BILLING = "billingaddress";
    private const string SHIPPING = "shippingaddress";
    private const string COMPANY = "companyaddress";

    #endregion


    #region "Public properties"

    /// <summary>
    /// Address type (BILLING is default)
    /// </summary>
    /// <remarks>Prefer using <see cref="AddressType"/> property.</remarks>
    public AddressTypeEnum AddressType
    {
        get
        {
            switch(ValidationHelper.GetString(GetValue("AddressType"), BILLING))
            {
                case SHIPPING:
                    return AddressTypeEnum.Shipping;
                case COMPANY:
                    return AddressTypeEnum.Company;
                default:
                    return AddressTypeEnum.Billing;
            }
        }
        set
        {
            switch (value)
            {
                case AddressType.Shipping:
                    SetValue("AddressType", SHIPPING);
                    break;
                case AddressType.Company:
                    SetValue("AddressType", COMPANY);
                    break;
                default:
                    SetValue("AddressType", BILLING);
                    break;
                    
            }
        }
    }


    /// <summary>
    /// The caption for the display checkbox displaying or hiding the web part..
    /// </summary>
    public string CheckboxCaption
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CheckboxCaption"), "");
        }
        set
        {
            SetValue("CheckboxCaption", value);
            chkShowAddress.Text = value;
        }
    }


    /// <summary>
    /// Alternative form name for this address web part.
    /// </summary>
    public string AddressForm
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AddressForm"), "");
        }
        set
        {
            SetValue("AddressForm", value);
        }
    }


    /// <summary>
    /// Gets or sets a value indicating whether propagating changes on postback is allowed.
    /// </summary>
    public bool PropagateChangesOnPostback
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PropagateChangesOnPostback"), false);
        }
        set
        {
            SetValue("PropagateChangesOnPostback", value);
        }
    }

    #endregion


    #region "Private properties"

    private AddressInfo CurrentCartAddress
    {
        get
        {
            switch (AddressType)
            {
                case AddressTypeEnum.Billing:
                    return ShoppingCart.ShoppingCartBillingAddress;
                case AddressTypeEnum.Shipping:
                    return ShoppingCart.ShoppingCartShippingAddress;
                case AddressTypeEnum.Company:
                    return ShoppingCart.ShoppingCartCompanyAddress;
                default:
                    return null;
            }
        }
        set
        {
            switch (AddressType)
            {
                case AddressTypeEnum.Billing:
                    ShoppingCart.ShoppingCartBillingAddress = value;
                    break;
                case AddressTypeEnum.Shipping:
                    ShoppingCart.ShoppingCartShippingAddress = value;
                    break;
                case AddressTypeEnum.Company:
                    ShoppingCart.ShoppingCartCompanyAddress = value;
                    break;
            }
        }
    }

    #endregion


    #region "Page methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!StopProcessing)
        {
            string[] splitFormName = AddressForm.Split('.');

            // UIForm can't process full path of alternative form if object type is already specified
            addressForm.AlternativeFormName = splitFormName.LastOrDefault();
            addressForm.OnBeforeSave += addressForm_OnBeforeSave;
            addressForm.OnBeforeDataLoad += addressForm_OnBeforeDataLoad;
            addressForm.OnAfterDataLoad += addressForm_OnAfterDataLoad;

            // Hide default submit button
            addressForm.SubmitButton.Visible = false;

            InitializeAddress();
        }

        // Propagate StopProcessing to address form to avoid JS errors for missing update panel
        addressForm.StopProcessing = StopProcessing;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Hide drop down for empty selection
        if (drpAddresses.Items.Count == 0)
        {
            pnlAddressSelector.Visible = false;
        }
    }

    #endregion


    #region "Events"

    private void addressForm_OnBeforeDataLoad(object sender, EventArgs e)
    {
        if (PropagateChangesOnPostback)
        {
            var countryField = addressForm.FormInformation?.GetFormField("AddressCountryID");
            if (countryField != null)
            {
                countryField.HasDependingFields = true;
            }
        }
    }


    private void addressForm_OnAfterDataLoad(object sender, EventArgs e)
    {
        if (!ViewMode.IsLiveSite())
        {
            // Suppress form validation on Check-in/Check-out actions
            addressForm.StopProcessing = true;

            return; 
        }

        CustomerInfo customer = ShoppingCart.Customer;
        AddressInfo address = addressForm.EditedObject as AddressInfo;

        // Preset personal name filed for new address and logged in user
        if ((customer != null) && (address != null) && (address.AddressID == 0) && string.IsNullOrEmpty(address.AddressPersonalName))
        {
            address.AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}".Trim();
        }

        if (PropagateChangesOnPostback)
        {
            // Propagate changes on postback if there is address info in UIForm
            if (((CurrentCartAddress != null) || (address != null))
                && RequestHelper.IsPostBack())
            {
                // Set first time user current address for postback tax recalculation
                if (CurrentCartAddress == null)
                {
                    CurrentCartAddress = address;
                }

                var formControl = addressForm.FieldControls["AddressCountryID"];
                if (formControl != null)
                {
                    formControl.Changed += CountrySelector_Changed;
                }
            }
        }
    }


    private void CountrySelector_Changed(object sender, EventArgs e)
    {
        if (CurrentCartAddress == null)
        {
            return;
        }

        // Get selected state from country selector in UIForm
        var countrySelectorControl = addressForm.FieldControls["AddressCountryID"];
        var addressStateID = countrySelectorControl?.GetOtherValue("AddressStateID");

        // Get selected country from UIForm
        var addressCountryId = addressForm.GetFieldValue("AddressCountryID");

        var newCountryId = ValidationHelper.GetInteger(addressCountryId, CurrentCartAddress.AddressCountryID);
        var newStateId = ValidationHelper.GetInteger(addressStateID, 0);

        bool evaluationNeeded = false;

        if (CurrentCartAddress.AddressCountryID != newCountryId)
        {
            CurrentCartAddress.AddressCountryID = newCountryId;
            evaluationNeeded = true;
        }

        if (CurrentCartAddress.AddressStateID != newStateId)
        {
            CurrentCartAddress.AddressStateID = newStateId;
            evaluationNeeded = true;
        }

        if (evaluationNeeded)
        {
            EvaluateCartAndRaiseCartChangedEvent();
        }
    }


    protected void addressForm_OnBeforeSave(object sender, EventArgs e)
    {
        // Cancel saving, just set current filled values into EditableObject through UIForm.SaveData method
        addressForm.StopProcessing = true;
    }


    protected void drpAddresses_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Get selected address
        int selectedAddressId = ValidationHelper.GetInteger(drpAddresses.SelectedValue, 0);
        AddressInfo address = AddressInfoProvider.GetAddressInfo(selectedAddressId);

        CurrentCartAddress = address;
        SetupSelectedAddress(address, true);
    }


    protected void chkShowAddress_OnCheckedChanged(object sender, EventArgs e)
    {
        if ((AddressType == AddressTypeEnum.Shipping) && !chkShowAddress.Checked)
        {
            // Clear address
            SetupSelectedAddress(null, true);
        }
        else
        {
            EvaluateCartAndRaiseCartChangedEvent();
        }
    }

    #endregion


    #region "Data methods"

    protected override void LoadStep(object sender, StepEventArgs e)
    {
        base.LoadStep(sender, e);

        // Hide/show display form checkbox
        EnsureOptionalCheckBox();

        // Clear shipping address if show check box is not checked and recalculate cart price
        if (PropagateChangesOnPostback && (AddressType == AddressTypeEnum.Shipping) && (!chkShowAddress.Checked) && CurrentCartAddress != null)
        {
            CurrentCartAddress = null;
            drpAddresses.SelectedValue = "0";
        }

        if (!StopProcessing)
        {
            SetupSelectedAddress(CurrentCartAddress);

            // Set default error label
            addressForm.ErrorLabel = lblError;

            drpAddresses.SelectedIndexChanged += drpAddresses_SelectedIndexChanged;
        }
    }


    /// <summary>
    /// Validates data
    /// </summary>
    protected override void ValidateStepData(object sender, StepEventArgs e)
    {
        base.ValidateStepData(sender, e);

        if (!StopProcessing)
        {
            if (!addressForm.ValidateData())
            {
                e.CancelEvent = true;
                return;
            }
            // Just set current filed values into EditableObject, saving was canceled in OnBeforeSave
            addressForm.SaveData(null, false);

            if (addressForm.EditedObject is AddressInfo address)
            {
                // Validate state
                if (StateInfoProvider.GetStates().WhereEquals("CountryID", address.AddressCountryID).TopN(1).HasResults())
                {
                    if (address.AddressStateID < 1)
                    {
                        e.CancelEvent = true;
                        addressForm.DisplayErrorLabel("AddressCountryID", ResHelper.GetString("com.address.nostate"));
                        return;
                    }
                }

                // Clear AddressName and AddressPersonalName to force their update (only if not present on the address form)
                if (!addressForm.FieldControls.Contains("AddressName"))
                {
                    address.AddressName = null;
                }
                if (!addressForm.FieldControls.Contains("AddressPersonalName"))
                {
                    address.AddressPersonalName = null;
                }

                // Assign validated new address to the current shopping cart
                // Address will be saved by customer detail web part (existing customer object is needed for the address)
                CurrentCartAddress = address;
            }
        }

        // Clear shipping address (StopProcessing is true when chkShowAddress is cleared)
        ClearShippingAddressIfNotUsed();
    }

    #endregion


    #region "Helper methods"

    private void EvaluateCartAndRaiseCartChangedEvent()
    {
        if (PropagateChangesOnPostback)
        {
            ShoppingCart.Evaluate();

            // Make sure that in-memory changes persist (unsaved address, etc.)
            Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(ShoppingCart);

            ComponentEvents.RequestEvents.RaiseEvent(this, null, SHOPPING_CART_CHANGED);
        }
    }


    /// <summary>
    /// Clears shipping address, if only billing address is required.
    /// </summary>
    private void ClearShippingAddressIfNotUsed()
    {
        // Clear shipping address if shipping checkbox is unchecked
        if ((AddressType == AddressTypeEnum.Shipping) && !chkShowAddress.Checked)
        {
            ShoppingCart.ShoppingCartShippingAddress = null;
        }
    }


    private void EnsureOptionalCheckBox()
    {
        // Show optional checkbox
        if (AddressType == AddressTypeEnum.Shipping)
        {
            chkShowAddress.Visible = true;
            chkShowAddress.Text = CheckboxCaption;

            pnlUiContext.Visible = true;
            drpAddresses.Visible = true;
            lblAddress.Visible = true;

            // Check show address checkbox in non-postback load (postback has view state) if there is selected shipping
            if ((!RequestHelper.IsPostBack()) && (ShoppingCart.ShoppingCartShippingAddress != null))
            {
                chkShowAddress.Checked = true;
            }

            // Hide web part content if show checkbox is not checked and controls are not hidden
            if (!chkShowAddress.Checked)
            {
                StopProcessing = true;
                pnlUiContext.Visible = false;
                drpAddresses.Visible = false;
                lblAddress.Visible = false;
            }
        }
    }


    /// <summary>
    /// Initialize customer's addresses in billing and shipping drop-down lists.
    /// </summary>
    private void InitializeAddress()
    {
        if (Customer == null)
        {
            drpAddresses.Visible = false;
            lblAddress.Visible = false;
            return;
        }

        if (drpAddresses.Items.Count == 0)
        {
            // Specifies addresses to display for the given customer, select addresses associated with the given shopping cart
            DataSet ds = AddressInfoProvider.GetAddresses(Customer.CustomerID);
            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                drpAddresses.DataSource = ds;
                drpAddresses.DataBind();

                // List item for the new item <(new), 0> item in the drop-down menu
                ListItem li = new ListItem(GetString("ShoppingCartOrderAddresses.NewAddress"), "0");
                drpAddresses.Items.Insert(0, li);
            }
        }
    }


    private void SetupSelectedAddress(AddressInfo selectedAddress, bool forceReload = false)
    {
        var addressToSetup = selectedAddress;

        // Set DDL value only for classic load
        if (!RequestHelper.IsPostBack())
        {
            if ((CurrentCartAddress == null && addressToSetup == null))
            {
                // Get last modified customer address
                addressToSetup = GetLastUsedAddress();
            }

            int addressId = addressToSetup?.AddressID ?? 0;

            if (drpAddresses.Items.FindByValue(addressId.ToString()) != null)
            {
                drpAddresses.SelectedValue = Convert.ToString(addressId);
                CurrentCartAddress = addressToSetup;
            }
        }

        addressForm.EditedObject = addressToSetup ?? new AddressInfo();

        // Do not reload data in postback, save action is postback, and reload will erase fields with changed data before saving into CurrentCartAddress
        if (!RequestHelper.IsPostBack() || forceReload)
        {
            addressForm.ReloadData();

            if (PropagateChangesOnPostback)
            {
                // CurrentCartAddress must be set after the form is reloaded (to prevent unwanted changes during addressForm_OnAfterDataLoad)
                CurrentCartAddress = addressToSetup;

                EvaluateCartAndRaiseCartChangedEvent();
            }
        }
        else if (PropagateChangesOnPostback)
        {
            CurrentCartAddress = addressToSetup;
        }
    }


    private AddressInfo GetLastUsedAddress()
    {
        return CacheHelper.Cache(() => AddressInfoProvider.GetAddresses(ShoppingCart.ShoppingCartCustomerID)
                                                          .OrderByDescending("AddressLastModified")
                                                          .TopN(1)
                                                          .FirstOrDefault(),
            new CacheSettings(ECommerceSettings.ProvidersCacheMinutes, "GetLastUsedAddressForCustomer", ShoppingCart.ShoppingCartCustomerID)
            {
                CacheDependency = CacheHelper.GetCacheDependency(new[]
                {
                    AddressInfo.OBJECT_TYPE + "|all"
                })
            });
    }

    #endregion
}