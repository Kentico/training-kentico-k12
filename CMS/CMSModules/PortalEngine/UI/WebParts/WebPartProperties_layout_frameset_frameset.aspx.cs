using System;

using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_WebParts_WebPartProperties_layout_frameset_frameset : CMSWebPartPropertiesPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check permissions for web part properties UI
        var currentUser = MembershipContext.AuthenticatedUser;
        if (!currentUser.IsAuthorizedPerUIElement("CMS.Design", "WebPartProperties.Layout"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WebPartProperties.Layout");
        }
    }
}