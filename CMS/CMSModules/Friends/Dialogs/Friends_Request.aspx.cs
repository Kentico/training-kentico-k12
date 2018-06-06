using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Friends_Dialogs_Friends_Request : CMSModalPage
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

        int requestedUserId = QueryHelper.GetInteger("requestid", 0);
        Page.Title = GetString("friends.addnewfriend");
        PageTitle.TitleText = Page.Title;
        FriendsRequest.UserID = userId;
        FriendsRequest.RequestedUserID = requestedUserId;
        FriendsRequest.OnCheckPermissions += FriendsRequest_OnCheckPermissions;

        if (requestedUserId != 0)
        {
            UserInfo requestedUser = UserInfoProvider.GetUserInfo(requestedUserId);
            string fullUserName = Functions.GetFormattedUserName(requestedUser.UserName, requestedUser.FullName, requestedUser.UserNickName, false);
            Page.Title = string.Format(GetString("friends.requestfriendshipwith"), HTMLHelper.HTMLEncode(fullUserName));
            PageTitle.TitleText = Page.Title;
        }
    }


    private void FriendsRequest_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Check if request is for current user or another user with permission to manage it
        if ((currentUser.UserID != userId) && !currentUser.IsAuthorizedPerResource("CMS.Friends", permissionType))
        {
            RedirectToAccessDenied("CMS.Friends", permissionType);
        }
    }
}