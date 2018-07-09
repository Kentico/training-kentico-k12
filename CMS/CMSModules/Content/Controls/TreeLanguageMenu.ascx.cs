using System;

using CMS.UIControls;


public partial class CMSModules_Content_Controls_TreeLanguageMenu : CMSUserControl
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Compare feature is not supported for content only sites. 
        LanguageMenu.DisplayCompare = !CurrentSite.SiteIsContentOnly;
    }
}