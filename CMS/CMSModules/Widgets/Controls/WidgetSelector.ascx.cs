using System;

using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.UIControls;


public partial class CMSModules_Widgets_Controls_WidgetSelector : CMSAdminControl
{
    #region "Variables"

    private string mAliasPath = String.Empty;
    protected int mPageTemplateId = 0;
    private string mZoneId = String.Empty;
    private bool mIsInline = false;
    private WidgetZoneTypeEnum mZoneType = WidgetZoneTypeEnum.None;
    private string mCultureCode = null;

    #endregion


    #region "Widget selector properties"

    /// <summary>
    /// Zone type.
    /// </summary>
    public WidgetZoneTypeEnum ZoneType
    {
        get
        {
            return mZoneType;
        }
        set
        {
            mZoneType = value;
        }
    }


    /// <summary>
    /// Page alias path of document where widget shoudld be placed.
    /// </summary>
    public string AliasPath
    {
        get
        {
            return mAliasPath;
        }
        set
        {
            mAliasPath = value;
        }
    }


    /// <summary>
    /// Preferred culture code to use along with alias path.
    /// </summary>
    public string CultureCode
    {
        get
        {
            if (string.IsNullOrEmpty(mCultureCode))
            {
                mCultureCode = LocalizationContext.PreferredCultureCode;
            }
            return mCultureCode;
        }
        set
        {
            mCultureCode = value;
        }
    }


    /// <summary>
    /// Page template ID.
    /// </summary>
    public int PageTemplateId
    {
        get
        {
            return mPageTemplateId;
        }
        set
        {
            mPageTemplateId = value;
        }
    }


    /// <summary>
    /// Gets or sets the ID of webpart(widget) zone where selected widget shoudld be placed.
    /// </summary>
    public string ZoneId
    {
        get
        {
            return mZoneId;
        }
        set
        {
            mZoneId = value;
        }
    }


    /// <summary>
    /// Gets or sets whether selector is loaded for inline widgets.
    /// </summary>
    public bool IsInline
    {
        get
        {
            return mIsInline;
        }
        set
        {
            mIsInline = value;
        }
    }


    /// <summary>
    /// Gets the value that indicates whether selector is loaded for dashboard widgets.
    /// </summary>
    public bool IsDashboard
    {
        get
        {
            if (PageTemplateId > 0)
            {
                PageTemplateInfo pti = PageTemplateInfoProvider.GetPageTemplateInfo(PageTemplateId);
                if ((pti != null) && (pti.PageTemplateType == PageTemplateTypeEnum.Dashboard))
                {
                    PortalContext.SetRequestViewMode(ViewModeEnum.DashboardWidgets);
                    return true;
                }
            }
            return false;
        }
    }

    #endregion


    #region "Selector properties"

    /// <summary>
    /// Gets or set the flat panel selected item.
    /// </summary>
    public string SelectedItem
    {
        get
        {
            return flatElem.SelectedItem;
        }
        set
        {
            flatElem.SelectedItem = value;
        }
    }


    /// <summary>
    /// Gets or sets name of javascript function used for passing selected value from flat selector.
    /// </summary>
    public string SelectFunction
    {
        get
        {
            return flatElem.UniFlatSelector.SelectFunction;
        }
        set
        {
            flatElem.UniFlatSelector.SelectFunction = value;
        }
    }


    /// <summary>
    /// If enabled, flat selector remembers selected item trough postbacks.
    /// </summary>
    public bool RememberSelectedItem
    {
        get
        {
            return flatElem.UniFlatSelector.RememberSelectedItem;
        }
        set
        {
            flatElem.UniFlatSelector.RememberSelectedItem = value;
        }
    }


