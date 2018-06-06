using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.SocialMarketing;
using CMS.UIControls;
using CMS.SocialMarketing.LinkedInInternal;

using Newtonsoft.Json;

using LinkedInHelper = CMS.SocialMarketing.LinkedInHelper;


public partial class CMSModules_SocialMarketing_Pages_LinkedInCompanyAccessTokenDialog : CMSModalPage
{
    #region "Variables"

    private Hashtable mParameters;
    private TokenManager mTokenManager;
    private string mTokenManagerStoreKey;

    #endregion


    #region "Properties"

    /// <summary>
    /// Parameters given from control that opened the dialog.
    /// </summary>
    private Hashtable Parameters
    {
        get
        {
            if (mParameters == null)
            {
                 // Load data from session
                var sessionKey = QueryHelper.GetString("dataKey", String.Empty);
                if (!String.IsNullOrEmpty(sessionKey))
                {
                    mParameters = WindowHelper.GetItem(sessionKey) as Hashtable;
                }
            }
            return mParameters;
        }
    }


    /// <summary>
    /// Key that is used to store token manager in the session.
    /// </summary>
    private string TokenManagerStoreKey
    {
        get
        {
            if (String.IsNullOrEmpty(mTokenManagerStoreKey))
            {
                mTokenManagerStoreKey = QueryHelper.GetString("tokenManagerKey", String.Empty);
                if (String.IsNullOrEmpty(mTokenManagerStoreKey))
                {
                    mTokenManagerStoreKey = "LinkedInTM" + Guid.NewGuid();
                }
            }
            return mTokenManagerStoreKey;
        }
    }


    /// <summary>
    /// Token manager used for authorization. Token manager instance is stored in the session because the same instance must be used
    /// at the beginning and end of the authorization process.
    /// </summary>
    private TokenManager TokenManager
    {
        get
        {
            return mTokenManager ?? (mTokenManager = SessionHelper.GetValue(TokenManagerStoreKey) as TokenManager);
        }
        set
        {
            mTokenManager = value;
            SessionHelper.SetValue(TokenManagerStoreKey, mTokenManager);
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckPermissions(ModuleName.SOCIALMARKETING, PermissionsEnum.Read.ToString());
        PageTitle.TitleText = ResHelper.GetString("sm.linkedin.account.companyaccesstoken");
        PageTitle.ShowFullScreenButton = false;
        PageTitle.ShowCloseButton = false;

        if (Parameters == null)
        {
            ShowError(GetString("dialogs.badhashtext"));

            return;
        }

        if (!SystemContext.IsFullTrustLevel)
        {
            ShowError(GetString("socialnetworking.fulltrustrequired"));
        }

        if (TokenManager != null)
        {
            // Check if user has denied application
            string error = QueryHelper.GetString("oauth_problem", String.Empty);
            if (error.EqualsCSafe("user_refused"))
            {
                ShowError(GetString("sm.linkedin.account.msg.accesstokenrefused"));

                return;
            }

            CompleteAuthorization();
        }
        else
        {
            BeginAuthorization();
        }
    }


    /// <summary>
    /// Completes the authorization process. Access tokens and list of administrated companies are retrived and set to control that opened the dialog.
    /// </summary>
    private void CompleteAuthorization()
    {
        LinkedInAccessToken token;
        try
        {
            token = LinkedInHelper.CompleteAuthorization(TokenManager);
        }
        catch (Exception ex)
        {
            LogAndShowError("LinkedInCompanyAccessToken", "AUTH_COMPLETE", ex);

            return;
        }
        finally
        {
            SessionHelper.Remove(TokenManagerStoreKey);
        }

        List<LinkedInCompany> companies;
        try
        {
            companies = LinkedInHelper.GetUserCompanies((string)Parameters["ApiKey"], (string)Parameters["ApiSecret"], token.AccessToken, token.AccessTokenSecret);
        }
        catch (Exception ex)
        {
            LogAndShowError("LinkedInCompanyAccessToken", "GET_COMPANIES", ex);

            return;
        }

        string formattedExpiration = token.Expiration.HasValue ? TimeZoneHelper.ConvertToUserTimeZone(token.Expiration.Value, true, MembershipContext.AuthenticatedUser, SiteContext.CurrentSite) : String.Empty;
        string json = JsonConvert.SerializeObject(
            new
            {
                accessToken = token.AccessToken,
                accessTokenSecret = token.AccessTokenSecret,
                tokenExpiration = token.Expiration.HasValue ? token.Expiration.Value.ToString("g", CultureInfo.InvariantCulture) : String.Empty,
                tokenExpirationString = formattedExpiration,
                tokenAppId = Parameters["AppInfoId"],
                companies
            },
            new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeHtml }
        );

        // Set retrieved access token to the opener window
        string script = String.Format(@"
if(wopener.linkedInCompanyControl && wopener.linkedInCompanyControl['{0}']) {{
    wopener.linkedInCompanyControl['{0}'].setData({1});
}}
CloseDialog();", Parameters["ClientID"], json);

        ScriptHelper.RegisterStartupScript(Page, typeof(string), "TokenScript", script, true);
    }


    /// <summary>
    /// Begins authorization process and redirects client to the LinkedIn authorization page.
    /// </summary>
    private void BeginAuthorization()
    {
        try
        {
            // Store token manager in the session
            TokenManager = new TokenManager((string)Parameters["ApiKey"], (string)Parameters["ApiSecret"]);
            LinkedInHelper.BeginAuthorization(TokenManager, URLHelper.AddParameterToUrl(RequestContext.CurrentURL, "tokenManagerKey", TokenManagerStoreKey));
        }
        catch (LinkedInApiUnauthorizedException)
        {
            // The keys in LinkedIn application are not valid
            ShowError(GetString("sm.linkedin.account.msg.unauthorized"));
        }
        catch (Exception ex)
        {
            LogAndShowError("LinkedInCompanyAccessToken", "AUTH_BEGIN", ex);
        }
    }

    #endregion
}
