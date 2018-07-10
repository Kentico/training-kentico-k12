using System;

using CMS.Base.Web.UI.ActionsConfig;
using CMS.UIControls;


public partial class CMSFormControls_LiveSelectors_InsertYouTubeVideo_Content : CMSLiveModalPage
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        EnsureScriptManager();
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentMaster.Page.Title = GetString("dialogs.youtube.inserttitle");
        PageTitle.TitleText = GetString("dialogs.youtube.inserttitle");
        CurrentMaster.Body.Attributes.Add("onbeforeunload", "$cmsj('.YouTubePreviewBox').remove();");

        HeaderAction action = new HeaderAction
        {
            Text = GetString("dialogs.youtube.goto"),
            Target = "_blank",
            RedirectUrl = "http://www.youtube.com"
        };

        CurrentMaster.HeaderActions.AddAction(action);
    }
}