using System;

using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Content_CMSDesk_Properties_Advanced_EditableContent_Default : CMSModalPage
{
    protected override void OnPreRender(EventArgs e)
    {
        if (CultureHelper.IsUICultureRTL())
        {
            ControlsHelper.ReverseFrames(colsFrameset);
        }

        tree.Attributes["src"] = "tree.aspx" + RequestContext.CurrentQueryString;
        main.Attributes["src"] = "main.aspx" + RequestContext.CurrentQueryString;
        base.OnPreRender(e);
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        var user = MembershipContext.AuthenticatedUser;

        // Check 'read' permissions
        if (!user.IsAuthorizedPerResource("CMS.Content", "Read"))
        {
            RedirectToAccessDenied("CMS.Content", "Read");
        }

        // Check UIProfile
        if (!user.IsAuthorizedPerUIElement("CMS.Content", "Properties.General"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "Properties.General");
        }

        if (!user.IsAuthorizedPerUIElement("CMS.Content", "General.Advanced"))
        {
            RedirectToUIElementAccessDenied("CMS.Content", "General.Advanced");
        }
    }
}