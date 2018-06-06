using System;

using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.Membership;

public partial class CMSWebParts_Community_Friends_FriendsDataSource : CMSAbstractWebPart
{
    #region "Variables"

    private UserInfo mCurrentUser = null;

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
            srcFriends.StopProcessing = value;
        }
    }

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets WHERE condition.
    /// </summary>
    public string WhereCondition
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WhereCondition"), "");
        }
        set
        {
            SetValue("WhereCondition", value);
            srcFriends.WhereCondition = value;
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
                int userId = ValidationHelper.GetInteger(GetValue("UserID"), srcFriends.UserID);
                if (userId > 0)
                {
                    mCurrentUser = UserInfoProvider.GetUserInfo(userId);
                }
                else
                {
                    mCurrentUser = MembershipContext.CurrentUserProfile;
                }
            }

            if (mCurrentUser == null)
            {
                mCurrentUser = MembershipContext.AuthenticatedUser;
            }
            return mCurrentUser;
        }
    }


    /// <summary>
    /// Gets or sets ORDER BY condition.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderBy"), string.Empty);
        }
        set
        {
            SetValue("OrderBy", value);
            srcFriends.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets status of friends to be selected.
    /// </summary>
    public FriendshipStatusEnum FriendStatus
    {
        get
        {
            return (FriendshipStatusEnum)ValidationHelper.GetInteger(GetValue("FriendStatus"), 0);
        }
        set
        {
            SetValue("FriendStatus", (int)value);
            srcFriends.FriendStatus = value;
        }
    }


    /// <summary>
    /// Gets or sets the source filter name.
    /// </summary>
    public string FilterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterName"), "");
        }
        set
        {
            SetValue("FilterName", value);
            srcFriends.SourceFilterName = value;
        }
    }


    /// <summary>
    /// Gets or sets the cache item name.
    /// </summary>
    public override string CacheItemName
    {
        get
        {
            return base.CacheItemName;
        }
        set
        {
            base.CacheItemName = value;
            srcFriends.CacheItemName = value;
        }
    }


    /// <summary>
    /// Cache dependencies, each cache dependency on a new line.
    /// </summary>
    public override string CacheDependencies
    {
        get
        {
            return ValidationHelper.GetString(base.CacheDependencies, srcFriends.CacheDependencies);
        }
        set
        {
            base.CacheDependencies = value;
            srcFriends.CacheDependencies = value;
        }
    }


    /// <summary>
    /// Gets or sets the cache minutes.
    /// </summary>
    public override int CacheMinutes
    {
        get
        {
            return base.CacheMinutes;
        }
        set
        {
            base.CacheMinutes = value;
            srcFriends.CacheMinutes = value;
        }
    }

    /// <summary>
    /// Gest or sets selected columns.
    /// </summary>
    public string Columns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Columns"), string.Empty);
        }
        set
        {
            SetValue("Columns", value);
            srcFriends.SelectedColumns = value;
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing || (CurrentUser == null))
        {
            // Do nothing
        }
        else
        {
            srcFriends.WhereCondition = WhereCondition;
            srcFriends.OrderBy = OrderBy;
            srcFriends.UserID = CurrentUser.UserID;
            srcFriends.FilterName = ID;
            srcFriends.SourceFilterName = FilterName;
            srcFriends.CacheItemName = CacheItemName;
            srcFriends.CacheDependencies = CacheDependencies;
            srcFriends.CacheMinutes = CacheMinutes;
            srcFriends.FriendStatus = FriendStatus;
            srcFriends.SelectedColumns = Columns;
        }
    }


    /// <summary>
    /// Clears cache.
    /// </summary>
    public override void ClearCache()
    {
        srcFriends.ClearCache();
    }

    #endregion
}