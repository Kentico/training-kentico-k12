using System;

using CMS.Base;

using System.Text;
using System.Web;

using CMS.Helpers;
using CMS.UIControls;
using CMS.WebServices;


public partial class CMSModules_REST_FormControls_GenerateHash : GlobalAdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.TitleText = GetString("rest.generateauthhash");
        btnAuthenticate.Text = GetString("rest.authenticate");
        btnAuthenticate.Click += btnAuthenticate_Click;
    }


    protected void btnAuthenticate_Click(object sender, EventArgs e)
    {
        string[] urls = txtUrls.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        StringBuilder newUrls = new StringBuilder();

        foreach (string url in urls)
        {
            string urlWithoutHash = URLHelper.RemoveParameterFromUrl(url, "hash");
            string newUrl = urlWithoutHash;
            string query = URLHelper.GetQuery(newUrl).TrimStart('?');

            string absolutePathPrefix;
            string relativeRestPath;
            if (RESTServiceHelper.TryParseRestUrlPath(newUrl, out absolutePathPrefix, out relativeRestPath))
            {
                string domain = URLHelper.GetDomain(newUrl);
                newUrl = URLHelper.RemoveQuery(relativeRestPath);

                // Rewrite the URL to physical URL
                string[] rewritten = BaseRESTService.RewriteRESTUrl(newUrl, query, domain, "GET");
                
                newUrl = absolutePathPrefix + rewritten[0].TrimStart('~') + "?" + rewritten[1];
                newUrl = HttpUtility.UrlDecode(newUrl);

                // Get the hash from real URL
                newUrls.AppendLine(URLHelper.AddParameterToUrl(urlWithoutHash, "hash", RESTServiceHelper.GetUrlPathAndQueryHash(newUrl)));
            }
            else
            {
                newUrls.AppendLine(url);
            }
        }

        txtUrls.Text = newUrls.ToString();
    }
}