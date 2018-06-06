using System;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Friends_CMSPages_Friends_Request : CMSLiveModalPage
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

        // Check if request is for current user or another user with permission to manage it
        if (currentUser.IsPublic() || ((currentUser.UserID != userId) && !currentUser.IsAuthorizedPerResource("CMS.Friends", "Manage")))
        {
            RedirectToAccessDenied("CMS.Friends", "Manage");
        }

        int requestedUserId = QueryHelper.GetInteger("requestid", 0);
        PageTitle.TitleText = GetString("friends.addnewfriend");
        FriendsRequest.UserID = userId;
        FriendsRequest.RequestedUserID = requestedUserId;
        FriendsRequest.IsLiveSite = true;

        if (requestedUserId != 0)
        {
            string fullUserName = String.Empty;

            UserInfo requestedUser = UserInfoProvider.GetUserInfo(requestedUserId);
            if (requestedUser != null)
            {
                fullUserName = Functions.GetFormattedUserName(requestedUser.UserName, requestedUser.FullName, requestedUser.UserNickName, true);
            }

            Page.Title = string.Format(GetString("friends.requestfriendshipwith"), HTMLHelper.HTMLEncode(fullUserName));
            PageTitle.TitleText = Page.Title;
        }
    }

}