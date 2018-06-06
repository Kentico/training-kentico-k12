using System;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.UIControls;


public partial class CMSModules_BizForms_Tools_BizForm_Edit_EditRecord : CMSBizFormPage
{
    protected BizFormInfo bfi = null;
    private int formId;
    private int formRecordId;


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        CurrentMaster.DisplayControlsPanel = true;

        // Check 'ReadData' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "ReadData"))
        {
            RedirectToAccessDenied("cms.form", "ReadData");
        }
        // Check 'EditData' permission
        else if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditData"))
        {
            RedirectToAccessDenied("cms.form", "EditData");
        }

        // Get form id from url
        formId = QueryHelper.GetInteger("formid", 0);
        if (formId > 0)
        {
            // Get form record id
            formRecordId = QueryHelper.GetInteger("formrecordid", 0);

            if (!RequestHelper.IsPostBack())
            {
                chkSendNotification.Checked = (formRecordId <= 0);
                chkSendAutoresponder.Checked = (formRecordId <= 0);
            }

            bfi = BizFormInfoProvider.GetBizFormInfo(formId);
            EditedObject = bfi;

            if (!RequestHelper.IsPostBack())
            {
                // Get form info
                if (bfi != null)
                {
                    // Set form
                    formElem.FormName = bfi.FormName;
                    formElem.ItemID = formRecordId;
                    formElem.ShowPrivateFields = true;
                }
            }

            formElem.FormRedirectToUrl = String.Empty;
            formElem.FormDisplayText = String.Empty;
            formElem.FormClearAfterSave = false;
            // Submit image button is based on ImageButton which does not have implemented automatic registration of Save header action in UI. 
            // Hide image button in UI even if path to image is configured
            formElem.ShowImageButton = false;

            formElem.OnBeforeSave += formElem_OnBeforeSave;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        // Create breadcrumbs
        CreateBreadcrumbs();

        base.OnPreRender(e);
    }


    /// <summary>
    /// Creates breadcrumbs for form item
    /// </summary>
    private void CreateBreadcrumbs()
    {
        string text = GetString(formElem.ItemID > 0 ? "BizForm_Edit_EditRecord.EditRecord" : "BizForm_Edit_EditRecord.NewRecord");

        // Initializes page title
        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = GetString("bizform.data"),
            RedirectUrl = "~/CMSModules/BizForms/Tools/BizForm_Edit_Data.aspx?formid=" + formId
        });

        PageBreadcrumbs.Items.Add(new BreadcrumbItem
        {
            Text = text
        });

        // Do not include type as breadcrumbs suffix
        UIHelper.SetBreadcrumbsSuffix("");
    }


    /// <summary>
    /// OnBefore save bizform.
    /// </summary>
    private void formElem_OnBeforeSave(object sender, EventArgs e)
    {
        formElem.EnableNotificationEmail = chkSendNotification.Checked;
        formElem.EnableAutoresponder = chkSendAutoresponder.Checked;
    }
}
