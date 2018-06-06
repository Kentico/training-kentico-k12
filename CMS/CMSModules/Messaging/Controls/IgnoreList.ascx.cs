using System;
using System.Data;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.Messaging;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Messaging_Controls_IgnoreList : CMSUserControl
{
    #region "Private variables"

    private string currentValues = String.Empty;

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
            EnsureChildControls();
            return usUsers.ZeroRowsText;
        }
        set
        {
            EnsureChildControls();
            usUsers.ZeroRowsText = value;
        }
    }


    /// <summary>
    /// Page size value.
    /// </summary>
    public string PageSize
    {
        get
        {
            EnsureChildControls();
            return usUsers.UniSelector.ItemsPerPage.ToString();
        }
        set
        {
            EnsureChildControls();
            usUsers.UniSelector.ItemsPerPage = ValidationHelper.GetInteger(value, 0);
        }
    }

    #endregion


    #region "Page events"

    protected override void EnsureChildControls()
    {
        base.EnsureChildControls();
        if (usUsers == null)
        {
            pnlIgnoreList.LoadContainer();
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            // Stop processing
            usUsers.StopProcessing = true;
        }
        else
        {
            var ui = MembershipContext.AuthenticatedUser;

            // Content is visible only for authenticated users
            if (AuthenticationHelper.IsAuthenticated())
            {
                usUsers.UniSelector.OnSelectionChanged += UniSelector_OnSelectionChanged;
                usUsers.IsLiveSite = IsLiveSite;
                usUsers.WhereCondition = "UserName NOT LIKE N'public'";

                // Global admin can see all users
                if (ui.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
                {
                    usUsers.DisplayUsersFromAllSites = true;
                }
                else
                {
                    usUsers.DisplayUsersFromAllSites = false;

                    // Show only users from current site for normal users
                    usUsers.SiteID = SiteContext.CurrentSiteID;

                    if (IsLiveSite && !MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
                    {
                        usUsers.HideDisabledUsers = true;
                        usUsers.HideHiddenUsers = true;
                        usUsers.HideNonApprovedUsers = true;
                    }
                }

                // Hide site selector
                usUsers.ShowSiteFilter = false;

                currentValues = GetContactListValues();

                if (!RequestHelper.IsPostBack())
                {
                    usUsers.Value = currentValues;
                }

                if (string.IsNullOrEmpty(ZeroRowsText))
                {
                    ZeroRowsText = GetString("messaging.ignorelist.nodatafound");
                }

                // Register modal dialog JS function
                ScriptHelper.RegisterDialogScript(Page);
            }
            else
            {
                Visible = false;
            }
        }
    }

    #endregion


    #region "Other events"

    protected void UniSelector_OnSelectionChanged(object sender, EventArgs e)
    {
        SaveUsers();
    }

    #endregion


    #region "Other methods"

    private static string GetContactListValues()
    {
        DataSet ignoreList = IgnoreListInfoProvider.GetIgnoreList(MembershipContext.AuthenticatedUser.UserID, null, null, 0, "UserName,UserNickname,IgnoreListIgnoredUserID");
        if (!DataHelper.DataSourceIsEmpty(ignoreList))
        {
            return TextHelper.Join(";", DataHelper.GetStringValues(ignoreList.Tables[0], "IgnoreListIgnoredUserID"));
        }

        return String.Empty;
    }


    private void SaveUsers()
    {
        bool falseValues = false;

        // Remove old items
        string newValues = ValidationHelper.GetString(usUsers.Value, null);
        string items = DataHelper.GetNewItemsInList(newValues, currentValues);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            // Add all new items to user
            foreach (string item in newItems)
            {
                int userId = ValidationHelper.GetInteger(item, 0);
                IgnoreListInfoProvider.RemoveFromIgnoreList(MembershipContext.AuthenticatedUser.UserID, userId);
            }
        }

        // Add new items
        items = DataHelper.GetNewItemsInList(currentValues, newValues);
        if (!String.IsNullOrEmpty(items))
        {
            string[] newItems = items.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            // Add all new items to user
            foreach (string item in newItems)
            {
                int userId = ValidationHelper.GetInteger(item, 0);
                IgnoreListInfoProvider.AddToIgnoreList(MembershipContext.AuthenticatedUser.UserID, userId);
            }
        }

        if (falseValues)
        {
            currentValues = GetContactListValues();
            usUsers.Value = currentValues;
        }

        ShowChangesSaved();
    }

    #endregion
}
