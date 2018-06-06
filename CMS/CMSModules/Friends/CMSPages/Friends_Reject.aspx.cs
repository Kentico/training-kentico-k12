using System;
using System.Linq;

using CMS.Community;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Friends_CMSPages_Friends_Reject : CMSLiveModalPage
{
    #region "Variables"

    protected int userId = 0;
    protected CurrentUserInfo currentUser = null;

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        // Check license
        if (DataHelper.GetNotEmpty(RequestContext.CurrentDomain, string.Empty) != string.Empty)
        {
            LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Friends);
        }

        userId = QueryHelper.GetInteger("userid", 0);
        currentUser = MembershipContext.AuthenticatedUser;
        int requestedId = QueryHelper.GetInteger("requestid", 0);
        int friendshipId = 0;

        // Check if request is for current user or another user with permission to manage it
        if (currentUser.IsPublic() || ((currentUser.UserID != userId) && !currentUser.IsAuthorizedPerResource("CMS.Friends", "Manage")))
        {
            RedirectToAccessDenied("CMS.Friends", "Manage");
        }

        FriendsReject.SelectedFriends = null;
        FriendsReject.IsLiveSite = true;

        PageTitle.TitleText = GetString("friends.rejectfriendship");
        // Multiple selection
        if (Request["ids"] != null)
        {
            string[] items = Request["ids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length > 0)
            {
                FriendsReject.SelectedFriends = items.Select(x => ValidationHelper.GetInteger(x, 0)).ToList();
                if (FriendsReject.SelectedFriends.Count == 1)
                {
                    friendshipId = FriendsReject.SelectedFriends.First();
                }
            }
        }
        // For one user
        else
        {
            FriendsReject.RequestedUserID = requestedId;
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
            string fullUserName = Functions.GetFormattedUserName(requestedUser.UserName, requestedUser.FullName, requestedUser.UserNickName, true);
            Page.Title = GetString("friends.rejectfriendshipwith") + " " + HTMLHelper.HTMLEncode(fullUserName);
            PageTitle.TitleText = Page.Title;
        }

        // Set current user
        FriendsReject.UserID = userId;
    }
}