using System;

using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.ContactManagement.Web.UI;
using CMS.Core;
using CMS.Helpers;
using CMS.UIControls;
using CMS.WorkflowEngine;


[Title("ma.ui.detail")]
[UIElement(ModuleName.ONLINEMARKETING, "PendingContacts", false, true)]

public partial class CMSModules_ContactManagement_Pages_Tools_PendingContacts_Process_Detail : CMSContactManagementPage
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
                if (IsDialog)
                {
                    ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "closePCWidgetDialog", ScriptHelper.GetScript("window.refreshPageOnClose = true; CloseDialog();"));
                }
                else
                {
                    URLHelper.Redirect(ResolveUrl(GetListingUrl()));
                }
                break;
        }
    }


    /// <summary>
    /// Gets URL of listing of processes of current contact.
    /// </summary>
    /// <returns>URL of listing</returns>
    private string GetListingUrl()
    {
        return "~/CMSModules/ContactManagement/Pages/Tools/PendingContacts/List.aspx?siteid=" + SiteID;
    }


    /// <summary>
    /// Breadcrumbs setting.
    /// </summary>
    private void SetBreadcrumbs()
    {
        string bcUrl = null;

        if (!IsDialog)
        {
            bcUrl = GetListingUrl();
        }

        SetBreadcrumb(0, GetString("ma.contact.pendingcontacts"), bcUrl, null, null);
        SetBreadcrumb(1, HTMLHelper.HTMLEncode(ContactInfoProvider.GetContactFullName(autoMan.ObjectID)), null, null, null);
   }

    #endregion
}
