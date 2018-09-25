using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using CMS.Core;
using CMS.Activities.Loggers;
using CMS.DocumentEngine;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.MembershipProvider;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.SiteProvider;


public partial class CMSWebParts_Membership_Logon_LogonMiniForm : CMSAbstractWebPart, ICallbackEventHandler
{
    #region "Local variables"

    private TextBox user = null;
    private TextBox pass = null;
    private LocalizedButton login = null;
    private LocalizedLabel lblUserName = null;
    private LocalizedLabel lblPassword = null;
    private ImageButton loginImg = null;
    private RequiredFieldValidator rfv = null;
    private Panel container = null;
    private string mDefaultTargetUrl = "";
    private string mUserNameText = "";
    private bool mShowUserNameLabel = false;
    private bool mShowPasswordLabel = false;

    #endregion


    #region "Private properties"

    /// <summary>
    /// Gets error displayed by login control
    /// </summary>
    private string DisplayedError
    {
        get
        {
            var failureLit = loginElem.FindControl("FailureText") as LocalizedLabel;
            if (failureLit != null)
            {
                return failureLit.Text;
            }

            return null;
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates if the username label should be displayed.
    /// </summary>
    public bool ShowUserNameLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowUserNameLabel"), mShowUserNameLabel);
        }
        set
        {
            SetValue("ShowUserNameLabel", value);
            mShowUserNameLabel = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates if the password label should be displayed.
    /// </summary>
    public bool ShowPasswordLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowPasswordLabel"), mShowPasswordLabel);
        }
        set
        {
            SetValue("ShowPasswordLabel", value);
            mShowPasswordLabel = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether image button is displayed instead of regular button.
    /// </summary>
    public bool ShowImageButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowImageButton"), false);
        }
        set
        {
            SetValue("ShowImageButton", value);
            login.Visible = !value;
            loginImg.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets an Image button URL.
    /// </summary>
    public string ImageUrl
    {
        get
        {
            return ResolveUrl(ValidationHelper.GetString(GetValue("ImageUrl"), loginImg.ImageUrl));
        }
        set
        {
            SetValue("ImageUrl", value);
            loginImg.ImageUrl = value;
        }
    }


    /// <summary>
    /// Gets or sets the logon failure text.
    /// </summary>
    public string FailureText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FailureText"), "");
        }
        set
        {
            if (!string.IsNullOrEmpty(value.Trim()))
            {
                SetValue("FailureText", value);
            }
        }
    }


