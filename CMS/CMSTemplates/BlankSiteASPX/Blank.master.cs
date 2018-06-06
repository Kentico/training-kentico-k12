using System;

using CMS.UIControls;

public partial class CMSTemplates_BlankSiteASPX_Blank : TemplateMasterPage
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        lblText.Text = "The web site doesn't contain any content. Sign in to <a href=\"" + ResolveUrl("~/Admin/cmsadministration.aspx") + "\">Administration</a> and edit the content.";
        ltlTags.Text = HeaderTags;
    }
}