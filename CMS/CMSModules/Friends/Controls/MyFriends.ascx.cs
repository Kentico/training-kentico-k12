using System;

using CMS.Base;
using CMS.DocumentEngine.Web.UI.Configuration;
using CMS.Helpers;
using CMS.Membership;
using CMS.UIControls;


public partial class CMSModules_Friends_Controls_MyFriends : CMSAdminControl
{
    #region "Private variables"

    protected string page = string.Empty;
    private bool mDisplayFriendsList = true;
    private bool mDisplayFriendsToApproval = true;
    private bool mDisplayFriendsRequested = true;
    private bool mDisplayFriendsRejected = true;
    private string mParameterName = "subpage";
    private int mUserId = 0;
    private string selectedPage = string.Empty;

    #endregion


    #region "Public properties"
    
    /// <summary>
    /// Gets or sets the value that indicates whether 'friends list' is displayed.
    /// </summary>
    public bool DisplayFriendsList
    {
        get
        {
            return mDisplayFriendsList;
        }
        set
        {
            mDisplayFriendsList = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'friends waiting for approval' is displayed.
    /// </summary>
    public bool DisplayFriendsToApproval
    {
        get
        {
            return mDisplayFriendsToApproval;
        }
        set
        {
            mDisplayFriendsToApproval = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'requested friends list' is displayed.
    /// </summary>
    public bool DisplayFriendsRequested
    {
        get
        {
            return mDisplayFriendsRequested;
        }
        set
        {
            mDisplayFriendsRequested = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'rejected friends list' is displayed.
    /// </summary>
    public bool DisplayFriendsRejected
    {
        get
        {
            return mDisplayFriendsRejected;
        }
        set
        {
            mDisplayFriendsRejected = value;
        }
    }


    /// <summary>
    /// Gets or sets the WebPart CSS class value.
    /// </summary>
    public string CssClass
    {
        get
        {
            return pnlBody.CssClass;
        }
        set
        {
            pnlBody.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the query string parameter name.
    /// </summary>
    public string ParameterName
    {
        get
        {
            return mParameterName;
        }
        set
        {
            mParameterName = value;
        }
    }


    /// <summary>
    /// Gets or sets User ID.
    /// </summary>
    public int UserID
    {
        get
        {
            return mUserId;
        }
        set
        {
            friendsListElem.UserID = value;
            friendsToApprovalListElem.UserID = value;
            friendsRequestedListElem.UserID = value;
            friendsRejectedListElem.UserID = value;
            mUserId = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        ReloadData();
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        friendsListElem.StopProcessing = true;
        friendsToApprovalListElem.StopProcessing = true;
        friendsRequestedListElem.StopProcessing = true;
        friendsRejectedListElem.StopProcessing = true;
        friendsRejectedListElem.UseEncapsulation = false;

        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Show content only for authenticated users
            if (AuthenticationHelper.IsAuthenticated())
            {
                // Set up controls
                friendsListElem.UserID = UserID;
                friendsToApprovalListElem.UserID = UserID;
                friendsRequestedListElem.UserID = UserID;
                friendsRejectedListElem.UserID = UserID;

                // Remove 'saved' parameter from querystring
                string absoluteUri = RequestContext.CurrentURL;

                // Menu initialization
                tabMenu.TabControlIdPrefix = "MyFriends";
                tabMenu.UrlTarget = "_self";

                if (DisplayFriendsRequested)
                {
                    tabMenu.AddTab(new TabItem
                    {
                        Text = GetString("friends.requestedfriendships"),
                        RedirectUrl = URLHelper.AddParameterToUrl(absoluteUri, ParameterName, "requested")
                    });
                    selectedPage = "requested";
                }

                if (DisplayFriendsRejected)
                {
                    tabMenu.AddTab(new TabItem
                    {
                        Text = GetString("friends.rejectedfriends"),
                        RedirectUrl = URLHelper.AddParameterToUrl(absoluteUri, ParameterName, "rejected")
                    });
                    selectedPage = "rejected";
                }

                if (DisplayFriendsList)
                {
                    tabMenu.AddTab(new TabItem
                    {
                        Text = GetString("friends.myfriends"),
                        RedirectUrl = URLHelper.AddParameterToUrl(absoluteUri, ParameterName, "friends")
                    });
                    selectedPage = "friends";
                }

                // Set css class
                pnlBody.CssClass = CssClass;

                // Get page url
                page = QueryHelper.GetText(ParameterName, selectedPage);

                // Set controls visibility
                plcFriends.Visible = false;
                plcApprovalFriends.Visible = false;
                plcRequestedFriends.Visible = false;
                friendsRejectedListElem.Visible = false;
                requestFriendshipElem1.UserID = UserID;
                requestFriendshipElem2.UserID = UserID;
                // Select current page
                switch (page)
                {
                    default:
                    case "friends":
                        tabMenu.SelectedTab = 2;
                        if (DisplayFriendsList)
                        {
                            plcFriends.Visible = true;
                            friendsListElem.StopProcessing = false;
                        }
                        if (DisplayFriendsToApproval)
                        {
                            plcApprovalFriends.Visible = true;
                            friendsToApprovalListElem.StopProcessing = false;
                        }
                        break;

                    case "requested":
                        tabMenu.SelectedTab = 0;
                        plcRequestedFriends.Visible = true;
                        friendsRequestedListElem.StopProcessing = false;
                        break;

                    case "rejected":
                        tabMenu.SelectedTab = 1;
                        friendsRejectedListElem.Visible = true;
                        friendsRejectedListElem.StopProcessing = false;
                        break;
                }
            }
            else
            {
                Visible = false;
            }
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        switch (page)
        {
            case "friends":
                if (!friendsListElem.HasData())
                {
                    friendsListElem.ZeroRowsText = GetString("friends.nofriends");
                }

                if (!friendsToApprovalListElem.HasData())
                {
                    plcApprovalFriends.Visible = false;
                }
                break;

            case "requested":
                if (!friendsRequestedListElem.HasData())
                {
                    friendsRequestedListElem.ZeroRowsText = GetString("friends.norequestedfriends");
                }
                break;

            case "rejected":
                if (!friendsRejectedListElem.HasData())
                {
                    friendsRejectedListElem.ZeroRowsText = GetString("friends.norejectedfriends");
                }
                break;
        }
    }


    /// <summary>
    /// Overridden SetValue - because of MyAccount webpart.
    /// </summary>
    /// <param name="propertyName">Name of the property to set</param>
    /// <param name="value">Value to set</param>
    public override bool SetValue(string propertyName, object value)
    {
        base.SetValue(propertyName, value);

        switch (propertyName.ToLowerCSafe())
        {
            case "userid":
                UserID = ValidationHelper.GetInteger(value, 0);
                break;
        }

        return true;
    }
}