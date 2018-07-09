using System;
using System.Data;

using CMS.Activities;
using CMS.Automation;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.ContactManagement;
using CMS.ContactManagement.Web.UI;
using CMS.Core;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.UIControls;
using CMS.WorkflowEngine;

// Parent object
[EditedObject(WorkflowInfo.OBJECT_TYPE_AUTOMATION, "processId")]
[Security(Resource = ModuleName.ONLINEMARKETING, UIElements = "EditProcess;EditProcessTriggers")]
public partial class CMSModules_ContactManagement_Pages_Tools_Automation_Process_Trigger_List : CMSAutomationPage
{
    #region "Page events"
    
    protected void Page_Load(object sender, EventArgs e)
    {
        int processId = QueryHelper.GetInteger("processId", 0);

        gridElem.WhereCondition = "TriggerWorkflowID = " + processId;

        // Add query parameters for breadcrumb to edit link
        gridElem.EditActionUrl = gridElem.EditActionUrl + "&processId=" + processId;

        gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;

        // Add new action
        headerActions.AddAction(new HeaderAction()
        {
            Text = GetString("ma.trigger.new"),
            RedirectUrl = "Edit.aspx?processId=" + processId
        });
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Check permissions to create new record
        if (!WorkflowStepInfoProvider.CanUserManageAutomationProcesses(CurrentUser, CurrentSiteName))
        {
            headerActions.Enabled = false;
        }
    }


    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "condition":
                return MacroRuleTree.GetRuleText(ValidationHelper.GetString(parameter, String.Empty));

            case "type":
                DataRowView row = parameter as DataRowView;
                if (row != null)
                {
                    ObjectWorkflowTriggerInfo trigger = new ObjectWorkflowTriggerInfo(row.Row);
                    if (!string.IsNullOrEmpty(trigger.TriggerTargetObjectType))
                    {
                        return GetTriggerDescription(trigger);
                    }
                    else
                    {
                        return AutomationHelper.GetTriggerName(trigger.TriggerType, trigger.TriggerObjectType);
                    }
                }
                return parameter;

            case "delete":
                if (!WorkflowStepInfoProvider.CanUserManageAutomationProcesses(CurrentUser, CurrentSiteName))
                {
                    CMSGridActionButton btn = (CMSGridActionButton)sender;
                    btn.Enabled = false;
                }
                return parameter;

            default:
                return parameter;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Returns transformation for trigger type.
    /// </summary>
    /// <param name="trigger">Trigger info object</param>
    private object GetTriggerDescription(ObjectWorkflowTriggerInfo trigger)
    {
        var objectTransformation = new ObjectTransformation(trigger.TriggerTargetObjectType, trigger.TriggerTargetObjectID);
        switch (trigger.TriggerObjectType)
        {
            case ScoreInfo.OBJECT_TYPE:
                objectTransformation.Transformation = string.Format(GetString("ma.trigger.scorereached.listing"), "{% DisplayName %}", trigger.TriggerParameters["ScoreValue"]);
                return objectTransformation;

            case ActivityInfo.OBJECT_TYPE:
                if (trigger.TriggerTargetObjectID == 0)
                {
                    return GetString("ma.trigger.anyActivityPerformed");
                }
                objectTransformation.Transformation = string.Format("{0} '{{% DisplayName %}}'", GetString("ma.trigger.performed"));
                return objectTransformation;
        }
        return null;
    }

    #endregion
}
