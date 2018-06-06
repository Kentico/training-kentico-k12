using System;

using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Membership_Users_UserPublicProfile : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Name of the alternative form (ClassName.AlternativeFormName)
    /// Default value is CMS.User.DisplayProfile
    /// </summary>
    public string AlternativeFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternativeFormName"), "CMS.User.DisplayProfile");
        }
        set
        {
            SetValue("AlternativeFormName", value);
        }
    }


    /// <summary>
    /// User name whose profile should be displayed.
    /// </summary>
    public string UserName
    {
        get
        {
            string userName = ValidationHelper.GetString(GetValue("UserName"), String.Empty);
            if (userName != String.Empty)
            {
                return userName;
            }

            // Back compatibility
            int userID = ValidationHelper.GetInteger(GetValue("UserID"), 0);
            if (userID != 0)
            {
                if (userID == MembershipContext.AuthenticatedUser.UserID)
                {
                    return MembershipContext.AuthenticatedUser.UserName;
                }

                UserInfo ui = UserInfoProvider.GetUserInfo(userID);
                if (ui != null)
                {
                    return ui.UserName;
                }
            }

            return String.Empty;
        }
        set
        {
            SetValue("UserName", value);
        }
    }


    /// <summary>
    /// No profile text.
    /// </summary>
    public string NoProfileText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NoProfileText"), "");
        }
        set
        {
            SetValue("NoProfileText", value);
        }
    }


    /// <summary>
    /// Indicates if field visibility should be applied on user form.
    /// </summary>
    public bool ApplyVisibility
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ApplyVisibility"), false);
        }
        set
        {
            SetValue("ApplyVisibility", value);
        }
    }


    /// <summary>
    /// This name is used if ApplyVisibility is 'true' to get visibility definition of current user.
    /// </summary>
    public string VisibilityFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("VisibilityFormName"), "");
        }
        set
        {
            SetValue("VisibilityFormName", value);
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Get user info
            UserInfo ui = GetUser();
            if (ui != null)
            {
                // Get alternative form info
                AlternativeFormInfo afi = AlternativeFormInfoProvider.GetAlternativeFormInfo(AlternativeFormName);
                if (afi != null)
                {
                    // Initialize data form
                    formElem.Visible = true;
                    formElem.Info = ui;
                    formElem.AlternativeFormFullName = AlternativeFormName;
                    formElem.IsLiveSite = true;
                    formElem.ApplyVisibility = ApplyVisibility;
                    formElem.VisibilityFormName = VisibilityFormName;
                    formElem.SubmitButton.Visible = false;
                }
                else
                {
                    lblError.Text = String.Format(GetString("altform.formdoesntexists"), AlternativeFormName);
                    lblError.Visible = true;
                    plcContent.Visible = false;
                }
            }
            else
            {
                // Hide data form
                formElem.Visible = false;
                lblNoProfile.Visible = true;
                lblNoProfile.Text = NoProfileText;
            }
        }
    }


    // Get user
    private UserInfo GetUser()
    {
        UserInfo ui = null;

        if (!String.IsNullOrEmpty(UserName))
        {
            ui = UserInfoProvider.GetUserInfo(UserName);
        }
        // Otherwise select current user
        else
        {
            ui = MembershipContext.CurrentUserProfile;
        }

        return ui;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Alter username according to GetFormattedUserName function
        if ((formElem != null) && (formElem.FieldEditingControls != null))
        {
            EditingFormControl userControl = formElem.FieldEditingControls["UserName"] as EditingFormControl;
            if (userControl != null)
            {
                string userName = ValidationHelper.GetString(userControl.Value, String.Empty);

                // Set back formatted username
                userControl.Value = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(userName, true));
            }
        }
    }
}