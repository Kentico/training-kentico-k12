using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.UIControls;
using CMS.WorkflowEngine;
using CMS.WorkflowEngine.GraphConfig;


/// <summary>
/// Code behind for control used to graph printing.
/// </summary>
public partial class CMSModules_Workflows_Controls_WorkflowDesigner : CMSUserControl
{
    #region "Private variables"

    private bool mReadOnly = false;

    private int mWorkflowId = 0;

    private WorkflowInfo mWorkflow = null;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Property used to select workflow to be printed.
    /// </summary>
    public int WorkflowID
    {
        get
        {
            return mWorkflowId;
        }
        set
        {
            mWorkflowId = value;
            mWorkflow = null;
        }
    }


    /// <summary>
    /// Property of workflow info object.
    /// </summary>
    private WorkflowInfo Workflow
    {
        get
        {
            if (mWorkflow == null)
            {
                mWorkflow = WorkflowInfoProvider.GetWorkflowInfo(WorkflowID);
                if ((mWorkflow != null) && (mWorkflow.WorkflowType != WorkflowType))
                {
                    RedirectToAccessDenied(GetString("workflow.type.notsupported"));
                    mWorkflow = null;
                }
            }
            return mWorkflow;
        }
    }


    /// <summary>
    /// Type of workflow to be edited.
    /// </summary>
    public WorkflowTypeEnum WorkflowType
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets workflow step ID to be selected.
    /// </summary>
    public int SelectedStepID
    {
        get;
        set;
    }


    /// <summary>
    /// Whether or not changes should be saved.
    /// </summary>
    public bool ReadOnly
    {
        get
        {
            return mReadOnly;
        }
        set
        {
            mReadOnly = value;
            toolbar.Visible = !value;
            toolbarContainer.Visible = !value;
            serviceChecker.StopProcessing = value;
        }
    }


    /// <summary>
    /// Width of the control pane
    /// </summary>
    public Unit Width
    {
        get;
        set;
    }


    /// <summary>
    /// Height of the control pane
    /// </summary>
    public Unit Height
    {
        get;
        set;
    }


    /// <summary>
    /// Steps user should be able to add to graph from toolbar.
    /// </summary>
    public List<WorkflowStepTypeEnum> ToolbarStepItems
    {
        get
        {
            return toolbar.StepItems;
        }
        set
        {
            toolbar.StepItems = value;
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
    /// Propagates visibility to toolbar.
    /// </summary>
    public override bool Visible
    {
        get
        {
            return base.Visible;
        }
        set
        {
            toolbarContainer.Visible = value;
            toolbar.Visible = value;
            base.Visible = value;
        }
    }


    /// <summary>
    /// URL of WCF service used for editing graph.
    /// </summary>
    public string ServiceUrl 
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
        CMSPage page = Page as CMSPage;
        if (page != null)
        {
            page.EnsureScriptManager();
        }

        CheckService();
        InitializeToolbar();
    }


    /// <summary>
    /// Prints the graph.
    /// </summary>
    /// <param name="sender">Sender of event</param>
    /// <param name="e">Arguments of event</param>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (!Visible)
        {
            return;
        }
        SetGraphConfiguration();
        if (!ReadOnly)
        {
            uniGraph.RegisterService(ServiceUrl);
        }
    }


    protected void serviceChecker_OnCheckFailed(object sender, EventArgs e)
    {
        ReadOnly = true;
    }

    #endregion


    #region "Protected methods"

    /// <summary>
    /// Checks WCF service.
    /// </summary>
    protected void CheckService()
    {
        if (string.IsNullOrEmpty(ServiceUrl))
        {
            ReadOnly = true;
        }
        serviceChecker.ServiceUrl = ServiceUrl;
        serviceChecker.MessagesPlaceHolder.UseRelativePlaceHolder = false;
        serviceChecker.MessagesPlaceHolder.OffsetY = 10;
        serviceChecker.OnCheckFailed += serviceChecker_OnCheckFailed;
    }


    /// <summary>
    /// Prepares control for read only/editable mode
    /// </summary>
    private void InitializeToolbar()
    {
        toolbarContainer.CssClass += " " + uniGraph.JsObjectName;
        toolbar.JsGraphObject = uniGraph.JsObjectName;
        toolbar.Workflow = Workflow;
    }


    /// <summary>
    /// Sets configuration to server control.
    /// </summary>
    private void SetGraphConfiguration()
    {
        if (Workflow != null)
        {
            uniGraph.GraphConfiguration = new WorkflowGraph(Workflow);
        }
        uniGraph.ReadOnly = ReadOnly;
        uniGraph.Width = Width;
        uniGraph.Height = Height;
        if (SelectedStepID != 0)
        {
            uniGraph.SelectedNodeID = SelectedStepID.ToString();
        }
    }

    #endregion
}