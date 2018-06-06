using System;
using System.Globalization;

using CMS;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.Modules;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;

[assembly: RegisterCustomClass("ContentPropertiesTabsControlExtender", typeof(ContentPropertiesTabsControlExtender))]

/// <summary>
/// Content edit tabs control extender
/// </summary>
public class ContentPropertiesTabsControlExtender : UITabsExtender
{
    /// <summary>
    /// Document manager
    /// </summary>
    public ICMSDocumentManager DocumentManager
    {
        get;
        set;
    }


    /// <summary>
    /// Initialization of tabs.
    /// </summary>
    public override void OnInitTabs()
    {
        var page = (CMSPage)Control.Page;

        // Setup the document manager
        DocumentManager = page.DocumentManager;

        ScriptHelper.RegisterScriptFile(Control.Page, "~/CMSModules/Content/CMSDesk/Properties/PropertiesTabs.js");

        Control.OnTabCreated += OnTabCreated;
    }


    protected void OnTabCreated(object sender, TabCreatedEventArgs e)
    {
        if (e.Tab == null)
        {
            return;
        }

        var tab = e.Tab;
        var element = e.UIElement;

        var manager = DocumentManager;
        var node = manager.Node;

        bool splitViewSupported = PortalContext.ViewMode != ViewModeEnum.EditLive;

        string elementName = element.ElementName.ToLowerCSafe();

        if (DocumentUIHelper.IsElementHiddenForNode(element, node))
        {
            e.Tab = null;
            return;
        }

        switch (elementName)
        {
            case "properties.languages":
                splitViewSupported = false;
                if (!CultureSiteInfoProvider.IsSiteMultilingual(SiteContext.CurrentSiteName) || !CultureSiteInfoProvider.LicenseVersionCheck())
                {
                    e.Tab = null;
                    return;
                }
                break;

            case "properties.security":
            case "properties.relateddocs":
            case "properties.linkeddocs":
                splitViewSupported = false;
                break;

            case "properties.variants":

                if (DataHelper.GetNotEmpty(RequestContext.CurrentDomain, "") != "")
                {
                    // Check license and whether content personalization is enabled and whether exists at least one variant for current template
                    if ((node == null)
                        || !LicenseHelper.IsFeatureAvailableInUI(FeatureEnum.ContentPersonalization, ModuleName.ONLINEMARKETING)
                        || !ResourceSiteInfoProvider.IsResourceOnSite("CMS.ContentPersonalization", SiteContext.CurrentSiteName)
                        || !PortalContext.ContentPersonalizationEnabled
                        || (VariantHelper.GetVariantID(VariantModeEnum.ContentPersonalization, node.GetUsedPageTemplateId(), String.Empty) <= 0))
                    {
                        e.Tab = null;
                        return;
                    }
                }
                break;

            case "properties.workflow":
            case "properties.versions":
                if (manager.Workflow == null)
                {
                    e.Tab = null;
                    return;
                }
                break;

            case "properties.personas":
                tab.RedirectUrl = URLHelper.AddParameterToUrl(tab.RedirectUrl, "objectid", manager.NodeID.ToString(CultureInfo.InvariantCulture));
                break;

            case "properties.template":
                if ((node == null)
                    || !MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.Design", "Design", node.NodeSiteName))
                {
                    e.Tab = null;
                }

                break;
        }

        // UI elements could have a different display name if content only document is selected
        tab.Text = DocumentUIHelper.GetUIElementDisplayName(element, node);

        // Ensure split view mode
        if (splitViewSupported && PortalUIHelper.DisplaySplitMode)
        {
            tab.RedirectUrl = DocumentUIHelper.GetSplitViewUrl(tab.RedirectUrl);
        }
    }
}
