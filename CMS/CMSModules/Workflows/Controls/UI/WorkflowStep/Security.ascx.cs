using System;
using System.Linq;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;
using CMS.WorkflowEngine;
using CMS.WorkflowEngine.Definitions;


public partial class CMSModules_Workflows_Controls_UI_WorkflowStep_Security : CMSAdminEditControl
{
    #region "Variables"

    private int siteId;
    private WorkflowStepInfo mWorkflowStep;
    private WorkflowInfo mWorkflow;
    private SourcePoint mCurrentSourcePoint;
    protected string currentRoles = string.Empty;
    protected string currentUsers = string.Empty;

    #endregion


    #region "Properties"

    /// <summary>
    /// Workflow step ID
    /// </summary>
    public int WorkflowStepID
    {
        get;
        set;
    }


    /// <summary>
    /// With source point GUID set, this control manages security for source point. With null manages security for workflow step.
    /// </summary>
    public Guid? SourcePointGuid
    {
        get;
        set;
    }


    /// <summary>
    /// Site ID
    /// </summary>
    public int SiteID
    {
        get;
        set;
    }


    /// <summary>
    /// Workflow step
    /// </summary>
    public WorkflowStepInfo WorkflowStep => mWorkflowStep ?? (mWorkflowStep = WorkflowStepInfoProvider.GetWorkflowStepInfo(WorkflowStepID));


    /// <summary>
    /// Workflow
    /// </summary>
    private WorkflowInfo Workflow
    {
        get
        {
            if (mWorkflow == null && WorkflowStep != null)
            {
                mWorkflow = WorkflowInfoProvider.GetWorkflowInfo(WorkflowStep.StepWorkflowID);
            }

            return mWorkflow;
        }
    }


    /// <summary>
    /// Source point on workflow step
    /// </summary>
    public SourcePoint CurrentSourcePoint
    {
        get
        {
            if ((mCurrentSourcePoint == null) && (SourcePointGuid != null))
            {
                mCurrentSourcePoint = WorkflowStep.StepDefinition.SourcePoints.FirstOrDefault(i => i.Guid == SourcePointGuid);
            }
            return mCurrentSourcePoint;
        }
    }


