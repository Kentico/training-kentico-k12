using System;

using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.OnlineForms.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;

// Edited object
[EditedObject(BizFormInfo.OBJECT_TYPE, "formId")]
[Action(0, "altforms.newformlink", "AlternativeForms_New.aspx?formid={?formid?}")]
[Security(Resource = "cms.form", Permission = "ReadForm")]
[UIElement("cms.form", "Forms.AlternativeForms")]
public partial class CMSModules_BizForms_Tools_AlternativeForms_AlternativeForms_List : CMSBizFormPage
{
    #region "Private variables"

    private BizFormInfo formInfo;

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

        // Init alternative forms listing
        listElem.FormClassID = FormInfo.FormClassID;
        listElem.OnEdit += listElem_OnEdit;
        listElem.OnDelete += listElem_OnDelete;
    }

    #endregion


    #region "Event handlers"

    private void listElem_OnEdit(object sender, object actionArgument)
    {
        string url = UIContextHelper.GetElementUrl("CMS.Form", "AlternativeForms.AlternativeFormProperties", false);
        string alternativeFormId = ValidationHelper.GetInteger(actionArgument, 0).ToString();
        url = URLHelper.AddParameterToUrl(url, "altformid", alternativeFormId);
        url = URLHelper.AddParameterToUrl(url, "formid", FormInfo.FormID.ToString());
        url = URLHelper.AddParameterToUrl(url, "objectid", alternativeFormId);

        URLHelper.Redirect(url);
    }


    private void listElem_OnDelete(object sender, object actionArgument)
    {
        // Check 'EditForm' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm", SiteInfoProvider.GetSiteName(formInfo.FormSiteID)))
        {
            RedirectToAccessDenied("cms.form", "EditForm");
        }

        AlternativeFormInfoProvider.DeleteAlternativeFormInfo(ValidationHelper.GetInteger(actionArgument, 0));
    }

    #endregion
}