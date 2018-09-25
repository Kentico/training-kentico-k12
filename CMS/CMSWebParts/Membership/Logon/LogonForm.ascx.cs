using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Core;
using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.MembershipProvider;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.SiteProvider;

public partial class CMSWebParts_Membership_Logon_LogonForm : CMSAbstractWebPart, ICallbackEventHandler
{
    #region "Variables"

    private string mDefaultTargetUrl = "";

    #endregion


    #region "Private properties"

    /// <summary>
    /// Gets error displayed by login control
    /// </summary>
    private string DisplayedError
    {
        get
        {
            var failureLit = Login1.FindControl("FailureText") as LocalizedLabel;
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
    /// Gets or sets the value that indicates whether retrieving of forgotten password is enabled.
    /// </summary>
    public bool AllowPasswordRetrieval
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowPasswordRetrieval"), true);
        }
        set
        {
            SetValue("AllowPasswordRetrieval", value);
            lnkPasswdRetrieval.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the sender e-mail (from).
    /// </summary>
    public string SendEmailFrom
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SendEmailFrom"), SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSSendPasswordEmailsFrom"));
        }
        set
        {
            SetValue("SendEmailFrom", value);
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
    /// Gets or sets the SkinID of the logon form.
    /// </summary>
    public override string SkinID
    {
        get
        {
            return base.SkinID;
        }
        set
        {
            base.SkinID = value;
            SetSkinID(value);
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
            if (value.Trim() != "")
            {
                SetValue("FailureText", value);
            }
        }
    }


    /// <summary>
    /// Gets or sets reset password url - this url is sent to user in e-mail when he wants to reset his password.
    /// </summary>
    public string ResetPasswordURL
    {
        get
        {
            string url = ValidationHelper.GetString(GetValue("ResetPasswordURL"), string.Empty);
            return DataHelper.GetNotEmpty(URLHelper.GetAbsoluteUrl(url), AuthenticationHelper.GetResetPasswordUrl(SiteContext.CurrentSiteName));
        }
        set
        {
            SetValue("ResetPasswordURL", value);
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
            // Do not process
        }
        else
        {
            SetValidationGroup(btnPasswdRetrieval, "_PasswordRetrieval");
            SetValidationGroup(Login1.FindControl("LoginButton"), "_Logon");

            var rfv = Login1.FindControl("rfvUserNameRequired") as RequiredFieldValidator;
            if (rfv != null)
            {
                SetValidationGroup(rfv, "_Logon");

                if (string.IsNullOrEmpty(rfv.ToolTip))
                {
                    rfv.ToolTip = GetString("LogonForm.NameRequired");
                }

                var enterNameText = GetString("LogonForm.EnterName");
                if (string.IsNullOrEmpty(rfv.Text))
                {
                    rfv.Text = enterNameText;
                }
                if (string.IsNullOrEmpty(rfv.ErrorMessage))
                {
                    rfv.ErrorMessage = enterNameText;
                }
            }

            CMSCheckBox chkItem = (CMSCheckBox)Login1.FindControl("chkRememberMe");
            if ((MFAuthenticationHelper.IsMultiFactorAuthEnabled) && (chkItem != null))
            {
                chkItem.Visible = false;
            }

            lnkPasswdRetrieval.Visible = pnlUpdatePasswordRetrieval.Visible = pnlUpdatePasswordRetrievalLink.Visible = AllowPasswordRetrieval;

            CMSTextBox txtUserName = (CMSTextBox)Login1.FindControl("UserName");
            if (txtUserName != null)
            {
                txtUserName.EnableAutoComplete = SecurityHelper.IsAutoCompleteEnabledForLogin(SiteContext.CurrentSiteName);
            }

            if (!RequestHelper.IsPostBack())
            {
                Login1.UserName = QueryHelper.GetString("username", string.Empty);
                // Set SkinID properties
                if (!StandAlone && (PageCycle < PageCycleEnum.Initialized) && (ValidationHelper.GetString(Page.StyleSheetTheme, "") == ""))
                {
                    SetSkinID(SkinID);
                }
            }

            // Register script to update logon error message
            LocalizedLabel failureLit = Login1.FindControl("FailureText") as LocalizedLabel;
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


    void Login1_LoginError(object sender, EventArgs e)
    {
        bool showError = true;

        // Ban IP addresses which are blocked for login
        if (MembershipContext.UserIsBanned)
        {
            DisplayError(GetString("banip.ipisbannedlogin"));
        }
        else if (AuthenticationHelper.DisplayAccountLockInformation(SiteContext.CurrentSiteName) && MembershipContext.UserAccountLockedDueToInvalidLogonAttempts)
        {
            DisplayAccountLockedError(GetString("invalidlogonattempts.unlockaccount.accountlocked"));
        }
        else if (AuthenticationHelper.DisplayAccountLockInformation(SiteContext.CurrentSiteName) && MembershipContext.UserAccountLockedDueToPasswordExpiration)
        {
            DisplayAccountLockedError(GetString("passwordexpiration.accountlocked"));
        }
        else if (MembershipContext.UserIsPartiallyAuthenticated && !MembershipContext.UserAuthenticationFailedDueToInvalidPasscode)
        {
            if (MembershipContext.MFAuthenticationTokenNotInitialized && MFAuthenticationHelper.DisplaySetupCode)
            {
                var lblTokenInfo = Login1.FindControl("lblTokenInfo") as LocalizedLabel;
                var lblTokenID = Login1.FindControl("lblTokenID") as LocalizedLabel;
                var plcTokenInfo = Login1.FindControl("plcTokenInfo");

                if (lblTokenInfo != null)
                {
                    lblTokenInfo.Text = string.Format("{0} {1}", GetString("mfauthentication.isRequired"), GetString("mfauthentication.token.get"));
                    lblTokenInfo.Visible = true;
                }

                if (lblTokenID != null)
                {
                    lblTokenID.Text = MFAuthenticationHelper.GetSetupCodeForUser(Login1.UserName);
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
            var plcPasscodeBox = Login1.FindControl("plcPasscodeBox");
            var plcLoginInputs = Login1.FindControl("plcLoginInputs");
            var plcTokenInfo = Login1.FindControl("plcTokenInfo");
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
    }


    /// <summary>
    /// OnLoad override (show hide password retrieval).
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        Login1.LoggedIn += Login1_LoggedIn;
        Login1.LoggingIn += Login1_LoggingIn;
        Login1.LoginError += Login1_LoginError;
        Login1.Authenticate += Login1_Authenticate;

        btnPasswdRetrieval.Click += btnPasswdRetrieval_Click;
    }


    /// <summary>
    /// Displays locked account error message.
    /// </summary>
    /// <param name="specificMessage">Specific part of the message.</param>
    private void DisplayAccountLockedError(string specificMessage)
    {
        var failureLabel = Login1.FindControl("FailureText") as LocalizedLabel;
        if (failureLabel != null)
        {
            string link = "<a href=\"#\" onclick=\"" + Page.ClientScript.GetCallbackEventReference(this, "null", "UpdateLabel_" + ClientID, "'" + failureLabel.ClientID + "'") + ";\">" + GetString("general.clickhere") + "</a>";
            DisplayError(string.Format(specificMessage + " " + GetString("invalidlogonattempts.unlockaccount.accountlockedlink"), link));
        }
    }


    private void SetValidationGroup(dynamic control, string postfix)
    {
        if (control != null)
        {
            try
            {
                control.ValidationGroup = ClientID + postfix;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
            {
                EventLogProvider.LogException("LogonForm", "EXCEPTION", ex);
            }
        }
    }


    /// <summary>
    /// Sets SkinId to all controls in logon form.
    /// </summary>
    private void SetSkinID(string skinId)
    {
        if (skinId != "")
        {
            Login1.SkinID = skinId;

            var controlNames = new string[]
            {
                "lblUserName",
                "lblPassword",
                "UserName",
                "Password",
                "chkRememberMe",
                "LoginButton"
            };

            foreach (string controlName in controlNames)
            {
                var control = Login1.FindControl(controlName);
                if (control != null)
                {
                    control.SkinID = skinId;
                }
            }
        }
    }


    /// <summary>
    /// Applies given stylesheet skin.
    /// </summary>
    public override void ApplyStyleSheetSkin(Page page)
    {
        SetSkinID(SkinID);

        base.ApplyStyleSheetSkin(page);
    }


    /// <summary>
    /// Displays error.
    /// </summary>
    /// <param name="msg">Message.</param>
    private void DisplayError(string msg)
    {
        var failureLit = Login1.FindControl("FailureText") as LocalizedLabel;

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
        DisplayError(string.Empty);
    }


    /// <summary>
    /// Retrieve the user password.
    /// </summary>
    private void btnPasswdRetrieval_Click(object sender, EventArgs e)
    {
        string value = txtPasswordRetrieval.Text.Trim();

        if (!String.IsNullOrEmpty(value) && ValidationHelper.IsEmail(value))
        {
            // Prepare return URL
            string returnUrl = RequestContext.CurrentURL;
            string userName = Login1.UserName;
            if (!string.IsNullOrEmpty(userName))
            {
                returnUrl = URLHelper.AddParameterToUrl(returnUrl, "username", userName);
            }

            AuthenticationHelper.ForgottenEmailRequest(value, SiteContext.CurrentSiteName, "LOGONFORM", SendEmailFrom, null, ResetPasswordURL, returnUrl);

            lblResult.Text = String.Format(GetString("LogonForm.EmailSent"), value);
            lblResult.Visible = true;

            pnlPasswdRetrieval.Visible = true;
        }
        else
        {
            lblResult.Text = String.Format(GetString("LogonForm.EmailNotValid"), value);
            lblResult.Visible = true;

            pnlPasswdRetrieval.Visible = true;
        }
    }


    /// <summary>
    /// Logged in handler.
    /// </summary>
    private void Login1_LoggedIn(object sender, EventArgs e)
    {
        // Set view mode to live site after login to prevent bar with "Close preview mode"
        PortalContext.ViewMode = ViewModeEnum.LiveSite;

        // Ensure response cookie
        CookieHelper.EnsureResponseCookie(FormsAuthentication.FormsCookieName);

        // Set cookie expiration
        if (Login1.RememberMeSet)
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
        string userName = Login1.UserName;

        // Get user name (test site prefix too)
        UserInfo ui = UserInfoProvider.GetUserInfoForSitePrefix(userName, SiteContext.CurrentSite);

        // Check whether safe user name is required and if so get safe username
        if (AuthenticationMode.IsMixedAuthentication() && UserInfoProvider.UseSafeUserName)
        {
            // Get info on the authenticated user            
            if (ui == null)
            {
                // User stored with safe name
                userName = ValidationHelper.GetSafeUserName(Login1.UserName, SiteContext.CurrentSiteName);

                // Find user by safe name
                ui = UserInfoProvider.GetUserInfoForSitePrefix(userName, SiteContext.CurrentSite);
                if (ui != null)
                {
                    // Authenticate user by site or global safe username
                    AuthenticationHelper.AuthenticateUser(ui.UserName, Login1.RememberMeSet);
                }
            }
        }

        if (ui != null)
        {
            // If user name is site prefixed, authenticate user manually 
            if (UserInfoProvider.IsSitePrefixedUser(ui.UserName))
            {
                AuthenticationHelper.AuthenticateUser(ui.UserName, Login1.RememberMeSet);
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
    /// Handling login authenticate event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Authenticate event arguments.</param>
    private void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (MFAuthenticationHelper.IsMultiFactorRequiredForUser(Login1.UserName))
        {
            var plcPasscodeBox = Login1.FindControl("plcPasscodeBox");
            var plcLoginInputs = Login1.FindControl("plcLoginInputs");
            var txtPasscode = Login1.FindControl("txtPasscode") as CMSTextBox;

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
            txtPasscode.Text = string.Empty;

            var provider = new CMSMembershipProvider();

            // Validate username and password
            if (plcLoginInputs.Visible)
            {
                if (provider.MFValidateCredentials(Login1.UserName, Login1.Password))
                {
                    // Show passcode screen
                    plcLoginInputs.Visible = false;
                    plcPasscodeBox.Visible = true;
                }
            }
            // Validate passcode
            else
            {
                if (provider.MFValidatePasscode(Login1.UserName, passcode))
                {
                    e.Authenticated = true;
                }
            }
        }
        else
        {
            try
            {
                e.Authenticated = Membership.Provider.ValidateUser(Login1.UserName, Login1.Password);
            }
            catch (ConfigurationException ex)
            {
                EventLogProvider.LogException("LogonForm", "VALIDATEUSER", ex);
                var provider = new CMSMembershipProvider();
                e.Authenticated = provider.ValidateUser(Login1.UserName, Login1.Password);
            }
        }
    }


    /// <summary>
    /// Logging in handler.
    /// </summary>
    private void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        if (((CMSCheckBox)Login1.FindControl("chkRememberMe")).Checked)
        {
            Login1.RememberMeSet = true;
        }
        else
        {
            Login1.RememberMeSet = false;
        }
    }


    /// <summary>
    /// Forgotten password retrieval toggle link click event.
    /// </summary>
    protected void lnkPasswdRetrieval_Click(object sender, EventArgs e)
    {
        pnlPasswdRetrieval.Visible = !pnlPasswdRetrieval.Visible;
    }


    ///<summary>
    /// Overrides the generation of the SPAN tag with custom tag.
    ///</summary>
    protected HtmlTextWriterTag TagKey
    {
        get
        {
            if (SiteContext.CurrentSite != null)
            {
                if (SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSControlElement").ToLowerCSafe().Trim() == "div")
                {
                    return HtmlTextWriterTag.Div;
                }
                else
                {
                    return HtmlTextWriterTag.Span;
                }
            }
            return HtmlTextWriterTag.Span;
        }
    }


    #region "ICallbackEventHandler Members"

    public string GetCallbackResult()
    {
        string result = "";
        UserInfo ui = UserInfoProvider.GetUserInfo(Login1.UserName);
        if (ui != null)
        {
            string siteName = SiteContext.CurrentSiteName;

            // Prepare return URL
            string returnUrl = RequestContext.CurrentURL;
            if (!string.IsNullOrEmpty(Login1.UserName))
            {
                returnUrl = URLHelper.AddParameterToUrl(returnUrl, "username", Login1.UserName);
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
