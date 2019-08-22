using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_Properties_Alias_List : CMSPropertiesPage
{
    #region "Private variables"

    bool mIsRoot;
    bool aliasChanged;
    String mOldAliasPath = String.Empty;
    

    #endregion


    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        // Culture independent data
        SplitModeAllwaysRefresh = true;

        // Non-version data is modified
        DocumentManager.UseDocumentHelper = false;

        base.OnInit(e);

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Content", "Properties.URLs"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties.URLs");
        }

        // Redirect to information page when no UI elements displayed
        if (pnlUIAlias.IsHidden && pnlUIDocumentAlias.IsHidden && pnlUIExtended.IsHidden && pnlUIPath.IsHidden)
        {
            RedirectToUINotAvailable();
        }

        // Enable split mode
        EnableSplitMode = true;

        // Init document manager events
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.OnAfterAction += DocumentManager_OnAfterAction;

        // Initialize node
        mIsRoot = Node.IsRoot();

        UniGridAlias.StopProcessing = pnlUIDocumentAlias.IsHidden;

        // Register the scripts
        ScriptHelper.RegisterLoader(Page);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Set where condition - show nothing when nodeId is zero
        UniGridAlias.WhereCondition = "AliasNodeID = " + NodeID;

        if (Node != null)
        {
            // Hide parts which are not relevant to content only nodes
            if (ShowContentOnlyProperties)
            {
                pnlUIPath.Visible = false;
                pnlUIExtended.Visible = false;
                pnlUIDocumentAlias.Visible = false;
                headAlias.Visible = false;
            }

            if (Node.NodeAliasPath == "/")
            {
                valAlias.Visible = false;
            }

            // Check modify permissions
            if ((MembershipContext.AuthenticatedUser.IsAuthorizedPerDocument(Node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied))
            {
                pnlAlias.Enabled = false;
                menuElem.Enabled = false;
                UniGridAlias.GridView.Enabled = false;
                btnNewAlias.Enabled = false;

                chkCustomExtensions.Enabled = false;
                ctrlURL.Enabled = false;
            }
            else
            {
                ltlScript.Text = ScriptHelper.GetScript("var node = " + NodeID + "; \n var deleteMsg = '" + GetString("DocumentAlias.DeleteMsg") + "';");

                UniGridAlias.OnAction += UniGridAlias_OnAction;
                UniGridAlias.OnExternalDataBound += UniGridAlias_OnExternalDataBound;
                btnNewAlias.OnClientClick = "window.location.replace('" + UrlResolver.ResolveUrl("Alias_Edit.aspx?nodeid=" + NodeID) + "'); return false;";
            }

            chkCustomExtensions.Text = GetString("GeneralProperties.UseCustomExtensions");
            valAlias.ErrorMessage = GetString("GeneralProperties.RequiresAlias");

            lblExtensions.Text = GetString("doc.urls.urlextensions") + ResHelper.Colon;

            txtAlias.MaxLength = TreePathUtils.MaxAliasLength;
            if (!mIsRoot)
            {
                txtAlias.Enabled = !TreePathUtils.AutomaticallyUpdateDocumentAlias(Node.NodeSiteName);
            }

            if (!RequestHelper.IsPostBack())
            {
                ReloadData();
            }

            // Get automatic URL path
            var culture = CultureHelper.GetShortCultureCode(Node.DocumentCulture);
            ctrlURL.AutomaticURLPath = TreePathUtils.GetUniqueUrlPath(Node, culture);

            // Reflect processing action
            pnlPageContent.Enabled = DocumentManager.AllowSave;
        }
    }


    protected object UniGridAlias_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "culture":
                return UniGridFunctions.CultureDisplayName(parameter);

            case "urlpath":
                {
                    // Parse the URL path
                    string urlPath = ValidationHelper.GetString(parameter, "");

                    return TreePathUtils.GetUrlPathDisplayName(urlPath);
                }
        }

        return parameter;
    }


    private void UniGridAlias_OnAction(string actionName, object actionArgument)
    {
        if (Node != null)
        {
            // Check modify permissions
            if (MembershipContext.AuthenticatedUser.IsAuthorizedPerDocument(Node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied)
            {
                return;
            }

            string action = DataHelper.GetNotEmpty(actionName, String.Empty).ToLowerCSafe();

            switch (action)
            {
                case "edit":
                    // Edit action
                    URLHelper.Redirect(UrlResolver.ResolveUrl("Alias_Edit.aspx?nodeid=" + NodeID + "&aliasid=" + Convert.ToString(actionArgument)));
                    break;

                case "delete":
                    // Delete action
                    int aliasId = ValidationHelper.GetInteger(actionArgument, 0);
                    if (aliasId > 0)
                    {
                        // Delete
                        DocumentAliasInfoProvider.DeleteDocumentAliasInfo(aliasId);

                        // Log synchronization
                        DocumentSynchronizationHelper.LogDocumentChange(Node, TaskTypeEnum.UpdateDocument, DocumentManager.Tree);
                    }
                    break;
            }
        }
    }


    protected void chkCustomExtensions_CheckedChanged(object sender, EventArgs e)
    {
        txtExtensions.Enabled = chkCustomExtensions.Checked;
        if (!chkCustomExtensions.Checked)
        {
            txtExtensions.Text = Node.DocumentExtensions;
        }
    }


    private void DocumentManager_OnAfterAction(object sender, DocumentManagerEventArgs e)
    {
        txtAlias.Text = Node.NodeAlias;

        // Check if alias was changed
        if (Node.NodeAliasPath != mOldAliasPath)
        {
            // Update all related MVTests
            ModuleCommands.OnlineMarketingMoveMVTests(Node.NodeAliasPath, mOldAliasPath, SiteContext.CurrentSiteID);
        }

        // Load the URL path
        LoadURLPath(Node);

        UniGridAlias.ReloadData();

        if ((!pnlUIAlias.IsHidden || !pnlUIPath.IsHidden) && aliasChanged && (PortalContext.ViewMode == ViewModeEnum.EditLive))
        {
            // Get the updated document url
            string url = UrlResolver.ResolveUrl(DocumentURLProvider.GetUrl(Node));

            // Register redirect script
            string reloadScript = "if (parent.parent.frames['header'] != null) { parent.parent.frames['header'].reloadPageUrl =" + ScriptHelper.GetString(url) + "; }";
            ScriptHelper.RegisterStartupScript(this, typeof(string), "reloadScript", reloadScript, true);
        }
    }


    private void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        aliasChanged = false;

        // ALIAS group is displayed
        if (!pnlUIAlias.IsHidden)
        {
            if (!String.IsNullOrEmpty(txtAlias.Text.Trim()) || mIsRoot)
            {
                string nodeAlias = txtAlias.Text.Trim();

                aliasChanged = (Node.NodeAlias != nodeAlias);

                mOldAliasPath = Node.NodeAliasPath;
                Node.NodeAlias = nodeAlias;
            }
            else
            {
                e.IsValid = false;
                e.ErrorMessage = GetString("general.errorvalidationerror");
                return;
            }
        }

        // PATH group is displayed
        if (!pnlUIPath.IsHidden)
        {
            // Validate URL path
            if (!ctrlURL.IsValid())
            {
                e.IsValid = false;
                e.ErrorMessage = ctrlURL.ValidationError;
                return;
            }

            aliasChanged |= (ctrlURL.URLPath != Node.DocumentUrlPath);

            Node.DocumentUseNamePathForUrlPath = !ctrlURL.IsCustom;
            if (!Node.DocumentUseNamePathForUrlPath)
            {
                Node.DocumentUrlPath = TreePathUtils.GetSafeUrlPath(ctrlURL.URLPath, Node.NodeSiteName);
            }
        }

        // EXTENDED group is displayed
        if (!pnlUIExtended.IsHidden)
        {
            Node.DocumentUseCustomExtensions = chkCustomExtensions.Checked;
            if (Node.DocumentUseCustomExtensions)
            {
                Node.DocumentExtensions = txtExtensions.Text;
            }
        }
    }


    private void ReloadData()
    {
        if (Node != null)
        {
            // Check modify permissions
            if (MembershipContext.AuthenticatedUser.IsAuthorizedPerDocument(Node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied)
            {
                btnNewAlias.Enabled = false;
                pnlAlias.Enabled = false;
                pnlURLPath.Enabled = false;
                pnlExtended.Enabled = false;
                pnlDocumentAlias.Enabled = false;
            }

            ctrlURL.IsCustom = !Node.DocumentUseNamePathForUrlPath;
            chkCustomExtensions.Checked = Node.DocumentUseCustomExtensions;

            txtExtensions.Text = Node.DocumentExtensions;
            txtAlias.Text = Node.NodeAlias;

            // Load the URL path
            LoadURLPath(Node);
        }
    }


    /// <summary>
    /// Loads the URL path to the UI
    /// </summary>
    private void LoadURLPath(TreeNode treeNode)
    {
        ctrlURL.URLPath = treeNode.DocumentUrlPath;

        txtExtensions.Enabled = chkCustomExtensions.Checked;

        if (mIsRoot)
        {
            txtAlias.Enabled = false;
            valAlias.Enabled = false;

            ctrlURL.Enabled = false;

            chkCustomExtensions.Enabled = false;
        }

        if (treeNode.IsLink)
        {
            ctrlURL.Enabled = false;
        }
    }

    #endregion
}
