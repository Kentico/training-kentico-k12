using System;
using System.Data;
using System.Web.UI;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Search;
using CMS.UIControls;


public partial class CMSModules_SmartSearch_Controls_UI_SearchIndex_Forum_List : CMSAdminListControl, IPostBackEventHandler
{
    #region "Variables"

    private int indexId = 0;
    private SearchIndexInfo sii = null;
    private SearchIndexSettings sis = null;
    private bool smartSearchEnabled = SettingsKeyInfoProvider.GetBoolValue("CMSSearchIndexingEnabled");

    #endregion


    #region "Properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
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
            plcMess.IsLiveSite = value;
            base.IsLiveSite = value;
        }
    }

    #endregion


    #region "Event handlers"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            return;
        }

        // Show panel with message how to enable indexing
        ucDisabledModule.TestSettingKeys = "CMSSearchIndexingEnabled";
        ucDisabledModule.InfoText = GetString("srch.searchdisabledinfo");

        indexId = QueryHelper.GetInteger("indexid", 0);

        UniGrid.OnExternalDataBound += UniGrid_OnExternalDataBound;
        UniGrid.OnAction += UniGrid_OnAction;
        UniGrid.OnDataReload += UniGrid_OnDataReload;
        UniGrid.HideControlForZeroRows = true;
        UniGrid.ShowActionsMenu = true;
        UniGrid.AllColumns = "id, ForumNames, SiteName, Type";

        sii = SearchIndexInfoProvider.GetSearchIndexInfo(indexId);
    }


    /// <summary>
    /// Reloads datagrid.
    /// </summary>
    private DataSet UniGrid_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        DataSet result = null;
        
        if (sii != null)
        {
            result = sii.IndexSettings.GetAll();

            if (!DataHelper.DataSourceIsEmpty(result))
            {
                // Set 'id' column to first position
                if (result.Tables[0].Columns["id"] != null)
                {
                    result.Tables[0].Columns["id"].SetOrdinal(0);
                }

                // Check if 'type' column exists
                if (result.Tables[0].Columns["type"] == null)
                {
                    result.Tables[0].Columns.Add(new DataColumn("type"));
                }
            }
        }
        return result;
    }


    /// <summary>
    /// Unigrid databound handler.
    /// </summary>
    private object UniGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        DataRowView drv = (parameter as DataRowView);

        switch (sourceName.ToLowerCSafe())
        {
            case "forumnames":
                string forumNames = HTMLHelper.HTMLEncode(TextHelper.LimitLength(drv["ForumNames"].ToString(), 50));
                if (string.IsNullOrEmpty(forumNames))
                {
                    return GetString("general.selectall");
                }
                else
                {
                    return forumNames;
                }

            case "sitename":
                string sites = HTMLHelper.HTMLEncode(TextHelper.LimitLength(drv["sitename"].ToString(), 50));
                if (String.IsNullOrEmpty(sites))
                {
                    sites = GetString("general.selectall");
                    ;
                }
                return sites;

            case "type":
                string type = HTMLHelper.HTMLEncode(TextHelper.LimitLength(drv["type"].ToString().ToLowerCSafe(), 50));
                if (type == "allowed")
                {
                    type = "<span class=\"StatusEnabled\">" + GetString("srch.list.allowed") + "</span>";
                }
                else if (type == "excluded")
                {
                    type = "<span class=\"StatusDisabled\">" + GetString("srch.list.excluded") + "</span>";
                }
                return type;
        }
        return null;
    }


    /// <summary>
    /// Unigrid on action handler.
    /// </summary>
    private void UniGrid_OnAction(string actionName, object actionArgument)
    {
        Guid guid;
        switch (actionName)
        {
            case "edit":
                guid = ValidationHelper.GetGuid(actionArgument, Guid.Empty);
                RaiseOnAction("edit", guid);
                break;

            case "delete":
                // Delete search index info object from database with it's dependences
                guid = ValidationHelper.GetGuid(actionArgument, Guid.Empty);

                sis = sii.IndexSettings;
                sis.DeleteSearchIndexSettingsInfo(guid);
                sii.IndexSettings = sis;
                SearchIndexInfoProvider.SetSearchIndexInfo(sii);

                // Show message about rebuilding
                if (smartSearchEnabled)
                {
                    ShowInformation(String.Format(GetString("srch.indexrequiresrebuild"), "<a href=\"javascript:" + Page.ClientScript.GetPostBackEventReference(this, "saved") + "\">" + GetString("General.clickhere") + "</a>"));
                }
                break;
        }
    }

    #endregion


    #region "IPostBackEventHandler Members"

    public void RaisePostBackEvent(string eventArgument)
    {
        if (eventArgument == "saved")
        {
            if (SearchHelper.CreateRebuildTask(indexId))
            {
                ShowInformation(GetString("srch.index.rebuildstarted"));
            }
            else
            {
                ShowError(GetString("index.nocontent"));
            }
        }
    }

    #endregion
}