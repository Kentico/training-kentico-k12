using System;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Helpers;
using CMS.UIControls;


[UIElement(ModuleName.MESSAGING, "MyMessages.Outbox")]
public partial class CMSModules_Messaging_MyMessages_MyMessages_Outbox : CMSContentManagementPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Header actions, etc. are done manually in contained control, so PageContent class that adds a padding to the whole page
        // is not wanted
        CurrentMaster.PanelContent.RemoveCssClass("PageContent");

        // Ensure custom page size
        if (!RequestHelper.IsPostBack())
        {
            outboxElem.Grid.PageSize = "15,50,100,##ALL##";
            outboxElem.Grid.Pager.DefaultPageSize = 15;
        }
    }


    protected void CheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Do not check permissions since user can always manage her messages
    }
}