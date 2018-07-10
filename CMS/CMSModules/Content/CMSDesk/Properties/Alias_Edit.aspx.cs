using System;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_Properties_Alias_Edit : CMSPropertiesPage
{
    #region "Private variables"

    private const string PROPERTIES_FOLDER = "~/CMSModules/Content/CMSDesk/Properties/";

    protected int aliasId = 0;
    int defaultNodeID;
    private DocumentAliasInfo mDocumentAlias;
    
    #endregion


    #region "Private properties"

    /// <summary>
    /// Document alias
    /// </summary>
    private DocumentAliasInfo DocumentAlias
    {
        get
        {
            return mDocumentAlias ?? (mDocumentAlias = aliasId > 0 ? DocumentAliasInfoProvider.GetDocumentAliasInfo(aliasId) : new DocumentAliasInfo());
        }
    }

    #endregion


    #region "Page events"

    protected override void OnPreInit(EventArgs e)
    {
        aliasId = QueryHelper.GetInteger("aliasid", 0);

        if (IsDialog)
        {
            MasterPageFile = "~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master";
        }

        // Must be called after the master page file is set
        base.OnPreInit(e);
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Content", "Properties.URLs"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties.URLs");
        }

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Content", "URLs.Aliases"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "URLs.Aliases");
        }

        ControlsHelper.FillListControlWithEnum<AliasActionModeEnum>(drpAction, "aliasaction", useStringRepresentation: true);


        // Disable document manager events
        DocumentManager.RegisterEvents = false;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptHelper.RegisterDialogScript(Page);

        btnOk.OnClientClick = DocumentManager.GetAllowSubmitScript();

        EditedObject = DocumentAlias;

        if (IsDialog)
        {
            PageTitle.TitleText = GetString("content.ui.urlsaliases");
        }
        else
        {
            HeaderActions.AddAction(new HeaderAction
            {
                Text = GetString("doc.urls.addnewalias"),
                RedirectUrl = ResolveUrl("Alias_Edit.aspx?nodeid=" + NodeID),
                ButtonStyle = ButtonStyle.Default
            });

            HeaderActions.AddAction(new HeaderAction
            {
                Text = GetString("doc.urls.viewallalias"),
                OnClientClick = "modalDialog('" + ResolveUrl(PROPERTIES_FOLDER + "Alias_AliasList.aspx") + "?nodeid=" + NodeID + "&dialog=1" + "','AliasManagement','90%','85%');",
                ButtonStyle = ButtonStyle.Default
            });
        }

        if (Node != null)
        {
            lblUrlInfoText.Text = Node.NodeAliasPath;

            // Check modify permissions
            if (!DocumentUIHelper.CheckDocumentPermissions(Node, PermissionsEnum.Modify))
            {
                ShowInformation(String.Format(GetString("cmsdesk.notauthorizedtoeditdocument"), Node.NodeAliasPath));

                txtURLExtensions.Enabled = false;

                ctrlURL.Enabled = false;

                cultureSelector.Enabled = false;
            }

            if (!RequestHelper.IsPostBack() && QueryHelper.GetInteger("saved", 0) == 1)
            {
                ShowChangesSaved();
            }

            lblDocumentCulture.Text = GetString("general.culture") + ResHelper.Colon;
            lblURLExtensions.Text = GetString("doc.urls.urlextensions") + ResHelper.Colon;

            // Show path of document alias only if dialog mode edit 
            pnlUrlInfo.Visible = IsDialog;

            // For dialog mode use DefaultNodeID 
            defaultNodeID = (IsDialog) ? QueryHelper.GetInteger("defaultNodeID", 0) : NodeID;

            CreateBreadcrumbs();

            cultureSelector.AllowDefault = false;
            cultureSelector.UniSelector.SpecialFields.Add(new SpecialField
            {
                Text = GetString("general.selectall"),
                Value = String.Empty
            });


            if (!RequestHelper.IsPostBack())
            {
                cultureSelector.Value = Node.DocumentCulture;

                // Edit existing alias
                if (DocumentAlias != null && DocumentAlias.AliasID > 0)
                {
                    txtURLExtensions.Text = DocumentAlias.AliasExtensions;
                    ctrlURL.URLPath = DocumentAlias.AliasURLPath;

                    cultureSelector.Value = DocumentAlias.AliasCulture;
                    PageBreadcrumbs.Items[1].Text = TreePathUtils.GetUrlPathDisplayName(DocumentAlias.AliasURLPath);

                    drpAction.SelectedValue = DocumentAlias.AliasActionMode.ToStringRepresentation();
                }
            }

            // Register js synchronization script for split mode
            if (QueryHelper.GetBoolean("refresh", false) && PortalUIHelper.DisplaySplitMode)
            {
                RegisterSplitModeSync(true, false, true);
            }
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Creates page breadcrumbs
    /// </summary>
    private void CreateBreadcrumbs()
    {
        // Initialize breadcrumbs
        string urls = GetString("Properties.Urls");
        string urlsUrl = string.Format(PROPERTIES_FOLDER + "Alias_List.aspx?nodeid={0}&compare=1", defaultNodeID);
        string addAlias = GetString("doc.urls.addnewalias");
        string aliasManagement = GetString("content.ui.urlsaliases");
        string managementUrl = PROPERTIES_FOLDER + "Alias_AliasList.aspx?nodeid=" + defaultNodeID;

        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = (IsDialog ? aliasManagement : urls),
            RedirectUrl = ResolveUrl(IsDialog ? managementUrl : urlsUrl)
        });

        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = addAlias
        });
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(ctrlURL.PlainURLPath))
        {
            ShowError(GetString("doc.urls.requiresurlpath"));
            return;
        }

        // Validate URL path
        if (!ctrlURL.IsValid())
        {
            ShowError(ctrlURL.ValidationError);
            return;
        }

        if (Node != null)
        {
            // Check modify permissions
            if (!DocumentUIHelper.CheckDocumentPermissions(Node, PermissionsEnum.Modify))
            {
                ShowError(String.Format(GetString("cmsdesk.notauthorizedtoreaddocument"), Node.NodeAliasPath));
                return;
            }

            // Check whether 
            if (!DocumentAliasInfoProvider.IsUnique(ctrlURL.URLPath, DocumentAlias.AliasID, Convert.ToString(cultureSelector.Value), txtURLExtensions.Text.Trim(), SiteContext.CurrentSiteName, true, NodeID))
            {
                ShowError(GetString("doc.urls.doacaliasnotunique"));
                return;
            }

            // Set object properties
            DocumentAlias.AliasURLPath = TreePathUtils.GetSafeUrlPath(ctrlURL.URLPath, Node.NodeSiteName);

            DocumentAlias.AliasExtensions = txtURLExtensions.Text.Trim();
            DocumentAlias.AliasCulture = ValidationHelper.GetString(cultureSelector.Value, "");
            DocumentAlias.AliasSiteID = Node.NodeSiteID;
            DocumentAlias.AliasActionMode = drpAction.SelectedValue.ToEnum<AliasActionModeEnum>();
            DocumentAlias.AliasNodeID = NodeID;

            // Insert into database
            DocumentAliasInfoProvider.SetDocumentAliasInfo(DocumentAlias);

            // Log synchronization
            DocumentSynchronizationHelper.LogDocumentChange(Node, TaskTypeEnum.UpdateDocument, Tree);

            aliasId = DocumentAlias.AliasID;

            string url = "Alias_Edit.aspx?saved=1&nodeid=" + NodeID + "&aliasid=" + aliasId + "&dialog=" + IsDialog;
            if (IsDialog)
            {
                url += "&defaultNodeID=" + defaultNodeID;
            }

            // Refresh the second frame in split mode
            if (PortalUIHelper.DisplaySplitMode)
            {
                url += "&refresh=1";
            }
            URLHelper.Redirect(UrlResolver.ResolveUrl(url));
        }
    }

    #endregion
}