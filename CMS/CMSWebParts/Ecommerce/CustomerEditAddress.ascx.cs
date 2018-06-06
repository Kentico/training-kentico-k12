using System;
using System.Linq;

using CMS.Ecommerce;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;


public partial class CMSWebParts_Ecommerce_CustomerEditAddress : CMSAbstractWebPart
{
    #region "Web part properties"

    /// <summary>
    /// Alternative form name to display in edit form, e.g. ecommerce.address.editaddress or editadress.
    /// </summary>
    public string AlternativeFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternativeFormName"), String.Empty);
        }
        set
        {
            SetValue("AlternativeFormName", value);
        }
    }


    /// <summary>
    /// Relative URL where user is redirected, after address is successfully modified.
    /// </summary>
    public string AfterSaveRedirectURL
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AfterSaveRedirectURL"), String.Empty);
        }
        set
        {
            SetValue("AfterSaveRedirectURL", value);
        }
    }


    /// <summary>
    /// ID of address which will be edited.
    /// </summary>
    public int EditedObjectID
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("EditedObjectID"), 0);
        }
        set
        {
            SetValue("EditedObjectID", value);
        }
    }


    /// <summary>
    /// Indicates if new address is created.
    /// </summary>
    public bool CreateNewAddress
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CreateNewAddress"), false);
        }
        set
        {
            SetValue("CreateNewAddress", value);
        }
    }


    /// <summary>
    /// Submit button label. Valid input is resource string.
    /// </summary>
    public string SubmitButtonResourceString
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SubmitButtonResourceString"), string.Empty);
        }
        set
        {
            SetValue("SubmitButtonResourceString", value);
        }
    }


    /// <summary>
    /// Check permission error message which is displayed when user is not allowed to modify address or address does not exists.
    /// </summary>
    public string CheckPermissionErrorMessage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CheckPermissionErrorMessage"), "");
        }
        set
        {
            SetValue("CheckPermissionErrorMessage", value);
        }
    }


    /// <summary>
    /// Displays required field mark next to field labels if fields are required. Default value is true.
    /// </summary>
    public bool MarkRequiredFields
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("MarkRequiredFields"), true);
        }
        set
        {
            SetValue("MarkRequiredFields", value);
        }
    }


    /// <summary>
    /// Displays colon behind label text in form. Default value is false.
    /// </summary>
    public bool UseColonBehindLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseColonBehindLabel"), false);
        }
        set
        {
            SetValue("UseColonBehindLabel", value);
        }
    }

    #endregion

    /// <summary>
    /// Gets current customer ID.
    /// </summary>
    public int CurrentCustomerID
    {
        get
        {
            return (ECommerceContext.CurrentCustomer == null) ? -1 : ECommerceContext.CurrentCustomer.CustomerID;
        }
    }


    public override void OnContentLoaded()
    {
        base.OnContentLoaded();

        SetupControl();
    }


    private void SetupControl()
    {
        if (StopProcessing)
        {
            return;
        }

        EditForm.RedirectUrlAfterSave = AfterSaveRedirectURL;
        EditForm.SubmitButton.ResourceString = SubmitButtonResourceString;
        EditForm.CssClass = CssClass;
        EditForm.MarkRequiredFields = MarkRequiredFields;
        EditForm.UseColonBehindLabel = UseColonBehindLabel;
        EditForm.OnBeforeSave += EditForm_OnBeforeSave;


        string[] splitFormName = AlternativeFormName.Split('.');
        // UIForm cant process full path of alternative form if object type is already specified.
        EditForm.AlternativeFormName = splitFormName.LastOrDefault();

        if (CurrentCustomerID <= 0)
        {
            ShowError(CheckPermissionErrorMessage);
        }
        else if (!CreateNewAddress)
        {
            // Customers edits existing address
            var address = AddressInfoProvider.GetAddressInfo(EditedObjectID);

            // Allow edit object if user has sufficient permissions to modify address object
            if ((address == null) || (address.AddressCustomerID != CurrentCustomerID))
            {
                ShowError(CheckPermissionErrorMessage);
            }
            else
            {
                EditForm.EditedObject = address;
            }
        }
    }


    void EditForm_OnBeforeSave(object sender, EventArgs e)
    {
        if (CreateNewAddress)
        {
            var newAddress = EditForm.EditedObject as AddressInfo;

            if (newAddress == null)
            {
                return;
            }

            // Ensure address name
            if (String.IsNullOrEmpty(newAddress.AddressName))
            {
                newAddress.AddressName = AddressInfoProvider.GetAddressName(newAddress);
            }

            // Ensure customer
            if (newAddress.AddressCustomerID <= 0)
            {
                newAddress.AddressCustomerID = CurrentCustomerID;
            }
        }
    }


    private void ShowError(string errorMessage)
    {
        lblError.Text = HTMLHelper.HTMLEncode(errorMessage);
        lblError.Visible = true;
        EditForm.StopProcessing = true;
    }
}
