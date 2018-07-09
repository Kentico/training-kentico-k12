using System;

using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_WebParts_WebPartProperties_layout_frameset_header : CMSWebPartPropertiesPage
{
    #region "Variables"

    private string mLayoutCodeName = null;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets code name of edited layout.
    /// </summary>
    private string LayoutCodeName
    {
        get
        {
            return mLayoutCodeName ?? (mLayoutCodeName = QueryHelper.GetString("layoutcodename", String.Empty));
        }
        set
        {
            mLayoutCodeName = value;
        }
    }

    #endregion


    #region "Page methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check permissions for web part properties UI
        var currentUser = MembershipContext.AuthenticatedUser;
        if (!currentUser.IsAuthorizedPerUIElement("CMS.Design", "WebPartProperties.Layout"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WebPartProperties.Layout");
        }

        WebPartInstance webPart = null;

        if (webpartId != "")
        {
            // Get pageinfo
            PageInfo pi = GetPageInfo(aliasPath, templateId, cultureCode);
            if (pi != null)
            {
                // Get page template
                PageTemplateInfo pti = pi.UsedPageTemplateInfo;

                // Get web part
                if ((pti != null) && ((pti.TemplateInstance != null)))
                {
                    webPart = pti.TemplateInstance.GetWebPart(instanceGuid, zoneVariantId, variantId) ?? pti.TemplateInstance.GetWebPart(webpartId);
                }
            }
        }

        // If the web part is not found, do not continue
        if (webPart != null)
        {
            WebPartInfo wpi = WebPartInfoProvider.GetWebPartInfo(webPart.WebPartType);
            if (wpi != null)
            {
                // Get the current layout name
                if (String.IsNullOrEmpty(LayoutCodeName))
                {
                    // Get the current layout name
                    LayoutCodeName = ValidationHelper.GetString(webPart.GetValue("WebPartLayout"), "");
                }

                // Load the web part information
                if (LayoutCodeName != "")
                {
                    WebPartLayoutInfo wpli = WebPartLayoutInfoProvider.GetWebPartLayoutInfo(wpi.WebPartName, LayoutCodeName);
                    SetEditedObject(wpli, null);
                }
            }
        }

        // Set page tabs
        InitTabs("webpartlayoutcontent");
        SetTab(0, GetString("general.general"), "webpartproperties_layout.aspx" + RequestContext.CurrentQueryString, null);
    }

    #endregion
}