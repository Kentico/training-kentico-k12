using System;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Friends_Administration_Users_User_Edit_Friends_Rejected : CMSUsersPage
{
    #region "Variables"

    protected int userId = 0;
    protected CurrentUserInfo currentUser = null;

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        userId = QueryHelper.GetInteger("userId", 0);
        currentUser = MembershipContext.AuthenticatedUser;
        if (userId <= 0 && currentUser != null)
        {
            userId = currentUser.UserID;
        }

        // Check 'read' permissions
        if (!currentUser.IsAuthorizedPerResource("CMS.Friends", "Read") && (currentUser.UserID != userId))
        {
            RedirectToAccessDenied("CMS.Friends", "Read");
        }

        // Check license
        if (DataHelper.GetNotEmpty(RequestContext.CurrentDomain, string.Empty) != string.Empty)
        {
            LicenseHelper.CheckFeatureAndRedirect(RequestContext.CurrentDomain, FeatureEnum.Friends);
        }

        if (userId > 0)
        {
            // Check that only global administrator can edit global administrator's accounts
            UserInfo ui = UserInfoProvider.GetUserInfo(userId);
            EditedObject = ui;

            if (!CheckGlobalAdminEdit(ui))
            {
                plcTable.Visible = false;
                lblError.Text = GetString("Administration-User_List.ErrorGlobalAdmin");
                lblError.Visible = true;
            }
            else
            {
                ScriptHelper.RegisterDialogScript(this);
                FriendsListRejected.UserID = userId;
                FriendsListRejected.UseEncapsulation = false;
                FriendsListRejected.OnCheckPermissions += CheckPermissions;
                FriendsListRejected.ZeroRowsText = GetString("friends.nouserrejectedfriends");
            }
        }
    }


    private void CheckPermissions(string permissionType, CMSAdminControl sender)
    {
        var currentUser = MembershipContext.AuthenticatedUser;
        if ((!currentUser.IsAuthorizedPerResource("CMS.Friends", permissionType)) && (currentUser.UserID != userId))
        {
            RedirectToAccessDenied("CMS.Friends", permissionType);
        }
    }
}
