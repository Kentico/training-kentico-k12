using System;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_WebParts_Development_WebPart_Design : PortalPage
{
    #region "Public properties"

    /// <summary>
    /// Document manager
    /// </summary>
    public override ICMSDocumentManager DocumentManager
    {
        get
        {
            // Enable document manager
            docMan.Visible = true;
            docMan.StopProcessing = false;
            docMan.RegisterSaveChangesScript = (PortalContext.ViewMode.IsEdit());

            return docMan;
        }
    }


    /// <summary>
    /// Local page messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return DocumentManager.MessagesPlaceHolder;
        }
    }

    #endregion


    #region "Page methods"

    private WebPartInfo wpi;
    private bool configAvailable;


    /// <summary>
    /// PreInit event handler.
    /// </summary>
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        // Init the page components
        PageManager = manPortal;
        manPortal.SetMainPagePlaceholder(plc);

        int webPartId = QueryHelper.GetInteger("webpartid", 0);

        wpi = WebPartInfoProvider.GetWebPartInfo(webPartId);
        UIContext.EditedObject = wpi;

        // If default configuration not available, do not display content
        configAvailable = wpi.HasDefaultConfiguration();

        var pt = wpi.GetVirtualPageTemplate();

        PageInfo pi = PageInfoProvider.GetVirtualPageInfo(pt);
        pi.DocumentNamePath = "/" + GetString("edittabs.design");

        DocumentContext.CurrentPageInfo = pi;

        // Set the design mode
        PortalContext.SetRequestViewMode(ViewModeEnum.DesignWebPart);

        ManagersContainer = plcManagers;
        ScriptManagerControl = manScript;
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (!configAvailable)
        {
            // Configuration not set
            actionsElem.AddAction(new HeaderAction()
            {
                Text = GetString("WebPartDesign.ConfigureDefault"),
                CommandName = "configure",
                Tooltip = GetString("WebPartDesign.NotConfigured")
                
            });
        }
        else
        {
            // Configuration set
            actionsElem.AddAction(new HeaderAction()
            {
                Text = GetString("WebPartDesign.RemoveConfiguration"),
                CommandName = "remove",
                OnClientClick = String.Format("return confirm('{0}');", GetString("webpartdesign.confirmremove")),
                Tooltip = GetString("WebPartDesign.Info")
            });
        }

        actionsElem.ActionPerformed += actionsElem_ActionPerformed;

        MessagesPlaceHolder.OffsetY = 9;
    }


    private void actionsElem_ActionPerformed(object sender, CommandEventArgs e)
    {
        if (wpi != null)
        {
            switch (e.CommandName)
            {
                case "remove":
                    wpi.RemoveDefaultConfiguration();
                    break;

                case "configure":
                    wpi.EnsureDefaultConfiguration();
                    break;

                case "reset":
                    wpi.ResetDefaultConfiguration();
                    break;
            }

            URLHelper.Redirect(Request.RawUrl);
        }
    }

    
    /// <summary>
    /// PreRender event handler.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Init the header tags
        ltlTags.Text = HeaderTags;

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "InfoScript", ScriptHelper.GetScript("function IsCMSDesk() { return true; }"));
    }

    #endregion
}