    /// <summary>
    /// Gets or sets the default target url (redirection when the user is logged in).
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
    /// Gets or sets the username text.
    /// </summary>
    public string UserNameText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("UserNameText"), mUserNameText);
        }
        set
        {
            if (value.Trim() != "")
            {
                SetValue("UserNameText", value);
                mUserNameText = value;
            }
        }
    }


    /// <summary>
    /// Gets or sets whether show error as popup window.
    /// </summary>
    public bool ErrorAsPopup
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ErrorAsPopup"), false);
        }
        set
        {
            SetValue("ErrorAsPopup", value);
        }
    }


    /// <summary>
    /// Gets or sets whether make login persistent.
    /// </summary>
    public bool PersistentLogin
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PersistentLogin"), false);
        }
        set
        {
            SetValue("PersistentLogin", value);
        }
    }

    #endregion


    #region "Overridden methods"

    /// <summary>
    /// Applies given stylesheet skin.
    /// </summary>
    public override void ApplyStyleSheetSkin(Page page)
    {
        SetSkinID(SkinID);
        base.ApplyStyleSheetSkin(page);
    }


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
    /// Pre render event handler.
    /// </summary>
    /// <param name="e">Event arguments</param>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Hide webpart for non-public users
        Visible &= MembershipContext.AuthenticatedUser.IsPublic();
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    private void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // WAI validation
            lblUserName = (LocalizedLabel)loginElem.FindControl("lblUserName");
            if (lblUserName != null)
            {
                lblUserName.Text = GetString("general.username");
                if (!ShowUserNameLabel)
                {
                    lblUserName.Attributes.Add("style", "display: none;");
                }
            }
            lblPassword = (LocalizedLabel)loginElem.FindControl("lblPassword");
            if (lblPassword != null)
            {
                lblPassword.Text = GetString("general.password");
                if (!ShowPasswordLabel)
                {
                    lblPassword.Attributes.Add("style", "display: none;");
                }
            }

            // Set properties for validator
            rfv = (RequiredFieldValidator)loginElem.FindControl("rfvUserNameRequired");
            rfv.ToolTip = GetString("LogonForm.NameRequired");
            rfv.Text = rfv.ErrorMessage = GetString("LogonForm.EnterName");
            rfv.ValidationGroup = ClientID + "_MiniLogon";

            // Set visibility of buttons
            login = (LocalizedButton)loginElem.FindControl("btnLogon");
            if (login != null)
            {
                login.Visible = !ShowImageButton;
                login.ValidationGroup = ClientID + "_MiniLogon";
            }

            loginImg = (ImageButton)loginElem.FindControl("btnImageLogon");
            if (loginImg != null)
            {
                loginImg.Visible = ShowImageButton;
                loginImg.ImageUrl = ImageUrl;
                loginImg.ValidationGroup = ClientID + "_MiniLogon";
            }

            // Ensure display control as inline and is used right default button
            container = (Panel)loginElem.FindControl("pnlLogonMiniForm");
            if (container != null)
            {
                container.Attributes.Add("style", "display: inline;");
                if (ShowImageButton)
                {
                    if (loginImg != null)
                    {
                        container.DefaultButton = loginImg.ID;
                    }
                    else if (login != null)
                    {
                        container.DefaultButton = login.ID;
                    }
                }
            }

            CMSTextBox txtUserName = (CMSTextBox)loginElem.FindControl("UserName");
            if (txtUserName != null)
            {
                txtUserName.EnableAutoComplete = SecurityHelper.IsAutoCompleteEnabledForLogin(SiteContext.CurrentSiteName);
            }

            if (!string.IsNullOrEmpty(UserNameText))
            {
                // Initialize javascript for focus and blur UserName textbox
                user = (TextBox)loginElem.FindControl("UserName");
                user.Attributes.Add("onfocus", "MLUserFocus_" + ClientID + "('focus');");
                user.Attributes.Add("onblur", "MLUserFocus_" + ClientID + "('blur');");
                string focusScript = "function MLUserFocus_" + ClientID + "(type)" +
                                     "{" +
                                     "var userNameBox = document.getElementById('" + user.ClientID + "');" +
                                     "if(userNameBox.value == '" + UserNameText + "' && type == 'focus')" +
                                     "{userNameBox.value = '';}" +
                                     "else if (userNameBox.value == '' && type == 'blur')" +
                                     "{userNameBox.value = '" + UserNameText + "';}" +
                                     "}";

                ScriptHelper.RegisterClientScriptBlock(this, GetType(), "MLUserNameFocus_" + ClientID,
                                                       ScriptHelper.GetScript(focusScript));
            }
            loginElem.LoggedIn += loginElem_LoggedIn;
            loginElem.LoggingIn += loginElem_LoggingIn;
            loginElem.LoginError += loginElem_LoginError;
            loginElem.Authenticate += loginElem_Authenticate;

            if (!RequestHelper.IsPostBack())
            {
                // Set SkinID properties
                if (!StandAlone && (PageCycle < PageCycleEnum.Initialized) && (ValidationHelper.GetString(Page.StyleSheetTheme, "") == ""))
                {
                    SetSkinID(SkinID);
                }
            }

            if (string.IsNullOrEmpty(loginElem.UserName))
            {
                loginElem.UserName = UserNameText;
            }

            // Register script to update logon error message
            LocalizedLabel failureLit = loginElem.FindControl("FailureText") as LocalizedLabel;
            if (failureLit != null)
            {
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append(@"
function UpdateLabel_", ClientID, @"(content, context) {
    var lbl = document.getElementById(context);
    if(lbl)
    {
        lbl.innerHTML = content;
        lbl.className = ""InfoLabel"";      
    }
}");
                ScriptHelper.RegisterClientScriptBlock(this, GetType(), "InvalidLogonAttempts_" + ClientID, sbScript.ToString(), true);
            }
        }
    }


    /// <summary>
    /// Displays error.
    /// </summary>
    /// <param name="msg">Message.</param>
    private void DisplayError(string msg)
    {
        var failureLit = loginElem.FindControl("FailureText") as LocalizedLabel;

        if (failureLit != null)
        {
            failureLit.Text = msg;
            failureLit.Visible = !string.IsNullOrEmpty(msg);
        }
    }


    /// <summary>
    /// Hides displayed error.
    /// </summary>
    private void HideError()
    {
        DisplayError("");
    }


    /// <summary>
    /// Sets SkinId to all controls in logon form.
    /// </summary>
    private void SetSkinID(string skinId)
    {
        if (skinId != "")
        {
            loginElem.SkinID = skinId;

            user = (TextBox)loginElem.FindControl("UserName");
            if (user != null)
            {
                user.SkinID = skinId;
            }

            pass = (TextBox)loginElem.FindControl("Password");
            if (pass != null)
            {
                pass.SkinID = skinId;
            }

            login = (LocalizedButton)loginElem.FindControl("btnLogon");
            if (login != null)
            {
                login.SkinID = skinId;
            }

            loginImg = (ImageButton)loginElem.FindControl("btnImageLogon");
            if (loginImg != null)
            {
                loginImg.SkinID = skinId;
            }
        }
    }

    #endregion


    #region "Logging handlers"

    /// <summary>
    /// Logged in handler.
    /// </summary>
    private void loginElem_LoggedIn(object sender, EventArgs e)
    {
        // Set view mode to live site after login to prevent bar with "Close preview mode"
        PortalContext.ViewMode = ViewModeEnum.LiveSite;

        // Ensure response cookie
        CookieHelper.EnsureResponseCookie(FormsAuthentication.FormsCookieName);

        // Set cookie expiration
        if (loginElem.RememberMeSet)
        {
            CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, DateTime.Now.AddYears(1), false);
        }
        else
        {
            // Extend the expiration of the authentication cookie if required
            if (!AuthenticationHelper.UseSessionCookies && (HttpContext.Current != null) && (HttpContext.Current.Session != null))
            {
                CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, DateTime.Now.AddMinutes(Session.Timeout), false);
            }
        }

        // Current username
        string userName = loginElem.UserName;

        // Get user name (test site prefix too)
        UserInfo ui = UserInfoProvider.GetUserInfoForSitePrefix(userName, SiteContext.CurrentSite);

        // Check whether safe user name is required and if so get safe username
        if (AuthenticationMode.IsMixedAuthentication() && UserInfoProvider.UseSafeUserName)
        {
            // User stored with safe name
            userName = ValidationHelper.GetSafeUserName(loginElem.UserName, SiteContext.CurrentSiteName);

            // Find user by safe name
            ui = UserInfoProvider.GetUserInfoForSitePrefix(userName, SiteContext.CurrentSite);
            if (ui != null)
            {
                // Authenticate user by site or global safe username
                AuthenticationHelper.AuthenticateUser(ui.UserName, loginElem.RememberMeSet);
            }
        }

        // Log activity (warning: CMSContext contains info of previous user)
        if (ui != null)
        {
            // If user name is site prefixed, authenticate user manually 
            if (UserInfoProvider.IsSitePrefixedUser(ui.UserName))
            {
                AuthenticationHelper.AuthenticateUser(ui.UserName, loginElem.RememberMeSet);
            }

            // Log activity
            Service.Resolve<IMembershipActivityLogger>().LogLogin(ui.UserName, DocumentContext.CurrentDocument);
        }

        // Redirect user to the return URL, or if is not defined redirect to the default target URL
        var redirectUrl = RequestContext.CurrentURL;
        string url = QueryHelper.GetString("ReturnURL", String.Empty);

        if (!String.IsNullOrEmpty(url) && URLHelper.IsLocalUrl(url))
        {
            redirectUrl = url;
        }
        else if (!String.IsNullOrEmpty(DefaultTargetUrl))
        {
            redirectUrl = ResolveUrl(DefaultTargetUrl);
        }

        URLHelper.Redirect(redirectUrl);
    }


    /// <summary>
    /// Logging in handler.
    /// </summary>
    private void loginElem_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        loginElem.RememberMeSet = PersistentLogin;
    }


    /// <summary>
    /// Handling login authenticate event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Authenticate event arguments.</param>
    private void loginElem_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (MFAuthenticationHelper.IsMultiFactorRequiredForUser(loginElem.UserName))
        {
            var plcPasscodeBox = loginElem.FindControl("plcPasscodeBox");
            var plcLoginInputs = loginElem.FindControl("plcLoginInputs");
            var txtPasscode = loginElem.FindControl("txtPasscode") as CMSTextBox;

            if (txtPasscode == null)
            {
                return;
            }
            if (plcPasscodeBox == null)
            {
                return;
            }
            if (plcLoginInputs == null)
            {
                return;
            }

            // Handle passcode
            string passcode = txtPasscode.Text;
            txtPasscode.Text = "";

            var provider = new CMSMembershipProvider();

            // Validate username and password
            if (plcLoginInputs.Visible)
            {
                if (provider.MFValidateCredentials(loginElem.UserName, loginElem.Password))
                {
                    // Show passcode screen
                    plcLoginInputs.Visible = false;
                    plcPasscodeBox.Visible = true;
                }
            }
            // Validate passcode
            else
            {
                if (provider.MFValidatePasscode(loginElem.UserName, passcode))
                {
                    e.Authenticated = true;
                }
            }
        }
        else
        {
            try
            {
                e.Authenticated = Membership.Provider.ValidateUser(loginElem.UserName, loginElem.Password);
            }
            catch (ConfigurationException ex)
            {
                EventLogProvider.LogException("LogonMiniForm", "VALIDATEUSER", ex);
                var provider = new CMSMembershipProvider();
                e.Authenticated = provider.ValidateUser(loginElem.UserName, loginElem.Password);
            }
        }

    }


    /// <summary>
    /// Login error handler.
    /// </summary>
    protected void loginElem_LoginError(object sender, EventArgs e)
    {
        bool showError = true;

        // Ban IP addresses which are blocked for login
        if (MembershipContext.UserIsBanned)
        {
            DisplayError(GetString("banip.ipisbannedlogin"));
        }
        // Check if account locked due to reaching maximum invalid logon attempts
        else if (AuthenticationHelper.DisplayAccountLockInformation(SiteContext.CurrentSiteName) && MembershipContext.UserAccountLockedDueToInvalidLogonAttempts)
        {
            string msg = GetString("invalidlogonattempts.unlockaccount.accountlocked");

            if (!ErrorAsPopup)
            {
                msg += " " + string.Format(GetString("invalidlogonattempts.unlockaccount.accountlockedlink"), GetLogonAttemptsUnlockingLink());
            }
            DisplayError(msg);
        }
        // Check if account locked due to password expiration
        else if (AuthenticationHelper.DisplayAccountLockInformation(SiteContext.CurrentSiteName) && MembershipContext.UserAccountLockedDueToPasswordExpiration)
        {
            string msg = GetString("passwordexpiration.accountlocked");
            
            if (!ErrorAsPopup)
            {
                msg += " " + string.Format(GetString("invalidlogonattempts.unlockaccount.accountlockedlink"), GetLogonAttemptsUnlockingLink());
            }
            DisplayError(msg);
        }
        else if (MembershipContext.UserIsPartiallyAuthenticated && !MembershipContext.UserAuthenticationFailedDueToInvalidPasscode)
        {
            if (MembershipContext.MFAuthenticationTokenNotInitialized && MFAuthenticationHelper.DisplaySetupCode)
            {
                var plcTokenInfo = loginElem.FindControl("plcTokenInfo");
                var lblTokenID = loginElem.FindControl("lblTokenID") as LocalizedLabel;

                if (lblTokenID != null)
                {
                    lblTokenID.Text = string.Format("{0} {1}", GetString("mfauthentication.label.token"), MFAuthenticationHelper.GetSetupCodeForUser(loginElem.UserName));
                }

                if (plcTokenInfo != null)
                {
                    plcTokenInfo.Visible = true;
                }
            }

            if (string.IsNullOrEmpty(DisplayedError))
            {
                HideError();
            }

            showError = false;
        }
        else if (!MembershipContext.UserIsPartiallyAuthenticated)
        {
            // Show login and password screen
            var plcPasscodeBox = loginElem.FindControl("plcPasscodeBox");
            var plcLoginInputs = loginElem.FindControl("plcLoginInputs");
            var plcTokenInfo = loginElem.FindControl("plcTokenInfo");
            if (plcLoginInputs != null)
            {
                plcLoginInputs.Visible = true;
            }
            if (plcPasscodeBox != null)
            {
                plcPasscodeBox.Visible = false;
            }
            if (plcTokenInfo != null)
            {
                plcTokenInfo.Visible = false;
            }
        }

        if (showError && string.IsNullOrEmpty(DisplayedError))
        {
            DisplayError(DataHelper.GetNotEmpty(FailureText, GetString("Login_FailureText")));
        }

        // Display the failure message in a client-side alert box
        if (ErrorAsPopup)
        {
            if (string.IsNullOrEmpty(DisplayedError))
            {
                return;
            }
            ScriptHelper.RegisterStartupScript(this, GetType(), "LoginError", ScriptHelper.GetScript("alert(" + ScriptHelper.GetString(HTMLHelper.StripTags(DisplayedError)) + ");"));

            // Hide error message
            HideError();
        }
    }


    /// <summary>
    /// Return link for unlocking logon attempts. 
    /// </summary>
    private string GetLogonAttemptsUnlockingLink()
    {
        var failureLit = loginElem.FindControl("FailureText") as LocalizedLabel;
        if (failureLit != null)
        {
            return "<a href=\"#\" onclick=\"" + Page.ClientScript.GetCallbackEventReference(this, "null", "UpdateLabel_" + ClientID, "'" + failureLit.ClientID + "'") + ";\">" + GetString("general.clickhere") + "</a>";
        }
        return "";
    }

    #endregion


    #region "ICallbackEventHandler Members"

    public string GetCallbackResult()
    {
        string result = "";
        UserInfo ui = UserInfoProvider.GetUserInfo(loginElem.UserName);
        if (ui != null)
        {
            string siteName = SiteContext.CurrentSiteName;

            // Prepare return URL
            string returnUrl = RequestContext.CurrentURL;
            if (!string.IsNullOrEmpty(loginElem.UserName))
            {
                returnUrl = URLHelper.AddParameterToUrl(returnUrl, "username", loginElem.UserName);
            }

            switch (UserAccountLockCode.ToEnum(ui.UserAccountLockReason))
            {
                case UserAccountLockEnum.MaximumInvalidLogonAttemptsReached:
                    result = AuthenticationHelper.SendUnlockAccountRequest(ui, siteName, "USERLOGON", SettingsKeyInfoProvider.GetValue(siteName + ".CMSSendPasswordEmailsFrom"), null, returnUrl);
                    break;

                case UserAccountLockEnum.PasswordExpired:
                    result = AuthenticationHelper.SendPasswordRequest(ui, siteName, "USERLOGON", SettingsKeyInfoProvider.GetValue(siteName + ".CMSSendPasswordEmailsFrom"), "Membership.PasswordExpired", null, AuthenticationHelper.GetResetPasswordUrl(siteName), returnUrl);
                    break;
            }
        }

        return result;
    }


    public void RaiseCallbackEvent(string eventArgument)
    {
    }

    #endregion
}