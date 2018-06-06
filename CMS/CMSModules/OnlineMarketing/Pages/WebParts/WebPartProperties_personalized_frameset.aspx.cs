using System;

using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_OnlineMarketing_Pages_WebParts_WebPartProperties_personalized_frameset : CMSWebPartPropertiesPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Design", "WebPartProperties.Variant"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "WebPartProperties.Variant");
        }

        frameContent.Attributes.Add("src", "webpartproperties_personalized.aspx" + RequestContext.CurrentQueryString);
        frameButtons.Attributes.Add("src", ResolveUrl("~/CMSModules/PortalEngine/UI/WebParts/webpartproperties_buttons.aspx" + RequestContext.CurrentQueryString));
    }
}