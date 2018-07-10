using System;

using CMS.Base;
using CMS.ContactManagement.Web.UI;
using CMS.Core;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.UIControls;
using CMS.WorkflowEngine;

// Edited object
[EditedObject(WorkflowActionInfo.OBJECT_TYPE_AUTOMATION, "objectId")]
public partial class CMSModules_ContactManagement_Pages_Tools_Automation_Action_Tab_General : CMSContactManagementConfigurationPage
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
        editElem.ObjectType = WorkflowActionInfo.OBJECT_TYPE_AUTOMATION;

        // Initialize redirection URL
        string url = UIContextHelper.GetElementUrl(ModuleName.ONLINEMARKETING, "ActionProperties", false);
        url = URLHelper.AddParameterToUrl(url, "objectid", "{%EditedObject.ID%}");
        url = URLHelper.AddParameterToUrl(url, "saved", "1");
        editElem.EditForm.RedirectUrlAfterCreate = url;
    }
}