using System;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.Modules;
using CMS.UIControls;


public partial class CMSModules_Friends_Administration_Users_User_Edit_Friends_Requested : CMSUsersPage
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

        // Check 'manage' permission
        bool friendsManagePermission = currentUser.IsAuthorizedPerResource("CMS.Friends", "Manage") || (currentUser.UserID == userId);

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
                FriendsListRequested.UserID = userId;
                FriendsListRequested.OnCheckPermissions += CheckPermissions;
                FriendsListRequested.ZeroRowsText = GetString("friends.nouserrequestedfriends");

                // Request friend link
                string script =
                    "function displayRequest(){ \n" +
                    "modalDialog('" + ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Friends/Dialogs/Friends_Request.aspx") + "?userid=" + userId + "&siteid=" + SiteID + "', 'rejectDialog', 810, 460);}";

                ScriptHelper.RegisterStartupScript(this, GetType(), "displayModalRequest", ScriptHelper.GetScript(script));

                HeaderAction action = new HeaderAction();
                action.Text = GetString("Friends_List.NewItemCaption");
                action.OnClientClick = "displayRequest();";
                action.RedirectUrl = null;
                action.Enabled = friendsManagePermission;
                CurrentMaster.HeaderActions.AddAction(action);
            }
        }
    }


    protected void CheckPermissions(string permissionType, CMSAdminControl sender)
    {
        var currentUser = MembershipContext.AuthenticatedUser;
        if ((!currentUser.IsAuthorizedPerResource("CMS.Friends", permissionType)) && (currentUser.UserID != userId))
        {
            RedirectToAccessDenied("CMS.Friends", permissionType);
        }
    }
}
