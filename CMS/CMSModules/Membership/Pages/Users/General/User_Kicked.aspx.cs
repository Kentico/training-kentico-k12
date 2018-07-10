using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;

[CheckLicence(FeatureEnum.OnlineUsers)]
public partial class CMSModules_Membership_Pages_Users_General_User_Kicked : CMSUsersPage
{
    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        // If session management is disabled inform about it
        if (!SessionManager.OnlineUsersEnabled)
        {
            // Set disabled module info
            ucDisabledModule.TestSettingKeys = "CMSUseSessionManagement";
            ucDisabledModule.InfoText = GetString("administration.users.online.disabled");

            gridElem.Visible = false;
        }
        else
        {
            SetHeaderActions();
            PrepareUnigrid();

            gridElem.HideFilterButton = true;
            gridElem.LoadGridDefinition();

            // Filter settings
            IUserFilter filter = (IUserFilter)gridElem.CustomFilter;
            filter.DisplayUserEnabled = false;
        }
    }


    protected override void CheckUIPermissions()
    {
        // Check UI Permissions for online marketing if needed
        if (QueryHelper.GetBoolean("isonlinemarketing", false))
        {
            CheckUIElementAccessHierarchical(ModuleName.ONLINEMARKETING, "On-line_users.KickedUsers");
        }
        else
        {
            base.CheckUIPermissions();
        }
    }

    #endregion


    #region "Unigrid"

    /// <summary>
    ///  On action event.
    /// </summary>
    private void gridElem_OnAction(string actionName, object actionArgument)
    {
        // Check "modify" permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.Users", "Modify"))
        {
            RedirectToAccessDenied("CMS.Users", "Modify");
        }

        int userId = ValidationHelper.GetInteger(actionArgument, 0);
        switch (actionName.ToLowerCSafe())
        {
            // Undo kick action
            case "undokick":
                SessionManager.RemoveUserFromKicked(userId);
                PrepareUnigrid();
                gridElem.ReBind();
                ShowConfirmation(GetString("kicked.cancel"));
                break;
        }
    }


    /// <summary>
    ///  On external databound event.
    /// </summary>
    private object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName)
        {
            case "formattedusername":
                return GetFormattedUsername(parameter);

            case "undokick":
                return ModifyUndoKickButton(sender, parameter);

            default:
                return "";
        }
    }


    /// <summary>
    /// Grid definition loaded.
    /// </summary>
    void gridElem_OnLoadColumns()
    {
        gridElem.GridActions.Actions.RemoveAt(2);
        gridElem.GridActions.Actions.RemoveAt(0);
    }

    #endregion


    #region "Unigrid  data bound methods"

    /// <summary>
    /// Get formatted user name.
    /// </summary>
    private string GetFormattedUsername(object parameter)
    {
        var drv = (DataRowView)parameter;
        if (drv != null)
        {
            var ui = new UserInfo(drv.Row);
            string userName = Functions.GetFormattedUserName(ui.UserName);
            if (AuthenticationHelper.UserKicked(ui.UserID))
            {
                return HTMLHelper.HTMLEncode(userName) + " <span style=\"color:#ee0000;\">" + GetString("administration.users.onlineusers.kicked") + "</span>";
            }

            return HTMLHelper.HTMLEncode(userName);
        }
        return "";
    }


    /// <summary>
    /// Displays button for undo kicked users.
    /// </summary>
    private object ModifyUndoKickButton(object sender, object parameter)
    {
        int userID = ValidationHelper.GetInteger(((DataRowView)((GridViewRow)parameter).DataItem).Row["UserID"], 0);
        var button = (CMSGridActionButton)sender;
        button.Enabled = AuthenticationHelper.UserKicked(userID);

        return "";
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Set header actions.
    /// </summary>
    private void SetHeaderActions()
    {
        HeaderAction action = new HeaderAction();
        action.Text = GetString("General.Refresh");
        action.RedirectUrl = RequestContext.CurrentURL;
        CurrentMaster.HeaderActions.AddAction(action);
    }


    /// <summary>
    /// Prepares unigrid.
    /// </summary>
    private void PrepareUnigrid()
    {
        gridElem.ObjectType = "cms.userlist";
        gridElem.WhereCondition = DataHelper.GetNotEmpty(SessionManager.GetKickedUsersWhere(), "1 = 0");

        // Setup unigrid events
        gridElem.OnAction += gridElem_OnAction;
        gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;
        gridElem.ZeroRowsText = GetString("general.nodatafound");
        gridElem.OnLoadColumns += gridElem_OnLoadColumns;
    }

    #endregion
}