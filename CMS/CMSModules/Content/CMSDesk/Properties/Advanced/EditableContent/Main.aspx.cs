using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.CKEditor.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;

using TreeNode = CMS.DocumentEngine.TreeNode;


public partial class CMSModules_Content_CMSDesk_Properties_Advanced_EditableContent_Main : CMSModalPage
{
    #region "Private variables"

    private const string EDITABLE_CONTENT_FOLDER = "~/CMSModules/Content/CMSDesk/Properties/Advanced/EditableContent/";

    private string keyName;
    private EditableContentType keyType;

    protected TreeNode node = null;
    private string content;
    private bool createNew;

    private Control invokeControl;

    private enum EditingForms
    {
        EditableImage = 0,
        HTMLEditor = 1,
        TextArea = 2,
        TextBox = 3
    };

    private enum EditableContentType
    {
        webpart = 0,
        region = 1
    };

    #endregion


    #region "Page events"

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        // Ensure document manager
        EnsureDocumentManager = true;
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        var user = MembershipContext.AuthenticatedUser;

        // Check 'read' permissions
        if (!user.IsAuthorizedPerResource("CMS.Content", "Read"))
        {
            RedirectToAccessDenied("CMS.Content", "Read");
        }

        // Check UIProfile
        if (!user.IsAuthorizedPerUIElement("CMS.Content", "Properties.General"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties.General");
        }

        if (!user.IsAuthorizedPerUIElement("CMS.Content", "General.Advanced"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "General.Advanced");
        }

        // Init document manager events
        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.OnAfterAction += DocumentManager_OnAfterAction;
        DocumentManager.OnValidateData += DocumentManager_OnValidateData;

        // Initialize node
        node = DocumentManager.Node;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!RequestHelper.IsPostBack())
        {
            // Fill dropdown list
            InitEditorOptions();

            // Inform user about saving
            if (QueryHelper.GetBoolean("imagesaved", false))
            {
                ShowChangesSaved();
                drpEditControl.SelectedIndex = 1;
            }
        }

        // Initialize HTML editor
        InitHTMLEditor();

        // Find post back invoker
        string invokerName = Page.Request.Params.Get(postEventSourceID);
        invokeControl = !string.IsNullOrEmpty(invokerName) ? Page.FindControl(invokerName) : null;

        // Set whether new item is to be created
        createNew = QueryHelper.GetBoolean("createNew", false);

        if (invokerName != null)
        {
            if (createNew && (invokeControl == drpEditControl))
            {
                createNew = true;
            }
            else
            {
                if (invokeControl == drpEditControl)
                {
                    createNew = false;
                }
            }
        }


        // Get query parameters
        keyName = QueryHelper.GetString("nodename", string.Empty);

        // Set editable content type enumeration
        switch (QueryHelper.GetString("nodetype", "webpart"))
        {
            case "webpart":
                keyType = EditableContentType.webpart;
                break;

            case "region":
                keyType = EditableContentType.region;
                break;
        }

        // Clear error
        ShowError("");


        // Show editing controls
        if ((createNew || (keyName != string.Empty)))
        {
            menuElem.Visible = true;
            menuElem.ShowSave = true;
            pnlEditableContent.Visible = true;
        }

        // Get content
        if (node != null)
        {
            // Initialize java scripts
            ltlScript.Text += ScriptHelper.GetScript("mainUrl = '" + ResolveUrl(EDITABLE_CONTENT_FOLDER + "main.aspx") + "?nodeid=" + node.NodeID + "';");

            if ((keyName != string.Empty) || createNew)
            {
                if (!RequestHelper.IsPostBack() && !createNew)
                {
                    txtName.Text = EditableItems.GetFirstKey(keyName);
                }

                if (!createNew)
                {
                    content = GetContent();

                    // Disable HTML editor if content is type of image
                    if (content != null)
                    {
                        if (content.StartsWithCSafe("<image>"))
                        {
                            ListItem li = drpEditControl.Items.FindByValue(Convert.ToInt32(EditingForms.HTMLEditor).ToString());
                            if (li != null)
                            {
                                drpEditControl.Items.Remove(li);
                            }
                        }
                    }
                }
            }
        }

