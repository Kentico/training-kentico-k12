using System;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine.Web.UI.Internal;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Widgets_LiveDialogs_WidgetProperties : CMSWidgetPropertiesLiveModalPage
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Public user is not allowed for widgets
        if (!AuthenticationHelper.IsAuthenticated())
        {
            RedirectToAccessDenied(GetString("widgets.security.notallowed"));
        }

        var viewMode = ViewModeCode.FromString(QueryHelper.GetString("viewmode", String.Empty));
        var hash = QueryHelper.GetString("hash", String.Empty);

        LiveSiteWidgetsParameters dialogparameters = new LiveSiteWidgetsParameters(aliasPath, viewMode)
        {
            ZoneId = zoneId,
            ZoneType = zoneType,
            InstanceGuid = instanceGuid,
            TemplateId = templateId,
            IsInlineWidget = inline
        };

        if (!dialogparameters.ValidateHash(hash))
        {
            return;
        }

        // Set page title 
        Page.Title = GetString(isNewWidget ? "widgets.propertiespage.titlenew" : "widgets.propertiespage.title");

        if ((widgetId != string.Empty) && (aliasPath != string.Empty))
        {
            // Get page info
            var siteName = SiteContext.CurrentSiteName;
            PageInfo pi = PageInfoProvider.GetPageInfo(siteName, aliasPath, LocalizationContext.PreferredCultureCode, null, SiteInfoProvider.CombineWithDefaultCulture(siteName));

            if (pi == null)
            {
                return;
            }

            // Get template instance
            PageTemplateInstance templateInstance = CMSPortalManager.GetTemplateInstanceForEditing(pi);

            // Get widget from instance
            WidgetInfo wi = null;
            if (!isNewWidget)
            {
                // Get the instance of widget
                WebPartInstance widgetInstance = templateInstance.GetWebPart(instanceGuid, widgetId);
                if (widgetInstance == null)
                {
                    return;
                }

                // Get widget info by widget name(widget type)
                wi = WidgetInfoProvider.GetWidgetInfo(widgetInstance.WebPartType);
            }
            // Widget instance hasn't created yet
            else
            {
                wi = WidgetInfoProvider.GetWidgetInfo(ValidationHelper.GetInteger(widgetId, 0));
            }


            if (wi != null)
            {
                WebPartZoneInstance zone = templateInstance.GetZone(zoneId);
                if (zone != null)
                {
                    var currentUser = MembershipContext.AuthenticatedUser;

                    bool checkSecurity = true;

                    // Check security
                    // It is group zone type but widget is not allowed in group
                    if (zone.WidgetZoneType == WidgetZoneTypeEnum.Group)
                    {
                        // Should always be, only group widget are allowed in group zone
                        if (wi.WidgetForGroup)
                        {
                            if (!currentUser.IsGroupAdministrator(pi.NodeGroupID))
                            {
                                RedirectToAccessDenied(GetString("widgets.security.notallowed"));
                            }

                            // All ok, don't check classic security
                            checkSecurity = false;
                        }
                    }

                    if (checkSecurity && !WidgetRoleInfoProvider.IsWidgetAllowed(wi, currentUser.UserID, AuthenticationHelper.IsAuthenticated()))
                    {
                        RedirectToAccessDenied(GetString("widgets.security.notallowed"));
                    }
                }
            }
        }
        // If all ok, set up frames
        rowsFrameset.Attributes.Add("rows", string.Format("{0}, *", TitleOnlyHeight));

        frameHeader.Attributes.Add("src", "widgetproperties_header.aspx" + RequestContext.CurrentQueryString);
        if (inline && !isNewWidget)
        {
            frameContent.Attributes.Add("src", ResolveUrl("~/CMSPages/Blank.htm"));
        }
        else
        {
            frameContent.Attributes.Add("src", "widgetproperties_properties_frameset.aspx" + RequestContext.CurrentQueryString);
        }
    }
}