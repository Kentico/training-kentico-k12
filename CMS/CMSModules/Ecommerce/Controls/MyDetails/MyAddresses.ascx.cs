using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Ecommerce_Controls_MyDetails_MyAddresses : CMSAdminControl
{
    private int mCustomerId = 0;


    /// <summary>
    /// Customer ID.
    /// </summary>
    public int CustomerId
    {
        get
        {
            return mCustomerId;
        }
        set
        {
            mCustomerId = value;
        }
    }


    /// <summary>
    /// Address ID.
    /// </summary>
    private int AddressId
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["addressid"], 0);
        }
        set
        {
            ViewState["addressid"] = value;
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
            ucCountrySelector.StopProcessing = value;
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
                InitControls();
            }
            else
            {
                // Hide if user is not authenticated
                Visible = false;
            }
        }
    }


    /// <summary>
    /// Control initialization.
    /// </summary>
    protected void InitControls()
    {
        // LIST
        gridAddresses.IsLiveSite = IsLiveSite;
        gridAddresses.OnExternalDataBound += gridAddresses_OnExternalDataBound;
        gridAddresses.WhereCondition = "AddressCustomerID = " + CustomerId;

        // Set pager links text on live site
        if (IsLiveSite)
        {
            gridAddresses.Pager.FirstPageText = "&lt;&lt;";
            gridAddresses.Pager.LastPageText = "&gt;&gt;";
            gridAddresses.Pager.PreviousPageText = "&lt;";
            gridAddresses.Pager.NextPageText = "&gt;";
            gridAddresses.Pager.PreviousGroupText = "...";
            gridAddresses.Pager.NextGroupText = "...";
        }


        btnNew.Text = GetString("Customer_Edit_Address_List.NewItemCaption");

        btnHiddenEdit.Click += btnHiddenEdit_Click;
        btnHiddenDelete.Click += btnHiddenDelete_Click;

        // EDIT
        rqvCity.ErrorMessage = GetString("Customer_Edit_Address_Edit.rqvCity");
        rqvLine.ErrorMessage = GetString("Customer_Edit_Address_Edit.rqvLine");
        rqvZipCode.ErrorMessage = GetString("Customer_Edit_Address_Edit.rqvZipCode");
        rqvPersonalName.ErrorMessage = GetString("Customer_Edit_Address_Edit.rqvPersonalName");

        btnOk.Text = GetString("General.OK");
        btnList.Text = GetString("Customer_Edit_Address_Edit.ItemListLink");

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "MyAddressesSetID", ScriptHelper.GetScript(
            "function setId(id) { \n" +
            "  var hidden = document.getElementById('" + hdnID.ClientID + "'); \n" +
            "  if (hidden != null) { \n" +
            "    hidden.value = id; \n" +
            "  } \n" +
            "}\n"));
    }


    protected void btnHiddenDelete_Click(object sender, EventArgs e)
    {
        // Get AddressId from the row
        AddressId = ValidationHelper.GetInteger(hdnID.Value, 0);

        var address = AddressInfoProvider.GetAddressInfo(AddressId);
        if ((address != null) && (address.AddressCustomerID == CustomerId))
        {
            // Check for the address dependencies
            if (address.Generalized.CheckDependencies())
            {
                lblError.Visible = true;
                lblError.Text = GetString("Ecommerce.DeleteDisabled");
                return;
            }

            // Delete AddressInfo object from database
            AddressInfoProvider.DeleteAddressInfo(address);

            gridAddresses.ReBind();
        }
    }


    protected void btnHiddenEdit_Click(object sender, EventArgs e)
    {
        // Get AddressId from the row
        AddressId = ValidationHelper.GetInteger(hdnID.Value, 0);

        var ai = AddressInfoProvider.GetAddressInfo(AddressId);
        if ((ai != null) && (ai.AddressCustomerID == CustomerId))
        {
            plhList.Visible = false;
            plhEdit.Visible = true;

            LoadData(ai);
        }
    }


    protected object gridAddresses_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        if (sourceName.ToLowerCSafe() == "actions")
        {
            string edit = "<a href=\"javascript: setId('" + Convert.ToString(parameter) + "'); " + Page.ClientScript.GetPostBackEventReference(btnHiddenEdit, "") + "\">" + GetString("general.edit") + "</a>";
            string delete = "<a href=\"javascript: if (confirm(" + ScriptHelper.GetString(GetString("Unigrid.Customer_Edit_Address.Actions.Delete.Confirmation")) + ")) { setId('" + Convert.ToString(parameter) + "'); " + Page.ClientScript.GetPostBackEventReference(btnHiddenDelete, "") + "} \">" + GetString("general.delete") + "</a>";
            return edit + "&nbsp;" + delete;
        }
        return parameter;
    }


    /// <summary>
    /// On btnNew click, go to empty form to create new address.
    /// </summary>
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        plhList.Visible = false;
        plhEdit.Visible = true;

        AddressId = 0;

        InitData();
    }


    /// <summary>
    /// New address form initialization.
    /// </summary>
    protected void InitData()
    {
        lblAddress.Text = "> " + GetString("Customer_Edit_Address_Edit.NewItemCaption");
        // Fill default values
        var customer = CustomerInfoProvider.GetCustomerInfo(mCustomerId);

        if (customer != null)
        {
            var customerAddress = ECommerceHelper.GetLastUsedOrDefaultAddress(mCustomerId);

            txtAddressDeliveryPhone.Text = customer.CustomerPhone;
            txtPersonalName.Text = customer.CustomerFirstName + " " + customer.CustomerLastName;

            txtAddressZip.Text = "";
            txtAddressLine1.Text = "";
            txtAddressLine2.Text = "";
            txtAddressCity.Text = "";
            ucCountrySelector.CountryID = customerAddress.AddressCountryID;
            ucCountrySelector.StateID = customerAddress.AddressStateID;
        }
    }


    /// <summary>
    /// Load address data into editing form.
    /// </summary>
    /// <param name="addressObj">AddressInfo object</param>
    protected void LoadData(AddressInfo addressObj)
    {
        lblAddress.Text = "> " + HTMLHelper.HTMLEncode(addressObj.AddressName);

        txtAddressZip.Text = addressObj.AddressZip;
        txtAddressDeliveryPhone.Text = addressObj.AddressPhone;
        txtPersonalName.Text = addressObj.AddressPersonalName;
        txtAddressLine1.Text = addressObj.AddressLine1;
        txtAddressLine2.Text = addressObj.AddressLine2;
        txtAddressCity.Text = addressObj.AddressCity;

        ucCountrySelector.CountryID = addressObj.AddressCountryID;
        ucCountrySelector.StateID = addressObj.AddressStateID;

        ucCountrySelector.ReloadData(true);
    }


    /// <summary>
    /// On btnList click, return to address list.
    /// </summary>
    protected void btnList_OnClick(object sender, EventArgs e)
    {
        plhList.Visible = true;
        plhEdit.Visible = false;
    }


    /// <summary>
    /// On btnOK click, save edited or new created address.
    /// </summary>
    protected void btnOK_OnClick(object sender, EventArgs e)
    {
        if (mCustomerId != 0)
        {
            // Check field emptiness
            string errorMessage = new Validator().NotEmpty(txtAddressLine1.Text, "Customer_Edit_Address_Edit.rqvLine").NotEmpty(txtAddressCity.Text, "Customer_Edit_Address_Edit.rqvCity").NotEmpty(txtAddressZip.Text, "Customer_Edit_Address_Edit.rqvZipCode").NotEmpty(txtPersonalName.Text, "Customer_Edit_Address_Edit.rqvPersonalName").Result;

            // Check country presence
            if ((errorMessage == "") && (ucCountrySelector.CountryID <= 0))
            {
                errorMessage = GetString("countryselector.selectedcountryerr");
            }

            if (errorMessage == "")
            {
                AddressInfo ai = null;
                // Create new addressinfo or get the existing one
                if (AddressId == 0)
                {
                    ai = new AddressInfo();
                    ai.AddressCustomerID = mCustomerId;
                }
                else
                {
                    ai = AddressInfoProvider.GetAddressInfo(AddressId);
                }

                if (ai != null)
                {
                    ai.AddressPersonalName = txtPersonalName.Text;
                    ai.AddressLine1 = txtAddressLine1.Text;
                    ai.AddressLine2 = txtAddressLine2.Text;
                    ai.AddressCity = txtAddressCity.Text;
                    ai.AddressZip = txtAddressZip.Text;
                    ai.AddressCountryID = ucCountrySelector.CountryID;
                    ai.AddressStateID = ucCountrySelector.StateID;
                    ai.AddressPhone = txtAddressDeliveryPhone.Text;
                    ai.AddressName = AddressInfoProvider.GetAddressName(ai);
                    // Save addressinfo
                    AddressInfoProvider.SetAddressInfo(ai);
                    AddressId = ai.AddressID;

                    lblInfo.Visible = true;
                    lblAddress.Text = "> " + HTMLHelper.HTMLEncode(ai.AddressName);
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = errorMessage;
            }
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
            case "customerid":
                CustomerId = ValidationHelper.GetInteger(value, 0);
                break;
        }

        return true;
    }
}
