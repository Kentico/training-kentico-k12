using System;
using System.Text;

using CMS.Core;
using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Modules;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.DocumentEngine;
using CMS.WebAnalytics;
using CMS.Protection;
using CMS.DataEngine;
using CMS.ExternalAuthentication;


public partial class CMSWebParts_Membership_Registration_LiveIDUsersRequiredData : CMSAbstractWebPart
{
    #region "Private variables"

    private WindowsLiveLogin.User liveUser;
    private string mDefaultTargetUrl = String.Empty;
    private IMembershipActivityLogger mMembershipActivityLogger;


    private IMembershipActivityLogger MembershipActivityLogger => mMembershipActivityLogger ?? (mMembershipActivityLogger = Service.Resolve<IMembershipActivityLogger>());

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates whether email to user should be sent.
    /// </summary>
    public bool SendWelcomeEmail
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SendWelcomeEmail"), true);
        }
        set
        {
            SetValue("SendWelcomeEmail", value);
        }
    }


    /// <summary>
    /// Gets or sets registration approval page URL.
    /// </summary>
    public string ApprovalPage
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ApprovalPage"), "");
        }
        set
        {
            SetValue("ApprovalPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the sender email (from).
    /// </summary>
    public string FromAddress
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("FromAddress"), SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSNoreplyEmailAddress"));
        }
        set
        {
            SetValue("FromAddress", value);
        }
    }


    /// <summary>
    /// Gets or sets the recipient email (to).
    /// </summary>
    public string ToAddress
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ToAddress"), SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSAdminEmailAddress"));
        }
        set
        {
            SetValue("ToAddress", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether after successful registration is 
    /// notification email sent to the administrator 
    /// </summary>
    public bool NotifyAdministrator
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("NotifyAdministrator"), false);
        }
        set
        {
            SetValue("NotifyAdministrator", value);
        }
    }


    /// <summary>
    /// Gets or sets the message which is displayed after successful registration.
    /// </summary>
    public string DisplayMessage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DisplayMessage"), "");
        }
        set
        {
            SetValue("DisplayMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets the value which enables abitity of new user to set password.
    /// </summary>
    public bool AllowFormsAuthentication
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowFormsAuthentication"), false);
        }
        set
        {
            SetValue("AllowFormsAuthentication", value);
            plcPasswordNew.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the value which enables abitity join liveid with existing account.
    /// </summary>
    public bool AllowExistingUser
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowExistingUser"), true);
        }
        set
        {
            SetValue("AllowExistingUser", value);
            plcPasswordNew.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the default target url (rediredction when the user is logged in).
    /// </summary>
    public string DefaultTargetUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DefaultTargetUrl"), mDefaultTargetUrl);
        }
        set
        {
            SetValue("DefaultTargetUrl", value);
            mDefaultTargetUrl = value;
        }
    }


    /// <summary>
    /// Gets or sets the value which determines the behaviour if no Live ID user.
    /// </summary>
    public bool HideForNoLiveID
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideForNoLiveID"), false);
        }
        set
        {
            SetValue("HideForNoLiveID", value);
        }
    }

    #endregion


    #region "Conversion properties"

    /// <summary>
    /// Gets or sets the conversion track name used after successful registration.
    /// </summary>
    public string TrackConversionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TrackConversionName"), "");
        }
        set
        {
            if ((value != null) && (value.Length > 400))
            {
                value = value.Substring(0, 400);
            }
            SetValue("TrackConversionName", value);
        }
    }


    /// <summary>
    /// Gets or sets the conversion value used after successful registration.
    /// </summary>
    public double ConversionValue
    {
        get
        {
            return ValidationHelper.GetDoubleSystem(GetValue("ConversionValue"), 0);
        }
        set
        {
            SetValue("ConversionValue", value);
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
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
            if (SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".CMSEnableWindowsLiveID"))
            {
                plcPasswordNew.Visible = AllowFormsAuthentication;
                pnlExistingUser.Visible = AllowExistingUser;

                liveUser = SessionHelper.GetValue("windowsliveloginuser") as WindowsLiveLogin.User;

                // There is no windows live user object stored in session - hide all
                if ((liveUser == null) && HideForNoLiveID)
                {
                    Visible = false;
                }

                // WAI validation
                lblPasswordNew.AssociatedControlClientID = passStrength.InputClientID;
            }
            else
            {
                // Error label is displayed in Design mode when Windows Live ID is disabled
                if (PortalContext.IsDesignMode(PortalContext.ViewMode))
                {
                    StringBuilder parameter = new StringBuilder();
                    parameter.Append(UIElementInfoProvider.GetApplicationNavigationString("cms", "Settings") + " -> ");
                    parameter.Append(GetString("settingscategory.cmsmembership") + " -> ");
                    parameter.Append(GetString("settingscategory.cmsmembershipauthentication") + " -> ");
                    parameter.Append(GetString("settingscategory.cmswindowsliveid"));
                    if (MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
                    {
                        // Make it link for Admin
                        parameter.Insert(0, "<a href=\"" + URLHelper.GetAbsoluteUrl(ApplicationUrlHelper.GetApplicationUrl("cms", "settings")) + "\" target=\"_top\">");
                        parameter.Append("</a>");
                    }

                    lblError.Text = String.Format(GetString("mem.liveid.disabled"), parameter.ToString());
                    plcError.Visible = true;
                    plcContent.Visible = false;
                }
                else
                {
                    Visible = false;
                }
            }
        }
    }


    /// <summary>
    /// Handles btnOkExist click, joins existing user with liveid token.
    /// </summary>
    protected void btnOkExist_Click(object sender, EventArgs e)
    {
        // Live user must be retrieved from session
        if (liveUser != null)
        {
            if (txtUserName.Text != String.Empty) // && (txtPassword.Text != String.Empty))
            {
                // Try to authenticate user
                UserInfo ui = AuthenticationHelper.AuthenticateUser(txtUserName.Text, txtPassword.Text, SiteContext.CurrentSiteName);

                // Check banned IPs
                BannedIPInfoProvider.CheckIPandRedirect(SiteContext.CurrentSiteName, BanControlEnum.Login);

                if (ui != null)
                {
                    // Add liveID token to user
                    ui.UserSettings.WindowsLiveID = liveUser.Id;
                    UserInfoProvider.SetUserInfo(ui);

                    // Remove live user object from session, won't be needed
                    Session.Remove("windowsliveloginuser");

                    // Set authentication cookie and redirect to page
                    SetAuthCookieAndRedirect(ui);
                }
                else // Invalid credentials
                {
                    lblError.Text = GetString("Login_FailureText");
                    plcError.Visible = true;
                }
            }
            else // User did not fill the form
            {
                lblError.Text = GetString("mem.liveid.fillloginform");
                plcError.Visible = true;
            }
        }
    }


    /// <summary>
    /// Handles btnOkNew click, creates new user and joins it with liveid token.
    /// </summary>
    protected void btnOkNew_Click(object sender, EventArgs e)
    {
        if (liveUser != null)
        {
            // Validate entered values
            string errorMessage = new Validator().IsRegularExp(txtUserNameNew.Text, "^([a-zA-Z0-9_\\-\\.@]+)$", GetString("mem.liveid.fillcorrectusername"))
                .IsEmail(txtEmail.Text, GetString("mem.liveid.fillvalidemail")).Result;

            string password = passStrength.Text.Trim();

            // If password is enabled to set, check it
            if (plcPasswordNew.Visible && (errorMessage == String.Empty))
            {
                if (password == String.Empty)
                {
                    errorMessage = GetString("mem.liveid.specifyyourpass");
                }
                else if (password != txtConfirmPassword.Text.Trim())
                {
                    errorMessage = GetString("webparts_membership_registrationform.passwordonotmatch");
                }

                // Check policy
                if (!passStrength.IsValid())
                {
                    errorMessage = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName);
                }
            }

            string siteName = SiteContext.CurrentSiteName;

            // Check whether email is unique if it is required
            if ((errorMessage == String.Empty) && !UserInfoProvider.IsEmailUnique(txtEmail.Text.Trim(), siteName, 0))
            {
                errorMessage = GetString("UserInfo.EmailAlreadyExist");
            }

            // Check reserved names
            if ((errorMessage == String.Empty) && UserInfoProvider.NameIsReserved(siteName, txtUserNameNew.Text.Trim()))
            {
                errorMessage = GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(txtUserNameNew.Text.Trim()));
            }

            if (errorMessage == String.Empty)
            {
                string userName = txtUserNameNew.Text.Trim();
                // Check if user with given username already exists
                UserInfo ui = UserInfoProvider.GetUserInfo(userName);
                UserInfo siteui = UserInfoProvider.GetUserInfo(UserInfoProvider.EnsureSitePrefixUserName(userName, SiteContext.CurrentSite));

                // User with given username is already registered
                if ((ui != null) || (siteui != null))
                {
                    plcError.Visible = true;
                    lblError.Text = GetString("mem.openid.usernameregistered");
                }
                else
                {
                    // Register new user
                    string error = DisplayMessage;
                    ui = AuthenticationHelper.AuthenticateWindowsLiveUser(liveUser.Id, siteName, false, ref error);
                    DisplayMessage = error;

                    if (ui != null)
                    {
                        // Set additional information
                        ui.UserName = ui.UserNickName = ui.FullName = userName;

                        // Ensure site prefixes
                        if (UserInfoProvider.UserNameSitePrefixEnabled(siteName))
                        {
                            ui.UserName = UserInfoProvider.EnsureSitePrefixUserName(userName, SiteContext.CurrentSite);
                        }

                        ui.Email = txtEmail.Text;

                        // Set password
                        if (plcPasswordNew.Visible)
                        {
                            UserInfoProvider.SetPassword(ui, password);

                            // If user can choose password then is not considered external(external user can't login in common way)
                            ui.IsExternal = false;
                        }

                        UserInfoProvider.SetUserInfo(ui);

                        // Remove live user object from session, won't be needed
                        Session.Remove("windowsliveloginuser");

                        // Send registration e-mails
                        AuthenticationHelper.SendRegistrationEmails(ui, ApprovalPage, true, SendWelcomeEmail);

                        // Notify administrator
                        bool requiresConfirmation = SettingsKeyInfoProvider.GetBoolValue(siteName + ".CMSRegistrationEmailConfirmation");
                        if (!requiresConfirmation && NotifyAdministrator && (FromAddress != String.Empty) && (ToAddress != String.Empty))
                        {
                            AuthenticationHelper.NotifyAdministrator(ui, FromAddress, ToAddress);
                        }

                        // Log user registration into the web analytics and track conversion if set
                        AnalyticsHelper.TrackUserRegistration(siteName, ui, TrackConversionName, ConversionValue);

                        MembershipActivityLogger.LogRegistration(ui.UserName, DocumentContext.CurrentDocument);

                        // Set authentication cookie and redirect to page
                        SetAuthCookieAndRedirect(ui);

                        // Display error message
                        if (!String.IsNullOrEmpty(DisplayMessage))
                        {
                            lblInfo.Visible = true;
                            lblInfo.Text = DisplayMessage;
                            plcForm.Visible = false;
                        }
                        else
                        {
                            URLHelper.Redirect(ResolveUrl("~/Default.aspx"));
                        }
                    }
                }
            }
            else
            {
                lblError.Text = errorMessage;
                plcError.Visible = true;
            }
        }
    }


    /// <summary>
    /// Helper method, set authentication cookie and redirect to return URL or default page.
    /// </summary>
    /// <param name="ui">User info</param>
    private void SetAuthCookieAndRedirect(UserInfo ui)
    {
        // Create autentification cookie
        if (ui.Enabled)
        {
            AuthenticationHelper.SetAuthCookieWithUserData(ui.UserName, true, Session.Timeout, new[] { "liveidlogin" });

            // Log activity
            MembershipActivityLogger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);
            
            // Redirect to default page    
            if (!String.IsNullOrEmpty(DefaultTargetUrl))
            {
                URLHelper.Redirect(ResolveUrl(DefaultTargetUrl));
            }
            // If there is some return page redirect there
            else if ((liveUser != null) && !string.IsNullOrEmpty(liveUser.Context))
            {
                URLHelper.Redirect(UrlResolver.ResolveUrl(liveUser.Context));
            }
            // Refresh current page to update see user signed in
            else
            {
                string url = RequestContext.CurrentURL;
                URLHelper.Redirect(url);
            }
        }
    }

    #endregion
}