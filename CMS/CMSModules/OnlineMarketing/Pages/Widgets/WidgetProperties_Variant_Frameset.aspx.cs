using System;

using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_OnlineMarketing_Pages_Widgets_WidgetProperties_Variant_Frameset : CMSWidgetPropertiesPage
{
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Design", "WidgetProperties.Variant"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WidgetProperties.Variant");
        }

        rowsFrameset.Attributes.Add("rows", "*, " + FooterFrameHeight);
        frameContent.Attributes.Add("src", "widgetproperties_variant.aspx" + RequestContext.CurrentQueryString);
        frameButtons.Attributes.Add("src", ResolveUrl("~/CMSModules/Widgets/Dialogs/widgetproperties_buttons.aspx" + RequestContext.CurrentQueryString));
    }
}