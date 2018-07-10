using System;

using CMS.Base;
using CMS.ContactManagement.Web.UI;
using CMS.UIControls;
using CMS.WorkflowEngine;

// Actions
[Action(0, "cms.workflowaction.action.new", "{%UIContextHelper.GetElementUrl(\"CMS.OnlineMarketing\", \"NewAction\", \"false\")|(encode)false%}")]

public partial class CMSModules_ContactManagement_Pages_Tools_Automation_Action_List : CMSContactManagementConfigurationPage
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        // Only global administrator can access automation process actions
        if (!CurrentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            RedirectToAccessDenied(GetString("security.accesspage.onlyglobaladmin"));
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        listElem.ObjectType = WorkflowActionInfo.OBJECT_TYPE_AUTOMATION;
    }
}
