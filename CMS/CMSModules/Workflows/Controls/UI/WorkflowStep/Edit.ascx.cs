using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.UIControls;
using CMS.WorkflowEngine;
using CMS.WorkflowEngine.Definitions;
using CMS.WorkflowEngine.Web.UI;

public partial class CMSModules_Workflows_Controls_UI_WorkflowStep_Edit : CMSAdminEditControl
{
    #region "Private variables"

    private bool? mShowTimeout;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets current workflow step info.
    /// </summary>
    private WorkflowStepInfo CurrentStepInfo
    {
        get
        {
            return (WorkflowStepInfo)editForm.EditedObject;
        }
    }


    /// <summary>
    /// Gets current workflow object.
    /// </summary>
    private WorkflowInfo CurrentWorkflow
    {
        get
        {
            return (WorkflowInfo)editForm.ParentObject;
        }
    }


    /// <summary>
    /// Indicates if timeout settings should be visible.
    /// </summary>
    private bool ShowTimeout
    {
        get
        {
            if (mShowTimeout == null)
            {
                // All steps except 
                mShowTimeout = CurrentStepInfo.StepAllowTimeout && (CurrentWorkflow != null) && !CurrentWorkflow.IsBasic;
            }
            return mShowTimeout.Value;
        }
    }


