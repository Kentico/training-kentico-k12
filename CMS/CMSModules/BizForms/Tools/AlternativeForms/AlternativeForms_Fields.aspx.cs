using System;

using CMS.Base.Web.UI;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


[EditedObject(AlternativeFormInfo.OBJECT_TYPE, "altformid")]
[Security(Resource = "cms.form", Permission = "ReadForm")]
[UIElement("cms.form", "AlternativeFormProperties.Fields")]
public partial class CMSModules_BizForms_Tools_AlternativeForms_AlternativeForms_Fields : CMSBizFormPage
{
    #region "Variables"

    private AlternativeFormInfo mAlternativeFormInfo;
    private BizFormInfo mBizFormInfo;

    #endregion


    #region "Properties"

    protected AlternativeFormInfo AlternativeFormInfo
    {
        get
        {
            return mAlternativeFormInfo ?? (mAlternativeFormInfo = EditedObject as AlternativeFormInfo);
        }
    }


    protected BizFormInfo BizFormInfo
    {
        get
        {
            return mBizFormInfo ?? (mBizFormInfo = BizFormInfoProvider.GetBizFormInfo(QueryHelper.GetInteger("formid", 0)));
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (AlternativeFormInfo == null || BizFormInfo == null)
        {
            RedirectToAccessDenied(GetString("general.invalidparameters"));
        }

        CurrentMaster.BodyClass += " FieldEditorBody";

        altFormFieldEditor.AlternativeFormID = AlternativeFormInfo.FormID;
        altFormFieldEditor.Mode = FieldEditorModeEnum.AlternativeBizFormDefinition;
        altFormFieldEditor.DisplayedControls = FieldEditorControlsEnum.Bizforms;
        altFormFieldEditor.OnBeforeSave += altFormFieldEditor_OnBeforeSave;

        ObjectEditMenu menu = (ObjectEditMenu)ControlsHelper.GetChildControl(Page, typeof(ObjectEditMenu));
        if (menu != null)
        {
            menu.AllowSave = MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm", SiteInfoProvider.GetSiteName(BizFormInfo.FormSiteID));
        }

        ScriptHelper.HideVerticalTabs(this);
    }
    

    protected void altFormFieldEditor_OnBeforeSave(object sender, EventArgs e)
    {
        // Check 'EditForm' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm", SiteInfoProvider.GetSiteName(BizFormInfo.FormSiteID)))
        {
            RedirectToAccessDenied("cms.form", "EditForm");
        }
    }

    #endregion
}
