using System;

using CMS.Core;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.UIControls;
using CMS.WorkflowEngine.Web.UI;


[Help("workflow_action_new")]

public partial class CMSModules_Workflows_Pages_WorkflowAction_New : CMSWorkflowPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        editElem.EditForm.RedirectUrlAfterCreate = URLHelper.AppendQuery(UIContextHelper.GetElementUrl(ModuleName.CMS, "EditWorkflowsAction", false), "objectID={%EditedObject.ID%}&saved=1");

        PageBreadcrumbs.AddBreadcrumb(new BreadcrumbItem()
        {
            Index = 0,
            RedirectUrl = UIContextHelper.GetElementUrl(ModuleName.CMS, "Workflows.Actions", false),
            Text = GetString("cms.workflowaction.action.list")
        });

        PageBreadcrumbs.AddBreadcrumb(new BreadcrumbItem()
        {
            Index = 1,
            Text = GetString("cms.workflowaction.action.new")
        });
    }
}
