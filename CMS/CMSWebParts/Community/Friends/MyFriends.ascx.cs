using System;

using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.Membership;

public partial class CMSWebParts_Community_Friends_MyFriends : CMSAbstractWebPart
{
    #region "Variables"

    private UserInfo mCurrentUser = null;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the WebPart CSS class value.
    /// </summary>
    public override string CssClass
    {
        get
        {
            return base.CssClass;
        }
        set
        {
            base.CssClass = value;
            myFriendsElem.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the query string parameter name.
    /// </summary>
    public string ParameterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ParameterName"), myFriendsElem.ParameterName);
        }
        set
        {
            SetValue("ParameterName", value);
            myFriendsElem.ParameterName = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'friends list' is displayed.
    /// </summary>
    public bool DisplayFriendsList
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFriendsList"), myFriendsElem.DisplayFriendsList);
        }
        set
        {
            SetValue("DisplayFriendsList", value);
            myFriendsElem.DisplayFriendsList = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'friends waiting for approval' is displayed.
    /// </summary>
    public bool DisplayFriendsToApproval
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFriendsToApproval"), myFriendsElem.DisplayFriendsToApproval);
        }
        set
        {
            SetValue("DisplayFriendsToApproval", value);
            myFriendsElem.DisplayFriendsToApproval = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'requested friends list' is displayed.
    /// </summary>
    public bool DisplayFriendsRequested
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFriendsRequested"), myFriendsElem.DisplayFriendsRequested);
        }
        set
        {
            SetValue("DisplayFriendsRequested", value);
            myFriendsElem.DisplayFriendsRequested = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'rejected friends list' is displayed.
    /// </summary>
    public bool DisplayFriendsRejected
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFriendsRejected"), myFriendsElem.DisplayFriendsRejected);
        }
        set
        {
            SetValue("DisplayFriendsRejected", value);
            myFriendsElem.DisplayFriendsRejected = value;
        }
    }


    /// <summary>
    /// User name.
    /// </summary>
    public string UserName
    {
        get
        {
            return CurrentUser.UserName;
        }
        set
        {
            SetValue("UserName", value);
        }
    }


    /// <summary>
    /// Gets or sets current user object.
    /// </summary>
    public new UserInfo CurrentUser
    {
        get
        {
            if (mCurrentUser == null)
            {
                mCurrentUser = UserInfoProvider.GetUserInfo(ValidationHelper.GetString(GetValue("UserName"), string.Empty));
            }

            if (mCurrentUser == null)
            {
                // Backward compatibility
                int userId = ValidationHelper.GetInteger(GetValue("UserID"), myFriendsElem.UserID);
                if (userId > 0)
                {
                    mCurrentUser = UserInfoProvider.GetUserInfo(userId);
                }
            }

            if (mCurrentUser == null)
            {
                mCurrentUser = MembershipContext.AuthenticatedUser;
            }
            return mCurrentUser;
        }
    }

    #endregion


    #region "Stop processing"

    /// <summary>
    /// Returns true if the control processing should be stopped.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return base.StopProcessing;
        }
        set
        {
            base.StopProcessing = value;
            myFriendsElem.StopProcessing = value;
        }
    }

    #endregion


    #region "Overidden"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Setup control.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing || (CurrentUser == null) || CurrentUser.IsPublic())
        {
            // Do nothing
            myFriendsElem.StopProcessing = true;
            Visible = false;
        }
        else
        {
            // Setup my friends control
            myFriendsElem.ParameterName = ParameterName;
            myFriendsElem.CssClass = CssClass;
            myFriendsElem.DisplayFriendsList = DisplayFriendsList;
            myFriendsElem.DisplayFriendsToApproval = DisplayFriendsToApproval;
            myFriendsElem.DisplayFriendsRequested = DisplayFriendsRequested;
            myFriendsElem.DisplayFriendsRejected = DisplayFriendsRejected;
            myFriendsElem.UserID = CurrentUser.UserID;
        }
    }

    #endregion
}