using System;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSModules_Content_CMSDesk_New_NewFile : CMSContentPage
{
    #region "Variables"

    private FormFieldInfo mFieldInfo;
    private FormFieldInfo mFileNameFieldInfo;
    private DataClassInfo mDataClass;
    private int mResizeToWidth;
    private int mResizeToHeight;
    private int mResizeToMaxSideSize;
    private string mAllowedExtensions;
    private string fileExtension;

    #endregion


    #region "Properties"

    /// <summary>
    /// Class field info.
    /// </summary>
    public FormFieldInfo FieldInfo
    {
        get
        {
            if (mFieldInfo == null)
            {
                FormInfo fi = FormHelper.GetFormInfo(DataClass.ClassName, true);

                // Get valid extensions from form field info
                mFieldInfo = fi.GetFormField("FileAttachment");
            }
            return mFieldInfo;
        }
    }


    /// <summary>
    /// Class field info for file name field.
    /// </summary>
    public FormFieldInfo FileNameFieldInfo
    {
        get
        {
            if (mFileNameFieldInfo == null)
            {
                FormInfo fi = FormHelper.GetFormInfo(DataClass.ClassName, true);

                // Get valid extensions from form field info
                mFileNameFieldInfo = fi.GetFormField("FileName");
            }
            return mFileNameFieldInfo;
        }
    }


    /// <summary>
    /// Data class info for the file type.
    /// </summary>
    public DataClassInfo DataClass
    {
        get
        {
            if (mDataClass == null)
            {
                // Get file document type settings
                mDataClass = DataClassInfoProvider.GetDataClassInfo(SystemDocumentTypes.File);
                if (mDataClass == null)
                {
                    throw new DocumentTypeNotExistsException("Class for file type is missing!");
                }
            }
            return mDataClass;
        }
    }


    /// <summary>
    /// Unique GUID.
    /// </summary>
    public Guid Guid
    {
        get
        {
            Guid guid = ValidationHelper.GetGuid(ViewState["Guid"], Guid.Empty);
            if (guid == Guid.Empty)
            {
                guid = Guid.NewGuid();
                ViewState["Guid"] = guid;
            }
            return guid;
        }
    }


    /// <summary>
    /// Resize to width.
    /// </summary>
    private int ResizeToWidth
    {
        get
        {
            return mResizeToWidth;
        }
        set
        {
            mResizeToWidth = value;
        }
    }


    /// <summary>
    /// Resize to height.
    /// </summary>
    private int ResizeToHeight
    {
        get
        {
            return mResizeToHeight;
        }
        set
        {
            mResizeToHeight = value;
        }
    }


    /// <summary>
    /// Resize to maximal side size.
    /// </summary>
    private int ResizeToMaxSideSize
    {
        get
        {
            return mResizeToMaxSideSize;
        }
        set
        {
            mResizeToMaxSideSize = value;
        }
    }


    /// <summary>
    /// Allowed extensions.
    /// </summary>
    private string AllowedExtensions
    {
        get
        {
            return mAllowedExtensions;
        }
        set
        {
            mAllowedExtensions = value;
        }
    }
    
    #endregion


    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        DocumentManager.OnSaveData += DocumentManager_OnSaveData;
        DocumentManager.OnValidateData += DocumentManager_OnValidateData;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Check if required fields exist ("FileName", "FileAttachment")
        if ((FieldInfo == null) || (FileNameFieldInfo == null))
        {
            ShowError(GetString("NewFile.SomeOfRequiredFieldsMissing"));
            pnlForm.Visible = false;
            return;
        }

        // Register progress script
        ScriptHelper.RegisterLoader(Page);

        if (!LicenseHelper.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.Documents, ObjectActionEnum.Insert))
        {
            RedirectToAccessDenied(String.Format(GetString("cmsdesk.documentslicenselimits"), ""));
        }

        // Check if need template selection, if so, then redirect to template selection page
        if (DataClass.ClassShowTemplateSelection)
        {
            URLHelper.Redirect("~/CMSModules/Content/CMSDesk/TemplateSelection.aspx" + RequestContext.CurrentQueryString);
        }

        DocumentManager.Mode = FormModeEnum.Insert;
        DocumentManager.ParentNodeID = QueryHelper.GetInteger("parentnodeid", 0);
        DocumentManager.NewNodeClassName = DataClass.ClassName;

        TreeNode parentNode = DocumentManager.ParentNode;
        if (!DocumentHelper.IsDocumentTypeAllowed(parentNode, DataClass.ClassID))
        {
            ShowError(GetString("Content.ChildClassNotAllowed"));
            pnlForm.Visible = false;
            return;
        }

        if (!MembershipContext.AuthenticatedUser.IsAuthorizedToCreateNewDocument(DocumentManager.ParentNode, DataClass.ClassName))
        {
            ShowError(GetString("Content.NotAuthorizedFile"));
            pnlForm.Visible = false;
            return;
        }

        // Set request timeout
        Server.ScriptTimeout = AttachmentHelper.ScriptTimeout;

        InitializeProperties();

        // Init direct uploader
        ucDirectUploader.GUIDColumnName = FieldInfo.Name;
        ucDirectUploader.AllowDelete = FieldInfo.AllowEmpty;
        ucDirectUploader.FormGUID = Guid;
        ucDirectUploader.ResizeToHeight = ResizeToHeight;
        ucDirectUploader.ResizeToWidth = ResizeToWidth;
        ucDirectUploader.ResizeToMaxSideSize = ResizeToMaxSideSize;
        ucDirectUploader.AllowedExtensions = AllowedExtensions;
        ucDirectUploader.NodeParentNodeID = DocumentManager.ParentNode.NodeID;
        ucDirectUploader.IsLiveSite = false;

        lblFileDescription.Text = GetString("NewFile.FileDescription");
        lblUploadFile.Text = GetString("NewFile.UploadFile");

        // Set title
        SetTitle(HTMLHelper.HTMLEncode(String.Format(GetString("content.newdocument"), DataClass.ClassDisplayName)));

        EnsureDocumentBreadcrumbs(PageBreadcrumbs, action: PageTitle.TitleText);
    }


    void DocumentManager_OnValidateData(object sender, DocumentManagerEventArgs e)
    {
        if (!ucDirectUploader.HasData())
        {
            e.IsValid = false;
            e.ErrorMessage = GetString("NewFile.ErrorEmpty");
        }
        else
        {
            // Get file extension
            fileExtension = ucDirectUploader.AttachmentName;
            fileExtension = Path.GetExtension(fileExtension).TrimStart('.');

            e.IsValid = IsExtensionAllowed(fileExtension);
            e.ErrorMessage = string.Format(GetString("NewFile.ExtensionNotAllowed"), fileExtension);
        }
    }


    void DocumentManager_OnSaveData(object sender, DocumentManagerEventArgs e)
    {
        e.UpdateDocument = false;

        TreeNode node = DocumentManager.Node;

        // Process file using direct uploader
        ProcessDirectUploader();

        // Save temporary attachments
        DocumentHelper.SaveTemporaryAttachments(node, Guid, SiteContext.CurrentSiteName);

        // Set additional values
        if (!string.IsNullOrEmpty(fileExtension))
        {
            // Update document extensions if no custom are used
            if (!node.DocumentUseCustomExtensions)
            {
                node.DocumentExtensions = "." + fileExtension;
            }
            node.SetValue("DocumentType", "." + fileExtension);
        }

        // Update the document
        DocumentHelper.UpdateDocument(node, DocumentManager.Tree);
    }


    protected void InitializeProperties()
    {
        string autoresize = ValidationHelper.GetString(FieldInfo.Settings["autoresize"], "").ToLowerCSafe();

        // Set custom settings
        if (autoresize == "custom")
        {
            if (FieldInfo.Settings["autoresize_width"] != null)
            {
                ResizeToWidth = ValidationHelper.GetInteger(FieldInfo.Settings["autoresize_width"], 0);
            }
            if (FieldInfo.Settings["autoresize_height"] != null)
            {
                ResizeToHeight = ValidationHelper.GetInteger(FieldInfo.Settings["autoresize_height"], 0);
            }
            if (FieldInfo.Settings["autoresize_maxsidesize"] != null)
            {
                ResizeToMaxSideSize = ValidationHelper.GetInteger(FieldInfo.Settings["autoresize_maxsidesize"], 0);
            }
        }
        // Set site settings
        else if (autoresize == "")
        {
            string siteName = SiteContext.CurrentSiteName;
            ResizeToWidth = ImageHelper.GetAutoResizeToWidth(siteName);
            ResizeToHeight = ImageHelper.GetAutoResizeToHeight(siteName);
            ResizeToMaxSideSize = ImageHelper.GetAutoResizeToMaxSideSize(siteName);
        }

        if (ValidationHelper.GetString(FieldInfo.Settings["extensions"], "") == "custom")
        {
            // Load allowed extensions
            AllowedExtensions = ValidationHelper.GetString(FieldInfo.Settings["allowed_extensions"], "");
        }
        else
        {
            // Use site settings
            string siteName = SiteContext.CurrentSiteName;
            AllowedExtensions = SettingsKeyInfoProvider.GetValue(siteName + ".CMSUploadExtensions");
        }
    }
    

    protected void ProcessDirectUploader()
    {
        TreeNode node = DocumentManager.Node;

        // Create new document
        string fileName = Path.GetFileNameWithoutExtension(ucDirectUploader.AttachmentName);

        int maxFileNameLength = FileNameFieldInfo.Size;
        if (fileName.Length > maxFileNameLength)
        {
            fileName = fileName.Substring(0, maxFileNameLength);
        }

        node.DocumentName = fileName;
        if (node.ContainsColumn("FileDescription"))
        {
            string fileDescription = txtFileDescription.Text.Trim();
            fileDescription = TextHelper.LimitLength(fileDescription, txtFileDescription.MaxLength);

            node.SetValue("FileDescription", fileDescription);
        }
        node.SetValue("FileAttachment", Guid.Empty);

        // Set default template ID
        node.SetDefaultPageTemplateID(DataClass.ClassDefaultPageTemplateID);

        // Ensures documents consistency (blog post hierarchy etc.)
        DocumentManager.EnsureDocumentsConsistency();

        // Insert the document
        DocumentHelper.InsertDocument(node, DocumentManager.ParentNode, DocumentManager.Tree);

        // Set the attachment GUID later - important when document is under workflow and  using check-in/check-out
        node.SetValue("FileAttachment", ucDirectUploader.Value);
    }


    /// <summary>
    /// Determines whether file with specified extension can be uploaded.
    /// </summary>
    /// <param name="extension">File extension to check</param>
    protected bool IsExtensionAllowed(string extension)
    {
        if (string.IsNullOrEmpty(AllowedExtensions))
        {
            return true;
        }


        // Remove starting dot from tested extension
        extension = extension.TrimStart('.').ToLowerCSafe();

        string extensions = ";" + AllowedExtensions.ToLowerCSafe() + ";";
        return ((extensions.Contains(";" + extension + ";")) || (extensions.Contains(";." + extension + ";")));
    }

    #endregion
}
