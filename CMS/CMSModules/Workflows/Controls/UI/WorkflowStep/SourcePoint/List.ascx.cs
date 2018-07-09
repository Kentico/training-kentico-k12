using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.UIControls;
using CMS.WorkflowEngine;
using CMS.WorkflowEngine.GraphConfig;
using CMS.WorkflowEngine.Web.UI;

public partial class CMSModules_Workflows_Controls_UI_WorkflowStep_SourcePoint_List : CMSAdminListControl
{
    #region "Variables"

    private WorkflowStepInfo mCurrentStepInfo = null;
    private WorkflowNode mCurrentNode = null;
    private int conditionPosition = 0;

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
    /// ID of workflow step containing listed source points. 
    /// </summary>
    public int WorkflowStepID
    {
        get;
        set;
    }


    /// <summary>
    /// Current workflow step info
    /// </summary>
    public WorkflowStepInfo CurrentStepInfo
    {
        get
        {
            if (mCurrentStepInfo == null)
            {
                mCurrentStepInfo = WorkflowStepInfoProvider.GetWorkflowStepInfo(WorkflowStepID);
            }
            return mCurrentStepInfo;
        }
    }


    /// <summary>
    /// Current workflow step representation in graph
    /// </summary>
    public WorkflowNode CurrentNode
    {
        get
        {
            if (mCurrentNode == null)
            {
                mCurrentNode = WorkflowNode.GetInstance(CurrentStepInfo);
            }
            return mCurrentNode;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            // Do nothing!
        }
        else
        {
            ShowInformation(GetString(CurrentNode.TypeResourceStringPrefix + "Tooltip"));
            gridElem.IsLiveSite = IsLiveSite;

            gridElem.OnDataReload += gridElem_OnDataReload;
            gridElem.OnAction += gridElem_OnAction;
            gridElem.OnBeforeDataReload += gridElem_OnBeforeDataReload;
            gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;
        }
    }


    #region "Grid event handlers"

    protected void gridElem_OnBeforeDataReload()
    {
        // Reset condition listing modifiers
        conditionPosition = 0;
    }


    protected DataSet gridElem_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        DataSet result = new DataSet();

        // Return all step's source points except for Timeout source point
        result.Tables.Add(DataHelper.ConvertToDataTable(CurrentStepInfo.StepDefinition.SourcePoints.Where(s => s.Type != SourcePointTypeEnum.Timeout)));
        return result;
    }


    protected void gridElem_OnAction(string actionName, object actionArgument)
    {
        Guid sourcePointGuid = ValidationHelper.GetGuid(actionArgument, Guid.Empty);
        if (sourcePointGuid != Guid.Empty)
        {
            string graphName = QueryHelper.GetString("graph", String.Empty);
            
            switch (actionName.ToLowerCSafe())
            {
                case "edit":
                    string url = URLHelper.AddParameterToUrl(UIContextHelper.GetElementUrl("CMS", "Workflows.EditCase", false), "workflowStepId", WorkflowStepID.ToString());
                    url = URLHelper.AddParameterToUrl(url, "isindialog", QueryHelper.GetBoolean("isindialog", false).ToString());
                    url = URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash(url));
                    url = URLHelper.AddParameterToUrl(url, "sourcepointGuid", sourcePointGuid.ToString());
                    url = URLHelper.AddParameterToUrl(url, "graph", graphName);
                    URLHelper.Redirect(url);
                    break;

                case "delete":
                    WorkflowStepInfo step = WorkflowStepInfoProvider.GetWorkflowStepInfo(WorkflowStepID);
                    if (step == null)
                    {
                        ShowError(GetString("graphservice.missingnode"));
                    }
                    string result = step.RemoveSourcePoint(sourcePointGuid);
                    if (!String.IsNullOrEmpty(result))
                    {
                        ShowError(result);
                    }
                    else
                    {
                        WorkflowScriptHelper.RefreshDesignerFromDialog(Page, CurrentStepInfo.StepID, graphName);
                    }
                    break;

                case "moveup":
                    CurrentStepInfo.MoveSourcePointUp(sourcePointGuid);
                    WorkflowScriptHelper.RefreshDesignerFromDialog(Page, CurrentStepInfo.StepID, graphName);;
                    break;

                case "movedown":
                    CurrentStepInfo.MoveSourcePointDown(sourcePointGuid);
                    WorkflowScriptHelper.RefreshDesignerFromDialog(Page, CurrentStepInfo.StepID, graphName);
                    break;
            }
        }
    }


    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        SourcePointTypeEnum sourcePointType = SourcePointTypeEnum.Standard;
        GridViewRow container = null;

        switch (sourceName.ToLowerCSafe())
        {
            case "label":
                return ResHelper.LocalizeString(HTMLHelper.HTMLEncode(ValidationHelper.GetString(parameter, String.Empty)));

            case "condition":
                string condition = MacroRuleTree.GetRuleText(ValidationHelper.GetString(parameter, String.Empty));
                return GetDecoratedCondition(condition);

            case "allowaction":
                // Default case can't be moved or deleted. Same goes for condition step type case
                container = (GridViewRow)parameter;
                sourcePointType = (SourcePointTypeEnum)ValidationHelper.GetInteger(DataHelper.GetDataRowViewValue((DataRowView)container.DataItem, "Type"), 0);
                if ((sourcePointType != SourcePointTypeEnum.SwitchCase) || (CurrentStepInfo.StepType == WorkflowStepTypeEnum.Condition))
                {
                    ((Control)sender).Visible = false;
                }
                break;
        }
        return parameter;
    }

    #endregion


    #region "Private methods"
            
    /// <summary>
    /// Returns label decorated with "if", "else if" or "else" depending on the position in sequence.
    /// </summary>
    /// <param name="label">Condition label</param>
    private string GetDecoratedCondition(string label)
    {
        conditionPosition++;
        label = label.Trim();

        // User choice has "If" all the way down
        if (CurrentStepInfo.StepType == WorkflowStepTypeEnum.Userchoice)
        {
            return String.IsNullOrEmpty(label) ? String.Empty : String.Format("<em>{0}</em><div>{1}</div>", GetString("workflowstep.sourcepoint.incase"), label);
        }

        // Other step types follow the "If - Else if - Else" pattern
        if (String.IsNullOrEmpty(label))
        {
            // "Else" is last and has no condition
            if (conditionPosition == (CurrentStepInfo.StepDefinition.SourcePoints.Count))
            {
                return String.Format("<em>{0}</em>", GetString("workflowstep.sourcepoint.else"));
            }
            else
            {
                return String.Empty;
            }
        }
        else
        {
            // "If" is first filled condition
            if ((CurrentStepInfo.StepType == WorkflowStepTypeEnum.Condition) && (conditionPosition == 1))
            {
                return String.Format("<em>{0}</em><div>{1}</div>", GetString("workflowstep.sourcepoint.if"), label);

            }
            else
            {
                return String.Format("<em>{0}</em><div>{1}</div>", GetString("workflowstep.sourcepoint.incase"), label);
            }
        }

    }

    #endregion
}