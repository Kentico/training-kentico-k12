using System;
using System.Linq;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.WorkflowEngine;
using CMS.WorkflowEngine.Web.UI;


public partial class CMSModules_Workflows_Workflow_Designer : CMSWorkflowPage
{
    #region "Constants"

    private const string SERVICEURL = "~/CMSModules/Workflows/Services/WorkflowDesignerService.svc";

    #endregion


    #region "Event handlers"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        designerElem.ServiceUrl = SERVICEURL;

        CssRegistration.RegisterBootstrap(this);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (WorkflowId <= 0)
        {
            designerElem.StopProcessing = true;
            return;
        }

        designerElem.WorkflowID = WorkflowId;
        designerElem.WorkflowType = WorkflowTypeEnum.Approval;

        if(!LicenseHelper.CheckFeature(RequestContext.CurrentDomain, FeatureEnum.AdvancedWorkflow))
        {
            designerElem.ReadOnly = true;
            MessagesPlaceHolder.OffsetY = 10;
            MessagesPlaceHolder.UseRelativePlaceHolder = false;
            ShowInformation(GetString("wf.licenselimitation"));
        }

        EditedObject = CurrentWorkflow;

        ScriptHelper.HideVerticalTabs(this);
    }

    #endregion
}
