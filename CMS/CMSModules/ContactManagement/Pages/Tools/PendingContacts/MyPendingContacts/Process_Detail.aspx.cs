using System;

using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.Core;
using CMS.Helpers;
using CMS.UIControls;
using CMS.WorkflowEngine;


[UIElement(ModuleName.ONLINEMARKETING, "MyContacts")]
public partial class CMSModules_ContactManagement_Pages_Tools_PendingContacts_MyPendingContacts_Process_Details : CMSContentManagementPage
{
    #region "Private variables"

    private WorkflowInfo workflow;

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        autoMan.OnAfterAction += AutomationManager_OnAfterAction;
        autoMan.StateObjectID = QueryHelper.GetInteger("stateid", 0);

        workflow = autoMan.Process;
        if (workflow != null)
        {
            menuElem.OnClientStepChanged = ClientScript.GetPostBackEventReference(pnlUp, null);
        }
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        SetBreadcrumbs();

        pnlContainer.Enabled = !autoMan.ProcessingAction;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Register the scripts
        if (!autoMan.RefreshActionContent)
        {
            ScriptHelper.RegisterLoader(Page);
        }
    }


    protected void AutomationManager_OnAfterAction(object sender, AutomationManagerEventArgs e)
    {
        switch (e.ActionName)
        {
            case ComponentEvents.AUTOMATION_REMOVE:
            case ComponentEvents.AUTOMATION_START:
                URLHelper.Redirect(ResolveUrl(GetListingUrl()));
                break;
        }
    }


    /// <summary>
    /// Gets URL of listing of processes of current contact.
    /// </summary>
    /// <returns>URL of listing</returns>
    private string GetListingUrl()
    {
        return String.Format("~/CMSModules/ContactManagement/Pages/Tools/PendingContacts/MyPendingContacts/PendingContacts.aspx?siteid={0}", QueryHelper.GetInteger("siteid", 0));
    }


    /// <summary>
    /// Breadcrumbs setting.
    /// </summary>
    private void SetBreadcrumbs()
    {
        string bcUrl = null;
        if (QueryHelper.GetInteger("dialogmode", 0) != 1)
        {
            bcUrl = GetListingUrl();
        }

        SetBreadcrumb(0, GetString("ma.contact.pendingcontacts"), bcUrl, null, null);
        SetBreadcrumb(1, HTMLHelper.HTMLEncode(ContactInfoProvider.GetContactFullName(autoMan.ObjectID)), null, null, null);
    }

    #endregion
}
