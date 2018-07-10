using System;
using System.Web;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.DeviceProfiles;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.Membership;
using CMS.Modules;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.Search;
using CMS.SiteProvider;
using CMS.UIControls;

using TreeNode = CMS.DocumentEngine.TreeNode;


public partial class CMSModules_Content_Controls_MasterPage : CMSPreviewControl
{
    #region "Variables"

    protected TreeNode node;
    protected TreeProvider tree;
    protected PageTemplateInfo pti;

    protected string mHead;
    protected string mBody;

    #endregion


    #region "Properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return pnlMessagePlaceholder;
        }
    }

    #endregion


    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ScriptHelper.RegisterClientScriptBlock(Page, typeof(String), "DivRegistration", "if (parent.InitScrollDivs != null) {parent.InitScrollDivs('" + pnlBody.ClientID + "');}", true);

        // Register save event
        ComponentEvents.RequestEvents.RegisterForEvent(ComponentEvents.SAVE, (s, args) => SaveData());

        if (PortalUIHelper.DisplaySplitMode)
        {
            ShowPreview = false;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        var previewState = GetPreviewStateFromCookies(MASTERPAGE);

        // Keep current user
        var user = MembershipContext.AuthenticatedUser;

        // Get document node
        tree = new TreeProvider(user);
        node = UIContext.EditedObject as TreeNode;

        // Register the dialog script
        ScriptHelper.RegisterDialogScript(Page);

        // Register save changes
        ScriptHelper.RegisterSaveChanges(Page);

        // Save changes support
        bool confirmChanges = SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".CMSConfirmChanges");
        string script = string.Empty;
        if (confirmChanges)
        {
            script = "CMSContentManager.confirmLeave=" + ScriptHelper.GetString(ResHelper.GetString("Content.ConfirmLeave", user.PreferredUICultureCode), true, false) + "; \n";
            script += "CMSContentManager.confirmLeaveShort=" + ScriptHelper.GetString(ResHelper.GetString("Content.ConfirmLeaveShort", user.PreferredUICultureCode), true, false) + "; \n";
        }
        else
        {
            script += "CMSContentManager.confirmChanges = false;";
        }

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "saveChangesScript", script, true);

        try
        {
            if (node != null)
            {
                DocumentContext.CurrentPageInfo = PageInfoProvider.GetPageInfo(node.NodeSiteName, node.NodeAliasPath, node.DocumentCulture, null, node.NodeID, false);

                // Title
                string title = DocumentContext.CurrentTitle;
                if (!string.IsNullOrEmpty(title))
                {
                    title = "<title>" + title + "</title>";
                }
                
                // Body class
                string bodyCss = DocumentContext.CurrentBodyClass;
                
                if (bodyCss != null && bodyCss.Trim() != "")
                {
                    bodyCss = "class=\"" + bodyCss + "\"";
                }
                else
                {
                    bodyCss = "";
                }

                // Metadata
                string meta = "<meta http-equiv=\"pragma\" content=\"no-cache\" />";

                string description = DocumentContext.CurrentDescription;
                if (description != "")
                {
                    meta += "<meta name=\"description\" content=\"" + description + "\" />";
                }

                string keywords = DocumentContext.CurrentKeyWords;
                if (keywords != "")
                {
                    meta += "<meta name=\"keywords\"  content=\"" + keywords + "\" />";
                }

                // Site style sheet
                string cssSiteSheet = "";

                int stylesheetId = DocumentContext.CurrentPageInfo.DocumentStylesheetID;

                CssStylesheetInfo cssInfo = CssStylesheetInfoProvider.GetCssStylesheetInfo((stylesheetId > 0) ? stylesheetId : SiteContext.CurrentSite.SiteDefaultStylesheetID);

                if (cssInfo != null)
                {
                    cssSiteSheet = CssLinkHelper.GetCssFileLink(CssLinkHelper.GetStylesheetUrl(cssInfo.StylesheetName));
                }

                // Theme CSS files
                string themeCssFiles = "";
                if (cssInfo != null)
                {
                    try
                    {
                        string directory = URLHelper.GetPhysicalPath(string.Format("~/App_Themes/{0}/", cssInfo.StylesheetName));
                        if (Directory.Exists(directory))
                        {
                            foreach (string file in Directory.GetFiles(directory, "*.css"))
                            {
                                themeCssFiles += CssLinkHelper.GetCssFileLink(CssLinkHelper.GetPhysicalCssUrl(cssInfo.StylesheetName, Path.GetFileName(file)));
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                // Add values to page
                mHead = FormatHTML(HighlightHTML(title + meta + cssSiteSheet + themeCssFiles), 2);
                mBody = bodyCss;
            }
        }
        catch
        {
            ShowError(GetString("MasterPage.PageEditErr"));
        }

        LoadData();

        // Add save action
        SaveAction save = new SaveAction();
        save.CommandArgument = ComponentEvents.SAVE_DATA;
        save.CommandName = ComponentEvents.SAVE_DATA;

        headerActions.ActionsList.Add(save);

        if (pti != null)
        {
            // Disable buttons for no-template
            bool actionsEnabled = (pti.PageTemplateId > 0);

            // Edit layout
            HeaderAction action = new HeaderAction
            {
                Text = GetString("content.ui.pagelayout"),
                Tooltip = GetString("pageplaceholder.editlayouttooltip"),
                OnClientClick = "EditLayout();return false;",
                Enabled = actionsEnabled
            };
            headerActions.ActionsList.Add(action);

            string elemUrl = ApplicationUrlHelper.GetElementDialogUrl("cms.design", "PageTemplate.EditPageTemplate", pti.PageTemplateId);

            // Edit page properties action
            action = new HeaderAction
            {
                Text = GetString("PageProperties.EditTemplateProperties"),
                Tooltip = GetString("PageProperties.EditTemplateProperties"),
                OnClientClick = "modalDialog('" + elemUrl + "', 'TemplateSelection', '85%', '85%', false);return false;",
                Enabled = actionsEnabled
            };

            CMSPagePlaceholder.RegisterEditLayoutScript(this, pti.PageTemplateId, node.NodeAliasPath, null);
            headerActions.ActionsList.Add(action);

            // Preview
            HeaderAction preview = new HeaderAction
            {
                Text = GetString("general.preview"),
                OnClientClick = "performToolbarAction('split');return false;",
                Visible = ((previewState == 0) && !PortalUIHelper.DisplaySplitMode),
                Tooltip = GetString("preview.tooltip")
            };
            headerActions.ActionsList.Add(preview);

            headerActions.ActionPerformed += headerActions_ActionPerformed;
        }

        RegisterInitScripts(pnlBody.ClientID, pnlMenu.ClientID, false);
    }


    /// <summary>
    /// Action performed
    /// </summary>
    void headerActions_ActionPerformed(object sender, CommandEventArgs e)
    {
        if (e.CommandName == ComponentEvents.SAVE_DATA)
        {
            SaveData();
        }
    }


    public void LoadData()
    {
        if (node != null)
        {
            string layoutText = String.Empty;

            // get template info
            pti = PageTemplateInfoProvider.GetPageTemplateInfo(node.GetUsedPageTemplateId());
            if ((pti == null) && (DocumentContext.CurrentPageInfo != null) && (DocumentContext.CurrentPageInfo.UsedPageTemplateInfo != null))
            {
                pti = DocumentContext.CurrentPageInfo.UsedPageTemplateInfo;
            }

            if (pti != null)
            {
                PageTemplateLayoutTypeEnum type = PageTemplateLayoutTypeEnum.PageTemplateLayout;
                // Try get device layout
                object layoutObject = PageTemplateDeviceLayoutInfoProvider.GetLayoutObject(pti, DeviceContext.CurrentDeviceProfile, out type);
                layoutText = pti.PageTemplateLayout;

                // Set layout text with dependence on current layout type
                switch (type)
                {
                    // Shared layouts
                    case PageTemplateLayoutTypeEnum.SharedLayout:
                    case PageTemplateLayoutTypeEnum.DeviceSharedLayout:
                    case PageTemplateLayoutTypeEnum.SharedLayoutMapped:
                        layoutText = (layoutObject as LayoutInfo).LayoutCode;
                        break;

                    // Custom device layout
                    case PageTemplateLayoutTypeEnum.DeviceLayout:
                        layoutText = (layoutObject as PageTemplateDeviceLayoutInfo).LayoutCode;
                        break;
                }
            }
            ltlLayoutCode.Text = HTMLHelper.EnsureHtmlLineEndings(HTMLHelper.HighlightHTML(layoutText));
            ltlLayoutCode.Text = RegexHelper.GetRegex("[ ](?![^<>]*>)").Replace(ltlLayoutCode.Text, "&nbsp;");

            // Load node data
            if (!RequestHelper.IsPostBack())
            {
                txtBodyCss.Text = node.NodeBodyElementAttributes;
                txtBodyScripts.Value = node.NodeBodyScripts;
                txtDocType.Text = node.NodeDocType;
                txtHeadTags.Value = node.NodeHeadTags;
            }
        }

        lblAfterDocType.Text = HighlightHTML("<html>") + "<br />" + AddSpaces(1) + HighlightHTML("<head>");
        lblAfterHeadTags.Text = AddSpaces(1) + HighlightHTML("</head>");
        lblAfterLayout.Text = AddSpaces(1) + HighlightHTML("</body>") + "<br />" + HighlightHTML("</html>");
        lblBodyEnd.Text = HighlightHTML(">");
        lblBodyStart.Text = AddSpaces(1) + HighlightHTML("<body " + HttpUtility.HtmlDecode(mBody));
    }


    protected override void OnPreRender(EventArgs e)
    {
        // Check whether virtual objects are allowed
        if (!SettingsKeyInfoProvider.VirtualObjectsAllowed)
        {
            ShowWarning(GetString("VirtualPathProvider.NotRunning"));
        }

        base.OnPreRender(e);
    }

    /// <summary>
    /// Format HTML text.
    /// </summary>
    /// <param name="inputHTML">Input HTML</param>
    /// <param name="level">Indentation level</param>
    public string FormatHTML(string inputHTML, int level)
    {
        return AddSpaces(level) + inputHTML.Replace(HttpUtility.HtmlEncode(">"), HttpUtility.HtmlEncode(">") + "<br />" + AddSpaces(level));
    }


    /// <summary>
    /// Add spaces.
    /// </summary>
    /// <param name="level">Indentation level</param>
    public string AddSpaces(int level)
    {
        string toReturn = "";
        for (int i = 0; i < level * 2; i++)
        {
            toReturn += "&nbsp;";
        }

        return toReturn;
    }


    /// <summary>
    /// Highlight HTML.
    /// </summary>
    /// <param name="inputHtml">Input HTML</param>
    public string HighlightHTML(string inputHtml)
    {
        return HTMLHelper.HighlightHTML(inputHtml);
    }


    private void SaveData()
    {
        if (node != null)
        {
            // Update fields
            node.NodeBodyElementAttributes = txtBodyCss.Text;
            node.NodeBodyScripts = txtBodyScripts.Value.ToString();
            node.NodeDocType = txtDocType.Text;
            node.NodeHeadTags = txtHeadTags.Value.ToString();

            // Update the node
            node.Update();

            // Update search index
            if (DocumentHelper.IsSearchTaskCreationAllowed(node))
            {
                SearchTaskInfoProvider.CreateTask(SearchTaskTypeEnum.Update, TreeNode.OBJECT_TYPE, SearchFieldsConstants.ID, node.GetSearchID(), node.DocumentID);
            }

            // Log synchronization
            DocumentSynchronizationHelper.LogDocumentChange(node, TaskTypeEnum.UpdateDocument, tree);

            RegisterRefreshScript();

            // Empty variable for exitwithoutchanges dialog
            ScriptHelper.RegisterClientScriptBlock(Page, typeof(String), "SubmitAction", "CMSContentManager.changed(false);", true);

            // Clear cache
            PageInfoCacheHelper.ClearCache();

            ShowChangesSaved();

            // Clear content changed flag
            DocumentManager.ClearContentChanged();
        }
    }

    #endregion
}
