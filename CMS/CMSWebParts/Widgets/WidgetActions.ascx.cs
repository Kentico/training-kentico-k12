using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Xml;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.SiteProvider;
using CMS.WorkflowEngine;
using CMS.PortalEngine.Web.UI.Internal;

public partial class CMSWebParts_Widgets_WidgetActions : CMSAbstractWebPart, IPostBackEventHandler
{
    #region "Variables"

    private WidgetZoneTypeEnum zoneType = WidgetZoneTypeEnum.None;
    private TreeProvider mTreeProvider = null;
    private bool resetAllowed = true;
    private WebPartZoneInstance zoneInstance = null;
    private List<WebPartZoneInstance> zoneInstances = new List<WebPartZoneInstance>();
    private string addScript = String.Empty;
    private bool headerActionsLoaded = false;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Indicates whether action buttons should be placed to the main edit menu if available
    /// </summary>
    public bool UseMainMenu
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseMainMenu"), true);
        }
        set
        {
            SetValue("UseMainMenu", value);
        }
    }


    /// <summary>
    /// Gets or sets widget zone type.
    /// </summary>
    public string WidgetZoneType
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WidgetZoneType"), String.Empty);
        }
        set
        {
            SetValue("WidgetZoneType", value);
        }
    }


    /// <summary>
    /// Gets or sets widget zone type.
    /// </summary>
    public string WidgetZoneID
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WidgetZoneID"), String.Empty);
        }
        set
        {
            SetValue("WidgetZoneID", value);
        }
    }


    /// <summary>
    /// Gets or sets text for add button.
    /// </summary>
    public string AddButtonText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AddButtonText"), String.Empty);
        }
        set
        {
            SetValue("AddButtonText", value);
        }
    }


    /// <summary>
    /// Gets or sets text for reset button.
    /// </summary>
    public string ResetButtonText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ResetButtonText"), String.Empty);
        }
        set
        {
            SetValue("ResetButtonText", value);
        }
    }


    /// <summary>
    /// Enables or disables reset button.
    /// </summary>
    public bool DisplayResetButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayResetButton"), true);
        }
        set
        {
            SetValue("DisplayResetButton", value);
        }
    }


    /// <summary>
    /// Enables or disables add widget button.
    /// </summary>
    public bool DisplayAddButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayAddButton"), true);
        }
        set
        {
            SetValue("DisplayAddButton", value);
        }
    }


    /// <summary>
    /// Enables or disables confirmation for reset button.
    /// </summary>
    public bool ResetConfirmationRequired
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ResetConfirmationRequired"), true);
        }
        set
        {
            SetValue("ResetConfirmationRequired", value);
        }
    }


    /// <summary>
    /// Returns instance of tree provider.
    /// </summary>
    public TreeProvider TreeProvider
    {
        get
        {
            if (mTreeProvider == null)
            {
                mTreeProvider = new TreeProvider(MembershipContext.AuthenticatedUser);
            }
            return mTreeProvider;
        }
        set
        {
            mTreeProvider = value;
        }
    }

    #endregion


    #region "Private properties"

    /// <summary>
    /// Gets a value indicating whether the widget actions are enabled (checks permissions, workflow step...).
    /// </summary>
    private bool WidgetActionsEnabled
    {
        get
        {
            return (zoneType != WidgetZoneTypeEnum.Editor) || (PortalManager.ViewMode != ViewModeEnum.EditDisabled);
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            if (CurrentPageInfo != null)
            {
                PageInfo pi = CurrentPageInfo;

                // Make visible, visibility according to the current state will be set later (solves issue with changing visibility during postbacks)
                Visible = true;

                CMSPagePlaceholder parentPlaceHolder = PortalHelper.FindParentPlaceholder(this);

                // Nothing to render, nothing to do
                if ((!DisplayAddButton && !DisplayResetButton) ||
                    ((parentPlaceHolder != null) && (parentPlaceHolder.UsingDefaultPage || (parentPlaceHolder.PageInfo.DocumentID != pi.DocumentID))))
                {
                    Visible = false;
                    return;
                }

                var currentUser = MembershipContext.AuthenticatedUser;
                zoneType = WidgetZoneType.ToEnum<WidgetZoneTypeEnum>();


                // Check security
                if (((zoneType == WidgetZoneTypeEnum.Group) && !currentUser.IsGroupAdministrator(pi.NodeGroupID))
                    || ((zoneType == WidgetZoneTypeEnum.User || zoneType == WidgetZoneTypeEnum.Dashboard) && !AuthenticationHelper.IsAuthenticated()))
                {
                    Visible = false;
                    resetAllowed = false;
                    return;
                }

                // Displaying - Editor zone only in edit mode, User/Group zone only in Live site/Preview mode
                if (((zoneType == WidgetZoneTypeEnum.Editor) && !PortalManager.ViewMode.IsOneOf(ViewModeEnum.Edit, ViewModeEnum.EditDisabled, ViewModeEnum.EditLive))
                    || (((zoneType == WidgetZoneTypeEnum.User) || (zoneType == WidgetZoneTypeEnum.Group)) && !PortalManager.ViewMode.IsOneOf(ViewModeEnum.LiveSite, ViewModeEnum.Preview))
                    || ((zoneType == WidgetZoneTypeEnum.Dashboard) && ((PortalManager.ViewMode != ViewModeEnum.DashboardWidgets) || (String.IsNullOrEmpty(PortalContext.DashboardName)))))
                {
                    Visible = false;
                    resetAllowed = false;
                    return;
                }

                // Get current document
                TreeNode currentNode = DocumentHelper.GetDocument(pi.DocumentID, TreeProvider);
                if (((zoneType == WidgetZoneTypeEnum.Editor) && (!currentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Editor, SiteContext.CurrentSiteName) || (currentUser.IsAuthorizedPerDocument(currentNode, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied))))
                {
                    Visible = false;
                    resetAllowed = false;
                    return;
                }

                // If use checkin checkout enabled, check if document is checkout by current user
                if (zoneType == WidgetZoneTypeEnum.Editor)
                {
                    if (currentNode != null)
                    {
                        WorkflowManager wm = WorkflowManager.GetInstance(TreeProvider);
                        // Get workflow info
                        WorkflowInfo wi = wm.GetNodeWorkflow(currentNode);

                        // Check if node is under workflow and if use checkin checkout enabled
                        if ((wi != null) && (wi.UseCheckInCheckOut(SiteContext.CurrentSiteName)))
                        {
                            int checkedOutBy = currentNode.DocumentCheckedOutByUserID;

                            // Check if document is checkout by current user
                            if (checkedOutBy != MembershipContext.AuthenticatedUser.UserID)
                            {
                                Visible = false;
                                resetAllowed = false;
                                return;
                            }
                        }
                    }
                }

                // Find widget zone
                PageTemplateInfo pti = pi.UsedPageTemplateInfo;

                // ZodeID specified directly
                if (!String.IsNullOrEmpty(WidgetZoneID))
                {
                    zoneInstance = pti.TemplateInstance.GetZone(WidgetZoneID);
                }

                // Zone not find or specified zone is not of correct type
                if ((zoneInstance != null) && (zoneInstance.WidgetZoneType != zoneType))
                {
                    zoneInstance = null;
                }

                // For delete all variants all zones are necessary
                if (parentPlaceHolder != null)
                {
                    var zones = parentPlaceHolder.WebPartZones;
                    if (zones != null)
                    {
                        foreach (CMSWebPartZone zone in zones)
                        {
                            if ((zone.ZoneInstance != null) && (zone.ZoneInstance.WidgetZoneType == zoneType))
                            {
                                zoneInstances.Add(zone.ZoneInstance);
                                if (zoneInstance == null)
                                {
                                    zoneInstance = zone.ZoneInstance;
                                }
                            }
                        }
                    }
                }

                // No suitable zones on the page, nothing to do
                if (zoneInstance == null)
                {
                    Visible = false;
                    resetAllowed = false;
                    return;
                }

                // Adding is enabled
                if (DisplayAddButton)
                {
                    btnAddWidget.Visible = true;
                    btnAddWidget.Text = GetAddWidgetButtonText();

                    int templateId = 0;
                    if (pi.UsedPageTemplateInfo != null)
                    {
                        templateId = pi.UsedPageTemplateInfo.PageTemplateId;
                    }

                    var dialogParams = new LiveSiteWidgetsParameters(pi.NodeAliasPath, ViewMode)
                    {
                        ZoneId = zoneInstance.ZoneID,
                        TemplateId = templateId,
                    };
                    var hash = dialogParams.GetHashString();

                    addScript = (PortalContext.ViewMode == ViewModeEnum.EditLive ? "OEDeactivateWebPartBorder({ webPartSpanId: $cmsj('.OnSiteMenuTable').parent().attr('id').replace('OE_OE_', 'OE_')}, null );" : String.Empty) + "NewWidget(new zoneProperties('" + zoneInstance.ZoneID + "', '" + pi.NodeAliasPath + "', '" + templateId + "', undefined, undefined, undefined, undefined, '" + ViewMode + "', '" + hash + "')); return false;";
                    btnAddWidget.Attributes.Add("onclick", addScript);
                }

                // Reset is enabled
                if (DisplayResetButton)
                {
                    btnReset.Visible = true;
                    btnReset.Text = GetResetButtonText();
                    btnReset.Click += new EventHandler(btnReset_Click);

                    // Add confirmation if required
                    if (ResetConfirmationRequired)
                    {
                        btnReset.Attributes.Add("onclick", "if (!confirm(" + ScriptHelper.GetString(PortalHelper.LocalizeStringForUI("widgets.resetzoneconfirmtext")) + ")) return false;");
                    }
                }

                // Set the panel css clas with dependence on actions zone type
                switch (zoneType)
                {
                    // Editor
                    case WidgetZoneTypeEnum.Editor:
                        pnlWidgetActions.CssClass = "EditorWidgetActions";
                        break;

                    // User
                    case WidgetZoneTypeEnum.User:
                        pnlWidgetActions.CssClass = "UserWidgetActions";
                        break;

                    // Group
                    case WidgetZoneTypeEnum.Group:
                        pnlWidgetActions.CssClass = "GroupWidgetActions";
                        break;
                }
            }
        }
    }


    /// <summary>
    /// Load event handler.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        SetupControl();

        if ((PagePlaceholder != null)
            && (PagePlaceholder.PortalManager != null)
            && (PagePlaceholder.PortalManager.CurrentEditMenu != null))
        {
            PagePlaceholder.PortalManager.CurrentEditMenu.OnBeforeReloadMenu += CurrentEditMenu_OnBeforeReloadMenu;
        }
    }


    /// <summary>
    /// PreRender event handler.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        CssRegistration.RegisterBootstrap(Page);

        if (!WidgetActionsEnabled)
        {
            // Hide the control when document editing is not allowed (i.e. non-editable workflow step)
            Visible = false;
        }
    }


    /// <summary>
    /// Handles the OnBeforeReloadMenu event of the CurrentEditMenu control.
    /// </summary>
    protected void CurrentEditMenu_OnBeforeReloadMenu(object sender, EventArgs e)
    {
        if (!headerActionsLoaded)
        {
            headerActionsLoaded = true;

            // Register "Add widget button" and "Reset to default" button
            RegisterHeaderActionButtons();
        }
    }


    /// <summary>
    /// Removes document content for all editor widgets in a given document.
    /// </summary>
    /// <param name="treeNode">Document to update</param>
    /// <param name="pti">Page template instance</param>
    /// <param name="omitUpdate">Indicates whether update on a given document should be omitted</param>
    private void ClearEditorWidgetsContent(TreeNode treeNode, PageTemplateInstance pti, bool omitUpdate)
    {
        var documentWebPartsXml = treeNode.GetStringValue("DocumentWebParts", String.Empty);

        if (String.IsNullOrEmpty(documentWebPartsXml))
        {
            return;
        }

        var webPartIds = new List<string>();

        // Get web part IDs from DocumentWebParts column
        webPartIds.AddRange(GetWebPartIDs(documentWebPartsXml));

        // Get web part IDs from page template (widget zones only) and combine them with existing list. 
        webPartIds.AddRange(GetWebPartIDs(pti.GetZonesXML(WidgetZoneTypeEnum.Editor)));

        // Remove web part IDs from DocumentContent
        foreach (var wpId in webPartIds.Distinct())
        {
            treeNode.DocumentContent.EditableWebParts.Remove(wpId);
        }

        treeNode.SetValue("DocumentContent", treeNode.DocumentContent.GetContentXml());

        if (!omitUpdate)
        {
            DocumentHelper.UpdateDocument(treeNode, TreeProvider);
        }
    }


    /// <summary>
    /// Returns collection of web part IDs found within a given XML string. 
    /// </summary>
    /// <param name="sourceXml">Source XML</param>
    private IEnumerable<string> GetWebPartIDs(string sourceXml)
    {
        if (String.IsNullOrEmpty(sourceXml))
        {
            return Enumerable.Empty<string>();
        }

        var tempDoc = new XmlDocument();
        tempDoc.LoadXml(sourceXml);
        return tempDoc.SelectNodes("//webpart").Cast<XmlNode>()
            .Select(n => n.Attributes["controlid"].Value)
            .ToList();
    }


    /// <summary>
    /// Handles reset button click. Resets zones of specified type to default settings.
    /// </summary>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        // Disable the reset action when document editing is not allowed (i.e. non-editable workflow step)
        if (!WidgetActionsEnabled)
        {
            resetAllowed = false;
        }

        // Security check
        if (!DisplayResetButton || !resetAllowed)
        {
            return;
        }

        PageInfo pi = CurrentPageInfo;

        if (pi == null)
        {
            return;
        }

        if ((zoneType == WidgetZoneTypeEnum.Editor) || (zoneType == WidgetZoneTypeEnum.Group))
        {
            // Clear document webparts/group webparts
            TreeNode node = DocumentHelper.GetDocument(pi.DocumentID, TreeProvider);

            if (node != null)
            {
                bool updateDocument = true;

                if (zoneType == WidgetZoneTypeEnum.Editor)
                {
                    if (ViewMode.IsEdit(true) || ViewMode.IsEditLive())
                    {
                        // Do not save the document to the database, keep it in only in the memory
                        updateDocument = false;

                        // Get the default template widgets
                        PortalContext.SaveEditorWidgets(pi.DocumentID, pi.UsedPageTemplateInfo.TemplateInstance.GetZonesXML(WidgetZoneTypeEnum.Editor));
                    }
                    else
                    {
                        node.SetValue("DocumentWebParts", String.Empty);
                    }

                    ClearEditorWidgetsContent(node, pi.UsedPageTemplateInfo.TemplateInstance, updateDocument);

                    // Delete all variants 
                    if (pi.UsedPageTemplateInfo != null)
                    {
                        foreach (WebPartZoneInstance zoneInstance in zoneInstances)
                        {
                            if (zoneInstance.WebPartsContainVariants)
                            {
                                VariantHelper.DeleteWidgetVariants(zoneInstance.ZoneID, pi.UsedPageTemplateInfo.PageTemplateId, node.DocumentID);
                            }
                        }
                    }
                }
                else if (zoneType == WidgetZoneTypeEnum.Group)
                {
                    node.SetValue("DocumentGroupWebParts", String.Empty);
                }

                if (updateDocument)
                {
                    // Save the document
                    DocumentHelper.UpdateDocument(node, TreeProvider);
                }
            }
        }
        else if (zoneType == WidgetZoneTypeEnum.User)
        {
            // Delete user personalization info
            PersonalizationInfo up = PersonalizationInfoProvider.GetUserPersonalization(MembershipContext.AuthenticatedUser.UserID, pi.DocumentID);
            PersonalizationInfoProvider.DeletePersonalizationInfo(up);

            // Clear cached values
            TreeNode node = DocumentHelper.GetDocument(pi.DocumentID, TreeProvider);
            if (node != null)
            {
                node.ClearCache();
            }
        }
        else if (zoneType == WidgetZoneTypeEnum.Dashboard)
        {
            // Delete user personalization info
            PersonalizationInfo up = PersonalizationInfoProvider.GetDashBoardPersonalization(MembershipContext.AuthenticatedUser.UserID, PortalContext.DashboardName, PortalContext.DashboardSiteName);
            PersonalizationInfoProvider.DeletePersonalizationInfo(up);

            // Clear cached page template
            if (pi.UsedPageTemplateInfo != null)
            {
                CacheHelper.TouchKey("cms.pagetemplate|byid|" + pi.UsedPageTemplateInfo.PageTemplateId);
            }
        }

        // Make redirect to see changes after load
        string url = RequestContext.CurrentURL;

        if (ViewMode.IsEdit(true) || ViewMode.IsEditLive())
        {
            // Ensure that the widgets will be loaded from the session layer (not from database) 
            url = URLHelper.UpdateParameterInUrl(url, "cmscontentchanged", "true");
        }

        URLHelper.Redirect(url);
    }


    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        SetupControl();
        base.ReloadData();
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Registers the header action buttons.
    /// </summary>
    private void RegisterHeaderActionButtons()
    {
        // Place actions to the main menu if required
        if (UseMainMenu && Visible)
        {
            // Try get current menu
            EditMenu em = PagePlaceholder.PortalManager.CurrentEditMenu;
            if (em != null)
            {
                // Add button
                if (DisplayAddButton)
                {
                    HeaderAction ha = new HeaderAction()
                    {
                        Enabled = WidgetActionsEnabled,
                        Text = GetAddWidgetButtonText(),
                        OnClientClick = addScript,
                        Tooltip = PortalHelper.LocalizeStringForUI("addwidget.tooltip"),
                        GenerateSeparatorBeforeAction = true,
                        ButtonStyle = ButtonStyle.Default
                    };

                    btnAddWidget.Visible = false;
                    em.AddExtraAction(ha);
                }

                // Reset button
                if (DisplayResetButton)
                {
                    HeaderAction ha = new HeaderAction
                    {
                        Enabled = WidgetActionsEnabled,
                        Text = GetResetButtonText(),
                        OnClientClick = "if (!confirm(" + ScriptHelper.GetString(PortalHelper.LocalizeStringForUI("widgets.resetzoneconfirmtext")) + ")) { return false; } else { " + ControlsHelper.GetPostBackEventReference(this, "reset") + " }",
                        Tooltip = PortalHelper.LocalizeStringForUI("resetwidget.tooltip"),
                        GenerateSeparatorBeforeAction = !DisplayAddButton,
                        ButtonStyle = ButtonStyle.Default
                    };

                    btnReset.Visible = false;
                    em.AddExtraAction(ha);
                }

                // Hide empty widget action panel
                pnlWidgetActions.Visible = false;
            }
        }
    }


    /// <summary>
    /// Gets the add widget button text.
    /// </summary>
    /// <returns></returns>
    private string GetAddWidgetButtonText()
    {
        return DataHelper.GetNotEmpty(AddButtonText, PortalHelper.LocalizeStringForUI("widgets.addwidget"));
    }


    /// <summary>
    /// Gets the reset button text.
    /// </summary>
    /// <returns></returns>
    private string GetResetButtonText()
    {
        return DataHelper.GetNotEmpty(ResetButtonText, PortalHelper.LocalizeStringForUI("widgets.resettodefault"));
    }

    #endregion


    #region "PostBack event"

    /// <summary>
    /// Raises the post back event.
    /// </summary>
    /// <param name="eventArgument">Event argument</param>
    public void RaisePostBackEvent(string eventArgument)
    {
        SecurityHelper.LogScreenLockAction();

        switch (eventArgument)
        {
            case "reset":
                // Reset_click handler must be handled by PostabackEvent due to registering the reset button later in the page life cycle (when changing workflow steps)
                btnReset_Click(this, null);
                break;

            default:
                break;
        }
    }

    #endregion
}
