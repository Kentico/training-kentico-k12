using System;
using System.Linq;

using CMS.Community;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Friends_Dialogs_Friends_Approve : CMSModalPage
{
    #region "Variables"

    protected int userId = 0;
    protected CurrentUserInfo currentUser = null;

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        userId = QueryHelper.GetInteger("userid", 0);
        currentUser = MembershipContext.AuthenticatedUser;

        // Check license
        if (DataHelper.GetNotEmpty(RequestContext.CurrentDomain, string.Empty) != string.Empty)
        {
            LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Friends);
        }

        FriendsApprove.SelectedFriends = null;
        FriendsApprove.OnCheckPermissions += FriendsApprove_OnCheckPermissions;

        int requestedId = QueryHelper.GetInteger("requestid", 0);
        int friendshipId = 0;
        Page.Title = GetString("friends.approvefriendship");
        PageTitle.TitleText = Page.Title;
        // Multiple selection
        if (Request["ids"] != null)
        {
            string[] items = Request["ids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length > 0)
            {
                FriendsApprove.SelectedFriends = items.Select(x => ValidationHelper.GetInteger(x, 0)).ToList();
                if (FriendsApprove.SelectedFriends.Count == 1)
                {
                    friendshipId = FriendsApprove.SelectedFriends.First();
                }
            }
        }
        // For one user
        else
        {
            FriendsApprove.RequestedUserID = requestedId;
        }

        FriendInfo fi = null;
        if (friendshipId != 0)
        {
            fi = FriendInfoProvider.GetFriendInfo(friendshipId);
            // Set edited object
            EditedObject = fi;
        }
        else if (requestedId != 0)
        {
            fi = FriendInfoProvider.GetFriendInfo(userId, requestedId);
            // Set edited object
            EditedObject = fi;
        }

        if (fi != null)
        {
            UserInfo requestedUser = (userId == fi.FriendRequestedUserID) ? UserInfoProvider.GetFullUserInfo(fi.FriendUserID) : UserInfoProvider.GetFullUserInfo(fi.FriendRequestedUserID);
            string fullUserName = Functions.GetFormattedUserName(requestedUser.UserName, requestedUser.FullName, requestedUser.UserNickName, false);
            Page.Title = GetString("friends.approvefriendshipwith") + " " + HTMLHelper.HTMLEncode(fullUserName);
            PageTitle.TitleText = Page.Title;
        }

        // Set current user
        FriendsApprove.UserID = userId;
    }


    private void FriendsApprove_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Check if approve is for current user or another user with permission to manage it
        if ((currentUser.UserID != userId) && !currentUser.IsAuthorizedPerResource("CMS.Friends", permissionType))
        {
            RedirectToAccessDenied("CMS.Friends", permissionType);
        }
    }
}