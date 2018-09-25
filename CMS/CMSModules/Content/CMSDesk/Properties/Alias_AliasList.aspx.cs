using System;
using System.Data;
using System.Text;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_Properties_Alias_AliasList : CMSModalPage
{
    protected override void OnInit(EventArgs e)
    {
        // Check site availability
        if (!ResourceSiteInfoProvider.IsResourceOnSite("CMS.Content", SiteContext.CurrentSiteName))
        {
            RedirectToResourceNotAvailableOnSite("CMS.Content");
        }

        // Check security for content and properties tab
        CMSPropertiesPage.CheckPropertiesSecurity();
        CMSContentPage.CheckSecurity();

        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {       
        // Init unigrid
        ugAlias.OnDataReload += UniGridAlias_OnDataReload;
        ugAlias.OnAction += UniGridAlias_OnAction;
        ugAlias.OnExternalDataBound += UniGridAlias_OnExternalDataBound;
        ugAlias.OnAfterRetrieveData += UniGridAlias_OnAfterRetrieveData;
        
        PageTitle.TitleText = GetString("content.ui.urlsaliases");
        
        ScriptHelper.RegisterTooltip(Page);
    }


    private DataSet UniGridAlias_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        var query = GetAliasesQuery(completeWhere, currentOrder, currentTopN, columns, currentOffset, currentPageSize);
        var aliases = query.Result;
        totalRecords = query.TotalRecords;

        return aliases;
    }


    private static ObjectQuery<DocumentAliasInfo> GetAliasesQuery(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize)
    {
        var columnList = GetColumns(columns);
        var where = SqlHelper.AddWhereCondition(completeWhere, "AliasSiteID = " + SiteContext.CurrentSiteID);
        var query = DocumentAliasInfoProvider.GetDocumentAliasesWithNodesDataQuery()
                                        .Source(s => s.InnerJoin<DataClassInfo>("NodeClassID", "ClassID"))
                                        .Where(where)
                                        .OrderBy(currentOrder)
                                        .TopN(currentTopN)
                                        .Columns(columnList);
        query.Offset = currentOffset;
        query.MaxRecords = currentPageSize;

        return query;
    }


    private static string[] GetColumns(string columns)
    {
        // Columns NodeSiteID, NodeACLID and NodeOwner are required for permission check. Do not remove them.
        const string REQUIRED_COLUMNS = "AliasID, AliasCulture AS DocumentCulture, AliasUrlPath, AliasExtensions, NodeAliasPath, NodeName, NodeParentID, ClassName, NodeSiteID, NodeACLID, NodeOwner";
        return SqlHelper.MergeColumns(columns, REQUIRED_COLUMNS).Split(',');
    }


    private DataSet UniGridAlias_OnAfterRetrieveData(DataSet ds)
    {
        return GetAliasesVisibleForCurrentUser(ds);
    }


    private DataSet GetAliasesVisibleForCurrentUser(DataSet ds)
    {
        return TreeSecurityProvider.FilterDataSetByPermissions(ds, NodePermissionsEnum.Read, MembershipContext.AuthenticatedUser);
    }


    private void UniGridAlias_OnAction(string actionName, object actionArgument)
    {
        int aliasID = ValidationHelper.GetInteger(actionArgument, 0);
        string action = DataHelper.GetNotEmpty(actionName, String.Empty).ToLowerCSafe();
        var alias = DocumentAliasInfoProvider.GetDocumentAliasInfo(aliasID);
        if (alias == null)
        {
            return;
        }

        TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
        TreeNode node = tree.SelectSingleNode(alias.AliasNodeID);
        if (node != null)
        {
            // Check modify permissions
            if (MembershipContext.AuthenticatedUser.IsAuthorizedPerDocument(node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied)
            {
                return;
            }

            // Edit only if node exists
            if (action == "edit")
            {
                URLHelper.Redirect(UrlResolver.ResolveUrl("Alias_Edit.aspx?nodeid=" + node.NodeID + "&aliasid=" + aliasID + "&defaultNodeID=" + NodeID + "&dialog=1"));
            }
        }

        // Delete even if node does not exist
        if (action == "delete")
        {
            if (aliasID > 0)
            {
                DocumentAliasInfoProvider.DeleteDocumentAliasInfo(aliasID);
                DocumentSynchronizationHelper.LogDocumentChange(node, TaskTypeEnum.UpdateDocument, tree);
            }
        }
    }


    protected object UniGridAlias_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        sourceName = sourceName.ToLowerCSafe();
        switch (sourceName)
        {
            case "culture":
                return UniGridFunctions.CultureDisplayName(parameter);

            case "documentname":
            {
                var data = (DataRowView)parameter;
                return GetDocumentNameColumnMarkup(data);
            }
        }

        return parameter;
    }


    private object GetDocumentNameColumnMarkup(DataRowView data)
    {
        var documentName = ValidationHelper.GetString(data["NodeName"], string.Empty);
        var documentAliasPath = ValidationHelper.GetString(DataHelper.GetDataRowViewValue(data, "NodeAliasPath"), String.Empty);
        var tooltip = GetTooltip(documentName, documentAliasPath);
        var pageTypeIcon = GetNodeIcon(Convert.ToString(data["ClassName"]));

        var sb = new StringBuilder();
        sb.Append(
            "<span style=\"cursor: help \" onmouseout=\"UnTip()\" onmouseover=\"Tip('", HTMLHelper.EncodeForHtmlAttribute(tooltip), "')\" >" + pageTypeIcon + "<span>",
            HTMLHelper.HTMLEncode(TextHelper.LimitLength(documentName, 50)),
            "</span></span>"
            );

        return sb.ToString();
    }


    private static string GetTooltip(string documentName, string documentAliasPath)
    {
        return "<strong>" + HTMLHelper.HTMLEncode(documentName) + "</strong>" + 
               (String.IsNullOrEmpty(documentAliasPath) ? "" : "<br />" + 
                                                               GetString("general.path") + 
                                                               ":&nbsp;" + 
                                                               HTMLHelper.HTMLEncode(documentAliasPath));
    }


    private string GetNodeIcon(string className)
    {
        var documentType = DataClassInfoProvider.GetDataClassInfo(className);
        var icon = UIHelper.GetDocumentTypeIcon(this, documentType.ClassName, (string)documentType.GetValue("ClassIconClass"));
        return icon;
    }
}
