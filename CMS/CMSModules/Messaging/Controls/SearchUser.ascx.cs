using System;
using System.Data;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Messaging;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_Controls_SearchUser : CMSUserControl
{
    #region "Constants"

    private const string CONTACT_ACTION = "contact";
    private const string IGNORE_ACTION = "ignore";
    private const string FORMATTED_NAME = "formattedusername";

    #endregion


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
            return gridUsers.ZeroRowsText;
        }
        set
        {
            gridUsers.ZeroRowsText = value;
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Set default zero rows text
        if (string.IsNullOrEmpty(ZeroRowsText))
        {
            ZeroRowsText = GetString("messaging.search.nousersfound");
        }

        // Initialize UniGrid
        gridUsers.IsLiveSite = IsLiveSite;
        gridUsers.OnDataReload += gridUsers_OnDataReload;
        gridUsers.OnExternalDataBound += gridUsers_OnExternalDataBound;
        gridUsers.OnAction += gridUsers_OnAction;
    }

    #endregion


    #region "Grid methods"

    protected object gridUsers_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName)
        {
            case FORMATTED_NAME:
                DataRowView drv = parameter as DataRowView;
                int userId = ValidationHelper.GetInteger(drv["UserID"], 0);
                return GetItemText(userId, drv["UserName"], drv["FullName"], drv["UserNickName"]);
        }
        return parameter;
    }


    protected void gridUsers_OnAction(string actionName, object actionArgument)
    {
        PerformAction(actionName, actionArgument);
    }


    protected DataSet gridUsers_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        var parameters = new QueryDataParameters();
        parameters.Add("@search", "%" + txtSearch.Text + "%");
        parameters.Add("@siteID", SiteContext.CurrentSite.SiteID);

        string where = "UserName NOT LIKE N'public'";

        // If user is not global administrator
        if (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            // Do not select hidden users
            where = SqlHelper.AddWhereCondition(where, "((UserIsHidden IS NULL) OR (UserIsHidden=0))");

            // Select only approved users
            where = SqlHelper.AddWhereCondition(where, "((UserWaitingForApproval IS NULL) OR (UserWaitingForApproval = 0))");

            // Select only enabled users
            where = SqlHelper.AddWhereCondition(where, UserInfoProvider.USER_ENABLED_WHERE_CONDITION);
        }

        // Load all users for current site
        if (SiteContext.CurrentSite != null)
        {
            // Public user has no actions
            if (MembershipContext.AuthenticatedUser.IsPublic())
            {
                gridUsers.GridView.Columns[0].Visible = false;
            }
        }

#pragma warning disable BH2501 // Do not use ExecuteQuery in UI.
        return ConnectionHelper.ExecuteQuery("cms.user.finduserinsite", parameters, where, "UserName ASC", currentTopN, "View_CMS_User.UserID,UserName,UserNickName,FullName", currentOffset, currentPageSize, ref totalRecords);
#pragma warning restore BH2501 // Do not use ExecuteQuery in UI.
    }


    /// <summary>
    /// Returns correct format of username info.
    /// </summary>
    /// <param name="userId">Selected user id</param>
    /// <param name="username">User name</param>
    /// <param name="usernickname">User nickname</param>
    /// <param name="fullname">User full name</param>
    protected string GetItemText(int userId, object username, object fullname, object usernickname)
    {
        string usrName = ValidationHelper.GetString(username, string.Empty);
        string nick = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(usrName, fullname.ToString(), usernickname.ToString(), IsLiveSite));
        return "<a href=\"javascript: window.parent.CloseAndRefresh(" + userId + ", " + ScriptHelper.GetString(usrName) + ", " + ScriptHelper.GetString(QueryHelper.GetText("mid", String.Empty)) + ", " + ScriptHelper.GetString(QueryHelper.GetText("hidid", String.Empty)) + ")\">" + nick + "</a>";
    }

    #endregion


    #region "Other methods"

    protected void PerformAction(string actionName, object actionArgument)
    {
        int currentid = ValidationHelper.GetInteger(actionArgument, 0);
        switch (actionName)
        {
            case CONTACT_ACTION:
                // Add user to contact list
                ContactListInfoProvider.AddToContactList(MembershipContext.AuthenticatedUser.UserID, currentid);
                ShowConfirmation(GetString("messaging.search.addedsuccessfulytocontactlist"));
                break;

            case IGNORE_ACTION:
                // Add user to ignore list
                IgnoreListInfoProvider.AddToIgnoreList(MembershipContext.AuthenticatedUser.UserID, currentid);
                ShowConfirmation(GetString("messaging.search.addedsuccessfulytoignorelist"));
                break;
        }
    }

    #endregion
}