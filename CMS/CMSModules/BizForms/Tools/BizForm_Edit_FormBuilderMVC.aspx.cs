using System;

using CMS.Helpers;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;

[EditedObject(BizFormInfo.OBJECT_TYPE, "formId")]
[Security(Resource = "CMS.Form", Permission = "ReadForm")]
[UIElement("CMS.Form", "Forms.FormBuldier")]
public partial class CMSModules_BizForms_Tools_BizForm_Edit_FormBuilderMVC : CMSBizFormPage
{
    private const string FORMBUILDER_ROUTE_TEMPLATE = "/Kentico.FormBuilder/Index/{0}";
    private const string BUILDER_MODE_QUERY_STRING_NAME = "builder";
    private const string FORM_BUILDER_MODE = "formBuilder";


    protected void Page_Load(object sender, EventArgs e)
    {
        var presentationUrl = SiteContext.CurrentSite.SitePresentationURL;
        if (String.IsNullOrEmpty(presentationUrl))
        {
            ShowError(ResHelper.GetString("bizform.formBuilderMVC.presentationURLMissing"));
            return;
        }
        
        var path = string.Format(FORMBUILDER_ROUTE_TEMPLATE, EditedForm.FormID);
        formBuilderFrame.Src = URLHelper.AddParameterToUrl(presentationUrl.TrimEnd('/') + VirtualContext.GetFormBuilderPath(path, CurrentUser.UserName), BUILDER_MODE_QUERY_STRING_NAME, FORM_BUILDER_MODE);
    }
}