using System;
using System.Data;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Messaging;
using CMS.UIControls;


public partial class CMSModules_Messaging_Controls_SelectFromContactList : CMSUserControl
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
            plcMess.IsLiveSite = value;
            base.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Zero rows text.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            EnsureChildControls();
            return gridContactList.ZeroRowsText;
        }
        set
        {
            EnsureChildControls();
            gridContactList.ZeroRowsText = value;
        }
    }


    /// <summary>
    /// Page size values separated with comma.
    /// </summary>
    public string PageSize
    {
        get
        {
            EnsureChildControls();
            return gridContactList.PageSize;
        }
        set
        {
            EnsureChildControls();
            gridContactList.PageSize = value;
        }
    }

    #endregion


    #region "Page events"

    protected override void EnsureChildControls()
    {
        base.EnsureChildControls();
        if (gridContactList == null)
        {
            pnlContactList.LoadContainer();
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            // Stop processing
            gridContactList.StopProcessing = true;
        }
        else
        {
            // Content is visible only for authenticated users
            if (AuthenticationHelper.IsAuthenticated())
            {
                if (string.IsNullOrEmpty(ZeroRowsText))
                {
                    ZeroRowsText = GetString("messaging.contactlist.nodatafound");
                }

                // Register modal dialog JS function
                ScriptHelper.RegisterDialogScript(Page);

                // Setup unigrid
                gridContactList.IsLiveSite = IsLiveSite;
                gridContactList.OnDataReload += gridContactList_OnDataReload;
                gridContactList.OnAction += gridContactList_OnAction;
                gridContactList.OnExternalDataBound += gridContactList_OnExternalDataBound;
                gridContactList.WhereCondition = GetWhereCondition();
            }
            else
            {
                Visible = false;
            }
        }
    }

    #endregion


    #region "Grid methods"

    protected object gridContactList_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName)
        {
            case "formattedusername":
                DataRowView drv = parameter as DataRowView;
                int userId = ValidationHelper.GetInteger(drv["ContactListContactUserID"], 0);
                return GetItemText(userId, drv["UserName"], drv["FullName"], drv["UserNickName"]);
        }
        return parameter;
    }


    protected void gridContactList_OnAction(string actionName, object actionArgument)
    {
        switch (actionName)
        {
            case "delete":
                int deletedUserId = ValidationHelper.GetInteger(actionArgument, 0);

                // If something is wrong return
                if (MembershipContext.AuthenticatedUser == null)
                {
                    return;
                }

                try
                {
                    // Deletes from contact list
                    ContactListInfoProvider.RemoveFromContactList(MembershipContext.AuthenticatedUser.UserID, deletedUserId);
                    ShowConfirmation(GetString("Messaging.ContactList.DeleteSuccessful"));
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
                break;
        }
    }


    protected DataSet gridContactList_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        return ContactListInfoProvider.GetContactList(MembershipContext.AuthenticatedUser.UserID, completeWhere, currentOrder, currentTopN, "UserName, UserNickname, FullName, ContactListContactUserID", currentOffset, currentPageSize, ref totalRecords);
    }


    /// <summary>
    /// Renders row item according to control settings.
    /// </summary>
    protected string GetItemText(int userId, object username, object fullname, object usernickname)
    {
        string usrName = ValidationHelper.GetString(username, string.Empty);
        string nick = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(usrName, fullname.ToString(), usernickname.ToString(), IsLiveSite));

        return "<a href=\"javascript: window.parent.CloseAndRefresh(" + userId + ", " + ScriptHelper.GetString(usrName) + ", " +
               ScriptHelper.GetString(QueryHelper.GetText("mid", String.Empty)) +
               ", " +
               ScriptHelper.GetString(QueryHelper.GetText("hidid", String.Empty)) +
               ")\">" + nick + "</a>";
    }


    /// <summary>
    /// Generate where condition
    /// </summary>
    private string GetWhereCondition()
    {
        if (IsLiveSite && !MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            // Hide hidden users
            string where = "((UserIsHidden IS NULL) OR (UserIsHidden=0))";

            // Hide disabled users
            where = SqlHelper.AddWhereCondition(where, UserInfoProvider.USER_ENABLED_WHERE_CONDITION);

            // Hide non-approved users
            where = SqlHelper.AddWhereCondition(where, "ContactListUserID IN (SELECT UserSettingsUserID FROM CMS_UserSettings WHERE ((UserWaitingForApproval IS NULL) OR (UserWaitingForApproval = 0)))");

            return where;
        }

        return null;
    }

    #endregion
}