        // Show div with content controls
        advancedEditables.Visible = true;
        // Hide all content controls
        txtAreaContent.Visible = txtContent.Visible = htmlContent.Visible = imageContent.Visible = false;

        // Set up editing forms
        switch (((EditingForms)Convert.ToInt32(drpEditControl.SelectedValue)))
        {
            case EditingForms.TextArea:
                txtAreaContent.Visible = true;
                break;

            case EditingForms.HTMLEditor:
                htmlContent.Visible = true;
                break;

            case EditingForms.EditableImage:
                imageContent.ViewMode = ViewModeEnum.Edit;
                imageContent.Visible = true;
                imageContent.ImageTitle = HTMLHelper.HTMLEncode(EditableItems.GetFirstKey(keyName));
                break;

            case EditingForms.TextBox:
                advancedEditables.Visible = false;
                txtContent.Visible = true;
                break;
        }
        // Set visibility of div based on content controls
        advancedEditables.Visible = txtAreaContent.Visible || htmlContent.Visible || imageContent.Visible;
        lblContent.Visible = txtContent.Visible;
    }


    protected override void OnPreRender(EventArgs e)
    {
        if (node != null)
        {
            // Load content to controls
            bool isMenuAction = false;
            foreach (Control control in menuElem.Controls)
            {
                if (control == invokeControl && control != menuElem.FindControl("btnSave"))
                {
                    isMenuAction = true;
                }
            }

            // Load content to text fields
            if (!RequestHelper.IsPostBack() || (invokeControl == drpEditControl) || isMenuAction)
            {
                txtAreaContent.Text = content;
                htmlContent.ResolvedValue = content;
                txtContent.Text = content;
            }

            if (DocumentManager.AllowSave)
            {
                SetEnableMode(true);
                LoadImageData(ViewModeEnum.Edit);
            }
            else
            {
                SetEnableMode(false);
                LoadImageData(ViewModeEnum.EditDisabled);
            }

            if (!string.IsNullOrEmpty(keyName))
            {
                // Prepare script for refresh menu in Tree
                string script = "parent.frames['tree'].location.replace('" +
                                ResolveUrl(EDITABLE_CONTENT_FOLDER + "tree.aspx") + "?nodeid=" +
                                node.NodeID + "&selectednodename=" + ScriptHelper.GetString(keyName, false) + "&selectednodetype=" + keyType + "');";

                // Script for UpdateMenu in tree
                ScriptHelper.RegisterStartupScript(this, typeof(string), "RefreshTree", ScriptHelper.GetScript(script));
            }

            if (!DocumentManager.AllowSave)
            {
                SetEnableMode(false);
            }
        }

        base.OnPreRender(e);
    }

    #endregion


    #region "Button handling"

    private void DocumentManager_OnValidateData(object sender, DocumentManagerEventArgs e)
    {
        string codeName = txtName.Text.Trim();
        // Validate
        string errorMessage = new Validator().NotEmpty(codeName, GetString("general.invalidcodename")).IsRegularExp(codeName, ValidationHelper.CodenameRegExp.ToString(), GetString("general.invalidcodename")).Result;
        if (errorMessage == string.Empty)
        {
            string value = null;
            EditingForms editingForm = (EditingForms)Convert.ToInt32(drpEditControl.SelectedValue);

            // Check content
            switch (editingForm)
            {
                case EditingForms.TextArea:
                    value = txtAreaContent.Text.Trim();
                    break;

                case EditingForms.HTMLEditor:
                    value = htmlContent.ResolvedValue;
                    break;

                case EditingForms.EditableImage:
                    value = imageContent.GetContent();
                    break;

                case EditingForms.TextBox:
                    value = txtContent.Text.Trim();
                    break;
            }

            errorMessage = new Validator().NotEmpty(value, GetString("EditableContent.NotEmpty")).Result;
        }

        if (!String.IsNullOrEmpty(errorMessage))
        {
            e.ErrorMessage = errorMessage;
            e.IsValid = false;
        }
    }


    private void DocumentManager_OnAfterAction(object sender, DocumentManagerEventArgs e)
    {
        EditingForms editingForm = (EditingForms)Convert.ToInt32(drpEditControl.SelectedValue);

        // Code name
        string codeName = txtName.Text.Trim().ToLowerCSafe();

        if (txtName.Text != string.Empty)
        {
            keyName = codeName;
        }

        // Refresh tree
        if (createNew)
        {
            ltlScript.Text +=
                ScriptHelper.GetScript("parent.frames['tree'].location.replace('" +
                                       ResolveUrl(EDITABLE_CONTENT_FOLDER + "tree.aspx") + "?nodeid=" +
                                       node.NodeID + "&selectednodename=" + ScriptHelper.GetString(codeName, false) + "&selectednodetype=" + keyType +
                                       "');SelectNode('" + ScriptHelper.GetString(codeName, false) + "', '" + keyType + "')");
            if (editingForm == EditingForms.EditableImage)
            {
                ltlScript.Text +=
                    ScriptHelper.GetScript("SelectNodeAfterImageSave(" + ScriptHelper.GetString(keyName) + ", '" + keyType + "');");
            }
        }
        else
        {
            if (e.ActionName == DocumentComponentEvents.UNDO_CHECKOUT)
            {
                // Refresh content
                content = GetContent();
                txtAreaContent.Text = content;
                htmlContent.ResolvedValue = content;
                txtContent.Text = content;
            }

            ltlScript.Text +=
                ScriptHelper.GetScript("RefreshNode(" + ScriptHelper.GetString(codeName) + ", '" + keyType + "', " + node.NodeID + ");");
        }
        createNew = false;
    }


    private void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        EditingForms editingForm = (EditingForms)Convert.ToInt32(drpEditControl.SelectedValue);
        // Get content to save
        switch (editingForm)
        {
            case EditingForms.TextArea:
                content = txtAreaContent.Text.Trim();
                break;

            case EditingForms.HTMLEditor:
                content = htmlContent.ResolvedValue;
                break;

            case EditingForms.EditableImage:
                content = imageContent.GetContent();
                break;

            case EditingForms.TextBox:
                content = txtContent.Text.Trim();
                break;
        }

        if (!QueryHelper.GetBoolean("imagesaved", false))
        {
            createNew = !node.DocumentContent.EditableWebParts.Contains(keyName) &&
                        !node.DocumentContent.EditableRegions.ContainsKey(keyName);
        }

        // Code name
        string codeName = txtName.Text.Trim().ToLowerCSafe();

        // Set PageInfo
        switch (keyType)
        {
            case EditableContentType.webpart:
                if (!createNew)
                {
                    // If editing -> remove old
                    node.DocumentContent.EditableWebParts.Remove(keyName);
                }

                if (!node.DocumentContent.EditableWebParts.ContainsKey(codeName))
                {
                    node.DocumentContent.EditableWebParts.Add(codeName, content);
                }
                else
                {
                    ShowError(GetString("EditableContent.ItemExists"));
                    return;
                }
                break;

            case EditableContentType.region:
                if (!createNew)
                {
                    // If editing -> remove old
                    node.DocumentContent.EditableRegions.Remove(keyName);
                }

                if (!node.DocumentContent.EditableRegions.ContainsKey(codeName))
                {
                    node.DocumentContent.EditableRegions.Add(codeName, content);
                }
                else
                {
                    ShowError(GetString("EditableContent.ItemExists"));
                    return;
                }
                break;
        }

        node.SetValue("DocumentContent", node.DocumentContent.GetContentXml());
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Loads data to editable image.
    /// </summary>
    protected void LoadImageData(ViewModeEnum mode)
    {
        // Set view mode
        PortalContext.ViewMode = mode;

        // Initialize editable image properties
        imageContent.DisplaySelectorTextBox = false;
        imageContent.SelectOnlyPublished = false;

        // Ensure loading image
        if (!string.IsNullOrEmpty(content))
        {
            if (content.StartsWithCSafe("<image>"))
            {
                // Initialize editable image
                imageContent.LoadContent(content);
            }
        }
    }


    /// <summary>
    /// Initializes HTML editor's settings.
    /// </summary>
    protected void InitHTMLEditor()
    {
        htmlContent.AutoDetectLanguage = false;
        htmlContent.DefaultLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        // Set direction
        htmlContent.ContentsLangDirection = LanguageDirection.LeftToRight;
        if (CultureHelper.IsPreferredCultureRTL())
        {
            htmlContent.ContentsLangDirection = LanguageDirection.RightToLeft;
        }
        if (SiteContext.CurrentSite != null)
        {
            htmlContent.EditorAreaCSS = PortalHelper.GetHtmlEditorAreaCss(SiteContext.CurrentSiteName);
        }
    }


    /// <summary>
    /// Initializes dropdown list with editing options.
    /// </summary>
    protected void InitEditorOptions()
    {
        drpEditControl.Items.AddRange(new[]
                                          {
                                              new ListItem(GetString("EditableContent.HTMLEditor"), Convert.ToInt32(EditingForms.HTMLEditor).ToString()),
                                              new ListItem(GetString("EditableContent.EditableImage"), Convert.ToInt32(EditingForms.EditableImage).ToString()),
                                              new ListItem(GetString("EditableContent.TextArea"), Convert.ToInt32(EditingForms.TextArea).ToString()),
                                              new ListItem(GetString("EditableContent.TextBox"), Convert.ToInt32(EditingForms.TextBox).ToString())
                                          });
    }


    /// <summary>
    /// Gets editable element content.
    /// </summary>
    protected string GetContent()
    {
        string content = null;
        if ((node != null) && !String.IsNullOrEmpty(keyName))
        {
            // Set content variable
            switch (keyType)
            {
                case EditableContentType.webpart:
                    if (node.DocumentContent.EditableWebParts.ContainsKey(keyName))
                    {
                        content =
                            ValidationHelper.GetString(
                                node.DocumentContent.EditableWebParts[keyName], string.Empty);
                    }
                    break;

                case EditableContentType.region:
                    if (node.DocumentContent.EditableRegions.ContainsKey(keyName))
                    {
                        content =
                            ValidationHelper.GetString(
                                node.DocumentContent.EditableRegions[keyName], string.Empty);
                    }
                    break;
            }
        }
        return content;
    }


    /// <summary>
    /// Sets enable mode of controls.
    /// </summary>
    /// <param name="enableMode">Value of enable mode</param>
    protected void SetEnableMode(bool enableMode)
    {
        // Set data controls mode
        lblEditControl.Enabled = enableMode;
        drpEditControl.Enabled = enableMode;
        lblName.Enabled = enableMode;
        txtName.Enabled = enableMode;

        // Set content preview mode
        txtAreaContent.Enabled = enableMode;
        htmlContent.Enabled = enableMode;
        lblContent.Enabled = enableMode;
        txtContent.Enabled = enableMode;
    }


    /// <summary>
    /// Adds the script to the output request window.
    /// </summary>
    /// <param name="script">Script to add</param>
    public override void AddScript(string script)
    {
        ScriptHelper.RegisterStartupScript(this, typeof(string), script.GetHashCode().ToString(), ScriptHelper.GetScript(script));
    }

    #endregion
}
