using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.ExternalAuthentication;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.WebAnalytics;

public partial class CMSWebParts_Membership_OpenID_OpenIDLogon : CMSAbstractWebPart
{
    #region "Variables"

    private CMSOpenIDHelper openIDhelper;
    private IMembershipActivityLogger mMembershipActivityLogger;


    private IMembershipActivityLogger MembershipActivityLogger => mMembershipActivityLogger ?? (mMembershipActivityLogger = Service.Resolve<IMembershipActivityLogger>());

    #endregion


    #region "Constants"

    protected const string PROVIDERS_LOCATION = "~/CMSWebparts/Membership/OpenID/OpenID_files/";

    protected const string USERNAME_MACRO = "##username##";
    protected const string SESSION_NAME_USERDATA = "OpenIDAuthenticatedUserData";
    protected const string SESSION_NAME_URL = "OpenIDProviderURL";

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets OpenID providers used for login.
    /// </summary>
    public string Providers
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Providers"), "");
        }
        set
        {
            SetValue("Providers", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether to show sign out link.
    /// </summary>
    public bool ShowSignOut
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowSignOut"), true);
        }
        set
        {
            SetValue("ShowSignOut", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that buttons will be used instead of links.
    /// </summary>
    public bool ShowAsButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowAsButton"), false);
        }
        set
        {
            SetValue("ShowAsButton", value);
        }
    }


    /// <summary>
    /// Gets or sets sign in button image URL.
    /// </summary>
    public string SignInImageURL
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SignInImageURL"), GetImageUrl(PROVIDERS_LOCATION + "signin.gif"));
        }
        set
        {
            SetValue("SignInImageURL", value);
        }
    }


    /// <summary>
    /// Gets or sets sign out button image URL.
    /// </summary>
    public string SignOutImageURL
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SignOutImageURL"), GetImageUrl(PROVIDERS_LOCATION + "signout.gif"));
        }
        set
        {
            SetValue("SignOutImageURL", value);
        }
    }


    /// <summary>
    /// Gets or sets sign in text.
    /// </summary>
    public string SignInText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SignInText"), "");
        }
        set
        {
            SetValue("SignInText", value);
        }
    }


    /// <summary>
    /// Gets or sets sign out text.
    /// </summary>
    public string SignOutText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SignOutText"), "");
        }
        set
        {
            SetValue("SignOutText", value);
        }
    }


    /// <summary>
    /// Indicates if BirthDate should be requested in registration process.
    /// </summary>
    public string BirthDateRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("BirthDate"), "");
        }
        set
        {
            SetValue("BirthDate", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the Country of the user.
    /// </summary>
    public string CountryRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Country"), "");
        }
        set
        {
            SetValue("Country", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the email of the user.
    /// </summary>
    public string EmailRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Email"), "");
        }
        set
        {
            SetValue("Email", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the full name of the user.
    /// </summary>
    public string FullNameRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FullName"), "");
        }
        set
        {
            SetValue("FullName", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the gender of the user.
    /// </summary>
    public string GenderRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Gender"), "");
        }
        set
        {
            SetValue("Gender", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the language of the user.
    /// </summary>
    public string LanguageRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Language"), "");
        }
        set
        {
            SetValue("Language", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the nickname of the user.
    /// </summary>
    public string NicknameRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Nickname"), "");
        }
        set
        {
            SetValue("Nickname", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the postal code of the user.
    /// </summary>
    public string PostalCodeRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PostalCode"), "");
        }
        set
        {
            SetValue("PostalCode", value);
        }
    }


    /// <summary>
    /// The level of interest a relying party has in the time zone of the user.
    /// </summary>
    public string TimeZoneRequest
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TimeZone"), "");
        }
        set
        {
            SetValue("TimeZone", value);
        }
    }


    /// <summary>
    /// Gets or sets the value indicating if textbox should be displayed.
    /// </summary>
    public bool DisplayTextbox
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayTextbox"), true);
        }
        set
        {
            SetValue("DisplayTextbox", value);
        }
    }


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
    /// Gets the sender email (from).
    /// </summary>
    private string FromAddress
    {
        get
        {
            return SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSNoreplyEmailAddress");
        }
    }


    /// <summary>
    /// Gets the recipient email (to).
    /// </summary>
    private string ToAddress
    {
        get
        {
            return SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSAdminEmailAddress");
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
            Visible = false;

            return;
        }
        
        // Check if OpenID module is enabled
        if (SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".CMSEnableOpenID"))
        {
            ltlScript.Text = ScriptHelper.GetScriptTag(PROVIDERS_LOCATION + "OpenIDSelector.js");
            lblError.Text = GetString("openid.invalidid");

            SetProviders();
            DisplayButtons();

            openIDhelper = new CMSOpenIDHelper();
            CheckStatus();
        }
        else
        {
            // Error label is displayed in Design mode when OpenID is disabled
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

                lblError.Text = String.Format(GetString("mem.openid.disabled"), parameter);
                lblError.Visible = true;
                txtInput.Visible = false;
            }
            else
            {
                Visible = false;
            }
        }
    }


    /// <summary>
    /// Prepares script with user-defined providers.
    /// </summary>
    protected void SetProviders()
    {
        // Set default value for providers
        string providers = null;

        if (!String.IsNullOrEmpty(Providers))
        {
            // Split providers by rows
            string[] rows = Providers.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Create javascript variable
            providers = "var providers = [";
            int i = 1;

            foreach (string row in rows)
            {
                string rowValue = row.Trim();
                if (i != 1)
                {
                    providers += ", ";
                }
                providers += "{";

                var parts = rowValue.Split('|');
                var part0 = string.Empty;
                var part1 = string.Empty;

                // Check if providers are filled in correctly
                if (parts.Length >= 2)
                {
                    part0 = parts[0];
                    part1 = parts[1];
                }

                // Split URL to get suffix and prefix
                string url_prefix = null;
                var url_suffix = part1;

                var customusername = false;
                // Check if URL contains macro for custom username
                if (part1.ToLowerCSafe().Contains(USERNAME_MACRO.ToLowerCSafe()))
                {
                    var urlParts = part1.Split(new[] { USERNAME_MACRO }, StringSplitOptions.None);
                    url_prefix = urlParts[0];
                    url_suffix = urlParts[1];
                    customusername = true;
                }

                // long name + short name
                providers += " longname: \"" + part0 + "\", shortname: \"" + part0 + "\",";

                // url prefix
                providers += " url_prefix: \"" + url_prefix + "\",";

                // url suffix
                providers += " url_suffix: \"" + url_suffix + "\",";

                // usercalled (blog name, user name, user id) which will be displayed before username textbox
                providers += " usercalled: \"username\",";

                // website url
                providers += " website: \"" + url_suffix + "\",";

                // icon
                if ((parts.Length > 2) && !String.IsNullOrEmpty(parts[2]))
                {
                    providers += " icon: \"" + URLHelper.GetAbsoluteUrl(PROVIDERS_LOCATION + parts[2].ToLowerCSafe()) + "\",";
                }
                else
                {
                    providers += " icon: \"" + URLHelper.GetAbsoluteUrl(PROVIDERS_LOCATION + "openid.png") + "\",";
                }

                // id (used to determine cookie)
                providers += " id: " + i + ",";

                // enable username input (openid1 = FALSE means no username input)
                providers += " openid1: " + customusername.ToString().ToLowerCSafe() + ",";

                // openid2
                providers += " openid2: true";

                providers += " }";
                i++;
            }

            providers += "];";
        }

        providers += "\n";
        providers += "var iconlocation = \"" + URLHelper.GetAbsoluteUrl(GetImageUrl("CMSModules/CMS_OpenID/")) + "\"; \n";
        providers += "var providerlocation = \"" + URLHelper.GetAbsoluteUrl(PROVIDERS_LOCATION) + "\";\n";
        if (DisplayTextbox)
        {
            providers += "var idselector_input_id = \"" + txtInput.ClientID + "\";\n";
        }
        else
        {
            providers += "var idselector_input_id = \"" + hdnValue.ClientID + "\";\n";
        }
        providers += "var displaytextbox = " + DisplayTextbox.ToString().ToLowerCSafe() + ";\n";
        providers += "var otheropenid = \"" + GetString("mem.openid.other") + "\";\n";
        providers += "var clicktosignin = \"" + GetString("mem.openid.click") + "\";\n";

        ltlProvidersVariables.Text = ScriptHelper.GetScript(providers);
    }


    /// <summary>
    /// Displays buttons depending on web part settings.
    /// </summary>
    protected void DisplayButtons()
    {
        // If user is already authenticated 
        if (AuthenticationHelper.IsAuthenticated())
        {
            var openId = CacheHelper.Cache(cs =>
                {
                    var id = OpenIDUserInfoProvider.GetOpenIDByUserID(MembershipContext.AuthenticatedUser.UserID);

                    if (cs.Cached)
                    {
                        var dependencyKey = UserSettingsInfo.OBJECT_TYPE + "|byid|" + MembershipContext.AuthenticatedUser.UserSettings.UserSettingsID;
                        cs.CacheDependency = CacheHelper.GetCacheDependency(dependencyKey);
                    }

                    return id;
                },
                new CacheSettings(CacheMinutes, UserSettingsInfo.OBJECT_TYPE, "openid", MembershipContext.AuthenticatedUser.UserSettings.UserSettingsID)
            );

            var isOpenIdUser = !String.IsNullOrEmpty(openId);

            // Signout is visible when ShowSignOut set to true and user has OpenID registered (this is ignored for design mode)
            if ((ShowSignOut && isOpenIdUser) || PortalContext.IsDesignMode(ViewMode))
            {
                // If text is set use text/button link
                if (!string.IsNullOrEmpty(SignOutText))
                {
                    // Button link
                    if (ShowAsButton)
                    {
                        pnlLogon.DefaultButton = btnSignOut.ID;
                        btnSignOut.Text = SignOutText;
                        btnSignOut.Visible = true;
                    }
                    // Text link
                    else
                    {
                        pnlLogon.DefaultButton = btnSignOutLink.ID;
                        btnSignOutLink.Text = SignOutText;
                        btnSignOutLink.Visible = true;
                    }
                }
                // Image link
                else
                {
                    pnlLogon.DefaultButton = btnSignOutImage.ID;
                    btnSignOutImage.ImageUrl = ResolveUrl(SignOutImageURL);
                    btnSignOutImage.Visible = true;
                    btnSignOutImage.ToolTip = GetString("webparts_membership_signoutbutton.signout");
                    btnSignOut.Text = GetString("webparts_membership_signoutbutton.signout");
                }
            }
            else
            {
                Visible = false;
            }

            txtInput.Visible = false;
            ltlScript.Visible = false;
            ltlProvidersVariables.Visible = false;
        }
        else
        {
            // If text is set use text/button link
            if (!string.IsNullOrEmpty(SignInText))
            {
                // Button link
                if (ShowAsButton)
                {
                    pnlLogon.DefaultButton = btnSignIn.ID;
                    btnSignIn.Text = SignInText;
                    btnSignIn.Visible = true;
                }
                // Text link
                else
                {
                    pnlLogon.DefaultButton = btnSignInLink.ID;
                    btnSignInLink.Text = SignInText;
                    btnSignInLink.Visible = true;
                }
            }
            // Image link
            else
            {
                pnlLogon.DefaultButton = btnSignInImage.ID;
                btnSignInImage.ImageUrl = ResolveUrl(SignInImageURL);
                btnSignInImage.Visible = true;
                btnSignInImage.ToolTip = GetString("webparts_membership_signoutbutton.signin");
                btnSignIn.Text = GetString("webparts_membership_signoutbutton.signin");
            }

            txtInput.Visible = true;
            ltlScript.Visible = true;
            ltlProvidersVariables.Visible = true;
        }

        // Hide textbox when applicable
        if (!DisplayTextbox)
        {
            txtInput.Visible = false;
        }
    }


    /// <summary>
    /// Checks status of current user.
    /// </summary>
    protected void CheckStatus()
    {
        // Get current site name
        string siteName = SiteContext.CurrentSiteName;
        string error = null;

        // Check return URL
        string returnUrl = QueryHelper.GetString("returnurl", null);
        returnUrl = HttpUtility.UrlDecode(returnUrl);

        // Get current URL
        string currentUrl = RequestContext.CurrentURL;
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "token");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.ns");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.mode");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.return_to");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.claimed_id");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.identity");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.assoc_handle");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.realm");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.response_nonce");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.signed");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.op_endpoint");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.pape.auth_level.nist");
        currentUrl = URLHelper.RemoveParameterFromUrl(currentUrl, "openid.sig");

        // Get OpenID response status
        switch (openIDhelper.CheckStatus())
        {
            // User is authenticated
            case CMSOpenIDHelper.RESPONSE_AUTHENTICATED:
                // Claimed ID not found  = save new user
                if (OpenIDUserInfoProvider.GetUserInfoByOpenID(openIDhelper.ClaimedIdentifier) == null)
                {
                    // Check whether additional user info page is set
                    string additionalInfoPage = SettingsKeyInfoProvider.GetValue(siteName + ".CMSRequiredOpenIDPage").Trim();

                    // No page set, user can be created
                    if (String.IsNullOrEmpty(additionalInfoPage))
                    {
                        // Register new user
                        UserInfo ui = AuthenticationHelper.AuthenticateOpenIDUser(openIDhelper.ClaimedIdentifier, ValidationHelper.GetString(SessionHelper.GetValue(SESSION_NAME_URL), null), siteName, false, true, ref error);

                        // If user was found or successfuly created
                        if (ui != null)
                        {
                            // Load values submited by OpenID provider
                            // Load date of birth
                            if (openIDhelper.BirthDate != DateTime.MinValue)
                            {
                                ui.UserSettings.UserDateOfBirth = openIDhelper.BirthDate;
                            }
                            // Load default country
                            if (openIDhelper.Culture != null)
                            {
                                ui.PreferredCultureCode = openIDhelper.Culture.Name;
                            }
                            // Load e-mail
                            if (!String.IsNullOrEmpty(openIDhelper.Email))
                            {
                                ui.Email = openIDhelper.Email;
                            }
                            // Nick name
                            if (!String.IsNullOrEmpty(openIDhelper.Nickname))
                            {
                                ui.UserSettings.UserNickName = openIDhelper.Nickname;
                            }
                            // User gender
                            if (openIDhelper.UserGender != null)
                            {
                                ui.UserSettings.UserGender = (int)openIDhelper.UserGender;
                            }

                            UserInfoProvider.SetUserInfo(ui);

                            // If user is enabled
                            if (ui.Enabled)
                            {
                                // Create autentification cookie
                                AuthenticationHelper.SetAuthCookieWithUserData(ui.UserName, true, Session.Timeout, new[] { "openidlogin" });
                                // Log activity
                                MembershipActivityLogger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);
                            }

                            // Notify administrator
                            if (NotifyAdministrator && !String.IsNullOrEmpty(FromAddress) && !String.IsNullOrEmpty(ToAddress))
                            {
                                AuthenticationHelper.NotifyAdministrator(ui, FromAddress, ToAddress);
                            }

                            // Log user registration into the web analytics and track conversion if set
                            AnalyticsHelper.TrackUserRegistration(siteName, ui, TrackConversionName, ConversionValue);

                            MembershipActivityLogger.LogRegistration(ui.UserName, DocumentContext.CurrentDocument);
                        }

                        // Redirect when authentication was succesfull
                        if (String.IsNullOrEmpty(error))
                        {
                            if (URLHelper.IsLocalUrl(returnUrl))
                            {
                                URLHelper.Redirect(returnUrl);
                            }
                            else
                            {
                                URLHelper.Redirect(currentUrl);
                            }
                        }
                        // Display error otherwise
                        else
                        {
                            lblError.Text = error;
                            lblError.Visible = true;
                        }
                    }
                    // Additional information page is set
                    else
                    {
                        // Store user object in session for additional use
                        StoreResponseInSession();

                        // Redirect to additional info page
                        string targetURL = URLHelper.GetAbsoluteUrl(additionalInfoPage);

                        if (URLHelper.IsLocalUrl(returnUrl))
                        {
                            // Add return URL to parameter
                            targetURL = URLHelper.AddParameterToUrl(targetURL, "returnurl", HttpUtility.UrlEncode(returnUrl));
                        }

                        URLHelper.Redirect(UrlResolver.ResolveUrl(targetURL));
                    }
                }
                // Claimed OpenID is in DB
                else
                {
                    // Login existing user
                    UserInfo ui = AuthenticationHelper.AuthenticateOpenIDUser(openIDhelper.ClaimedIdentifier, ValidationHelper.GetString(SessionHelper.GetValue(SESSION_NAME_URL), null), siteName, false, true, ref error);

                    if ((ui != null) && (ui.Enabled))
                    {
                        // Create autentification cookie
                        AuthenticationHelper.SetAuthCookieWithUserData(ui.UserName, true, Session.Timeout, new[] { "openilogin" });

                        // Log activity
                        MembershipActivityLogger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);

                        // Redirect user
                        if (URLHelper.IsLocalUrl(returnUrl))
                        {
                            URLHelper.Redirect(UrlResolver.ResolveUrl(URLHelper.GetAbsoluteUrl(returnUrl)));
                        }
                        else
                        {
                            URLHelper.Redirect(currentUrl);
                        }
                    }
                    // Display error which occured during authentication process
                    else if (!String.IsNullOrEmpty(error))
                    {
                        lblError.Text = error;
                        lblError.Visible = true;
                    }
                    // Otherwise is user disabled
                    else
                    {
                        lblError.Text = GetString("membership.userdisabled");
                        lblError.Visible = true;
                    }
                }
                break;

            // Authentication was canceled
            case CMSOpenIDHelper.RESPONSE_CANCELED:
                lblError.Text = GetString("openid.logincanceled");
                lblError.Visible = true;
                break;

            // Authentication failed
            case CMSOpenIDHelper.RESPONSE_FAILED:
                lblError.Text = GetString("openid.loginfailed");
                lblError.Visible = true;
                break;
        }
    }


    /// <summary>
    /// Stores data from OpenID response in session.
    /// </summary>
    private void StoreResponseInSession()
    {
        Dictionary<string, object> response = new Dictionary<string, object>();

        response["BirthDate"] = openIDhelper.BirthDate;
        response["ClaimedIdentifier"] = openIDhelper.ClaimedIdentifier;
        response["Country"] = openIDhelper.Country;
        response["Culture"] = openIDhelper.Culture;
        response["Email"] = openIDhelper.Email;
        response["FullName"] = openIDhelper.FullName;
        response["Language"] = openIDhelper.Language;
        response["Nickname"] = openIDhelper.Nickname;
        response["PostalCode"] = openIDhelper.PostalCode;
        response["TimeZone"] = openIDhelper.TimeZone;
        response["UserGender"] = openIDhelper.UserGender;

        SessionHelper.SetValue(SESSION_NAME_USERDATA, response);
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Sign In clicked event handler.
    /// </summary>
    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        openIDhelper = new CMSOpenIDHelper();

        string openidURL = DisplayTextbox ? txtInput.Text.Trim() : hdnValue.Value;

        // Check if validation was successful
        if (CMSOpenIDHelper.IsValid(openidURL))
        {
            // Store ProviderURL for later use
            SessionHelper.SetValue(SESSION_NAME_URL, openidURL);

            // Send request
            string response = openIDhelper.SendRequest(openidURL, BirthDateRequest, CountryRequest, EmailRequest, FullNameRequest, GenderRequest, LanguageRequest, NicknameRequest, PostalCodeRequest, TimeZoneRequest);

            if (!String.IsNullOrEmpty(response))
            {
                lblError.Visible = true;
                lblError.Text = response;
            }
        }
        else
        {
            if (DisplayTextbox)
            {
                lblError.Text = GetString("mem.openid.enterproviderurl");
            }
            else
            {
                lblError.Text = GetString("mem.openid.selectprovider");
            }
            lblError.Visible = true;
        }
    }


    /// <summary>
    /// Sign out button clicked.
    /// </summary>
    protected void btnSignOut_Click(object sender, EventArgs e)
    {
        if (AuthenticationHelper.IsAuthenticated())
        {
            // Sign out from CMS
            AuthenticationHelper.SignOut();
            Response.Cache.SetNoStore();

            // Clear used session
            SessionHelper.Remove(SESSION_NAME_URL);
            SessionHelper.Remove(SESSION_NAME_USERDATA);

            // Redirect to return URL
            string returnUrl = QueryHelper.GetString("returnurl", null);

            if (!URLHelper.IsLocalUrl(returnUrl))
            {
                returnUrl = RequestContext.CurrentURL;
            }

            URLHelper.Redirect(UrlResolver.ResolveUrl(URLHelper.GetAbsoluteUrl(HttpUtility.UrlDecode(returnUrl))));
        }
    }

    #endregion
}