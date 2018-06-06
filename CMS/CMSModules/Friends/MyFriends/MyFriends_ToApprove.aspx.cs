using System;

using CMS.Base.Web.UI;
using CMS.Membership;
using CMS.UIControls;


[UIElement("CMS.Friends", "MyWaitingForApproval")]
public partial class CMSModules_Friends_MyFriends_MyFriends_ToApprove : CMSContentManagementPage
{
    #region "Variables"

    protected CurrentUserInfo currentUser = null;

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = MembershipContext.AuthenticatedUser;
        ScriptHelper.RegisterDialogScript(this);
        FriendsListToApprove.UserID = currentUser.UserID;
        FriendsListToApprove.OnCheckPermissions += CheckPermissions;
        FriendsListToApprove.ZeroRowsText = GetString("friends.nowaitingfriends");
    }

    #endregion


    #region "Other events"

    protected void CheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Do not check permissions since user can always manage her friends
    }

    #endregion
}
