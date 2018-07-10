using System;
using System.Linq;

using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine;

/// <summary>
/// Customer registration web part for checkout process
/// </summary>
public partial class CMSWebParts_Ecommerce_Checkout_Forms_CustomerDetail : CMSCheckoutWebPart
{
    #region "Constants"

    const string COMPANY_TYPE = "Company";

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets a value indicating whether propagating changes on postback is allowed.
    /// </summary>
    /// <remarks>
    /// Use this property if dummy AccountType selector field is defined in customer`s alternative form set in <see cref="AlternativeFormName"/> property.
    /// </remarks>
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


    /// <summary>
    /// Alternative form name for this web part.
    /// </summary>
    public string AlternativeFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternativeFormName"), "");
        }
        set
        {
            SetValue("AlternativeFormName", value);
        }
    }


    /// <summary>
    /// Gets the customer type selector [Personal/Company]. Returns null if alternative form does not include this field.
    /// </summary>
    private FormEngineUserControl TypeSelector
    {
        get
        {
            return customerForm.FieldControls["AccountType"];
        }
    }

    #endregion


    #region "Life cycle"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        customerForm.OnBeforeSave += customerForm_OnBeforeSave;

        string[] splitFormName = AlternativeFormName.Split('.');
        // UIForm cant process full path of alternative form if object type is already specified.
        customerForm.AlternativeFormName = splitFormName.LastOrDefault();

        InitCustomerForm();
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        customerForm.SubmitButton.Visible = false;
    }

    #endregion


    #region "Form events"

    protected void customerForm_OnBeforeSave(object sender, EventArgs e)
    {
        // Cancel saving, just set current filed values into EditableObject through UIForm.SaveData method
        customerForm.StopProcessing = true;
    }

    #endregion


    #region "Wizard methods"

    protected override void StepLoaded(object sender, StepEventArgs e)
    {
        base.StepLoaded(sender, e);

        // The default value is Personal, but if customer has some company fields filled, the Company account type should be selected
        if (!RequestHelper.IsPostBack())
        {
            SetCompanyAccountType();
        }

        // Propagate changes on postback if there is customer with company type and some tax registration id
        if (PropagateChangesOnPostback && (ShoppingCart.Customer != null))
        {
            ChangeTaxRegistrationIdBasedOnAccountType();
        }
    }


    protected override void ValidateStepData(object sender, StepEventArgs e)
    {
        base.ValidateStepData(sender, e);

        if (!customerForm.ValidateData())
        {
            if (e != null)
            {
                e.CancelEvent = true;
            }
        }
    }


    protected override void SaveStepData(object sender, StepEventArgs e)
    {
        base.SaveStepData(sender, e);

        // Just set current filed values into EditableObject, saving was canceled in OnBeforeSave
        customerForm.SaveData(null, false);

        var customer = customerForm.EditedObject as CustomerInfo;
        if (customer == null)
        {
            throw new InvalidOperationException("Customer cannot be null while saving step data.");
        }

        var typeSelector = TypeSelector;

        // Clear company fields for non-company type
        if ((typeSelector != null) && !typeSelector.Value.Equals(COMPANY_TYPE))
        {
            customer.CustomerCompany = "";
            customer.CustomerOrganizationID = "";
            customer.CustomerTaxRegistrationID = "";
        }
        
        if (customer.CustomerID < 1 && ShoppingCart.ShoppingCartUserID > 0)
        {
            // Connect newly created customer with registered user -> if user returns to the site, customer`s data will be filled
            customer.CustomerUserID = ShoppingCart.ShoppingCartUserID;
        }
        customer.CustomerSiteID = ShoppingCart.ShoppingCartSiteID;

        var shoppingService = Service.Resolve<IShoppingService>();
        shoppingService.SetCustomer(customer);

        // Address cannot be inserted before customer is saved (otherwise it would be an orphan address)
        // In case of address update - AddressName and AddressPersonalName should be re-generated according current customer info (if not present on the address form)
        if (((shoppingService.GetBillingAddress() == null) ||
            (shoppingService.GetShippingAddress() == null) ||
            (ShoppingCart.ShoppingCartCompanyAddress != null))
            && (customer.CustomerID > 0))
        {
            SaveCustomerAddresses(ShoppingCart);
        }

        ShoppingCartInfoProvider.SetShoppingCartInfo(ShoppingCart);

        // Update contact with customer details
        var dci = DataClassInfoProvider.GetDataClassInfo(customer.TypeInfo.ObjectClassName);
        var mapper = new ContactDataMapper(dci.ClassName, dci.ClassContactOverwriteEnabled);
        var checker = new CustomerContactDataPropagationChecker();
        Service.Resolve<IContactDataInjector>().Inject(customer, ContactID, mapper, checker);
    }

    #endregion


    private void InitCustomerForm()
    {
        customerForm.OnAfterDataLoad += CustomerForm_OnAfterDataLoad;

        if (ShoppingCart.Customer != null)
        {
            customerForm.EditedObject = ShoppingCart.Customer;
        }

        if (!RequestHelper.IsPostBack())
        {
            customerForm.ReloadData();
        }

        // Set first time user customer for postback tax recalculation
        if (PropagateChangesOnPostback && (ShoppingCart.Customer == null))
        {
            ShoppingCart.Customer = customerForm.EditedObject as CustomerInfo;
        }
    }


    private void CustomerForm_OnAfterDataLoad(object sender, EventArgs e)
    {
        // Suppress form validation on Check-in/Check-out actions 
        customerForm.StopProcessing = !ViewMode.IsLiveSite();
    }


    private void ChangeTaxRegistrationIdBasedOnAccountType()
    {
        var typeSelector = TypeSelector;
        var isPersonalType = (typeSelector != null) && !typeSelector.Value.Equals(COMPANY_TYPE);

        var taxRegistrationID = isPersonalType ? string.Empty : ValidationHelper.GetString(customerForm.GetFieldValue("CustomerTaxRegistrationID"), string.Empty);

        if (ShoppingCart.Customer.CustomerTaxRegistrationID != taxRegistrationID)
        {
            ShoppingCart.Customer.CustomerTaxRegistrationID = taxRegistrationID;
            ShoppingCart.Evaluate();

            // Make sure that in-memory changes persist (unsaved address, etc.)
            Service.Resolve<ICurrentShoppingCartService>().SetCurrentShoppingCart(ShoppingCart);

            ComponentEvents.RequestEvents.RaiseEvent(this, null, SHOPPING_CART_CHANGED);
        }
    }


    private void SetCompanyAccountType()
    {
        var typeSelector = TypeSelector;

        if (typeSelector != null && CustomerRepresentsCompany(ShoppingCart.Customer))
        {
            typeSelector.Value = COMPANY_TYPE;
        }
    }


    private static bool CustomerRepresentsCompany(CustomerInfo customer)
    {
        return (customer != null) && (!string.IsNullOrEmpty(customer.CustomerOrganizationID) ||
                                     !string.IsNullOrEmpty(customer.CustomerTaxRegistrationID) ||
                                     !string.IsNullOrEmpty(customer.CustomerCompany));
    }
}