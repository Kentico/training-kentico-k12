using System;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Membership;
using CMS.Modules;
using CMS.UIControls;


[UIElement("CMS.Friends", "MyApprovedFriends")]
public partial class CMSModules_Friends_MyFriends_MyFriends_Approved : CMSContentManagementPage
{
    #region "Variables"

    protected CurrentUserInfo currentUser = null;

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = MembershipContext.AuthenticatedUser;
        UrlResolver.ResolveUrl(GetImageUrl("Objects/CMS_Friend/"));
        ScriptHelper.RegisterDialogScript(this);
        FriendsList.UserID = currentUser.UserID;
        FriendsList.OnCheckPermissions += CheckPermissions;
        FriendsList.ZeroRowsText = GetString("friends.nofriends");

        // Request friend link
        string script =
            "function displayRequest(){ \n" +
            "modalDialog('" + ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Friends/Dialogs/Friends_Request.aspx") + "?userid=" + currentUser.UserID + "', 'rejectDialog', 810, 460);}";

        ScriptHelper.RegisterStartupScript(this, GetType(), "displayModalRequest", ScriptHelper.GetScript(script));

        HeaderAction action = new HeaderAction();
        action.Text = GetString("Friends_List.NewItemCaption");
        action.OnClientClick = "displayRequest()";
        CurrentMaster.HeaderActions.AddAction(action);
    }

    #endregion


    #region "Other events"

    protected void CheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Do not check permissions since user can always manage her friends
    }

    #endregion
}
