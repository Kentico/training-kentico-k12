using System;
using System.Web.UI;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.ExternalAuthentication;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Membership.Web.UI;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.SiteProvider;

public partial class CMSWebParts_Membership_Logon_SignOutButton : CMSAbstractWebPart
{
    #region "Variables"

    private string mSignOutText = ResHelper.LocalizeString("{$Webparts_Membership_SignOutButton.SignOut$}");
    private string mSignInText = ResHelper.LocalizeString("{$Webparts_Membership_SignOutButton.SignIn$}");

    private bool mShowOnlyWhenAuthenticated = true;
    private bool mShowAsLink = false;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets the sign out text.
    /// </summary>
    public string SignOutText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SignOutText"), mSignOutText);
        }
        set
        {
            SetValue("SignOutText", value);
            mSignOutText = value;
        }
    }


    /// <summary>
    /// Gets or sets the sign in text.
    /// </summary>
    public string SignInText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SignInText"), mSignInText);
        }
        set
        {
            SetValue("SignInText", value);
            mSignInText = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the webpart is shown as button or as text link.
    /// </summary>
    public bool ShowAsLink
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowAsLink"), mShowAsLink);
        }
        set
        {
            SetValue("ShowAsLink", value);
            mShowAsLink = value;
        }
    }


    /// <summary>
    /// Gets or sets the URL where user is redirected after sign out.
    /// </summary>
    public string RedirectToUrl
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("RedirectToURL"), RequestContext.CurrentURL);
        }
        set
        {
            SetValue("RedirectToURL", value);
        }
    }


    /// <summary>
    /// Gets or sets the path where user is redirected before sign in.
    /// </summary>
    public string SignInUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SignInPageUrl"), "");
        }
        set
        {
            SetValue("SignInPageUrl", value);
        }
    }


    /// <summary>
    /// Gets or sets the path where user is redirected after sign in.
    /// </summary>
    public string ReturnPath
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ReturnPath"), "");
        }
        set
        {
            SetValue("ReturnPath", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether webpart is shown only when the user is authenticated.
    /// </summary>
    public bool ShowOnlyWhenAuthenticated
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowOnlyWhenAuthenticated"), mShowOnlyWhenAuthenticated);
        }
        set
        {
            SetValue("ShowOnlyWhenAuthenticated", value);
            mShowOnlyWhenAuthenticated = value;
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
            btnSignOut.SkinID = value;
        }
    }

    #endregion


    #region "Methods"

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Visible = true;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
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
            // Show only desired button
            btnSignOut.Visible = !ShowAsLink;
            btnSignOutLink.Visible = ShowAsLink;

            if (!MembershipContext.AuthenticatedUser.IsPublic())
            {
                // Hide for windows authentication
                if (AuthenticationMode.IsWindowsAuthentication())
                {
                    Visible = false;
                }
                else
                {
                    // Set signout text
                    btnSignOutLink.Text = SignOutText;
                    btnSignOut.Text = SignOutText;

                    // Set logout scripts
                    string logOutScript = SignOutScriptHelper.GetSignOutOnClickScript(Page);
                    btnSignOutLink.OnClientClick = logOutScript;
                    btnSignOut.OnClientClick = logOutScript;
                }
            }
            else
            {
                // Set signin text
                btnSignOutLink.Text = SignInText;
                btnSignOut.Text = SignInText;
            }
        }

        if (!StandAlone && (PageCycle < PageCycleEnum.Initialized) && (ValidationHelper.GetString(Page.StyleSheetTheme, "") == ""))
        {
            btnSignOut.SkinID = SkinID;
        }


        // if user is not authenticated and ShowOnlyWhenAuthenticated is set
        if (MembershipContext.AuthenticatedUser.IsPublic() && ShowOnlyWhenAuthenticated)
        {
            Visible = false;
        }
    }


    /// <summary>
    /// SignOut handler.
    /// </summary>
    protected void btnSignOut_Click(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            // Do not process
        }
        else
        {
            if (AuthenticationHelper.IsAuthenticated())
            {
                string redirectUrl = RedirectToUrl;

                // If the user has registered Windows Live ID
                if (!String.IsNullOrEmpty(MembershipContext.AuthenticatedUser.UserSettings.WindowsLiveID))
                {
                    // Get data from auth cookie
                    string[] userData = AuthenticationHelper.GetUserDataFromAuthCookie();

                    // If user has logged in using Windows Live ID, then sign him out from Live too
                    if ((userData != null) && (Array.IndexOf(userData, "liveidlogin") >= 0))
                    {
                        string siteName = SiteContext.CurrentSiteName;

                        // Get LiveID settings
                        string appId = SettingsKeyInfoProvider.GetValue(siteName + ".CMSApplicationID");
                        string secret = SettingsKeyInfoProvider.GetValue(siteName + ".CMSApplicationSecret");

                        // Check valid Windows LiveID parameters
                        if ((appId != string.Empty) && (secret != string.Empty))
                        {
                            WindowsLiveLogin wll = new WindowsLiveLogin(appId, secret);

                            // Redirect to Windows Live and back to "home" page
                            string defaultAliasPath = SettingsKeyInfoProvider.GetValue(siteName + ".CMSDefaultAliasPath");
                            string url = DocumentURLProvider.GetUrl(defaultAliasPath);
                            redirectUrl = wll.GetLogoutUrl(URLHelper.GetAbsoluteUrl(url));
                        }
                    }
                }

                PortalContext.ViewMode = ViewModeEnum.LiveSite;
                AuthenticationHelper.SignOut();

                Response.Cache.SetNoStore();
                URLHelper.Redirect(UrlResolver.ResolveUrl(redirectUrl));
            }
            else
            {
                string returnUrl = null;
                string signInUrl = null;

                if (SignInUrl != "")
                {
                    signInUrl = ResolveUrl(DocumentURLProvider.GetUrl(MacroResolver.ResolveCurrentPath(SignInUrl)));
                }
                else
                {
                    signInUrl = AuthenticationHelper.GetSecuredAreasLogonPage(SiteContext.CurrentSiteName);
                }

                if (ReturnPath != "")
                {
                    returnUrl = ResolveUrl(DocumentURLProvider.GetUrl(MacroResolver.ResolveCurrentPath(ReturnPath)));
                }
                else
                {
                    returnUrl = RequestContext.CurrentURL;
                }

                if (signInUrl != "")
                {
                    // Prevent multiple returnUrl parameter
                    returnUrl = URLHelper.RemoveParameterFromUrl(returnUrl, "returnUrl");
                    URLHelper.Redirect(UrlResolver.ResolveUrl(URLHelper.UpdateParameterInUrl(signInUrl, "returnurl", Server.UrlEncode(returnUrl))));
                }
            }
        }
    }


    /// <summary>
    /// Applies given stylesheet skin.
    /// </summary>
    /// <param name="page">Page</param>
    public override void ApplyStyleSheetSkin(Page page)
    {
        btnSignOut.SkinID = SkinID;

        base.ApplyStyleSheetSkin(page);
    }


    /// <summary>
    /// PreInit handler.
    /// </summary>
    protected void CMSWebParts_Search_cmssearchboxl_PreInit(object sender, EventArgs e)
    {
        // Set SkinID property
        if (!StandAlone && (PageCycle < PageCycleEnum.Initialized))
        {
            btnSignOut.SkinID = SkinID;
        }
    }

    #endregion
}