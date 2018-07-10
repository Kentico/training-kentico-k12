using System;

using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_WebParts_WebPartProperties_layout_frameset : CMSWebPartPropertiesPage
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        rowsFrameset.Attributes.Add("rows", "56, *, " + FooterFrameHeight);

        // Check permissions for web part properties UI
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Design", "WebPartProperties.Layout"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WebPartProperties.Layout");
        }

        frameMenu.Attributes.Add("src", "webpartproperties_layout_menu.aspx" + RequestContext.CurrentQueryString);
        frameButtons.Attributes.Add("src", "webpartproperties_buttons.aspx" + RequestContext.CurrentQueryString);
        // Content frame is loaded by menu page
    }
}
