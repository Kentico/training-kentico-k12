using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.Synchronization;
using CMS.UIControls;


[Security("CMS.Content", null, "Properties.Template")]
[Security("CMS.Design", "Design", null)]
[UIElement(ModuleName.CONTENT, "Properties.Template")]
public partial class CMSModules_Content_CMSDesk_Properties_Template : CMSPropertiesPage
{
    #region "Variables & constants"

    private TreeNode node;

    private CurrentUserInfo currentUser;

    private bool hasModifyPermission;

    private bool selectorEnabled = true;

    private PageTemplateInfo pageTemplateInfo;

    private ICMSDocumentManager mDocumentManager;

    #endregion


    #region "Properties"

    /// <summary>
    /// Use document site as current site
    /// </summary>
    public override string CurrentSiteName
    {
        get
        {
            return (Node != null) ? Node.NodeSiteName : base.CurrentSiteName;
        }
        set
        {

        }
    }


    /// <summary>
    /// Selected template ID
    /// </summary>
    public int SelectedTemplateID
    {
        get
        {
            return ValidationHelper.GetInteger(hdnSelected.Value, 0);
        }
        set
        {
            hdnSelected.Value = value.ToString();
        }
    }


    /// <summary>
    /// Document manager control.
    /// </summary>
    public override ICMSDocumentManager DocumentManager
    {
        get
        {
            if (mDocumentManager == null)
            {
                // Use base implementation of DocumentManager and apply custom initialization
                mDocumentManager = base.DocumentManager;

                // Non-version data is modified
                mDocumentManager.UseDocumentHelper = false;
            }

            return mDocumentManager;
        }
    }

    #endregion


    #region "Page methods"

