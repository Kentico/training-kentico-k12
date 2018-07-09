using System;
using System.Linq;

using CMS.Core;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;


public partial class CMSWebParts_Ecommerce_CustomerCompanyDetails : CMSAbstractWebPart
{
    #region "Web part properties"

    /// <summary>
    /// Alternative form name to display in edit form, e.g. ecommerce.customer.editcompanydetails or editcompanydetails.
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
    /// Relative URL where user is redirected, after customer company details are successfully modified.
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

        if (ECommerceContext.CurrentCustomer == null)
        {
            Visible = false;
            return;
        }

        EditForm.RedirectUrlAfterSave = AfterSaveRedirectURL;
        EditForm.SubmitButton.ResourceString = SubmitButtonResourceString;
        EditForm.CssClass = CssClass;
        EditForm.MarkRequiredFields = MarkRequiredFields;
        EditForm.UseColonBehindLabel = UseColonBehindLabel;
        EditForm.OnAfterSave += EditForm_OnAfterSave;

        string[] splitFormName = AlternativeFormName.Split('.');

        // UIForm cant process full path of alternative form if object type is already specified.
        EditForm.AlternativeFormName = splitFormName.LastOrDefault();
        EditForm.EditedObject = ECommerceContext.CurrentCustomer;
    }


    private void EditForm_OnAfterSave(object sender, EventArgs eventArgs)
    {
        var currentShoppingCart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart();
        if (currentShoppingCart != null)
        {
            currentShoppingCart.Customer = EditForm.EditedObject as CustomerInfo;
        }
    }
}
