using System;
using System.Linq;

using CMS.Base.Web.UI;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.PortalEngine.Web.UI;
using CMS.Base;
using CMS.DocumentEngine;

using TreeNode = CMS.DocumentEngine.TreeNode;

public partial class CMSEdit_default : AbstractCMSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Redirect to the web site root by default
        string returnUrl = URLHelper.ResolveUrl("~/");

        // Check whether on-site editing is enabled
        if (PortalHelper.IsOnSiteEditingEnabled(SiteContext.CurrentSiteName))
        {
            var cui = MembershipContext.AuthenticatedUser;
            // Check the permissions
            if (cui.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Editor,SiteContext.CurrentSiteName)  && cui.IsAuthorizedPerResource("cms.content", "ExploreTree") && cui.IsAuthorizedPerResource("cms.content", "Read"))
            {
                // Set edit-live view mode
                PortalContext.SetViewMode(ViewModeEnum.EditLive);
            }
            else
            {
                // Redirect to access denied page when the current user does not have permissions for the OnSite editing
                CMSPage.RedirectToUINotAvailable();
            }

            // Try get return URL
            string queryUrl = QueryHelper.GetString("editurl", String.Empty);
            if (!String.IsNullOrEmpty(queryUrl) && (queryUrl.StartsWithCSafe("~/") || queryUrl.StartsWithCSafe("/")))
            {
                returnUrl = URLHelper.ResolveUrl(queryUrl);
            }
            // Use default alias path if return url isn't defined
            else
            {
                string aliasPath = PageInfoProvider.GetDefaultAliasPath(RequestContext.CurrentDomain, SiteContext.CurrentSiteName);
                if (!String.IsNullOrEmpty(aliasPath))
                {
                    // Get the document which will be displayed for the default alias path
                    TreeProvider tr = new TreeProvider();
                    TreeNode node = tr.SelectSingleNode(SiteContext.CurrentSiteName, aliasPath, LocalizationContext.PreferredCultureCode, true);
                    if (node != null)
                    {
                        aliasPath = node.NodeAliasPath;
                    }

                    returnUrl = DocumentURLProvider.GetUrl(aliasPath);
                    returnUrl = UrlResolver.ResolveUrl(returnUrl);
                }
            }

            // Remove view mode value from query string
            returnUrl = URLHelper.RemoveParameterFromUrl(returnUrl, "viewmode");
        }

        // Redirect to the requested page
        URLHelper.Redirect(returnUrl);
    }
}
