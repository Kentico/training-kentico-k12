using System;
using System.Collections;
using System.Data;

using CMS.Base;
using CMS.Base.Web.UI;

using System.Linq;

using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Membership_Pages_Users_User_Edit_Roles : CMSUsersPage
{
    #region "Protected variables"

    protected int siteId = 0;
    protected int userId = 0;
    protected UserInfo ui = null;
    protected string currentValues = string.Empty;

    #endregion


    #region "Events"

    /// <summary>
    /// Page_load event.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check permissions and UI elements
        var user = MembershipContext.AuthenticatedUser;
        if (user != null)
        {
            if (!user.IsAuthorizedPerUIElement("CMS.Users", "CmsDesk.Roles"))
            {
                RedirectToUIElementAccessDenied("CMS.Users", "CmsDesk.Roles");
            }

            if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.Roles", "Read"))
            {
                RedirectToAccessDenied("CMS.Roles", "Read");
            }
        }

        ScriptHelper.RegisterJQuery(Page);

        // Get user id and site Id from query
        userId = QueryHelper.GetInteger("userid", 0);

        // Show content placeholder where site selector can be shown
        CurrentMaster.DisplaySiteSelectorPanel = true;

        if ((SiteID > 0) && !MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            plcSites.Visible = false;
            CurrentMaster.DisplaySiteSelectorPanel = false;
        }

        if (userId > 0)
        {
            // Check that only global administrator can edit global administrator's accounts
            ui = UserInfoProvider.GetUserInfo(userId);
            CheckUserAvaibleOnSite(ui);
            EditedObject = ui;

            if (!CheckGlobalAdminEdit(ui))
            {
                plcTable.Visible = false;
                ShowError(GetString("Administration-User_List.ErrorGlobalAdmin"));
                return;
            }


            // Set site selector
            siteSelector.DropDownSingleSelect.AutoPostBack = true;
            siteSelector.AllowAll = false;
            siteSelector.AllowEmpty = false;

            // Global roles only for global admin
            if (MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
            {
                siteSelector.AllowGlobal = true;
            }

            // Only sites assigned to user
            siteSelector.UserId = userId;
            siteSelector.OnlyRunningSites = false;
            siteSelector.UniSelector.OnSelectionChanged += UniSelector_OnSelectionChanged;

            if (!RequestHelper.IsPostBack())
            {
                siteId = SiteContext.CurrentSiteID;

                // If user is member of current site
                if (UserSiteInfoProvider.GetUserSiteInfo(userId, siteId) != null)
                {
                    // Force uniselector to preselect current site
                    siteSelector.Value = siteId;
                }

                // Force to load data
                siteSelector.Reload(true);
            }

            // Get truly selected item
            siteId = ValidationHelper.GetInteger(siteSelector.Value, 0);
        }

        usRoles.OnSelectionChanged += usRoles_OnSelectionChanged;
        string siteIDWhere = (siteId <= 0) ? " SiteID IS NULL " : " SiteID =" + siteId;
        usRoles.WhereCondition = siteIDWhere + " AND RoleGroupID IS NULL";

        usRoles.SelectItemPageUrl = "~/CMSModules/Membership/Pages/Users/User_Edit_Add_Item_Dialog.aspx";
        usRoles.ListingWhereCondition = siteIDWhere + " AND RoleGroupID IS NULL AND UserID=" + userId;
        usRoles.ReturnColumnName = "RoleID";
        usRoles.DynamicColumnName = false;
        usRoles.GridName = "User_Role_List.xml";
        usRoles.AdditionalColumns = "ValidTo";
        usRoles.OnAdditionalDataBound += usMemberships_OnAdditionalDataBound;
        usRoles.DialogWindowHeight = 760;

        // Exclude generic roles
        string genericWhere = null;
        ArrayList genericRoles = RoleInfoProvider.GetGenericRoles();
        if (genericRoles.Count != 0)
        {
            foreach (string role in genericRoles)
            {
                genericWhere += "'" + SqlHelper.EscapeQuotes(role) + "',";
            }

            genericWhere = genericWhere.TrimEnd(',');
            usRoles.WhereCondition += " AND ( RoleName NOT IN (" + genericWhere + ") )";
        }

        // Get the active roles for this site
        var data = UserRoleInfoProvider.GetUserRoles().Where("UserID = " + userId + " AND RoleID IN (SELECT RoleID FROM CMS_Role WHERE SiteID IS NULL OR SiteID = " + siteId + ")").Columns("RoleID");
        if (data.Any())
        {
            currentValues = TextHelper.Join(";", DataHelper.GetStringValues(data.Tables[0], "RoleID"));
        }

        // If not postback or site selection changed
        if (!RequestHelper.IsPostBack() || (siteId != Convert.ToInt32(ViewState["rolesOldSiteId"])))
        {
            // Set values
            usRoles.Value = currentValues;
        }

        // Store selected site id
        ViewState["rolesOldSiteId"] = siteId;

        string script = "function setNewDateTime(date) {$cmsj('#" + hdnDate.ClientID + "').val(date);}";
        ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "key", ScriptHelper.GetScript(script));

        string eventTarget = Request[postEventSourceID];
        string eventArgument = Request[postEventArgumentID];
        if (eventTarget == ucCalendar.DateTimeTextBox.UniqueID)
        {
            if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.Users", "ManageUserRoles"))
            {
                RedirectToAccessDenied("CMS.Users", "Manage user roles");
            }

            int id = ValidationHelper.GetInteger(hdnDate.Value, 0);
            if (id != 0)
            {
                DateTime dt = ValidationHelper.GetDateTime(eventArgument, DateTimeHelper.ZERO_TIME);
                UserRoleInfo uri = UserRoleInfoProvider.GetUserRoleInfo(userId, id);
                if (uri != null)
                {
                    uri.ValidTo = dt;
                    UserRoleInfoProvider.SetUserRoleInfo(uri);

                    // Invalidate user
                    UserInfoProvider.InvalidateUser(userId);

                    ShowChangesSaved();
                }
            }
        }
    }


    /// <summary>
    /// Callback event for create calendar icon.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="sourceName">Event source name</param>
    /// <param name="parameter">Event parameter</param>
    /// <param name="val">Value from basic external data bound event</param>
    private object usMemberships_OnAdditionalDataBound(object sender, string sourceName, object parameter, object val)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "calendar":
                DataRowView drv = (parameter as DataRowView);
                string itemID = drv[usRoles.ReturnColumnName].ToString();
                string iconID = "icon_" + itemID;
                string date = drv["ValidTo"].ToString();
                string postback = ControlsHelper.GetPostBackEventReference(ucCalendar.DateTimeTextBox, "#").Replace("'#'", "$cmsj('#" + ucCalendar.DateTimeTextBox.ClientID + "').val()");
                string onClick = String.Empty;

                ucCalendar.DateTimeTextBox.Attributes["OnChange"] = postback;

                if (!ucCalendar.UseCustomCalendar)
                {
                    onClick = "$cmsj('#" + hdnDate.ClientID + "').val('" + itemID + "');" + ucCalendar.GenerateNonCustomCalendarImageEvent();
                }
                else
                {
                    onClick = "$cmsj('#" + hdnDate.ClientID + "').val('" + itemID + "'); var dt = $cmsj('#" + ucCalendar.DateTimeTextBox.ClientID + "'); dt.val('" + date + "'); dt.cmsdatepicker('setLocation','" + iconID + "'); dt.cmsdatepicker('show');";
                }

                 var button = new CMSGridActionButton
                {
                    ToolTip = GetString("membership.changevalidity"),
                    IconCssClass = "icon-calendar",
                    OnClientClick = onClick + "return false;",
                    ID = iconID
                };

                val = button.GetRenderedHTML();

                break;
        }

        return val;
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Display message if user has no site
        if ((!siteSelector.UniSelector.HasData) && (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin)))
        {
            ShowError(GetString("Administration-User_Edit_Roles.UserWithNoSites"));
        }
    }


    /// <summary>
    /// Handles site selection change event.
    /// </summary>
    protected void UniSelector_OnSelectionChanged(object sender, EventArgs e)
    {
        pnlUpdate.Update();
    }


    protected void usRoles_OnSelectionChanged(object sender, EventArgs e)
    {
        SaveData();
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Check whether current user is allowed to modify another user.
    /// </summary>
    /// <param name="userId">Modified user</param>
    /// <returns>"" or error message.</returns>
    protected static string ValidateGlobalAndDeskAdmin(UserInfo ui)
    {
        string result = String.Empty;

        if (MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            return result;
        }

        if (ui == null)
        {
            result = ResHelper.GetString("Administration-User.WrongUserId");
        }
        else
        {
            if (ui.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
            {
                result = ResHelper.GetString("Administration-User.NotAllowedToModify");
            }
        }
        return result;
    }


    /// <summary>
    /// Saves data.
    /// </summary>
    private void SaveData()
    {
        // Check "modify" permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.Users", "ManageUserRoles"))
        {
            RedirectToAccessDenied("CMS.Users", "Manage user roles");
        }

        bool saved = false;
        string result = ValidateGlobalAndDeskAdmin(ui);
        if (result != String.Empty)
        {
            ShowError(result);
            return;
        }

        // Remove old items
        string newValues = ValidationHelper.GetString(usRoles.Value, null);
        string items = DataHelper.GetNewItemsInList(newValues, currentValues);

        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (newItems != null)
            {
                // Add all new items to site
                foreach (string item in newItems)
                {
                    int roleID = ValidationHelper.GetInteger(item, 0);

                    var uri = UserRoleInfoProvider.GetUserRoleInfo(userId, roleID);
                    UserRoleInfoProvider.DeleteUserRoleInfo(uri);
                }

                saved = true;
            }
        }

        // Add new items
        items = DataHelper.GetNewItemsInList(currentValues, newValues);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (newItems != null)
            {
                DateTime dt = ValidationHelper.GetDateTime(hdnDate.Value, DateTimeHelper.ZERO_TIME);

                // Add all new items to site
                foreach (string item in newItems)
                {
                    int roleID = ValidationHelper.GetInteger(item, 0);
                    UserRoleInfoProvider.AddUserToRole(userId, roleID, dt);
                }

                saved = true;
            }
        }

        if (saved)
        {
            ShowChangesSaved();
            usRoles.Reload(true);
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        if (RequestHelper.IsPostBack())
        {
            pnlBasic.Update();
        }

        base.OnPreRender(e);
    }

    #endregion
}
