using System;

using CMS.FormEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.OnlineForms.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


[Security(Resource = "cms.form", Permission = "ReadForm")]
[UIElement("cms.form", "AlternativeFormProperties.Layout")]
public partial class CMSModules_BizForms_Tools_AlternativeForms_AlternativeForms_Layout : CMSBizFormPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentMaster.PanelContent.CssClass = string.Empty;

        layoutElem.FormLayoutType = FormLayoutTypeEnum.Bizform;
        layoutElem.ObjectID = QueryHelper.GetInteger("altformid", 0);
        layoutElem.IsAlternative = true;
        layoutElem.OnBeforeSave += layoutElem_OnBeforeSave;
        layoutElem.IsAuthorizedForAscxEditingFunction = () =>
        {
            return MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.globalpermissions", "editcode");
        }; 

        // Load CSS style sheet to editor area
        var currentSite = SiteContext.CurrentSite;
        if (currentSite != null)
        {
            int cssId = currentSite.SiteDefaultEditorStylesheet;
            if (cssId == 0) // Use default site CSS if none editor CSS is specified
            {
                cssId = currentSite.SiteDefaultStylesheetID;
            }
            layoutElem.CssStyleSheetID = cssId;
        }
    }


    private void layoutElem_OnBeforeSave(object sender, EventArgs e)
    {
        // Check 'EditForm' permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.form", "EditForm"))
        {
            RedirectToAccessDenied("cms.form", "EditForm");
        }
    }
}