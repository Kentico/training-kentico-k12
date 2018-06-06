using System;

using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


[Title("BizForm_New.HeaderCaption")]
[UIElement("CMS.Form", "Form.AddForm")]
public partial class CMSModules_BizForms_Tools_BizForm_New : CMSBizFormPage
{
    private const string bizFormNamespace = "BizForm";
    private string mFormTablePrefix;


    /// <summary>
    /// Returns prefix for bizform table name.
    /// </summary>
    private string FormTablePrefix
    {
        get
        {
            if (string.IsNullOrEmpty(mFormTablePrefix))
            {
                mFormTablePrefix = String.Format("Form_{0}_", ValidationHelper.GetIdentifier(SiteContext.CurrentSiteName));
            }

            return mFormTablePrefix;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // Check 'CreateForm' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "CreateForm"))
        {
            RedirectToAccessDenied("cms.form", "CreateForm");
        }

        // Validator initializations
        rfvFormDisplayName.ErrorMessage = GetString("BizForm_Edit.ErrorEmptyFormDispalyName");
        rfvFormName.ErrorMessage = GetString("BizForm_Edit.ErrorEmptyFormName");
        rfvTableName.ErrorMessage = GetString("BizForm_Edit.ErrorEmptyFormTableName");

        // Control initializations
        lblFormDisplayName.Text = GetString("BizForm_Edit.FormDisplayNameLabel");
        lblFormName.Text = GetString("BizForm_Edit.FormNameLabel");
        lblTableName.Text = GetString("BizForm_Edit.TableNameLabel");
        lblPrefix.Text = FormTablePrefix + "&nbsp;";
        // Remove prefix length from maximum allowed length of table name
        txtTableName.MaxLength = 100 - FormTablePrefix.Length;
        btnOk.Text = GetString("General.OK");

        // Page title control initialization
        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = GetString("BizForm_Edit.ItemListLink"),
            RedirectUrl = "~/CMSModules/BizForms/Tools/BizForm_List.aspx"
        });

        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = GetString("BizForm_Edit.NewItemCaption")
        });
    }


    /// <summary>
    /// Sets data to database.
    /// </summary>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (!BizFormInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.BizForms, ObjectActionEnum.Insert))
        {
            ShowError(GetString("LicenseVersion.BizForm"));
            return;
        }

        string formDisplayName = txtFormDisplayName.Text.Trim();
        string formName = txtFormName.Text.Trim();
        string tableName = txtTableName.Text.Trim();

        string errorMessage = new Validator().NotEmpty(formDisplayName, rfvFormDisplayName.ErrorMessage).
                                              NotEmpty(formName, rfvFormName.ErrorMessage).
                                              NotEmpty(tableName, rfvTableName.ErrorMessage).
                                              IsIdentifier(formName, GetString("bizform_edit.errorformnameinidentifierformat")).
                                              IsIdentifier(tableName, GetString("BizForm_Edit.ErrorFormTableNameInIdentifierFormat")).Result;

        if (!String.IsNullOrEmpty(errorMessage))
        {
            ShowError(errorMessage);
            return;
        }

        var bizFormObj = new BizFormInfo
        {
            FormDisplayName = formDisplayName,
            FormName = formName,
            FormSiteID = SiteContext.CurrentSiteID,
            FormEmailAttachUploadedDocs = true,
            FormItems = 0,
            FormClearAfterSave = false,
            FormLogActivity = true
        };

        // Ensure the code name
        bizFormObj.Generalized.EnsureCodeName();

        // Table name is combined from prefix ('BizForm_<sitename>_') and custom table name
        string safeFormName = ValidationHelper.GetIdentifier(bizFormObj.FormName);
        bizFormObj.FormName = safeFormName;

        string className = bizFormNamespace + "." + safeFormName;

        // Generate the table name
        if (String.IsNullOrEmpty(tableName) || (tableName == InfoHelper.CODENAME_AUTOMATIC))
        {
            tableName = safeFormName;
        }
        tableName = FormTablePrefix + tableName;

        TableManager tm = new TableManager(null);

        // TableName wont be longer than 60 letters and will be unique
        if (tableName.Length > 60)
        {
            string tmpTableName = tableName.Substring(0, 59);
            int x = 1;
            do
            {
                tableName = tmpTableName + x;
                x++;
            } while (tm.TableExists(tableName));
        }

        // TableName should be unique
        if (tm.TableExists(tableName))
        {
            ShowError(string.Format(GetString("bizform_edit.errortableexists"), tableName));
            return;
        }

        // If first letter of safeFormName is digit, add "PK" to beginning
        string primaryKey = BizFormInfoProvider.GenerateFormPrimaryKeyName(bizFormObj.FormName);
        try
        {
            // Create new table in DB
            tm.CreateTable(tableName, primaryKey);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BIZFORM_NEW", EventType.ERROR, ex);
            ShowError(string.Format(GetString("bizform_edit.createtableerror"), tableName));
            return;
        }

        // Create the BizForm class
        DataClassInfo dci = BizFormInfoProvider.CreateBizFormDataClass(className, formDisplayName, tableName, primaryKey);
        try
        {
            // Create new bizform dataclass
            using (CMSActionContext context = new CMSActionContext())
            {
                // Disable logging of tasks
                context.DisableLogging();

                DataClassInfoProvider.SetDataClassInfo(dci);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BIZFORM_NEW", EventType.ERROR, ex);
            ShowError(ex.Message);

            CleanUpOnError(tableName, tm, dci);
            return;
        }

        // Create new bizform
        bizFormObj.FormClassID = dci.ClassID;
        try
        {
            BizFormInfoProvider.SetBizFormInfo(bizFormObj);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BIZFORM_NEW", EventType.ERROR, ex);
            ShowError(ex.Message);

            CleanUpOnError(tableName, tm, dci, bizFormObj);
            return;
        }

        // Redirect to Form builder tab
        string url = UIContextHelper.GetElementUrl("CMS.Form", "Forms.Properties", false, bizFormObj.FormID);
        url = URLHelper.AddParameterToUrl(url, "tabname", "Forms.FormBuldier");
        URLHelper.Redirect(url);
    }


    private void CleanUpOnError(string tableName, TableManager tm, DataClassInfo dci = null, BizFormInfo bizForm = null)
    {
        if (tm.TableExists(tableName))
        {
            tm.DropTable(tableName);
        }
        if ((bizForm != null) && (bizForm.FormID > 0))
        {
            BizFormInfoProvider.DeleteBizFormInfo(bizForm);
        }
        if ((dci != null) && (dci.ClassID > 0))
        {
            DataClassInfoProvider.DeleteDataClassInfo(dci);
        }
    }
}