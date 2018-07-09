using System;
using System.Web.UI.WebControls;

using CMS.Activities;
using CMS.Automation;
using CMS.ContactManagement;
using CMS.ContactManagement.Web.UI;
using CMS.Helpers;
using CMS.UIControls;
using CMS.WorkflowEngine;

// Edited object
[EditedObject(ObjectWorkflowTriggerInfo.OBJECT_TYPE, "objectworkflowtriggerId")]

// Breadcrumbs
[Breadcrumbs]
[Breadcrumb(0, "ma.trigger.list", "~/CMSModules/ContactManagement/Pages/Tools/Automation/Process/Trigger/List.aspx?processId={?processId?}", null)]
[Breadcrumb(1, Text = "{%EditedObject.TriggerDisplayName%}", ExistingObject = true)]
[Breadcrumb(1, ResourceString = "ma.trigger.new", NewObject = true)]

[Help("ma_trigger_edit")]
public partial class CMSModules_ContactManagement_Pages_Tools_Automation_Process_Trigger_Edit : CMSAutomationPage
{
    #region "Variables"

    private WorkflowInfo mProcess;

    #endregion


    #region "Properties"

    /// <summary>
    /// 
    /// </summary>
    public ObjectWorkflowTriggerInfo Trigger
    {
        get
        {
            return (ObjectWorkflowTriggerInfo)editForm.EditedObject;
        }
    }


    /// <summary>
    /// Current process.
    /// </summary>
    public WorkflowInfo Process
    {
        get
        {
            if (mProcess == null)
            {
                mProcess = WorkflowInfoProvider.GetWorkflowInfo(QueryHelper.GetInteger("processid", 0));
            }
            return mProcess;
        }
    }


    /// <summary>
    /// Whether current setting might cause e-mail floods.
    /// </summary>
    protected bool IsDangerousRecurrence
    {
        get
        {
            if(Process.WorkflowRecurrenceType != ProcessRecurrenceTypeEnum.Recurring)
            {
                return false;
            }

            // Contact changed and activity created are dangerous triggers. Score changed trigger fires only once, when contact exceeds given score.
            bool contactChangedTrigger = (Trigger.TriggerObjectType == ContactInfo.OBJECT_TYPE) && (Trigger.TriggerType == WorkflowTriggerTypeEnum.Change);
            bool activityTrigger = (Trigger.TriggerObjectType == ActivityInfo.OBJECT_TYPE);
            return (contactChangedTrigger || activityTrigger);
        }
    }


    /// <summary>
    /// Selected type value derived from drop-down list of trigger types.
    /// </summary>
    protected WorkflowTriggerTypeEnum SelectedTriggerType
    {
        get
        {
            int value = ValidationHelper.GetInteger(ddlType.SelectedValue.Split('|')[0], 0);
            return (WorkflowTriggerTypeEnum)Enum.Parse(typeof(WorkflowTriggerTypeEnum), value.ToString());
        }
    }


