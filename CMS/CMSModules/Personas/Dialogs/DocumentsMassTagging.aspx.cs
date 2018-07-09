using System;
using System.Collections;
using System.Collections.Generic;

using CMS.Base.Web.UI;

using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.Personas;
using CMS.SiteProvider;
using CMS.UIControls;


/// <summary>
/// Users selects personas on this dialog which should be assigned to or removed from the documents which were 
/// chosen before (on the document listing).
/// </summary>
public partial class CMSModules_Personas_Dialogs_DocumentsMassTagging : CMSModalPage
{
    private Hashtable mParameters;

    /// <summary>
    /// Parameters which specifies information needed to perform action, such as which documents should be tagged. 
    /// This hashtable is filled in DocumentListMassActionsExtender class.
    /// </summary>
    private Hashtable Parameters
    {
        get
        {
            return mParameters ?? (mParameters = (Hashtable)WindowHelper.GetItem(QueryHelper.GetString("params", "")));
        }
    }


    private MultipleDocumentsActionTypeEnum MassActionType
    {
        get
        {
            return (MultipleDocumentsActionTypeEnum)Parameters["PersonasMassAction.MassActionType"];
        }
    }


    protected void Page_Load(object sender, EventArgs ea)
    {
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(ModuleName.CONTENT, "Modify"))
        {
            RedirectToAccessDenied(ModuleName.CONTENT, "Modify");
        }

        try
        {
            QueryHelper.ValidateHash("hash");

            btnPerformAction.Click += BtnPerformActionOnClick;
            string actionResourceString = MassActionType == MultipleDocumentsActionTypeEnum.Tag ? "personas.massaction.tag" : "personas.massaction.untag";

            btnPerformAction.ResourceString = actionResourceString;
            SetTitle(GetString(actionResourceString));
        }
        catch (Exception e)
        {
            EventLogProvider.LogException("PersonasDocumentMassAction", "Internal error", e);
            RedirectToInformation("Internal error. Please see event log for more details.");
        }

        // Remove double padding
        CurrentMaster.PanelContent.RemoveCssClass("dialog-content");
    }


    private void BtnPerformActionOnClick(object sender, EventArgs eventArgs)
    {
        PerformMassAction();
        CloseDialog();
    }


    private void PerformMassAction()
    {
        var massActionScope = (MassActionScopeEnum)Parameters["PersonasMassAction.MassActionScope"];

        List<int> nodeIDs = massActionScope == MassActionScopeEnum.SelectedItems ? GetNodeIDsForSelectedDocuments() : GetNodeIDsForAllDocuments();
        List<int> personaIDs = GetSelectedPersonasIDs();

        MultipleDocumentsActionFactory.GetActionImplementation(MassActionType).PerformAction(nodeIDs, personaIDs);
    }


    private List<int> GetNodeIDsForSelectedDocuments()
    {
        return (List<int>)Parameters["PersonasMassAction.SelectedNodeIDs"];
    }


    private List<int> GetNodeIDsForAllDocuments()
    {
        string currentWhereCondition = (string)Parameters["PersonasMassAction.CurrentWhereCondition"];
        string nodeAliasPath = (string)Parameters["PersonasMassAction.NodeAliasPath"];
        int nodeLevel = (int)Parameters["PersonasMassAction.NodeLevel"];
        int classID = (int)Parameters["PersonasMassAction.ClassID"];
        bool allLevels = (bool)Parameters["PersonasMassAction.AllLevels"];

        var query = DocumentHelper.GetDocuments()
                                    .OnSite(SiteContext.CurrentSiteID)
                                    .Path(nodeAliasPath.TrimEnd('/') + "/%")
                                    .AllCultures();

        // LatestVersion() must be used here to be sure that data is fetched from the same view as on document listing page.
        query = query.LatestVersion();

        if (classID > 0)
        {
            string documentTypeName = DataClassInfoProvider.GetClassName(classID);

            query = query.Type(documentTypeName, q => q.Where(currentWhereCondition));
        }
        else
        {
            query = query.Where(currentWhereCondition);
        }

        if (!allLevels)
        {
            query = query.NestingLevel(nodeLevel + 1);
        }

        query = query.Column("NodeID").Distinct();

        return query.Select(treeNode => treeNode.NodeID).ToList();
    }


    private List<int> GetSelectedPersonasIDs()
    {
        HiddenField hidItem = (HiddenField)selPersonas.FindControl("hidItem");

        return hidItem.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => ValidationHelper.GetInteger(s, 0)).ToList();
    }


    private void CloseDialog()
    {
        string script = ScriptHelper.GetScript(@"
if (wopener.RefreshGrid) {
    wopener.RefreshGrid();
}
setTimeout('CloseDialog()', 200);
");

        ScriptHelper.RegisterStartupScript(Page, typeof (string), "CloseWindow", script);
    }
}