    /// <summary>
    /// Indicates if the control should perform the operations.
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
            siteSelector.StopProcessing = value;
            usRoles.StopProcessing = value;
            usUsers.StopProcessing = value;
        }
    }


    /// <summary>
    /// Indicates if the control is used on the live site.
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
            siteSelector.IsLiveSite = value;
            usRoles.IsLiveSite = value;
            usUsers.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Roles security of workflow step or its source point
    /// </summary>
    private WorkflowStepSecurityEnum RolesSecurity
    {
        get
        {
            if (SourcePointGuid == null)
            {
                return WorkflowStep.StepRolesSecurity;
            }

            return CurrentSourcePoint?.StepRolesSecurity ?? WorkflowStepSecurityEnum.Default;
        }
        set
        {
            if (SourcePointGuid == null)
            {
                WorkflowStep.StepRolesSecurity = value;
            }
            else
            {
                if (CurrentSourcePoint != null)
                {
                    CurrentSourcePoint.StepRolesSecurity = value;
                }
            }
        }
    }


    /// <summary>
    /// Users security of workflow step or its source point
    /// </summary>
    private WorkflowStepSecurityEnum UsersSecurity
    {
        get
        {
            if (SourcePointGuid == null)
            {
                return WorkflowStep.StepUsersSecurity;
            }

            return CurrentSourcePoint?.StepUsersSecurity ?? WorkflowStepSecurityEnum.Default;
        }
        set
        {
            if (SourcePointGuid == null)
            {
                WorkflowStep.StepUsersSecurity = value;
            }
            else
            {
                if (CurrentSourcePoint != null)
                {
                    CurrentSourcePoint.StepUsersSecurity = value;
                }
            }
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (WorkflowStepID <= 0)
        {
            StopProcessing = true;
            return;
        }

        EditedObject = WorkflowStep;

        if (SourcePointGuid != null)
        {
            ShowInformation(GetString("workflowsteppoint.securityInfo"));
        }
        else if (WorkflowStep.StepAllowBranch)
        {
            ShowInformation(GetString("workflowstep.securityInfo"));
        }

        if (Workflow.IsAutomation)
        {
            headRoles.ResourceString = "processstep.rolessecurity";
            headUsers.ResourceString = "processstep.userssecurity";
        }
        else
        {
            headRoles.ResourceString = (SourcePointGuid == null) ? "workflowstep.rolessecurity" : "workflowsteppoint.rolessecurity";
            headUsers.ResourceString = (SourcePointGuid == null) ? "workflowstep.userssecurity" : "workflowsteppoint.userssecurity";
        }

        // Set site selector
        siteSelector.AllowGlobal = true;
        siteSelector.DropDownSingleSelect.AutoPostBack = true;
        siteSelector.AllowAll = false;
        siteSelector.OnlyRunningSites = false;
        siteSelector.UniSelector.OnSelectionChanged += UniSelector_OnSelectionChanged;
        usRoles.OnSelectionChanged += usRoles_OnSelectionChanged;
        usUsers.OnSelectionChanged += usUsers_OnSelectionChanged;
        usRoles.ObjectType = RoleInfo.OBJECT_TYPE;
        usUsers.ObjectType = UserInfo.OBJECT_TYPE;
        rbRoleType.SelectedIndexChanged += rbRoleType_SelectedIndexChanged;
        rbUserType.SelectedIndexChanged += rbUserType_SelectedIndexChanged;

        if (!RequestHelper.IsPostBack())
        {
            siteId = SiteID;
            siteSelector.Value = siteId;
            string resPrefix = (SourcePointGuid == null) ? "workflowstep" : "workflowsteppoint";
            ControlsHelper.FillListControlWithEnum<WorkflowStepSecurityEnum>(rbRoleType, resPrefix + ".security");
            ControlsHelper.FillListControlWithEnum<WorkflowStepSecurityEnum>(rbUserType, resPrefix + ".usersecurity");
            rbRoleType.SelectedValue = ((int)RolesSecurity).ToString();
            rbUserType.SelectedValue = ((int)UsersSecurity).ToString();
        }
        else
        {
            // Make sure the current site is always selected
            int selectedId = ValidationHelper.GetInteger(siteSelector.Value, 0);
            if (selectedId == 0)
            {
                selectedId = SiteID;
                siteSelector.Value = SiteID;
            }
            siteId = selectedId;
        }

        // If global role selected - set siteID to 0
        if (siteSelector.GlobalRecordValue == siteId.ToString())
        {
            siteId = 0;
        }

        string siteIDWhere = (siteId == 0) ? "SiteID IS NULL" : "SiteID = " + siteId;
        usRoles.WhereCondition = siteIDWhere + " AND RoleGroupID IS NULL";
        usUsers.WhereCondition = "(UserIsHidden = 0 OR UserIsHidden IS NULL)";

        // Get the active roles for this site
        string where = $"[StepID] = {WorkflowStepID} AND [StepSourcePointGuid] {(SourcePointGuid == null ? "IS NULL" : $"= '{SourcePointGuid}'")}";

        var roles = WorkflowStepRoleInfoProvider.GetWorkflowStepRoles()
            .Where(where)
            .Column("RoleID")
            .GetListResult<int>();
        currentRoles = string.Join(";", roles.ToArray());

        // Get the active users for this site
        var users = WorkflowStepUserInfoProvider.GetWorkflowStepUsers()
            .Where(where)
            .Column("UserID")
            .GetListResult<int>();
        currentUsers = string.Join(";", users.ToArray());

        // Init lists when security type changes
        if (!RequestHelper.IsPostBack() || ControlsHelper.GetPostBackControlID(Page).StartsWith(rbRoleType.UniqueID, StringComparison.Ordinal) || ControlsHelper.GetPostBackControlID(Page).StartsWith(rbUserType.UniqueID, StringComparison.Ordinal))
        {
            usRoles.Value = currentRoles;
            usUsers.Value = currentUsers;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (WorkflowStepID > 0)
        {
            usRoles.Visible = RolesSecurity != WorkflowStepSecurityEnum.Default;
            plcRolesBox.Visible = usRoles.Visible;
            usUsers.Visible = UsersSecurity != WorkflowStepSecurityEnum.Default;
        }
    }


    protected void rbRoleType_SelectedIndexChanged(object sender, EventArgs e)
    {
        RolesSecurity = (WorkflowStepSecurityEnum)ValidationHelper.GetInteger(rbRoleType.SelectedValue, 0);
        WorkflowStep.Update();
        ShowChangesSaved();
    }


    protected void rbUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        UsersSecurity = (WorkflowStepSecurityEnum)ValidationHelper.GetInteger(rbUserType.SelectedValue, 0);
        WorkflowStep.Update();
        ShowChangesSaved();
    }


    protected void usRoles_OnSelectionChanged(object sender, EventArgs e)
    {
        SaveRolesData();
        pnlUpdateRoles.Update();
    }


    protected void usUsers_OnSelectionChanged(object sender, EventArgs e)
    {

        SaveUsersData();
        pnlUpdateUsers.Update();
    }

    #endregion


    #region "Control handling"

    /// <summary>
    /// Handles site selection change event.
    /// </summary>
    protected void UniSelector_OnSelectionChanged(object sender, EventArgs e)
    {
        pnlUpdateRoles.Update();
    }


    /// <summary>
    /// Saves roles data
    /// </summary>
    private void SaveRolesData()
    {
        Guid stepSourcePointGuid = SourcePointGuid ?? Guid.Empty;

        // Remove old items
        string newValues = ValidationHelper.GetString(usRoles.Value, null);
        string items = DataHelper.GetNewItemsInList(newValues, currentRoles);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Add all new items to site
            foreach (string item in newItems)
            {
                int roleId = ValidationHelper.GetInteger(item, 0);
                // If role is authorized, remove it
                WorkflowStepRoleInfo wsr = WorkflowStepRoleInfoProvider.GetWorkflowStepRoleInfo(WorkflowStepID, roleId, stepSourcePointGuid);
                wsr?.Delete();
            }
        }

        // Add new items
        items = DataHelper.GetNewItemsInList(currentRoles, newValues);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Add all new items to site
            foreach (string item in newItems)
            {
                int roleId = ValidationHelper.GetInteger(item, 0);

                // If role is not authorized, authorize it
                if (WorkflowStepRoleInfoProvider.GetWorkflowStepRoleInfo(WorkflowStepID, roleId, stepSourcePointGuid) == null)
                {
                    WorkflowStepRoleInfoProvider.AddRoleToWorkflowStep(WorkflowStepID, roleId, stepSourcePointGuid);
                }
            }
        }

        ShowChangesSaved();
    }


    /// <summary>
    /// Saves users data
    /// </summary>
    private void SaveUsersData()
    {
        Guid stepSourcePointGuid = SourcePointGuid ?? Guid.Empty;

        // Remove old items
        string newValues = ValidationHelper.GetString(usUsers.Value, null);
        string items = DataHelper.GetNewItemsInList(newValues, currentUsers);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Add all new items to site
            foreach (string item in newItems)
            {
                int userId = ValidationHelper.GetInteger(item, 0);
                // If user is authorized, remove it
                WorkflowStepUserInfo wsu = WorkflowStepUserInfoProvider.GetWorkflowStepUserInfo(WorkflowStepID, userId, stepSourcePointGuid);
                wsu?.Delete();
            }
        }

        // Add new items
        items = DataHelper.GetNewItemsInList(currentUsers, newValues);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Add all new items to site
            foreach (string item in newItems)
            {
                int userId = ValidationHelper.GetInteger(item, 0);

                // If user is not authorized, authorize it
                if (WorkflowStepUserInfoProvider.GetWorkflowStepUserInfo(WorkflowStepID, userId, stepSourcePointGuid) == null)
                {
                    WorkflowStepUserInfoProvider.AddUserToWorkflowStep(WorkflowStepID, userId, stepSourcePointGuid);
                }
            }
        }

        ShowChangesSaved();
    }

    #endregion
}