    protected override void OnInit(EventArgs e)
    {
        // Culture independent data
        SplitModeAllwaysRefresh = true;

        base.OnInit(e);

        // Keep current user info
        currentUser = MembershipContext.AuthenticatedUser;

        // Keep node instance
        node = Node;

        if (node.IsRoot())
        {
            plcUILevels.Visible = false;
        }

        // Init document manager events
        DocumentManager.OnValidateData += DocumentManager_OnValidateData;
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.OnAfterAction += DocumentManager_OnAfterAction;

        EnableSplitMode = true;

        // Register the scripts
        ScriptHelper.RegisterLoader(this);
        ScriptHelper.RegisterDialogScript(this);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SetPropertyTab(TAB_TEMPLATE);

        // Setup child controls
        inheritElem.Node = Node;

        // Keep information whether current user has modify permission
        if (node != null)
        {
            PageTemplateCategoryInfo category = PageTemplateCategoryInfoProvider.GetPageTemplateCategoryInfo("/");
            int id = (category != null) ? category.CategoryId : 0;

            hasModifyPermission = DocumentUIHelper.CheckDocumentPermissions(node, PermissionsEnum.Modify);
            btnSelect.OnClientClick = "modalDialog('" + ResolveUrl("~/CMSModules/PortalEngine/UI/Layout/PageTemplateSelector.aspx") + "?rootcategoryid=" + id + "&documentid=" + node.DocumentID + "&nodeguid=" + node.NodeGUID + "', 'PageTemplateSelection', '90%', '85%'); return false;";
        }

        btnSelect.Text = GetString("PageProperties.Select");
        btnClone.OnClientClick = "return confirm(" + ScriptHelper.GetLocalizedString("Template.ConfirmClone") + ");";


        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "SelectTemplate", ScriptHelper.GetScript(
@"
function RefreshPage() {
    document.location.replace(document.location);
}

function SetTemplateToHdnField(templateId) {
    document.getElementById('" + hdnSelected.ClientID + @"').value = templateId;
}

function OnSaveAsNewPageTemplate(templateId, selectorId) {
    SetTemplateToHdnField(templateId);
    RefreshPage();
}

function OnSelectPageTemplate(templateId, selectorId) {
    SetTemplateToHdnField(templateId);
    " + ClientScript.GetPostBackEventReference(btnSelect, String.Empty) + @"
}"));

        // Reflect processing action
        pnlInherits.Enabled = DocumentManager.AllowSave;

        if (!RequestHelper.IsPostBack())
        {
            LoadData();
        }

        ReloadControls();

        HandleCultureSettings();
    }


    protected override void OnPreRender(EventArgs e)
    {
        // Ensure visibility
        EnsureEditingForm(hasModifyPermission && !DocumentManager.ProcessingAction);

        base.OnPreRender(e);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Handles visibility of culture settings radio buttons.
    /// </summary>
    private void HandleCultureSettings()
    {
        // Check multilingual mode
        if (CultureSiteInfoProvider.IsSiteMultilingual(SiteContext.CurrentSiteName))
        {
            return;
        }

        if (radThisCulture.Checked)
        {
            radThisCulture.ResourceString = "Template.OwnOne";
            plcAllCultures.Visible = false;
            plcThisCulture.Visible = true;
        }
        else
        {
            radAllCultures.ResourceString = "Template.OwnOne";
            plcThisCulture.Visible = false;
            plcAllCultures.Visible = true;
        }
    }


    /// <summary>
    /// Radio buttons event hander
    /// </summary>
    protected void RadChanged(object sender, EventArgs e)
    {
        ReloadControls();
    }


    /// <summary>
    /// Gets the inherited page template from the parent node
    /// </summary>
    /// <param name="currentNode">Document node</param>
    protected int GetInheritedPageTemplateId(TreeNode currentNode)
    {
        string aliasPath = currentNode.NodeAliasPath;

        // For root, there is no inheritance possible
        if (String.IsNullOrEmpty(aliasPath) || (aliasPath == "/"))
        {
            return 0;
        }

        aliasPath = TreePathUtils.GetParentPath(aliasPath);

        // Get the parent page info
        PageInfo pi = PageInfoProvider.GetPageInfo(currentNode.NodeSiteName, aliasPath, currentNode.DocumentCulture, null, currentNode.NodeParentID, true);
        if (pi == null)
        {
            return 0;
        }

        // Get template used by the page info
        pageTemplateInfo = pi.UsedPageTemplateInfo;

        return pageTemplateInfo != null ? pageTemplateInfo.PageTemplateId : 0;
    }


    /// <summary>
    /// Reloads the controls on the page to the appropriate state
    /// </summary>
    protected void ReloadControls()
    {
        node = Node;

        if (node.NodeAliasPath == "/")
        {
            // For root, inherit option means no page template
            radInherit.ResourceString = "Template.NoTemplate";
        }

        // Get the template ID
        int templateId = SelectedTemplateID;
        string suffix = null;
        bool inherit = radInherit.Checked;

        plcUIClone.Visible = false;
        plcUIEdit.Visible = false;
        plcUISave.Visible = false;

        if (inherit)
        {
            // Inherited
            selectorEnabled = false;

            // Inherit
            templateId = GetInheritedPageTemplateId(node);

            if (templateId > 0)
            {
                suffix = " " + GetString("Template.Inherited");
            }
        }
        else
        {
            // Standard selection
            selectorEnabled = true;
        }

        // Set modal dialogs
        string modalScript = String.Format("modalDialog('{0}?startingpath=/&templateId={1}&siteid={2}&documentid={3}&inherits={4}', 'SaveNewTemplate', 720, 430); return false;", ResolveUrl("~/CMSModules/PortalEngine/UI/Layout/SaveNewPageTemplate.aspx"), templateId, SiteContext.CurrentSiteID, Node.DocumentID, radInherit.Checked);
        btnSave.OnClientClick = modalScript;

        String url = ApplicationUrlHelper.GetElementDialogUrl("cms.design", "PageTemplate.EditPageTemplate", templateId, String.Format("aliaspath={0}", node.NodeAliasPath));
        btnEditTemplateProperties.OnClientClick = "modalDialog('" + url + "', 'Template edit', '95%', '95%');return false;";

        // Load the page template name
        pageTemplateInfo = PageTemplateInfoProvider.GetPageTemplateInfo(templateId);
        if (pageTemplateInfo != null)
        {
            txtTemplate.Text = ResHelper.LocalizeString(pageTemplateInfo.DisplayName);

            plcUISave.Visible = true;
            plcUIEdit.Visible = (!pageTemplateInfo.IsReusable || currentUser.IsAuthorizedPerUIElement("CMS.Content", "Template.ModifySharedTemplates"));
            plcUIClone.Visible = pageTemplateInfo.IsReusable || inherit;
        }
        else
        {
            txtTemplate.Text = GetString("Template.SelectorNoTemplate");
        }

        txtTemplate.Text += suffix;
    }


    /// <summary>
    /// Ensures visibility of the displayed controls
    /// </summary>
    /// <param name="modify">Indicates whether current user has modify permission</param>
    private void EnsureEditingForm(bool modify)
    {
        // Template selector
        txtTemplate.Enabled = modify && selectorEnabled;
        btnSelect.Enabled = txtTemplate.Enabled;

        // Clone as ad-hoc
        btnClone.Enabled = modify;

        var pageTemplate = PageTemplateInfoProvider.GetPageTemplateInfo(Node.GetUsedPageTemplateId());

        if (SynchronizationHelper.UseCheckinCheckout)
        {
            if ((pageTemplate != null) && pageTemplate.Generalized.IsCheckedOut && !pageTemplate.Generalized.IsCheckedOutByUser(MembershipContext.AuthenticatedUser))
            {
                btnClone.Enabled = false;

                var objectType = TypeHelper.GetNiceObjectTypeName(pageTemplate.TypeInfo.ObjectType);
                var objectName = HTMLHelper.HTMLEncode(ResHelper.LocalizeString(pageTemplate.Generalized.ObjectDisplayName));
                btnClone.ToolTip = HTMLHelper.StripTags(String.Format(GetString("ObjectEditMenu.CheckOutToEdit"), objectType, objectName));
            }
        }

        if (!btnClone.Enabled)
        {
            // Remove client click action
            btnClone.OnClientClick = "return false;";
        }

        // Edit menu
        menuElem.Enabled = modify;

        // Inherits panel visibility
        pnlInherits.Visible = modify
            &&
            // Inherited page template
            ((pageTemplate == null)
            // Portal engine only and not master template
            || ((pageTemplate.PageTemplateType == PageTemplateTypeEnum.Portal) && !pageTemplate.ShowAsMasterTemplate));

        // Radio buttons
        radInherit.Enabled = modify;
        if (!currentUser.IsAuthorizedPerUIElement("CMS.Content", "Template.Inherit"))
        {
            radInherit.Attributes.Add("disabled", "disabled");
        }
        radAllCultures.Enabled = modify;
        radThisCulture.Enabled = modify;
    }


    /// <summary>
    /// Loads the initial data from the document
    /// </summary>
    private void LoadData()
    {
        node = Node;
        if (node == null)
        {
            return;
        }

        if (node.IsRoot())
        {
            // Hide inheritance options for root node
            pnlInherits.Visible = false;
        }
        else
        {
            inheritElem.Value = Node.NodeInheritPageLevels;
        }

        if (node.NodeInheritPageTemplate)
        {
            // Document inherits template
            radInherit.Checked = true;
        }
        else
        {
            // Document has its own template
            int templateId = node.GetUsedPageTemplateId();
            radInherit.Checked = false;

            if (node.NodeTemplateForAllCultures)
            {
                radAllCultures.Checked = true;
            }
            else
            {
                radThisCulture.Checked = true;
            }

            // Set selected template ID
            SelectedTemplateID = templateId;
        }

        ReloadControls();
    }

    #endregion


    #region "Button handling"

    /// <summary>
    /// Fires after the template selection
    /// </summary>
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        ReloadControls();
    }


    /// <summary>
    /// Clone button event handler
    /// </summary>
    protected void btnClone_Click(object sender, EventArgs e)
    {
        if ((pageTemplateInfo != null) && hasModifyPermission)
        {
            node = Node;

            // Clone the info
            string docName = node.GetDocumentName();
            string displayName = "Ad-hoc: " + docName;

            PageTemplateInfo newInfo = PageTemplateInfoProvider.CloneTemplateAsAdHoc(pageTemplateInfo, displayName, SiteContext.CurrentSiteID, node.NodeGUID);

            newInfo.Description = String.Format(GetString("PageTemplate.AdHocDescription"), Node.DocumentNamePath);
            PageTemplateInfoProvider.SetPageTemplateInfo(newInfo);

            CheckOutTemplate(newInfo);

            // Assign the selected template for all cultures and save
            SelectedTemplateID = newInfo.PageTemplateId;

            if (radInherit.Checked)
            {
                radAllCultures.Checked = true;
                radInherit.Checked = false;
            }

            DocumentManager.SaveDocument();
        }
    }


    /// <summary>
    /// Checks out the page template
    /// </summary>
    /// <param name="pageTemplateInfo">Page template to check out</param>
    private void CheckOutTemplate(PageTemplateInfo pageTemplateInfo)
    {
        var objectManager = CMSObjectManager.GetCurrent(Page);
        if ((objectManager != null) && CMSObjectManager.KeepNewObjectsCheckedOut)
        {
            objectManager.CheckOutObject(pageTemplateInfo);
        }
    }


    protected void DocumentManager_OnValidateData(object sender, DocumentManagerEventArgs e)
    {
        if (radInherit.Checked)
        {
            return;
        }

        // Set the selected template ID
        int templateId = SelectedTemplateID;
        if (templateId <= 0)
        {
            e.IsValid = false;
            e.ErrorMessage = GetString("newpage.templateerror");
        }
    }


    private void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        node = Node;

        if (radInherit.Checked)
        {
            // Set 0 as inherited
            SetTemplateId(0);

            node.NodeInheritPageTemplate = true;
        }
        else
        {
            // Set the selected template ID
            int templateId = SelectedTemplateID;
            SetTemplateId(templateId);

            bool templateSelected = (templateId > 0);

            node.NodeInheritPageTemplate = !templateSelected;

            if (!templateSelected)
            {
                radInherit.Checked = true;
                radThisCulture.Checked = false;
                radAllCultures.Checked = false;

                txtTemplate.Enabled = false;
                btnSelect.Enabled = false;
            }
        }

        node.SetValue("NodeInheritPageLevels", inheritElem.Value);
    }


    private void DocumentManager_OnAfterAction(object sender, DocumentManagerEventArgs e)
    {
        // Ensure default combination if page template changed
        if (PortalContext.MVTVariantsEnabled)
        {
            int templateId = Node.GetUsedPageTemplateId();

            ModuleCommands.OnlineMarketingEnsureDefaultCombination(templateId);
        }

        ReloadControls();
    }


    /// <summary>
    /// Sets the template id to the current document
    /// </summary>
    /// <param name="templateId">Page template ID</param>
    private void SetTemplateId(int templateId)
    {
        Node.NodeTemplateID = radAllCultures.Checked ? templateId : 0;

        Node.DocumentPageTemplateID = templateId;
        Node.NodeTemplateForAllCultures = radAllCultures.Checked;
    }

    #endregion
}