using System;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;


public partial class CMSModules_Ecommerce_Controls_MyDetails_MyDetails : CMSAdminControl
{
    #region "Properties"

    /// <summary>
    /// Customer info object.
    /// </summary>
    public CustomerInfo Customer
    {
        get
        {
            return mCustomer;
        }
        set
        {
            mCustomer = value;
        }
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


    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMessages;
        }
    }

    /// <summary>
    /// Indicates, if Business radio button option was selected.
    /// </summary>
    private bool IsBusiness
    {
        get
        {
            return radAccType.SelectedValue == BUSINESS;
        }
    }

    #endregion


    #region "Delegates & Events"

    /// <summary>
    /// Inform on new customer creation.
    /// </summary>
    public delegate void CustomerCreated();

    /// <summary>
    /// Fired when new customer is created.
    /// </summary>
    public event CustomerCreated OnCustomerCrated;

    #endregion


    #region "Variables & Constants"

    private CustomerInfo mCustomer;

    private const string BUSINESS = "Business";
    private const string PERSONAL = "Personal";

    #endregion


    #region "Page events"

    /// <summary>
    /// On Init page event.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        radAccType.Items.Add(new ListItem(GetString("com.customer.personalaccount"), PERSONAL));
        radAccType.Items.Add(new ListItem(GetString("com.customer.businessaccount"), BUSINESS));
    }

    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Preselect radio button on first load
        if (!RequestHelper.IsPostBack() && (Customer != null) && Customer.CustomerHasCompanyInfo)
        {
            radAccType.SelectedValue = BUSINESS;
        }

        if (StopProcessing)
        {
            return;
        }

        // Hide if user is not authenticated, setting up form data are not required
        if (!AuthenticationHelper.IsAuthenticated())
        {
            Visible = false;
            return;
        }

        btnOk.Text = GetString("General.OK");

        // Displays/hides company info region
        plcCompanyInfo.Visible = IsBusiness;
        lblTaxRegistrationID.Text = GetString("Customers_Edit.lblTaxRegistrationID");
        lblOrganizationID.Text = GetString("Customers_Edit.lblOrganizationID");

        if (mCustomer != null)
        {
            if (!RequestHelper.IsPostBack())
            {
                // Fill editing form
                LoadData();

                // Show that the customer was created or updated successfully
                if (QueryHelper.GetString("saved", String.Empty) == "1")
                {
                    ShowChangesSaved();
                }
            }
        }
        else
        {
            if (!RequestHelper.IsPostBack())
            {
                txtCustomerEmail.Text = MembershipContext.AuthenticatedUser.Email;
            }

            // Show message
            ShowInformation(GetString("MyAccount.MyDetails.CreateNewCustomer"));
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Load form data.
    /// </summary>
    public void LoadData()
    {
        // Fill the form
        txtCustomerCompany.Text = mCustomer.CustomerCompany;
        txtCustomerEmail.Text = mCustomer.CustomerEmail;
        txtCustomerFirstName.Text = mCustomer.CustomerFirstName;
        txtCustomerLastName.Text = mCustomer.CustomerLastName;
        txtCustomerPhone.Text = mCustomer.CustomerPhone;
        txtOraganizationID.Text = mCustomer.CustomerOrganizationID;
        txtTaxRegistrationID.Text = mCustomer.CustomerTaxRegistrationID;

        if (mCustomer.CustomerHasCompanyInfo)
        {
            radAccType.SelectedValue = BUSINESS;
            plcCompanyInfo.Visible = true;
        }
        else
        {
            radAccType.SelectedValue = PERSONAL;
        }
    }


    /// <summary>
    /// Sets data to database.
    /// </summary>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        string siteName = SiteContext.CurrentSiteName;

        if ((txtCustomerCompany.Text.Trim() == "" || !IsBusiness) &&
            ((txtCustomerFirstName.Text.Trim() == "") || (txtCustomerLastName.Text.Trim() == "")))
        {
            errorMessage = GetString("Customers_Edit.errorInsert");
        }

        // At least company name has to be filled when company account is selected
        if (errorMessage == "" && IsBusiness)
        {
            errorMessage = new Validator().NotEmpty(txtCustomerCompany.Text, GetString("customers_edit.errorCompany")).Result;
        }

        // Check the following items if complete company info is required for company account
        if (errorMessage == "" && ECommerceSettings.RequireCompanyInfo(siteName) && IsBusiness)
        {
            errorMessage = new Validator().NotEmpty(txtOraganizationID.Text, GetString("customers_edit.errorOrganizationID"))
                .NotEmpty(txtTaxRegistrationID.Text, GetString("customers_edit.errorTaxRegID")).Result;
        }

        if (errorMessage == "")
        {
            errorMessage = new Validator().IsEmail(txtCustomerEmail.Text.Trim(), GetString("customers_edit.erroremailformat"), true)
                 .MatchesCondition(txtCustomerPhone.Text.Trim(), k => k.Length < 50, GetString("customers_edit.errorphoneformat")).Result;
        }

        plcCompanyInfo.Visible = IsBusiness;

        if (errorMessage == "")
        {
            // If customer doesn't already exist, create new one
            if (mCustomer == null)
            {
                mCustomer = new CustomerInfo();
                mCustomer.CustomerUserID = MembershipContext.AuthenticatedUser.UserID;
            }

            mCustomer.CustomerEmail = txtCustomerEmail.Text.Trim();
            mCustomer.CustomerLastName = txtCustomerLastName.Text.Trim();
            mCustomer.CustomerPhone = txtCustomerPhone.Text.Trim();
            mCustomer.CustomerFirstName = txtCustomerFirstName.Text.Trim();
            mCustomer.CustomerCreated = DateTime.Now;

            if (IsBusiness)
            {
                mCustomer.CustomerCompany = txtCustomerCompany.Text.Trim();
                mCustomer.CustomerOrganizationID = txtOraganizationID.Text.Trim();
                mCustomer.CustomerTaxRegistrationID = txtTaxRegistrationID.Text.Trim();
            }
            else
            {
                mCustomer.CustomerCompany = "";
                mCustomer.CustomerOrganizationID = "";
                mCustomer.CustomerTaxRegistrationID = "";
            }

            // Update customer data
            CustomerInfoProvider.SetCustomerInfo(mCustomer);

            // Update corresponding contact data
            int currentContactId = ModuleCommands.OnlineMarketingGetCurrentContactID();
            ModuleCommands.OnlineMarketingCreateRelation(mCustomer.CustomerID, MembershipType.ECOMMERCE_CUSTOMER, currentContactId);
            ContactInfoProvider.UpdateContactFromExternalData(
                mCustomer, 
                DataClassInfoProvider.GetDataClassInfo(CustomerInfo.TYPEINFO.ObjectClassName).ClassContactOverwriteEnabled,
                currentContactId);

            // Let others now that customer was created
            if (OnCustomerCrated != null)
            {
                OnCustomerCrated();

                ShowChangesSaved();
            }
            else
            {
                URLHelper.Redirect(URLHelper.AddParameterToUrl(RequestContext.CurrentURL, "saved", "1"));
            }
        }
        else
        {
            //Show error
            ShowError(errorMessage);
        }
    }


    /// <summary>
    /// Overridden SetValue - because of MyAccount webpart.
    /// </summary>
    /// <param name="propertyName">Name of the property to set</param>
    /// <param name="value">Value to set</param>
    public override bool SetValue(string propertyName, object value)
    {
        base.SetValue(propertyName, value);

        switch (propertyName.ToLowerCSafe())
        {
            case "customer":
                GeneralizedInfo gi = value as GeneralizedInfo;
                if (gi != null)
                {
                    Customer = gi.MainObject as CustomerInfo;
                }
                break;
        }

        return true;
    }

    #endregion
}