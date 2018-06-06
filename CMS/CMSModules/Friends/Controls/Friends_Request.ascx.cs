using System;
using System.Collections.Generic;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Community;
using CMS.Community.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;


public partial class CMSModules_Friends_Controls_Friends_Request : FriendsActionControl
{
    #region "Public properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Indicates if control is used on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            base.IsLiveSite = value;
            selectUser.IsLiveSite = value;
            plcMess.IsLiveSite = value;
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Set user id
        UserID = QueryHelper.GetInteger("userid", 0);
        RequestedUserID = QueryHelper.GetInteger("requestid", 0);
        int siteId = QueryHelper.GetInteger("siteid", -1);

        if (RequestedUserID != 0)
        {
            plcUserSelect.Visible = false;
        }

        bool isGlobalAdmin = MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin);

        RedirectIfRequestIsInvalid(UserID, RequestedUserID);

        // Show site filter for global admin only and if the site is not specified
        selectUser.ShowSiteFilter = ((siteId == 0) && isGlobalAdmin);

        selectUser.WhereCondition = "UserName NOT LIKE N'public'";

        if (!isGlobalAdmin)
        {
            selectUser.HideDisabledUsers = true;
            selectUser.HideHiddenUsers = true;
            selectUser.HideNonApprovedUsers = true;
        }

        // Enable automatic approval
        plcAdministrator.Visible = CanApprove();

        if (!ModuleManager.IsModuleLoaded(ModuleName.MESSAGING))
        {
            chkSendMessage.Visible = false;
            chkSendMessage.Checked = false;
        }
    }


    /// <summary>
    /// Formats username of sender and recipients.
    /// </summary>
    public override string GetFormattedUserName(string userName, string fullName, string nickName)
    {
        return Functions.GetFormattedUserName(userName, fullName, nickName, IsLiveSite);
    }


    /// <summary>
    /// Redirects user to an AccessDenied Page with proper error message if
    /// request parameters are not correct.
    /// </summary>
    /// <param name="userID">ID of the user, sending the request.</param>
    /// <param name="requestID">ID of th recepient of the request.</param>
    private void RedirectIfRequestIsInvalid(int userID, int requestID)
    {
        string message = VerifyRequestParameters(userID, requestID);

        if (!String.IsNullOrEmpty(message))
        {
            UseCMSDeskAccessDeniedPage = !IsLiveSite;
            RedirectToAccessDenied(message);
        }
    }

    /// <summary>
    /// Verifies request parameters and generates a proper erroe message if
    /// request parameters are not correct.
    /// </summary>
    /// <param name="userID">ID of the user, sending the request.</param>
    /// <param name="requestID">ID of th recepient of the request.</param>
    private string VerifyRequestParameters(int userID, int requestID)
    {
        UserInfo requestedUser = null;
        if (requestID > 0)
        {
            requestedUser = UserInfoProvider.GetUserInfo(requestID);
        }
        UserInfo requestingUser = UserInfoProvider.GetUserInfo(userID);

        if (((requestedUser == null) && (requestID != 0)) || (requestingUser == null))
        {
            return GetString("user.error_doesnoteexist");
        }

        string errMsg = VerifyRequestedUser(requestedUser) ?? VerifyRequestingUser(requestingUser);

        return errMsg;
    }


    /// <summary>
    /// Verifies whether current user has permissions to create a friend-request on behalf of <paramref name="requestingUser"/>
    /// </summary>
    /// <param name="requestingUser">User on behalf of which the friend request is attempted.</param>
    /// <returns>Error message if there is an error. Null otherwise.</returns>
    private string VerifyRequestingUser(UserInfo requestingUser)
    {
        if (!currentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            if (CanApprove())
            {
                if (!requestingUser.IsInSite(SiteContext.CurrentSiteName))
                {
                    return GetString("user.error_doesnoteexist");
                }
            }
            else if (requestingUser.UserID != currentUser.UserID)
            {
                return GetString("friends.error_permission_createfriendsonbehalf");
            }
        }

        return null;
    }


    /// <summary>
    /// Verifies whether current user has permissions to send a friend-request to <paramref name="requestedUser"/>
    /// </summary>
    /// <param name="requestedUser">User to which the friend request is being sent.</param>
    /// <returns>Error message if there is an error. Null otherwise.</returns>
    private string VerifyRequestedUser(UserInfo requestedUser)
    {
        if (!currentUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin) && (requestedUser != null))
        {
            if (!requestedUser.IsInSite(SiteContext.CurrentSiteName)
                    || (requestedUser.UserIsHidden)
                    || (!requestedUser.Enabled)
                    || (requestedUser.UserSettings.UserWaitingForApproval))
            {
                return GetString("user.error_doesnoteexist");
            }
        }

        return null;
    }

    #endregion


    #region "Button handling"

    protected void btnRequest_Click(object senderObject, EventArgs e)
    {
        RaiseOnCheckPermissions(PERMISSION_MANAGE, this);

        string message = string.Empty;

        // Requested user id not set explicitly
        if (RequestedUserID == 0)
        {
            RequestedUserID = ValidationHelper.GetInteger(selectUser.Value, 0);
        }

        // Both users have to be specified
        if ((RequestedUserID == 0) || (UserID == 0))
        {
            message = GetString("friends.friendrequired");
        }
        else
        {
            UserInfo requestedUser = UserInfoProvider.GetUserInfo(RequestedUserID);
            message = requestedUser == null ? GetString("user.error_doesnoteexist") : VerifyRequestedUser(requestedUser);
            if (String.IsNullOrEmpty(message))
            {
                if (!FriendInfoProvider.FriendshipExists(UserID, RequestedUserID))
                {
                    // Set up control
                    Comment = txtComment.Text;
                    SendMail = chkSendEmail.Checked;
                    SendMessage = chkSendMessage.Checked;
                    SelectedFriends = new List<int>();
                    SelectedFriends.Add(RequestedUserID);
                    AutomaticApprovment = chkAutomaticApprove.Checked;

                    message = PerformAction(FriendsActionEnum.Request);
                }
                else
                {
                    message = GetString("friends.friendshipexists");
                }
            }
        }

        if (!String.IsNullOrEmpty(message))
        {
            ShowError(message);
        }
        else
        {
            // Register wopener script
            ScriptHelper.RegisterWOpenerScript(Page);

            btnRequest.Enabled = false;
            selectUser.Enabled = false;
            txtComment.Enabled = false;
            chkAutomaticApprove.Enabled = false;
            chkSendEmail.Enabled = false;
            chkSendMessage.Enabled = false;
            ShowConfirmation(GetString("friends.friendshiprequested"));

            const string refreshScript = "if (window.top && window.top.refreshFriendsList) { window.top.refreshFriendsList(); } else if (window.opener && window.opener.refreshFriendsList) { window.opener.refreshFriendsList(); }";
            ScriptHelper.RegisterStartupScript(Page, typeof(string), "closeFriendsDialogFriendsList", refreshScript, true);

            ScriptHelper.RegisterStartupScript(Page,typeof(string),"closeFriendsDialog","CloseDialog(true);",true);
        }
    }

    #endregion
}
