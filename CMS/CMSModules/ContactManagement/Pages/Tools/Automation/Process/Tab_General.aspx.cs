using System;

using CMS.ContactManagement.Web.UI;
using CMS.Core;
using CMS.UIControls;
using CMS.WorkflowEngine;

// Edited object
[EditedObject(WorkflowInfo.OBJECT_TYPE_AUTOMATION, "processid")]
[Security(Resource=ModuleName.ONLINEMARKETING, UIElements = "EditProcess;EditProcessGeneral")]
public partial class CMSModules_ContactManagement_Pages_Tools_Automation_Process_Tab_General : CMSAutomationPage
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Check permissions
        editElem.Form.SecurityCheck.Resource = ModuleName.ONLINEMARKETING;
        editElem.Form.SecurityCheck.Permission = "ManageProcesses";
    }
}