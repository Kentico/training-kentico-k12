using System;
using System.Collections.Generic;

using CMS.Base;

using System.Linq;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.UIControls;
using CMS.UIControls.UniMenuConfig;
using CMS.WorkflowEngine;
using CMS.WorkflowEngine.Web.UI;


public partial class CMSModules_Workflows_Controls_WorkflowDesignerToolbar : UniGraphToolbar
{
    #region "Private variables"

    private List<WorkflowStepTypeEnum> mStepItems = new List<WorkflowStepTypeEnum>();

    private StepTypeUniMenuItems mInjector = null;

    #endregion


    #region "Properties"

    /// <summary>
    /// Factory for uniMenu items.
    /// </summary>
    protected StepTypeUniMenuItems Factory
    {
        get
        {
            if (mInjector == null)
            {
                mInjector = new StepTypeUniMenuItems(JsGraphObject);
            }
            return mInjector;
        }
    }


    /// <summary>
    /// Steps user should be able to add to graph from toolbar.
    /// </summary>
    public List<WorkflowStepTypeEnum> StepItems
    {
        get
        {
            if (mStepItems.Count == 0 && Visible)
            {
                InitStepItems();
            }
            return mStepItems;
        }
        set
        {
            mStepItems = value;
        }
    }


    /// <summary>
    /// Groups of uniMenu.
    /// </summary>
    public override List<Group> ToolbarGroups
    {
        get
        {
            return toolbar.Groups;
        }
        set
        {
            toolbar.Groups = value;
        }
    }


    /// <summary>
    /// In this control visibility has same function as stopping processing. Only values are inversed.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return !Visible;
        }
        set
        {
            Visible = !value;
        }
    }


    /// <summary>
    /// Property of workflow info object.
    /// </summary>
    public WorkflowInfo Workflow
    {
        get;
        set;
    }

    #endregion


    #region "Event handlers"

    protected override void OnLoad(EventArgs e)
    {
        if (!Visible)
        {
            return;
        }

        InitResources();
        SetBaseNodeItems();
        AddActionGroup(editToolbar.Search);
        RegisterInitScript();

        base.OnLoad(e);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Initializes default set of menu items in steps group.
    /// </summary>
    private void InitStepItems()
    {
        StepItems = new List<WorkflowStepTypeEnum>()
        {
            {WorkflowStepTypeEnum.Standard},
            {WorkflowStepTypeEnum.Condition},
            {WorkflowStepTypeEnum.Multichoice},
            {WorkflowStepTypeEnum.MultichoiceFirstWin},
            {WorkflowStepTypeEnum.Userchoice},
            {WorkflowStepTypeEnum.Wait}
        };

        if (Workflow.IsDocumentWorkflow)
        {
            StepItems.Add(WorkflowStepTypeEnum.DocumentPublished);
            StepItems.Add(WorkflowStepTypeEnum.DocumentArchived);
        }
        else if (Workflow.IsAutomation)
        {
            StepItems.Add(WorkflowStepTypeEnum.Finished);
        }
    }


    /// <summary>
    /// Registers script initializing toolbar.
    /// </summary>
    private void RegisterInitScript()
    {
        string script = string.Format("graphControlHandler.initToolbar('{0}');", pnlAjax.ClientID);
        ScriptHelper.RegisterStartupScript(Page, typeof (string), ClientID, script, true);
    }


    /// <summary>
    /// Initializes default paths to controls.
    /// </summary>
    private void InitResources()
    {
        NodesControlPath = "~/CMSAdminControls/UI/UniMenu/UniGraphToolbar/NodesGroup.ascx";
        //NodesGroupResourceString = "workflowToolbar.stepsGroup";
        editToolbar.JsGraphObject = JsGraphObject;
    }


    /// <summary>
    /// Sets menu items in nodes group based on workflow step types.
    /// </summary>
    private void SetBaseNodeItems()
    {
        NodeItems = Factory.GetSettingsObject(StepItems);
        NodeItems = FilterList(NodeItems, editToolbar.Search);
    }


    /// <summary>
    /// Adds group for actions.
    /// </summary>
    /// <param name="where">Pattern to be used to filter actions</param>
    private void AddActionGroup(string where)
    {
        string condition = "ActionEnabled = 1";

        // Allowed objects
        condition = SqlHelper.AddWhereCondition(condition, "ActionAllowedObjects IS NULL OR ActionAllowedObjects LIKE N'%" + Workflow.WorkflowAllowedObjects + "%'");

        // Workflow type
        condition = SqlHelper.AddWhereCondition(condition, Workflow.IsAutomation ? "ActionWorkflowType = " + (int)Workflow.WorkflowType : "ActionWorkflowType = " + (int)Workflow.WorkflowType + " OR ActionWorkflowType IS NULL");

        InfoDataSet<WorkflowActionInfo> actions = WorkflowActionInfoProvider.GetWorkflowActions().Where(condition).OrderBy("ActionDisplayName").Columns("ActionID, ActionThumbnailGUID, ActionThumbnailClass, ActionDisplayName, ActionDescription, ActionIconGUID, ActionIconClass").TypedResult;

        List<Item> nodesMenuItems = Factory.GetSettingsObjectBy(actions);

        nodesMenuItems = FilterList(nodesMenuItems, where);
        nodesMenuItems = nodesMenuItems.OrderBy(i => i.Text).ToList();

        if (nodesMenuItems.Count > 0)
        {
            KeyValuePair<string, object> nodesValue = new KeyValuePair<string, object>("NodesMenuItems", nodesMenuItems);
            AppendGroup(NodesControlPath, nodesValue, "");
        }
    }


    /// <summary>
    /// Filters unimenu items in list by specified name.
    /// </summary>
    /// <param name="list">List to be filtered</param>
    /// <param name="pattern">Name to be filtered by</param>
    /// <returns>Filtered list</returns>
    private List<Item> FilterList(List<Item> list, string pattern)
    {
        if (!string.IsNullOrEmpty(pattern))
        {
            return list.Where(n => n.Text.ToLowerCSafe().Contains(pattern.ToLowerCSafe()) || n.Tooltip.ToLowerCSafe().Contains(pattern.ToLowerCSafe())).ToList<Item>();
        }
        return list;
    }

    #endregion
}
