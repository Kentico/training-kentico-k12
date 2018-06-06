using System;

using CMS.Base.Web.UI;
using CMS.Helpers;


public partial class CMSModules_Messaging_Controls_MessageMenu : CMSContextMenuControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string menuId = ContextMenu.MenuID;
        string parentElemId = ContextMenu.ParentElementClientID;

        string actionPattern = "ContextMessageAction_" + parentElemId + "('{0}', GetContextMenuParameter('" + menuId + "'));";

        // Main menu
        lblReply.Text = ResHelper.GetString("messaging.reply");
        pnlReply.Attributes.Add("onclick", string.Format(actionPattern, "reply"));

        lblForward.Text = ResHelper.GetString("messaging.forward");
        pnlForward.Attributes.Add("onclick", string.Format(actionPattern, "forward"));

        lblMarkRead.Text = ResHelper.GetString("Messaging.Action.MarkAsRead");
        pnlMarkRead.Attributes.Add("onclick", string.Format(actionPattern, "markread"));

        lblMarkUnread.Text = ResHelper.GetString("Messaging.Action.MarkAsUnread");
        pnlMarkUnread.Attributes.Add("onclick", string.Format(actionPattern, "markunread"));
    }
}