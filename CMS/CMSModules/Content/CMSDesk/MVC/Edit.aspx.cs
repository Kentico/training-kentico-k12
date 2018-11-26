using System;

using CMS.Base.Web.UI;
using CMS.DocumentEngine.PageBuilder;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;

public partial class CMSModules_Content_CMSDesk_MVC_Edit : CMSContentPage
{
    private bool dataPropagated;
    private string actionName;


    /// <summary>
    /// Identifies the instance of editing.
    /// </summary>
    public Guid InstanceGUID
    {
        get
        {
            Guid guid = ValidationHelper.GetGuid(ViewState["InstanceGUID"], Guid.Empty);
            if (guid == Guid.Empty)
            {
                guid = Guid.NewGuid();
                ViewState["InstanceGUID"] = guid;
            }
            return guid;
        }
        set
        {
            ViewState["InstanceGUID"] = value;
        }
    }


    /// <summary>
    /// Overridden messages placeholder for correct positioning
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
        set
        {
            plcMess = value;
        }
    }


    /// <summary>
    /// OnPreInit
    /// </summary>
    protected override void OnPreInit(EventArgs e)
    {
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.OnAfterAction += DocumentManager_OnAfterAction;
        DocumentManager.RedirectForNonExistingDocument = false;

        base.OnPreInit(e);
    }


    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Setup Edit menu
        editMenu.NodeID = NodeID;
        editMenu.CultureCode = CultureCode;
        editMenu.UseSmallIcons = true;
        editMenu.IsLiveSite = false;

        editMenu.HeaderActions.OnGetActionScript += GetActionScript;
        ((CMSDocumentManager)DocumentManager).OnGetActionScript += GetActionScript;

        ScriptHelper.RegisterJQuery(this);
    }


    private string GetActionScript(object sender, GetActionScriptEventArgs e)
    {
        if (IsSavePerformingAction(e.ActionName) && DocumentManager.AllowSave)
        {
            return $"window.CMS && window.CMS.PageBuilder && window.CMS.PageBuilder.save({ScriptHelper.GetString(e.OriginalScript)}); return false;";
        }

        return e.OriginalScript;
    }


    private bool IsSavePerformingAction(string actionName)
    {
        switch (actionName)
        {
            case ComponentEvents.SAVE:
            case DocumentComponentEvents.APPROVE:
            case DocumentComponentEvents.PUBLISH:
            case DocumentComponentEvents.ARCHIVE:
            case DocumentComponentEvents.REJECT:
            case DocumentComponentEvents.CHECKIN:
                return true;
        }

        return false;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (TryGetUrl(out string url))
        {
            pageview.Src = "about:blank";
            RegisterMessagingScript(url);
        }
        else
        {
            pageview.Attributes.Add("src", url);
        }
    }


    private void RegisterMessagingScript(string url)
    {
        var uri = new Uri(url);
        var targetOrigin = uri.GetLeftPart(UriPartial.Authority);

        ScriptHelper.RegisterModule(this, "CMS.Builder/PageBuilder/Messaging", new
        {
            frameId = pageview.ClientID,
            frameUrl = url,
            guid = InstanceGUID,
            origin = targetOrigin,
            documentGuid = Node.DocumentGUID,
            // Delete dislpayed variants in session storage on full page refresh or for undo checkout action (variant can be removed)
            deleteDisplayedVariants = !RequestHelper.IsPostBack() || string.Equals(actionName, DocumentComponentEvents.UNDO_CHECKOUT, StringComparison.OrdinalIgnoreCase)
        });
    }


    private bool TryGetUrl(out string url)
    {
        url = PageBuilderHelper.GetPreviewLink(Node, MembershipContext.AuthenticatedUser.UserName);
        if (url == null)
        {
            url = URLHelper.ResolveUrl(AdministrationUrlHelper.GetInformationUrl("document.nopreviewavailable"));
            return false;
        }

        if (DocumentManager.AllowSave)
        {
            url = PageBuilderHelper.AddEditModeParameter(url);
        }

        if (dataPropagated)
        {
            url = PageBuilderHelper.AddClearPageCacheParameter(url);
        }

        return true;
    }


    private void DocumentManager_OnAfterAction(object sender, DocumentManagerEventArgs e)
    {
        actionName = e.ActionName;
        if (!IsSavePerformingAction(e.ActionName))
        {
            return;
        }

        if (!DocumentManager.SaveChanges)
        {
            return;
        }

        new TempPageBuilderWidgetsPropagator(e.Node).Delete(InstanceGUID);
    }


    private void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        if (!DocumentManager.SaveChanges)
        {
            return;
        }

        new TempPageBuilderWidgetsPropagator(e.Node).Propagate(InstanceGUID);
        dataPropagated = true;
    }
}
