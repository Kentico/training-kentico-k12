using System;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.SiteProvider;
using CMS.Synchronization;
using CMS.UIControls;

// Edited object
[EditedObject(BizFormInfo.OBJECT_TYPE, "formId")]
[Security(Resource = "CMS.Form", Permission = "ReadForm")]
[UIElement("CMS.Form", "Forms.Layout")]
public partial class CMSModules_BizForms_Tools_BizForm_Edit_Layout : CMSBizFormPage
{
    #region "Variables"

    protected HeaderAction attachments = null;
    private BizFormInfo formInfo;
    private const string mAttachmentsActionClass = "attachments-header-action";

    #endregion


    #region "Properties"

    protected BizFormInfo FormInfo
    {
        get
        {
            return formInfo ?? (formInfo = EditedObject as BizFormInfo);
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (EditedObject == null)
        {
            RedirectToAccessDenied(GetString("general.invalidparameters")); 
        }

        CurrentMaster.PanelContent.CssClass = string.Empty;

        layoutElem.FormLayoutType = FormLayoutTypeEnum.Bizform;
        layoutElem.ObjectID = FormInfo.FormID;
        layoutElem.ObjectType = BizFormInfo.OBJECT_TYPE;
        layoutElem.ObjectCategory = ObjectAttachmentsCategories.LAYOUT;
        layoutElem.IsLiveSite = false;
        layoutElem.OnBeforeSave += layoutElem_OnBeforeSave;
        layoutElem.OnAfterSave += layoutElem_OnAfterSave;
        layoutElem.IsAuthorizedForAscxEditingFunction = () => { 
            return MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.globalpermissions", "editcode"); 
        }; 

        // Load CSS style sheet to editor area
        if (SiteContext.CurrentSite != null)
        {
            int cssId = SiteContext.CurrentSite.SiteDefaultEditorStylesheet;
            if (cssId == 0) // Use default site CSS if none editor CSS is specified
            {
                cssId = SiteContext.CurrentSite.SiteDefaultStylesheetID;
            }
            layoutElem.CssStyleSheetID = cssId;
         }

    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Add attachment button to header actions
        InitHeaderActions();

        if (layoutElem.CustomLayoutEnabled)
        {
            // Register paste attachment script
            const string script = @"// Pasting image URL to CKEditor - requires other function 'InsertHTML' -  see Layout control
function PasteImage(imageurl) {
    imageurl = '<img src=""' + imageurl + '"" />';
    return InsertHTML(imageurl);
}";
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "PasteScript_" + ClientID, script, true);
        }
        else
        {
            attachments.Enabled = false;
        }
    }

    #endregion


    #region "Event handlers"

    private void layoutElem_OnAfterSave(object sender, EventArgs e)
    {    
        // Delete all attachments if layout was deleted
        if (FormInfo != null)
        {
            var dci = DataClassInfoProvider.GetDataClassInfo(FormInfo.FormClassID);
            if ((dci != null) && string.IsNullOrEmpty(dci.ClassFormLayout))
            {
                // Delete all attachments
                MetaFileInfoProvider.DeleteFiles(FormInfo.FormID, BizFormInfo.OBJECT_TYPE, ObjectAttachmentsCategories.LAYOUT);
            }
        }

        // Log synchronization
        SynchronizationHelper.LogObjectChange(FormInfo, TaskTypeEnum.UpdateObject);
    }


    private void layoutElem_OnBeforeSave(object sender, EventArgs e)
    {
        // Check 'EditForm' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm", SiteInfoProvider.GetSiteName(FormInfo.FormSiteID)))
        {
            RedirectToAccessDenied("cms.form", "EditForm");
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Initializes header actions.
    /// </summary>
    protected void InitHeaderActions()
    {
        bool isAuthorized = CurrentUser.IsAuthorizedPerResource("cms.form", "EditForm");

        int attachCount = 0;
        if (isAuthorized)
        {
            // Get number of attachments
            InfoDataSet<MetaFileInfo> ds = MetaFileInfoProvider.GetMetaFiles(FormInfo.FormID, BizFormInfo.OBJECT_TYPE, ObjectAttachmentsCategories.LAYOUT, null, null, "MetafileID", -1);
            attachCount = ds.Items.Count;

            // Register attachments count update module
            ScriptHelper.RegisterModule(this, "CMS/AttachmentsCountUpdater", new { Selector = "." + mAttachmentsActionClass, Text = GetString("general.attachments") });

            // Register dialog scripts
            ScriptHelper.RegisterDialogScript(Page);
        }

        // Prepare metafile dialog URL
        string metaFileDialogUrl = ResolveUrl(@"~/CMSModules/AdminControls/Controls/MetaFiles/MetaFileDialog.aspx");
        string query = string.Format("?objectid={0}&objecttype={1}", FormInfo.FormID, BizFormInfo.OBJECT_TYPE);
        metaFileDialogUrl += string.Format("{0}&category={1}&hash={2}", query, ObjectAttachmentsCategories.LAYOUT, QueryHelper.GetHash(query));

        // Init attachment button
        attachments = new HeaderAction
        {
            Text = GetString("general.attachments") + ((attachCount > 0) ? " (" + attachCount + ")" : string.Empty),
            Tooltip = GetString("general.attachments"),
            OnClientClick = string.Format(@"if (modalDialog) {{modalDialog('{0}', 'Attachments', '700', '500');}}", metaFileDialogUrl) + " return false;",
            Enabled = isAuthorized,
            Visible = !layoutElem.IsEditedObjectLocked(),
            CssClass = mAttachmentsActionClass,
            ButtonStyle = ButtonStyle.Default,
        };
        layoutElem.AddExtraHeaderAction(attachments);
    }

    #endregion
}
