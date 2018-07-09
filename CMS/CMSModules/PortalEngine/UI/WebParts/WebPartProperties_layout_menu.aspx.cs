using System;

using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_WebParts_WebPartProperties_layout_menu : CMSWebPartPropertiesPage
{
    #region "Variables"

    private WebPartInfo webPartInfo = null;

    /// <summary>
    /// Current page info.
    /// </summary>
    private PageInfo pi = null;

    /// <summary>
    /// Page template info.
    /// </summary>
    private PageTemplateInfo pti = null;


    /// <summary>
    /// Current web part.
    /// </summary>
    private WebPartInstance webPart = null;

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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        CurrentMaster.DisplaySiteSelectorPanel = true;

        if (webpartId != "")
        {
            // Get pageinfo
            pi = GetPageInfo(aliasPath, templateId, cultureCode);
            if (pi == null)
            {
                RedirectToInformation(GetString("WebPartProperties.WebPartNotFound"));
                return;
            }

            // Get page template
            pti = pi.UsedPageTemplateInfo;
            if ((pti != null) && ((pti.TemplateInstance != null)))
            {
                webPart = pti.TemplateInstance.GetWebPart(instanceGuid, zoneVariantId, variantId) ?? pti.TemplateInstance.GetWebPart(webpartId);
            }
        }

        bool hide = false;

        // If the web part is not found, do not continue
        // Redirect to information is in main window, just hide selector
        if (webPart == null)
        {
            hide = true;
        }
        else
        {
            webPartInfo = WebPartInfoProvider.GetWebPartInfo(webPart.WebPartType);
            if (webPartInfo == null)
            {
                hide = true;
            }

            // Get the current layout name
            if (String.IsNullOrEmpty(LayoutCodeName))
            {
                mLayoutCodeName = ValidationHelper.GetString(webPart.GetValue("WebPartLayout"), "");
            }
        }

        if (hide)
        {
            pnlContent.Visible = false;
            return;
        }

        // Strings
        lblLayouts.Text = GetString("WebPartPropertise.LayoutList");

        // Add default drop down items
        selectLayout.ShowDefaultItem = true;

        // Add new item
        if (CurrentUser.IsAuthorizedPerResource("CMS.Design", "EditCode"))
        {
            selectLayout.ShowNewItem = true;
        }

        // Set where condition
        selectLayout.WhereCondition = "WebPartLayoutWebPartID =" + webPartInfo.WebPartID;

        // Hide loader, it appears on wrong position because of small frame
        selectLayout.UniSelector.OnBeforeClientChanged = "if (window.Loader) { window.Loader.hide(); }";

        // Load layouts
        if (!RequestHelper.IsPostBack() && (LayoutCodeName != ""))
        {
            selectLayout.Value = LayoutCodeName;
        }

        // Reload the content page if required
        if (!RequestHelper.IsPostBack() || QueryHelper.GetBoolean("reload", false))
        {
            LoadContentPage();
        }
    }


    /// <summary>
    /// Selected index changed.
    /// </summary>
    protected void drpLayouts_Changed(object sender, EventArgs ea)
    {
        if (webPartInfo != null)
        {
            LoadContentPage();
        }
    }


    /// <summary>
    /// Registers a script to load the layout page to the content frame.
    /// </summary>
    private void LoadContentPage()
    {
        var query = RequestContext.CurrentQueryString;
        var selectedLayout = selectLayout.Value.ToString();

        query = URLHelper.AddParameterToUrl(query, "layoutcodename", selectedLayout);
        query = URLHelper.AddParameterToUrl(query, "noreload", "true");
        query = URLHelper.RemoveParameterFromUrl(query, "tabmode");

        var script = ScriptHelper.GetScript(@"parent.frames['webpartpropertiescontent'].location = 'webpartproperties_layout.aspx" + query + "';");
        ScriptHelper.RegisterStartupScript(Page, typeof(string), "SetWebPartLayout", script);
    }

    #endregion
}
