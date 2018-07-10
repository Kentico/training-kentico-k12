using System;

using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.ContactManagement.Web.UI;
using CMS.Helpers;
using CMS.WorkflowEngine;


public partial class CMSModules_ContactManagement_Pages_Tools_Automation_Process_Process_Detail : CMSAutomationPage
{
    #region "Variables"

    private WorkflowInfo workflow = null;

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


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        SetBreadcrumb(0, GetString("ma.contact.contacts"), ResolveUrl(GetListingUrl()), null, null);
        SetBreadcrumb(1, HTMLHelper.HTMLEncode(ContactInfoProvider.GetContactFullName(autoMan.ObjectID)), null, null, null);

        pnlContainer.Enabled = !autoMan.ProcessingAction;
    }


    /// <summary>
    /// Gets URL of listing of processes of current contact.
    /// </summary>
    /// <returns>URL of listing</returns>
    private string GetListingUrl()
    {
        return "~/CMSModules/ContactManagement/Pages/Tools/Automation/Process/Tab_Contacts.aspx?processid=" + autoMan.Process.WorkflowID;
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

    #endregion
}
