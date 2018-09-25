using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using CMS.Base;
using CMS.Activities.Loggers;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.Modules;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.Protection;
using CMS.SiteProvider;
using CMS.WebAnalytics;

public partial class CMSWebParts_Membership_OpenID_OpenIDUserRequiredData : CMSAbstractWebPart
{
    #region "Constants"

    protected const string SESSION_NAME_USERDATA = "OpenIDAuthenticatedUserData";
    protected const string SESSION_NAME_URL = "OpenIDProviderURL";

    #endregion


    #region "Variables"

    private string mDefaultTargetUrl = null;
    private string userProviderUrl = null;
    Dictionary<string, object> response;
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
    /// Gets or sets the value which enables abitity join OpenID with existing account.
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
    /// Gets or sets the value which determines the behaviour if no OpenID user stored in SESSION.
    /// </summary>
    public bool HideForNoOpenID
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideForNoOpenID"), true);
        }
        set
        {
            SetValue("HideForNoOpenID", value);
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
        if (!StopProcessing)
        {
            plcError.Visible = false;

            // Check if OpenID module is enabled
            if (!SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".CMSEnableOpenID") && !plcError.Visible)
            {
                // Error label is displayed only in Design mode
                if (PortalContext.IsDesignMode(PortalContext.ViewMode))
                {
                    StringBuilder parameter = new StringBuilder();
                    parameter.Append(UIElementInfoProvider.GetApplicationNavigationString("cms", "Settings") + " -> ");
                    parameter.Append(GetString("settingscategory.cmsmembership") + " -> ");
                    parameter.Append(GetString("settingscategory.cmsmembershipauthentication") + " -> ");
                    parameter.Append(GetString("settingscategory.cmsopenid"));
                    if (MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
                    {
                        // Make it link for Admin
                        parameter.Insert(0, "<a href=\"" + URLHelper.GetAbsoluteUrl(ApplicationUrlHelper.GetApplicationUrl("cms", "settings")) + "\" target=\"_top\">");
                        parameter.Append("</a>");
                    }

                    lblError.Text = String.Format(GetString("mem.openid.disabled"), parameter.ToString());
                    plcError.Visible = true;
                    plcContent.Visible = false;
                }
                // In other modes is webpart hidden
                else
                {
                    Visible = false;
                }
            }

            // Display webpart when no error occured
            if (!plcError.Visible && Visible)
            {
                if (!AuthenticationHelper.IsAuthenticated())
                {
                    plcPasswordNew.Visible = AllowFormsAuthentication;
                    pnlExistingUser.Visible = AllowExistingUser;

                    // Initialize OpenID session
                    response = (Dictionary<string, object>)SessionHelper.GetValue(SESSION_NAME_USERDATA);

                    userProviderUrl = ValidationHelper.GetString(SessionHelper.GetValue(SESSION_NAME_URL), null);

                    // Check that OpenID is not already registered
                    if (response != null)
                    {
                        UserInfo ui = OpenIDUserInfoProvider.GetUserInfoByOpenID((string)response["ClaimedIdentifier"]);

                        // OpenID is already registered to some user
                        if (ui != null)
                        {
                            plcContent.Visible = false;
                            plcError.Visible = true;
                            lblError.Text = GetString("mem.openid.openidregistered");
                        }
                    }

                    // There is no OpenID response object stored in session - hide all
                    if (response == null)
                    {
                        if (HideForNoOpenID)
                        {
                            Visible = false;
                        }
                    }
                    else if (!RequestHelper.IsPostBack())
                    {
                        LoadData();
                    }
                }
                // Hide webpart for authenticated users
                else
                {
                    Visible = false;
                }
            }
        }
        // Hide control when StopProcessing = TRUE
        else
        {
            Visible = false;
        }
    }


    /// <summary>
    /// Loads textboxes with provider-supplied data.
    /// </summary>
    private void LoadData()
    {
        string nick = (string)response["Nickname"];
        string email = (string)response["Email"];
        if (!String.IsNullOrEmpty(nick))
        {
            txtUserNameNew.Text = txtUserName.Text = nick;
        }
        if (!String.IsNullOrEmpty(email))
        {
            txtEmail.Text = email;
        }
    }


    /// <summary>
    /// Handles btnOkExist click, joins existing user with OpenID.
    /// </summary>
    protected void btnOkExist_Click(object sender, EventArgs e)
    {
        // OpenID response object must be retrieved from session
        if (response != null)
        {
            if (txtUserName.Text != String.Empty)
            {
                // Try to authenticate user
                UserInfo ui = AuthenticationHelper.AuthenticateUser(txtUserName.Text, txtPassword.Text, SiteContext.CurrentSiteName);

                // Check banned IPs
                BannedIPInfoProvider.CheckIPandRedirect(SiteContext.CurrentSiteName, BanControlEnum.Login);

                if (ui != null)
                {
                    // Check if user is not already registered with different OpenID provider
                    string openID = OpenIDUserInfoProvider.GetOpenIDByUserID(ui.UserID);
                    if (String.IsNullOrEmpty(openID))
                    {
                        // Add OpenID token to user
                        OpenIDUserInfoProvider.AddOpenIDToUser((string)response["ClaimedIdentifier"], userProviderUrl, ui.UserID);

                        // Remove user info from session
                        SessionHelper.Remove(SESSION_NAME_USERDATA);
                        SessionHelper.Remove(SESSION_NAME_URL);

                        // Set authentication cookie and redirect to page
                        SetAuthCookieAndRedirect(ui);
                    }
                    // User is already registered under different OpenID provider
                    else
                    {
                        lblError.Text = GetString("mem.openid.alreadyregistered");
                        plcError.Visible = true;
                    }
                }
                else // Invalid credentials
                {
                    lblError.Text = GetString("Login_FailureText");
                    plcError.Visible = true;
                }
            }
            else // User did not fill the form
            {
                lblError.Text = GetString("mem.openid.fillloginform");
                plcError.Visible = true;
            }
        }
    }


    /// <summary>
    /// Handles btnOkNew click, creates new user and joins it with openID token.
    /// </summary>
    protected void btnOkNew_Click(object sender, EventArgs e)
    {
        if (response != null)
        {
            // Validate entered values
            string errorMessage = new Validator().IsRegularExp(txtUserNameNew.Text, "^([a-zA-Z0-9_\\-\\.@]+)$", GetString("mem.openid.fillcorrectusername"))
                .IsEmail(txtEmail.Text, GetString("mem.openid.fillvalidemail")).Result;
            string siteName = SiteContext.CurrentSiteName;
            string password = passStrength.Text;

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

            // Check whether email is unique if it is required
            if (string.IsNullOrEmpty(errorMessage) && !UserInfoProvider.IsEmailUnique(txtEmail.Text.Trim(), siteName, 0))
            {
                errorMessage = GetString("UserInfo.EmailAlreadyExist");
            }

            // Check reserved names
            if (string.IsNullOrEmpty(errorMessage) && UserInfoProvider.NameIsReserved(siteName, txtUserNameNew.Text.Trim()))
            {
                errorMessage = GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(txtUserNameNew.Text.Trim()));
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                // Check if user with given username already exists
                UserInfo ui = UserInfoProvider.GetUserInfo(txtUserNameNew.Text.Trim());

                // User with given username is already registered
                if (ui != null)
                {
                    plcError.Visible = true;
                    lblError.Text = GetString("mem.openid.usernameregistered");
                }
                else
                {
                    string error = DisplayMessage;
                    // Register new user
                    ui = AuthenticationHelper.AuthenticateOpenIDUser((string)response["ClaimedIdentifier"], ValidationHelper.GetString(SessionHelper.GetValue(SESSION_NAME_URL), null), siteName, true, false, ref error);
                    DisplayMessage = error;

                    // If user successfully created
                    if (ui != null)
                    {
                        // Set additional information
                        ui.UserName = ui.UserNickName = ui.FullName = txtUserNameNew.Text.Trim();
                        ui.Email = txtEmail.Text;

                        // Load values submitted by OpenID provider
                        // Load date of birth
                        DateTime birthdate = (DateTime)response["BirthDate"];
                        if (birthdate != DateTime.MinValue)
                        {
                            ui.UserSettings.UserDateOfBirth = birthdate;
                        }
                        // Load default country
                        var culture = (System.Globalization.CultureInfo)response["Culture"];
                        if (culture != null)
                        {
                            ui.PreferredCultureCode = culture.Name;
                        }
                        // Nick name
                        string nick = (string)response["Nickname"];
                        if (!String.IsNullOrEmpty(nick))
                        {
                            ui.UserSettings.UserNickName = nick;
                        }
                        // Full name
                        string full = (string)response["FullName"];
                        if (!String.IsNullOrEmpty(full))
                        {
                            ui.FullName = full;
                        }
                        // User gender
                        var gender = (int?)response["UserGender"];
                        if (gender != null)
                        {
                            ui.UserSettings.UserGender = (int)gender;
                        }
                        // Set password
                        if (plcPasswordNew.Visible)
                        {
                            UserInfoProvider.SetPassword(ui, password);

                            // If user can choose password then is not considered external(external user can't login in common way)
                            ui.IsExternal = false;
                        }

                        // Set user
                        UserInfoProvider.SetUserInfo(ui);

                        // Clear used session
                        SessionHelper.Remove(SESSION_NAME_URL);
                        SessionHelper.Remove(SESSION_NAME_USERDATA);

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

                        if (!String.IsNullOrEmpty(DisplayMessage))
                        {
                            lblInfo.Visible = true;
                            lblInfo.Text = DisplayMessage;
                            plcForm.Visible = false;
                        }
                        else
                        {
                            URLHelper.Redirect("~/Default.aspx");
                        }
                    }
                }
            }
            // Validation failed - display error message
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
            AuthenticationHelper.SetAuthCookieWithUserData(ui.UserName, true, Session.Timeout, new string[] { "openidlogin" });

            // Log activity
            MembershipActivityLogger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);

            string returnUrl = QueryHelper.GetString("returnurl", null);

            // Redirect to ReturnURL
            if (URLHelper.IsLocalUrl(returnUrl))
            {
                URLHelper.Redirect(HttpUtility.UrlDecode(returnUrl));
            }
            // Redirect to default page
            else if (!String.IsNullOrEmpty(DefaultTargetUrl))
            {
                URLHelper.Redirect(ResolveUrl(DefaultTargetUrl));
            }
            // Otherwise refresh current page
            else
            {
                URLHelper.Redirect(RequestContext.CurrentURL);
            }
        }
    }

    #endregion
}