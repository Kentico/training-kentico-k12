using System;

using CMS.Base.Web.UI;
using CMS.Membership;
using CMS.UIControls;


[UIElement("CMS.Friends", "MyRejectedFriends")]
public partial class CMSModules_Friends_MyFriends_MyFriends_Rejected : CMSContentManagementPage
{
    #region "Variables"

    protected CurrentUserInfo currentUser = null;

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = MembershipContext.AuthenticatedUser;
        ScriptHelper.RegisterDialogScript(this);
        FriendsListRejected.UserID = currentUser.UserID;
        FriendsListRejected.UseEncapsulation = false;
        FriendsListRejected.OnCheckPermissions += CheckPermissions;
        FriendsListRejected.ZeroRowsText = GetString("friends.norejectedfriends");
    }

    #endregion


    #region "Other events"

    protected void CheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Do not check permissions since user can always manage her friends
    }

    #endregion
}