    /// <summary>
    /// Selected object type value derived from drop-down list of trigger types.
    /// </summary>
    protected string SelectedObjectType
    {
        get
        {
            return ddlType.SelectedValue.Split('|')[1];
        }
    }

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        editForm.OnAfterDataLoad += editForm_OnAfterDataLoad;
        editForm.OnBeforeSave += EditForm_OnBeforeSave;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Process == null)
        {
            RedirectToInformation("editedobject.notexists");
        }

        // Add site and workflow query parameters to redirect URL
        editForm.RedirectUrlAfterCreate = editForm.RedirectUrlAfterCreate + "&processid=" + Process.WorkflowID;

        // Activity selector settings
        ucActivityType.UniSelector.ReturnColumnName = "ActivityTypeID";

        // IScore type selector settings
        ucScoreType.SetValue("AllowAll", false);

        SetResolverName();

        // Hide context macros as they are not working in trigger conditions
        tMacroCondition.EditingControl.SetValue("DisplayRuleType", 1);
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (IsDangerousRecurrence && string.IsNullOrEmpty(MessagesPlaceHolder.ErrorText))
        {
            ShowWarning(GetString("ma.dangerousRecurrence.message"), GetString("ma.dangerousRecurrence.description"), null);
        }
        if (!RequestHelper.IsPostBack() && (Trigger.TriggerID != 0))
        {
            LoadCustomFormValues();
        }
        VisibilitySettings();
    }


    /// <summary>
    /// After data load event handler.
    /// </summary>
    protected void editForm_OnAfterDataLoad(object sender, EventArgs e)
    {
        // Initialize new trigger
        if (Trigger.TriggerID == 0)
        {
            Trigger.TriggerWorkflowID = Process.WorkflowID;
        }

        FillDdlType();
    }


    /// <summary>
    /// Before save event handler.
    /// </summary>
    protected void EditForm_OnBeforeSave(object sender, EventArgs e)
    {
        // Save custom values.
        Trigger.TriggerType = SelectedTriggerType;
        Trigger.TriggerObjectType = SelectedObjectType;
        if (SelectedObjectType == ActivityInfo.OBJECT_TYPE)
        {
            Trigger.TriggerTargetObjectType = ActivityTypeInfo.OBJECT_TYPE;
            Trigger.TriggerTargetObjectID = ValidationHelper.GetInteger(ucActivityType.Value, 0);
            Trigger.TriggerParameters = null;
        }
        else if (SelectedObjectType == ScoreInfo.OBJECT_TYPE)
        {
            Trigger.TriggerTargetObjectType = ScoreInfo.OBJECT_TYPE;
            Trigger.TriggerTargetObjectID = GetValidScoreType();
            Trigger.TriggerParameters = GetValidScoreParameters();
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Sets context resolver name for a selected object type.
    /// </summary>
    private void SetResolverName()
    {
        switch (SelectedObjectType)
        {
            case ActivityInfo.OBJECT_TYPE:
                tMacroCondition.EditingControl.ResolverName = "contactactivityresolver";
                break;

            case ScoreInfo.OBJECT_TYPE:
                tMacroCondition.EditingControl.ResolverName = "contactscoreresolver";
                break;

            default:
                tMacroCondition.EditingControl.ResolverName = "contactresolver";
                break;
        }
    }


    /// <summary>
    /// Validates score and returns selected type of score.
    /// </summary>
    private int GetValidScoreType()
    {
        int score = ValidationHelper.GetInteger(ucScoreType.Value, 0);
        if ((score == 0) && !editForm.StopProcessing)
        {
            ShowError(GetString("ma.trigger.noScore"));
            editForm.StopProcessing = true;
        }
        return score;
    }


    /// <summary>
    /// Validates score parameters and returns their string representation.
    /// </summary>
    private ObjectParameters GetValidScoreParameters()
    {
        if (!ucScoreProperties.HasValue && !editForm.StopProcessing)
        {
            ShowError(GetString("ma.trigger.invalidScoreParam"));
            editForm.StopProcessing = true;
        }
        return ucScoreProperties.Value as ObjectParameters;
    }


    /// <summary>
    /// Loads custom form values to form.
    /// </summary>
    private void LoadCustomFormValues()
    {
        if (Trigger.TriggerObjectType == ActivityInfo.OBJECT_TYPE)
        {
            ucActivityType.Value = Trigger.TriggerTargetObjectID;
        }
        else if (Trigger.TriggerObjectType == ScoreInfo.OBJECT_TYPE)
        {
            ucScoreType.Value = Trigger.TriggerTargetObjectID;
            ucScoreProperties.Value = Trigger.TriggerParameters;
        }
        ddlType.SelectedValue = GetDdlTypeItem(Trigger.TriggerType, Trigger.TriggerObjectType).Value;
    }


    /// <summary>
    /// Sets visibility of components.
    /// </summary>
    private void VisibilitySettings()
    {
        pnlActivityType.Visible = (SelectedObjectType == ActivityInfo.OBJECT_TYPE);
        pnlScoreProperties.Visible = (SelectedObjectType == ScoreInfo.OBJECT_TYPE);
    }


    /// <summary>
    /// Fills drop-down list with items.
    /// </summary>
    private void FillDdlType()
    {
        AddDdlTypeListItem(WorkflowTriggerTypeEnum.Creation, ContactInfo.OBJECT_TYPE);
        AddDdlTypeListItem(WorkflowTriggerTypeEnum.Change, ContactInfo.OBJECT_TYPE);
        
        AddDdlTypeListItem(WorkflowTriggerTypeEnum.Creation, ActivityInfo.OBJECT_TYPE, GetString("ma.trigger.performed"));
        AddDdlTypeListItem(WorkflowTriggerTypeEnum.Change, ScoreInfo.OBJECT_TYPE, GetString("ma.trigger.scorereached.general"));
    }


    /// <summary>
    /// Returns item from drop-down list of trigger types.
    /// </summary>
    /// <param name="type">Type of trigger</param>
    /// <param name="objectType">Object trigger</param>
    private ListItem GetDdlTypeItem(WorkflowTriggerTypeEnum type, string objectType)
    {
        string formatedValue = GetDdlTypeFormatedValue(type, objectType);
        return ddlType.Items.FindByValue(formatedValue);
    }


    /// <summary>
    /// Returns value in same format as in drop-down list of trigger types. 
    /// </summary>
    /// <param name="type">Type of trigger</param>
    /// <param name="objectType">Object type</param>
    private string GetDdlTypeFormatedValue(WorkflowTriggerTypeEnum type, string objectType)
    {
        return string.Format("{0}|{1}", ((int)type).ToString(), objectType);
    }


    /// <summary>
    /// Adds item to drop-down list of trigger types.
    /// </summary>
    /// <param name="type">Type of trigger</param>
    /// <param name="objectType">Object type</param>
    private void AddDdlTypeListItem(WorkflowTriggerTypeEnum type, string objectType)
    {
        string text = AutomationHelper.GetTriggerName(type, objectType);
        string value = GetDdlTypeFormatedValue(type, objectType);
        ddlType.Items.Add(new ListItem(text, value));
    }


    /// <summary>
    /// Adds item to drop-down list of trigger types.
    /// </summary>
    /// <param name="type">Type of trigger</param>
    /// <param name="objectType">Object type</param>
    /// <param name="text">Text of the item</param>
    private void AddDdlTypeListItem(WorkflowTriggerTypeEnum type, string objectType, string text)
    {
        string value = GetDdlTypeFormatedValue(type, objectType);
        ddlType.Items.Add(new ListItem(text, value));
    }

    #endregion
}
