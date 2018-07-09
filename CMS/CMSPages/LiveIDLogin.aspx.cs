using System;
using System.Web;
using System.Text.RegularExpressions;

using CMS.Core;
using CMS.Activities.Loggers;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.ExternalAuthentication;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;

/// <summary>
/// This page handles the login, logout and clearcookie Web Auth
/// actions.  When you create a Windows Live application, you must
/// specify the URL of this handler page.
/// </summary>
public partial class CMSPages_LiveIDLogin : CMSPage
{
    #region "Private fields"

    private readonly string defaultPage = UrlResolver.ResolveUrl("~/Default.aspx");
    private readonly string loginPage = AuthenticationHelper.GetSecuredAreasLogonPage(SiteContext.CurrentSiteName);

    private String siteName = String.Empty;
    private String relativeURL = String.Empty;
    private String conversionName = String.Empty;
    private String conversionValue = String.Empty;
    private IMembershipActivityLogger mMembershipActivityLogger;


    private IMembershipActivityLogger MembershipActivityLogger => mMembershipActivityLogger ?? (mMembershipActivityLogger = Service.Resolve<IMembershipActivityLogger>());

    #endregion


    #region "Methods"

    /// <summary>
    /// Parse parameters from session
    /// </summary>
    /// <param name="parameters">LiveID login parameters</param>
    private void ParseParameters(String[] parameters)
    {
        if ((parameters != null) && (parameters.Length == 3))
        {
            relativeURL = HttpUtility.UrlDecode(parameters[0]);
            conversionName = HttpUtility.UrlDecode(parameters[1]);
            conversionValue = HttpUtility.UrlDecode(parameters[2]);
        }
    }


    /// <summary>
    /// Get user information and logs user (register if no user found)
    /// </summary>
    private void ProcessLiveIDLogin()
    {
        // Get authorization code from URL        
        String code = QueryHelper.GetString("code", String.Empty);

        // Additional info page for login
        string additionalInfoPage = SettingsKeyInfoProvider.GetValue(siteName + ".CMSLiveIDRequiredUserDataPage");

        // Create windows login object        
        WindowsLiveLogin wwl = new WindowsLiveLogin(siteName);

        // Windows live User
        WindowsLiveLogin.User liveUser = null;
        if (!WindowsLiveLogin.UseServerSideAuthorization)
        {
            if (!RequestHelper.IsPostBack())
            {
                // If client authentication, get token displayed in url after # from window.location
                String script = ControlsHelper.GetPostBackEventReference(this, "#").Replace("'#'", "window.location");
                ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "PostbackScript", ScriptHelper.GetScript(script));
            }
            else
            {
                // Try to get full url from event argument
                string fullurl = Request[postEventArgumentID];

                // Authentication token - use to get uid
                String token = ParseToken(fullurl, @"authentication_token=([\w\d.-]+)&");

                // User token - this token is used in server auth. scenario. It's stored in user object (for possible further use) so parse it too and store it
                String accessToken = ParseToken(fullurl, @"access_token=([%\w\d.-/]+)&");
                
                if (token != String.Empty)
                {
                    // Return context from session
                    GetLoginInformation();

                    // Authenticate user by found token
                    liveUser = wwl.AuthenticateClientToken(token, relativeURL, accessToken);
                    if (liveUser != null)
                    {
                        // Set info to refresh to parent page
                        ScriptHelper.RegisterWOpenerScript(Page);
                        CreateCloseScript("");
                    }
                }
            }
        }
        else
        {
            GetLoginInformation();

            // Process login via Live ID
            liveUser = wwl.ProcessLogin(code, relativeURL);
        }

