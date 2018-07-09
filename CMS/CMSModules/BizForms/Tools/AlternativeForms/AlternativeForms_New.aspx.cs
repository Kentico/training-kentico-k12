using System;

using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.UIControls;


[Breadcrumbs]
[Breadcrumb(0, TargetUrl = "~/CMSModules/BizForms/Tools/AlternativeForms/AlternativeForms_List.aspx?formid={?formid?}", ResourceString = "altforms.listlink")]
[Breadcrumb(1, ResourceString = "altform.newbread")]
[Security(Resource = "cms.form", Permission = "EditForm", UIElements = "Forms.Properties;Forms.AlternativeForms;AlternativeForms.CreateNewForm")]
[Help("alternative_forms_general", "helpTopic")]
[UIElement("cms.form", "AlternativeForms.CreateNewForm")]
public partial class CMSModules_BizForms_Tools_AlternativeForms_AlternativeForms_New : CMSBizFormPage
{
    #region "Variables"

    private BizFormInfo bfi;

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Get needed IDs
        int formId = QueryHelper.GetInteger("formid", 0);

        bfi = BizFormInfoProvider.GetBizFormInfo(formId);
        if (bfi != null)
        {
            altFormEdit.EditedObject.FormClassID = bfi.FormClassID;

            altFormEdit.OnAfterSave += altFormEdit_OnAfterSave;

            altFormEdit.RedirectUrlAfterCreate = String.Empty;
        }
    }


    protected void altFormEdit_OnAfterSave(object sender, EventArgs e)
    {
        string url = UIContextHelper.GetElementUrl("CMS.Form", "AlternativeForms.AlternativeFormProperties", false);
        string alternativeFormId = ((AlternativeFormInfo)EditedObject).FormID.ToString();
        url = URLHelper.AddParameterToUrl(url, "altformid", alternativeFormId);
        url = URLHelper.AddParameterToUrl(url, "formid", bfi.FormID.ToString());
        url = URLHelper.AddParameterToUrl(url, "saved", "1");
        url = URLHelper.AddParameterToUrl(url, "objectid", alternativeFormId);

        URLHelper.Redirect(url);
    }

    #endregion
}