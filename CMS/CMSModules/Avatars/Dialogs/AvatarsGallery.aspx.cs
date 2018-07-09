using System;

using CMS.UIControls;


public partial class CMSModules_Avatars_Dialogs_AvatarsGallery : CMSModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetSaveJavascript("addToHidden(); return false;");
        PageTitle.TitleText = GetString("avat.selectavatar");
    }
}