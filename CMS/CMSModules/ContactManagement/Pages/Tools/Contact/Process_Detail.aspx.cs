using System;

using CMS.Base.Web.UI;
using CMS.ContactManagement.Web.UI;
using CMS.Core;
using CMS.Helpers;
using CMS.UIControls;
using CMS.WorkflowEngine;

[UIElement(ModuleName.ONLINEMARKETING, "ContactProcesses")]
public partial class CMSModules_ContactManagement_Pages_Tools_Contact_Process_Detail : CMSContactManagementPage
{
    #region "Variables"

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
        return "~/CMSModules/ContactManagement/Pages/Tools/Contact/Tab_Processes.aspx?objectid=" + autoMan.ObjectID;
    }


    /// <summary>
    /// Breadcrumbs setting.
    /// </summary>
    private void SetBreadcrumbs()
    {
        SetBreadcrumb(0, GetString("ma.contact.processes"), GetListingUrl(), null, null);
        SetBreadcrumb(1, HTMLHelper.HTMLEncode(autoMan.Process.WorkflowDisplayName), null, null, null);
    }

    #endregion
}
