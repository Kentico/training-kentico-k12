using System;

using CMS.FormEngine;
using CMS.Membership;
using CMS.OnlineForms.Web.UI;
using CMS.UIControls;


[EditedObject(AlternativeFormInfo.OBJECT_TYPE, "altformid")]
[Security(Resource = "cms.form", Permission = "ReadForm")]
[UIElement("cms.form", "AlternativeFormProperties.General")]
public partial class CMSModules_BizForms_Tools_AlternativeForms_AlternativeForms_Edit_General : CMSBizFormPage
{
    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        altFormEdit.OnBeforeSave += altFormEdit_OnBeforeSave;
    }


    protected void altFormEdit_OnBeforeSave(object sender, EventArgs e)
    {
        // Check 'EditForm' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm"))
        {
            RedirectToAccessDenied("cms.form", "EditForm");
        }
    }
    
    #endregion
}