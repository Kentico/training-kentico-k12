using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Membership_Controls_MyProfile : CMSUserControl
{
    #region "Variables"

    // Default form name
    private string mAlternativeFormName = "";

    private bool mAllowEditVisibility;
    public bool mMarkRequiredFields;
    public bool mUseColonBehindLabel = true;
    public string mAfterSaveRedirectURL;
    public string mFormCSSClass;
    public string mSubmitButtonResourceString;
    public string mValidationErrorMessage;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Gets or sets the alternative form name(displays or edits user profile).
    /// </summary>
    public string AlternativeFormName
    {
        get
        {
            return mAlternativeFormName;
        }
        set
        {
            mAlternativeFormName = value;
            editProfileForm.AlternativeFormFullName = value;
            editProfileForm.VisibilityFormName = value;
        }
    }


    /// <summary>
    /// Indicates if field visibility could be edited on user form.
    /// </summary>
    public bool AllowEditVisibility
    {
        get
        {
            return mAllowEditVisibility;
        }
        set
        {
            mAllowEditVisibility = value;
            editProfileForm.AllowEditVisibility = value;
        }
    }


    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            base.IsLiveSite = value;
            editProfileForm.IsLiveSite = value;
            plcMess.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Displays required field mark next to field labels if fields are required. Default value is false.
    /// </summary>
    public bool MarkRequiredFields
    {
        get
        {
            return mMarkRequiredFields;
        }
        set
        {
            mMarkRequiredFields = value;
            editProfileForm.MarkRequiredFields = value;
        }
    }


    /// <summary>
    /// Displays colon behind label text in form. Default value is true.
    /// </summary>
    public bool UseColonBehindLabel
    {
        get
        {
            return mUseColonBehindLabel;
        }
        set
        {
            mUseColonBehindLabel = value;
            editProfileForm.UseColonBehindLabel = value;
        }
    }


    /// <summary>
    /// Relative URL where user is redirected, after form content is successfully modified.
    /// </summary>
    public string AfterSaveRedirectURL
    {
        get
        {
            return mAfterSaveRedirectURL;
        }
        set
        {
            mAfterSaveRedirectURL = value;
            editProfileForm.RedirectUrlAfterSave = value;
        }
    }


    /// <summary>
    /// Form css class.
    /// </summary>
    public string FormCSSClass
    {
        get
        {
            return mFormCSSClass;
        }
        set
        {
            mFormCSSClass = value;
            editProfileForm.CssClass = value;
        }
    }


    /// <summary>
    /// Submit button label. Valid input is resource string.
    /// </summary>
    public string SubmitButtonResourceString
    {
        get
        {
            return mSubmitButtonResourceString;
        }
        set
        {
            mSubmitButtonResourceString = value;
            editProfileForm.SubmitButton.ResourceString = value;
        }
    }


    /// <summary>
    /// Message used when validation fails.
    /// </summary>
    public string ValidationErrorMessage
    {
        get
        {
            return mValidationErrorMessage;
        }
        set
        {
            mValidationErrorMessage = value;
            editProfileForm.ValidationErrorMessage = value;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Sets edited user.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        // Show only to authenticated users
        if ((MembershipContext.AuthenticatedUser != null) && AuthenticationHelper.IsAuthenticated())
        {
            // Register handler for special actions
            editProfileForm.OnBeforeSave += editProfileForm_OnBeforeSave;
            editProfileForm.OnAfterSave += editProfileForm_OnAfterSave;

            // Get up-to-date info of current user and use it for the form
            UserInfo ui = UserInfoProvider.GetUserInfo(MembershipContext.AuthenticatedUser.UserID);
            if (ui != null)
            {
                editProfileForm.Info = ui.Clone();
                editProfileForm.AlternativeFormFullName = AlternativeFormName;
                editProfileForm.VisibilityFormName = AlternativeFormName;
                editProfileForm.AllowEditVisibility = AllowEditVisibility;
                editProfileForm.IsLiveSite = IsLiveSite;
                editProfileForm.RedirectUrlAfterSave = AfterSaveRedirectURL;
                editProfileForm.SubmitButton.ResourceString = SubmitButtonResourceString;
                editProfileForm.CssClass = FormCSSClass;
                editProfileForm.MarkRequiredFields = MarkRequiredFields;
                editProfileForm.UseColonBehindLabel = UseColonBehindLabel;
                editProfileForm.ValidationErrorMessage = ValidationErrorMessage;
            }
        }
        else
        {
            Visible = false;
        }        
    }


    /// <summary>
    /// OnAfterSave handler.
    /// </summary>
    private void editProfileForm_OnAfterSave(object sender, EventArgs e)
    {
        // Update current contact info
        var classInfo = DataClassInfoProvider.GetDataClassInfo(editProfileForm.ClassName);
        ContactInfoProvider.UpdateContactFromExternalData(editProfileForm.Info, classInfo.ClassContactOverwriteEnabled, ModuleCommands.OnlineMarketingGetCurrentContactID());
    }


    /// <summary>
    /// OnBeforeSave handler.
    /// </summary>
    private void editProfileForm_OnBeforeSave(object sender, EventArgs e)
    {
        // If avatarUser id column is set
        if (editProfileForm.Data.GetValue("UserAvatarID") != DBNull.Value)
        {
            // If Avatar not set, rewrite to null value
            if (ValidationHelper.GetInteger(editProfileForm.Data.GetValue("UserAvatarID"), 0) == 0)
            {
                editProfileForm.Data.SetValue("UserAvatarID", DBNull.Value);
            }
        }

        // Set full name as first name + last name
        if (MembershipContext.AuthenticatedUser != null)
        {
            string firstName = ValidationHelper.GetString(editProfileForm.Data.GetValue("FirstName"), "");
            string lastName = ValidationHelper.GetString(editProfileForm.Data.GetValue("LastName"), "");
            string middleName = ValidationHelper.GetString(editProfileForm.Data.GetValue("MiddleName"), "");

            String fullName = ValidationHelper.GetString(editProfileForm.Data.GetValue("FullName"), "");
            var cui = MembershipContext.AuthenticatedUser;

            // Change full name only if it was automatically generated (= is equals to first + middle + last name)
            if (fullName == UserInfoProvider.GetFullName(cui.FirstName, cui.MiddleName, cui.LastName))
            {
                editProfileForm.Data.SetValue("FullName", UserInfoProvider.GetFullName(firstName, middleName, lastName));
            }
        }

        // Ensure unique user email
        string email = ValidationHelper.GetString(editProfileForm.Data.GetValue("Email"), "").Trim();

        // Get current user info
        UserInfo ui = editProfileForm.Info as UserInfo;

        // Check if user email is unique in all sites where he belongs
        if (!UserInfoProvider.IsEmailUnique(email, ui))
        {
            ShowError(GetString("UserInfo.EmailAlreadyExist"));
            editProfileForm.StopProcessing = true;
        }

        #region "Reserved names - nickname"

        // Don't check reserved names for global administrator
        if (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            // Check for reserved user names like administrator, sysadmin, ...
            string nickName = ValidationHelper.GetString(editProfileForm.Data.GetValue("UserNickName"), "");

            if (UserInfoProvider.NameIsReserved(SiteContext.CurrentSiteName, nickName))
            {
                ShowError(GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(nickName)));
                editProfileForm.StopProcessing = true;
            }
        }

        #endregion
    }


    /// <summary>
    /// PreRender.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ControlsHelper.RegisterPostbackControl(editProfileForm.SubmitButton);

        // Alter username according to GetFormattedUserName function
        if ((editProfileForm != null) && (editProfileForm.FieldEditingControls != null))
        {
            EditingFormControl userControl = editProfileForm.FieldEditingControls["UserName"];
            if (userControl != null)
            {
                string userName = ValidationHelper.GetString(userControl.Value, String.Empty);

                // Set back formatted username
                userControl.Value = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(userName, IsLiveSite));
            }
        }
    }

    #endregion
}