    /// <summary>
    /// UIForm used to edit workflow step
    /// </summary>
    public UIForm EditForm
    {
        get
        {
            return editForm;
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            // Do nothing!
        }
        else
        {
            editForm.OnAfterValidate += editForm_OnAfterValidate;
            editForm.OnBeforeSave += editForm_OnBeforeSave;
            editForm.OnAfterSave += editForm_OnAfterSave;

            pnlTimeout.Visible = ShowTimeout;
            if (CurrentStepInfo != null)
            {
                LoadData(CurrentStepInfo);
            }
        }
    }

   
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (CurrentStepInfo != null)
        {
            // Display timeout target source point selector
            plcTimeoutTarget.Visible = ucTimeout.TimeoutEnabled && ucTimeoutTarget.IsVisible();
        }
    }


    /// <summary>
    /// Loads data of edited workflow from DB into TextBoxes.
    /// </summary>
    protected void LoadData(WorkflowStepInfo wsi)
    {
        // Timeout UI is always enabled for wait step type
        ucTimeout.AllowNoTimeout = (wsi.StepType != WorkflowStepTypeEnum.Wait);

        // Display action parameters form only for action step type
        if (wsi.StepIsAction)
        {
            WorkflowActionInfo action = WorkflowActionInfoProvider.GetWorkflowActionInfo(wsi.StepActionID);
            if (action != null)
            {
                if (!RequestHelper.IsPostBack())
                {
                    pnlContainer.CssClass += " " + action.ActionName.ToLowerCSafe();
                }
                ucActionParameters.FormInfo = new FormInfo(action.ActionParameters);
                lblParameters.Text = String.Format(GetString("workflowstep.parameters"), HTMLHelper.HTMLEncode(ResHelper.LocalizeString(action.ActionDisplayName)));
            }

            ucActionParameters.BasicForm.AllowMacroEditing = true;
            ucActionParameters.BasicForm.ShowValidationErrorMessage = false;
            ucActionParameters.BasicForm.ResolverName = WorkflowHelper.GetResolverName(CurrentWorkflow);
            ucActionParameters.Parameters = wsi.StepActionParameters;
            ucActionParameters.ReloadData(!RequestHelper.IsPostBack());
            ucActionParameters.Visible = ucActionParameters.CheckVisibility();

        }

        plcParameters.Visible = ucActionParameters.Visible;

        if (plcTimeoutTarget.Visible)
        {
            ucTimeoutTarget.WorkflowStepID = CurrentStepInfo.StepID;
        }

        // Initialize condition edit for certain step types
        ucSourcePointEdit.StopProcessing = true;

        if ((CurrentWorkflow != null) && !CurrentWorkflow.IsBasic)
        {
            bool conditionStep = (wsi.StepType == WorkflowStepTypeEnum.Condition);
            if (conditionStep || (wsi.StepType == WorkflowStepTypeEnum.Wait) || (!wsi.StepIsStart && !wsi.StepIsAction && !wsi.StepIsFinished && (wsi.StepType != WorkflowStepTypeEnum.MultichoiceFirstWin)))
            {
                // Initialize source point edit control
                var sourcePoint = CurrentStepInfo.StepDefinition.DefinitionPoint;
                if (sourcePoint != null)
                {
                    plcCondition.Visible = true;
                    lblCondition.ResourceString = conditionStep ? "workflowstep.conditionsettings" : "workflowstep.advancedsettings";

                    ucSourcePointEdit.StopProcessing = false;
                    ucSourcePointEdit.SourcePointGuid = sourcePoint.Guid;
                    ucSourcePointEdit.SimpleMode = !conditionStep;
                    ucSourcePointEdit.ShowCondition = (wsi.StepType != WorkflowStepTypeEnum.Userchoice) && (wsi.StepType != WorkflowStepTypeEnum.Multichoice) && (wsi.StepType != WorkflowStepTypeEnum.MultichoiceFirstWin);
                    ucSourcePointEdit.RuleCategoryNames = CurrentWorkflow.IsAutomation ? ModuleName.ONLINEMARKETING : WorkflowInfo.OBJECT_TYPE;
                }
            }
        }

        if (!RequestHelper.IsPostBack())
        {
            if (ShowTimeout)
            {
                ucTimeout.TimeoutEnabled = wsi.StepDefinition.TimeoutEnabled;
                ucTimeout.ScheduleInterval = wsi.StepDefinition.TimeoutInterval;
            }
        }
    }


    protected void editForm_OnBeforeSave(object sender, EventArgs e)
    {
        if (editForm.Mode == FormModeEnum.Update)
        {
            ucSourcePointEdit.SaveData(false);
            ucActionParameters.SaveData(false);
            SetFormValues(CurrentStepInfo);
        }
        else
        {
            SetFormValues(CurrentStepInfo);
            EnsureStepsOrder();
        }
    }


    protected void editForm_OnAfterValidate(object sender, EventArgs e)
    {
        editForm.StopProcessing = !ValidateData();
    }


    protected void editForm_OnAfterSave(object sender, EventArgs e)
    {
        // Refresh updated node
        WorkflowScriptHelper.RefreshDesignerFromDialog(Page, CurrentStepInfo.StepID, QueryHelper.GetString("graph", String.Empty));
    }


    /// <summary>
    /// Ensures correct steps order
    /// </summary>
    private void EnsureStepsOrder()
    {
        // Ensure correct order for basic workflow
        if ((CurrentWorkflow != null) && CurrentWorkflow.IsBasic)
        {
            // Get published step info for the proper position
            WorkflowStepInfo psi = WorkflowStepInfoProvider.GetPublishedStep(CurrentWorkflow.WorkflowID);
            if (psi != null)
            {
                CurrentStepInfo.StepOrder = psi.StepOrder;
                // Move the published step down
                psi.StepOrder += 1;
                WorkflowStepInfoProvider.SetWorkflowStepInfo(psi);

                // Move the archived step down
                WorkflowStepInfo asi = WorkflowStepInfoProvider.GetArchivedStep(CurrentWorkflow.WorkflowID);
                if (asi != null)
                {
                    asi.StepOrder += 1;
                    WorkflowStepInfoProvider.SetWorkflowStepInfo(asi);
                }
            }
        }
    }


    /// <summary>
    /// Validates the data, returns true if succeeded.
    /// </summary>
    public bool ValidateData()
    {
        // Validate source point control
        if (!ucSourcePointEdit.ValidateData())
        {
            return false;
        }

        // Validate action properties control
        if (CurrentStepInfo.StepIsAction && !ucActionParameters.ValidateData())
        {
            return false;
        }

        return !ucTimeout.Visible || !String.IsNullOrEmpty(ucTimeout.ScheduleInterval) || !ucTimeout.TimeoutEnabled;
    }


    /// <summary>
    /// Sets values from edit form to edited workflows step info
    /// </summary>
    /// <param name="wsi">Edited workflow step info</param>
    private void SetFormValues(WorkflowStepInfo wsi)
    {
        if (wsi == null)
        {
            return;
        }

        if (ShowTimeout)
        {
            Step definition = wsi.StepDefinition;
            definition.TimeoutEnabled = ucTimeout.TimeoutEnabled;
            definition.TimeoutInterval = ucTimeout.ScheduleInterval;
            if (ucTimeoutTarget.Visible)
            {
                Guid timeouTarget = ucTimeoutTarget.SourcePointGuid;
                // Add timeout source point
                if (wsi.StepAllowDefaultTimeoutTarget && !definition.SourcePoints.Exists(s => (s is TimeoutSourcePoint)))
                {
                    TimeoutSourcePoint timeout = new TimeoutSourcePoint();
                    // Timeout source point is selected
                    if (!definition.SourcePoints.Exists(s => (s.Guid == timeouTarget)))
                    {
                        timeout.Guid = timeouTarget;
                    }

                    definition.SourcePoints.Add(timeout);
                }
                definition.TimeoutTarget = timeouTarget;
            }
            else
            {
                // Remove timeout source point
                var timeoutPoints = definition.SourcePoints.FindAll(s => (s is TimeoutSourcePoint));
                foreach (var t in timeoutPoints)
                {
                    string result = wsi.RemoveSourcePoint(t.Guid);
                    if (result != null)
                    {
                        ShowError(result);
                    }
                }
            }
        }

        if (wsi.StepIsAction)
        {
            wsi.StepActionParameters.LoadData(ucActionParameters.Parameters.GetData());
        }

        if (plcCondition.Visible)
        {
            CurrentStepInfo.StepDefinition.DefinitionPoint.Text = ucSourcePointEdit.CurrentSourcePoint.Text;
            CurrentStepInfo.StepDefinition.DefinitionPoint.Tooltip = ucSourcePointEdit.CurrentSourcePoint.Tooltip;
            CurrentStepInfo.StepDefinition.DefinitionPoint.Condition = ucSourcePointEdit.CurrentSourcePoint.Condition;
            CurrentStepInfo.StepDefinition.DefinitionPoint.Label = ucSourcePointEdit.CurrentSourcePoint.Label;
        }
    }

    #endregion
}