    /// <summary>
    /// Enables  or disables stop processing.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return base.StopProcessing;
        }
        set
        {
            base.StopProcessing = value;
            flatElem.StopProcessing = value;
            treeElem.StopProcessing = value;
            EnableViewState = !value;
        }
    }


    /// <summary>
    /// Indicates if control is used on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            base.IsLiveSite = value;
            treeElem.IsLiveSite = value;
            flatElem.IsLiveSite = value;
        }
    }

    #endregion


    #region "Page methods and events"

    /// <summary>
    /// OnInit.
    /// </summary>    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        treeElem.SelectPath = "/";
    }


    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            return;
        }

        treeElem.OnItemSelected += treeElem_OnItemSelected;

        if (!RequestHelper.IsPostBack())
        {
            // Select root by default
            ResetToDefault();
        }

        // Different behavior of flat selector for different group zones
        if (IsInline)
        {
            // Inline widget
            flatElem.SelectInlineWidgets = true;
            treeElem.SelectInlineWidgets = true;
        }
        else if (IsDashboard)
        {
            // Dashboard zone
            flatElem.SelectDashboardWidgets = true;
            treeElem.SelectDashboardWidgets = true;
        }
        else if (!String.IsNullOrEmpty(ZoneId))
        {
            // Get pageinfo
            PageInfo pi = CMSWebPartPropertiesPage.GetPageInfo(AliasPath, PageTemplateId, CultureCode);

            PageTemplateInstance templateInstance = CMSPortalManager.GetTemplateInstanceForEditing(pi);
            if (templateInstance != null)
            {
                WidgetZoneTypeEnum zoneType = ZoneType;

                // Get settings of the zone if present in the template
                WebPartZoneInstance zone = templateInstance.GetZone(ZoneId);
                if ((zoneType == WidgetZoneTypeEnum.None) && (zone != null))
                {
                    zoneType = zone.WidgetZoneType;
                }

                // Set flags to flat element by type of widget zone
                if (zoneType == WidgetZoneTypeEnum.Group)
                {
                    flatElem.SelectGroupWidgets = true;
                    treeElem.SelectGroupWidgets = true;
                    flatElem.GroupID = pi.NodeGroupID;
                }
                else if (zoneType == WidgetZoneTypeEnum.User)
                {
                    flatElem.SelectUserWidgets = true;
                    treeElem.SelectUserWidgets = true;
                }
                else if (zoneType == WidgetZoneTypeEnum.Editor)
                {
                    flatElem.SelectEditorWidgets = true;
                    treeElem.SelectEditorWidgets = true;
                }
            }
        }
    }


    /// <summary>
    /// Page prerender.
    /// </summary>
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            return;
        }

        // Pass currently selected category to flat selector
        if (RequestHelper.IsPostBack())
        {
            flatElem.TreeSelectedItem = treeElem.SelectedItem;
        }
    }


    /// <summary>
    /// On tree element item selected.
    /// </summary>
    /// <param name="selectedValue">Selected value</param> 
    protected void treeElem_OnItemSelected(string selectedValue)
    {
        flatElem.TreeSelectedItem = selectedValue;

        // Clear search box and pager
        flatElem.UniFlatSelector.ResetToDefault();
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Reloads data.
    /// </summary>
    /// <param name="reloadFlat">If true, flat selector is reloaded</param>
    public override void ReloadData(bool reloadFlat)
    {
        treeElem.ReloadData();
        if (reloadFlat)
        {
            flatElem.ReloadData();
        }
    }


    /// <summary>
    /// Selects root category in tree, clears search condition and resets pager to first page.
    /// </summary>
    public void ResetToDefault()
    {
        // Select root by default            
        WidgetCategoryInfo wci = WidgetCategoryInfoProvider.GetWidgetCategoryInfo("/");
        if (wci != null)
        {
            flatElem.SelectedCategory = wci;

            // Select and expand root node                
            treeElem.SelectedItem = wci.WidgetCategoryID.ToString();
            treeElem.SelectPath = "/";
        }

        // Clear search condition and resets pager to first page
        flatElem.UniFlatSelector.ResetToDefault();
    }

    #endregion
}