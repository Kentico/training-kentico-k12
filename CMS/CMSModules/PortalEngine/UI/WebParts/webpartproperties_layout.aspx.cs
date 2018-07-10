using System;

using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_WebParts_webpartproperties_layout : CMSWebPartPropertiesPage
{
    String layoutCodeName = String.Empty;
    WebPartLayoutInfo wpli = null;

    protected override void OnPreInit(EventArgs e)
    {
        WebPartInstance webPart = null;

        // Check permissions for web part properties UI
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Design", "WebPartProperties.Layout"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WebPartProperties.Layout");
        }

        if (webpartId != "")
        {
            // Get pageinfo
            PageInfo pi = GetPageInfo(aliasPath, templateId, cultureCode);
            if (pi == null)
            {
                return;
            }

            // Get page template
            PageTemplateInfo pti = pi.UsedPageTemplateInfo;
            if ((pti != null) && ((pti.TemplateInstance != null)))
            {
                webPart = pti.TemplateInstance.GetWebPart(instanceGuid, zoneVariantId, variantId) ?? pti.TemplateInstance.GetWebPart(webpartId);
            }

        }

        if (webPart != null)
        {
            WebPartInfo wpi = WebPartInfoProvider.GetWebPartInfo(webPart.WebPartType);
            layoutCodeName = QueryHelper.GetString("layoutcodename", String.Empty);

            if (String.IsNullOrEmpty(layoutCodeName))
            {
                // Get the current layout name
                layoutCodeName = ValidationHelper.GetString(webPart.GetValue("WebPartLayout"), "");
            }

            if (wpi != null)
            {
                wpli = WebPartLayoutInfoProvider.GetWebPartLayoutInfo(wpi.WebPartName, layoutCodeName);
                if (wpli != null)
                {
                    if ((layoutCodeName != "|default|") && (layoutCodeName != "|new|") && !QueryHelper.GetBoolean("tabmode", false))
                    {
                        string url = UIContextHelper.GetElementUrl("CMS.Design", "Design.WebPartProperties.LayoutEdit", false, wpli.WebPartLayoutID);
                        url = URLHelper.AppendQuery(url, RequestContext.CurrentQueryString);
                        url = URLHelper.AddParameterToUrl(url, "tabmode", "1");
                        URLHelper.Redirect(url);
                    }
                }
            }
        }
        base.OnPreInit(e);
    }


    protected override void CreateChildControls()
    {
        if (wpli != null)
        {
            UIContext.EditedObject = wpli;
        }

        ucHierarchy.PreviewObjectName = layoutCodeName;
        ucHierarchy.PreviewURLSuffix = "&previewguid=" + instanceGuid;
        ucHierarchy.IgnoreSessionValues = true;

        base.CreateChildControls();
    }
}