        // Authorization successful
        if (liveUser != null)
        {
            // Find user by ID
            UserInfo winUser = UserInfoProvider.GetUserInfoByWindowsLiveID(liveUser.Id);

            string error = String.Empty;

            // Register new user
            if (winUser == null)
            {
                // Check whether additional user info page is set                
                // No page set, user can be created/sign
                if (additionalInfoPage == String.Empty)
                {
                    // Create new user
                    UserInfo ui = AuthenticationHelper.AuthenticateWindowsLiveUser(liveUser.Id, siteName, true, ref error);

                    // Remove live user object from session, won't be needed
                    Session.Remove("windowsliveloginuser");

                    // If user was found or successfully created
                    if ((ui != null) && (ui.Enabled))
                    {
                        double resolvedConversionValue = ValidationHelper.GetDouble(MacroResolver.Resolve(conversionValue), 0);

                        // Log user registration into the web analytics and track conversion if set
                        AnalyticsHelper.TrackUserRegistration(siteName, ui, conversionName, resolvedConversionValue);

                        MembershipActivityLogger.LogRegistration(ui.UserName, DocumentContext.CurrentDocument);

                        SetAuthCookieAndRedirect(ui);
                    }
                    // User not created
                    else
                    {
                        if (WindowsLiveLogin.UseServerSideAuthorization)
                        {
                            WindowsLiveLogin.ClearCookieAndRedirect(loginPage);
                        }
                        else
                        {
                            CreateCloseScript("clearcookieandredirect");
                        }
                    }
                }
                // Required data page exists
                else
                {
                    // Store user object in session for additional info page
                    SessionHelper.SetValue("windowsliveloginuser", liveUser);

                    if (WindowsLiveLogin.UseServerSideAuthorization)
                    {
                        // Redirect to additional info page
                        URLHelper.Redirect(UrlResolver.ResolveUrl(additionalInfoPage));
                    }
                    else
                    {
                        CreateCloseScript("redirectToAdditionalPage");
                    }
                }
            }
            else
            {
                UserInfo ui = AuthenticationHelper.AuthenticateWindowsLiveUser(liveUser.Id, siteName, true, ref error);

                // If user was found 
                if ((ui != null) && (ui.Enabled))
                {
                    SetAuthCookieAndRedirect(ui);
                }
            }
        }
    }


    /// <summary>
    /// Parse token from given URL
    /// </summary>
    /// <param name="url">URL to search</param>
    /// <param name="pattern">Regex search pattern</param>
    private string ParseToken(String url, String pattern)
    {
        string token = String.Empty;
        Regex reg = RegexHelper.GetRegex(pattern);
        MatchCollection col = reg.Matches(url);
        if (col.Count > 0)
        {
            // Token found
            token = col[0].Groups[1].ToString();
        }

        return token;
    }


    /// <summary>
    /// Creates close script (in ltlSCript)
    /// </summary>
    /// <param name="param">Script parameter</param>
    public void CreateCloseScript(String param)
    {
        ltlScript.Text = ScriptHelper.GetScript("if (wopener !== 'undefined' && wopener.refreshLiveID != null) {wopener.refreshLiveID('" + param + "');} CloseDialog();");
    }


    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.WindowsLiveID);

        siteName = SiteContext.CurrentSiteName;
        // Site name must be set
        if (string.IsNullOrEmpty(siteName))
        {
            return;
        }

        ProcessLiveIDLogin();
    }


    /// <summary>
    /// Get login information from session (returnUrl, conversionName, conversionValue)
    /// </summary>
    private void GetLoginInformation()
    {
        // Get login parameters 
        String[] parameters = SessionHelper.GetValue("LiveIDInformtion") as String[];
        ParseParameters(parameters);
        Session.Remove("LiveIDInformtion");
    }


    /// <summary>
    /// Helper method, set authentication cookie and redirect to return URL or default page.
    /// </summary>
    /// <param name="ui">User info</param>
    private void SetAuthCookieAndRedirect(UserInfo ui)
    {
        // Create authentication cookie
        AuthenticationHelper.SetAuthCookieWithUserData(ui.UserName, false, Session.Timeout, new[] { "liveidlogin" });

        MembershipActivityLogger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);

        // Redirect will be used on parent window
        if (WindowsLiveLogin.UseServerSideAuthorization)
        {
            // If there is some return URL redirect there        
            if (!String.IsNullOrEmpty(relativeURL))
            {
                URLHelper.Redirect(ResolveUrl(relativeURL));
            }
            else // Redirect to default page
            {
                URLHelper.Redirect(defaultPage);
            }
        }
    }

    #endregion
}