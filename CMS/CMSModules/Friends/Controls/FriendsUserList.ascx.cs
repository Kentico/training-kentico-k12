using System;
using System.Data;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Friends_Controls_FriendsUserList : CMSUserControl
{
    #region "Variables"

    private bool mShowItemAsLink = true;
    private bool mShowAddLink = false;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Zero rows text.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return gridFriends.ZeroRowsText;
        }
        set
        {
            gridFriends.ZeroRowsText = value;
        }
    }


    /// <summary>
    /// Size of the page.
    /// </summary>
    public string PageSize
    {
        get
        {
            return gridFriends.PageSize;
        }
        set
        {
            gridFriends.PageSize = value;
        }
    }


    /// <summary>
    /// Determines whether to show add new item link.
    /// </summary>
    public bool ShowAddLink
    {
        get
        {
            return mShowAddLink;
        }
        set
        {
            mShowAddLink = value;
        }
    }


    /// <summary>
    /// Determines whether to show items as links.
    /// </summary>
    public bool ShowItemAsLink
    {
        get
        {
            return mShowItemAsLink;
        }
        set
        {
            mShowItemAsLink = value;
        }
    }
    
    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            // Stop processing
        }
        else
        {
            var user = MembershipContext.AuthenticatedUser;
            
            // Content is visible only for authenticated users
            if (AuthenticationHelper.IsAuthenticated())
            {
                if (string.IsNullOrEmpty(ZeroRowsText))
                {
                    ZeroRowsText = GetString("friends.nofriendsfound");
                }
                // Register modal dialog JS function
                ScriptHelper.RegisterDialogScript(Page);

                gridFriends.OrderBy = "UserName";
                gridFriends.OnExternalDataBound += gridFriends_OnExternalDataBound;
                gridFriends.IsLiveSite = IsLiveSite;
                
                // Where condition
                string where = "FriendStatus = " + Convert.ToInt32(FriendshipStatusEnum.Approved);
               
                // If not global admin, filter out friends from different sites
                if (!user.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
                {
                    where = SqlHelper.AddWhereCondition(where, "UserID IN(SELECT UserID FROM CMS_UserSite WHERE SiteID=" + SiteContext.CurrentSiteID + " )");
                }
                gridFriends.WhereCondition = where;
                // Add parameter @UserID
                if (gridFriends.QueryParameters == null)
                {
                    gridFriends.QueryParameters = new QueryDataParameters();
                }
                gridFriends.QueryParameters.Add("@UserID", user.UserID);
            }
            else
            {
                Visible = false;
            }
        }
    }

    #endregion


    #region "Grid methods"
    
    protected object gridFriends_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName)
        {
            case "formattedusername":
                DataRowView drv = parameter as DataRowView;
                int userId = ValidationHelper.GetInteger(drv["UserID"], 0);
                return GetItemText(userId, drv["UserName"], drv["FullName"], drv["UserNickName"]);
        }

        return parameter;
    }


    /// <summary>
    /// Renders row item according to control settings.
    /// </summary>
    /// <param name="userId">Selected user id</param>
    /// <param name="username">User Name</param>
    /// <param name="fullname">User full name</param>
    /// <param name="usernickname">Nickname</param>
    protected string GetItemText(int userId, object username, object fullname, object usernickname)
    {
        string usrName = ValidationHelper.GetString(username, string.Empty);
        string nick = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(usrName, fullname.ToString(), usernickname.ToString(), IsLiveSite));

        if (ShowItemAsLink)
        {
            return "<a href=\"javascript: window.parent.CloseAndRefresh(" + userId + ", " + ScriptHelper.GetString(usrName) + ", " +
                   ScriptHelper.GetString(QueryHelper.GetText("mid", String.Empty)) + ", " + ScriptHelper.GetString(QueryHelper.GetText("hidid", String.Empty)) +
                   ")\">" + nick + "</a>";
        }

        return nick;
    }

    #endregion
}
