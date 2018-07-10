using System;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.Messaging;
using CMS.UIControls;


public partial class CMSModules_Messaging_CMSPages_SendMessage : CMSLiveModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check license
        LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Messaging);

        // Setup master page
        CurrentMaster.PanelContent.AddCssClass("dialog-content");

        // Initializes page title control
        PageTitle.TitleText = GetString("messaging.sendmessage");
        Title = GetString("messaging.sendmessage");

        int requestedUserId = QueryHelper.GetInteger("requestid", 0);
        if (requestedUserId != 0)
        {
            UserInfo requestedUser = UserInfoProvider.GetFullUserInfo(requestedUserId);
            string fullUserName = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(requestedUser.UserName, requestedUser.FullName, requestedUser.UserNickName, true));
            Page.Title = GetString("messaging.sendmessageto") + " " + fullUserName;
            PageTitle.TitleText = Page.Title;
        }

        // Initilaize new message
        ucSendMessage.DisplayMessages = true;
        ucSendMessage.DefaultRecipient = QueryHelper.GetString("requestid", string.Empty);
        ucSendMessage.SendButtonClick += SendButon;
        ucSendMessage.CloseButtonClick += CloseButon;
        ucSendMessage.SendMessageMode = MessageActionEnum.New;
        ucSendMessage.DisplayCloseButton = true;
        ucSendMessage.UsePromptDialog = false;
    }


    private void SendButon(object sender, EventArgs e)
    {
        if (ucSendMessage.ErrorMessage == string.Empty)
        {
            ucSendMessage.SendButton.Enabled = false;
            ucSendMessage.BBEditor.Enabled = false;
            ucSendMessage.SubjectBox.Enabled = false;
            ucSendMessage.FromBox.Enabled = false;
            ucSendMessage.CancelButton.Attributes.Add("onclick", "wopener.location.replace(wopener.location);");
            ucSendMessage.CancelButton.ResourceString = "general.Close";
        }
    }


    private void CloseButon(object sender, EventArgs e)
    {
        // Close
        ScriptHelper.RegisterStartupScript(this, GetType(), "closeSendDialog", ScriptHelper.GetScript("CloseDialog();"));
    }
}
