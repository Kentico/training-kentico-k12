using System;
using System.Text;

using CMS.Base.Web.UI;
using CMS.ExternalAuthentication;
using CMS.Helpers;
using CMS.UIControls;


public partial class CMSModules_Membership_Pages_LinkedIn_LinkedInAccessTokenPage : CMSModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.TitleText = GetString("socialnetworking.linkedin.accesstoken");
        PageTitle.ShowFullScreenButton = false;
        PageTitle.ShowCloseButton = false;

        var data = LinkedInProvider.GetLinkedInData();
        if (data.SettingsMissing)
        {
            lblStatus.Text = GetString("socialnetworking.linkedin.apisettingsmissing");
            return;
        }

        var returnUrl = new Uri(URLHelper.GetAbsoluteUrl(LinkedInProvider.ACCESS_TOKEN_PAGE));

        // User allowed access
        if (!String.IsNullOrEmpty(data.Code))
        {
            // Authenticate and retrieve tokens
            if (LinkedInProvider.Authorize(data, returnUrl, out var token))
            {
                // Return access token values and close the window
                var script = new StringBuilder("if(wopener.setAccessTokenToTextBox){ wopener.setAccessTokenToTextBox('")
                    .AppendFormat("{0}', '{1}'); CloseDialog(); }}", data.EditorId, token.AccessToken);
                ScriptHelper.RegisterStartupScript(Page, typeof(string), "TokenScript", ScriptHelper.GetScript(script.ToString()));
            }
            else
            {
                // Error occurred while communicating with LinkedIn
                lblStatus.Text = GetString("socialnetworking.authorizationerror");
            }

            return;
        }

        // User denied access
        if (data.UserDeniedAccess)
        {
            // Close the window
            var script = new StringBuilder("if(wopener.setAccessTokenToTextBox){ CloseDialog(); }");

            ScriptHelper.RegisterStartupScript(Page, typeof(string), "TokenScript", ScriptHelper.GetScript(script.ToString()));

            return;
        }

        LinkedInProvider.OpenAuthorizationPage(data, returnUrl);
    }
}
