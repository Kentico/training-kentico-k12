using System;

using CMS.UIControls;


public partial class CMSModules_Avatars_CMSPages_PublicAvatarsGallery : CMSLiveModalPage
{
    #region "Events"

    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.TitleText = GetString("avat.selectavatar");
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        btnOk.Enabled = avatarsGallery.HasData();
    }

    #endregion
}