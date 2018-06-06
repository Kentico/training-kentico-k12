using System;

using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Base;
using CMS.Membership;

public partial class CMSWebParts_Community_Friends_FriendsViewer : CMSAbstractWebPart
{
    #region "Variables"

    private UserInfo mCurrentUser = null;

    #endregion


    #region "Public properties"

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
    /// Gets or sets ORDER BY condition.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderBy"), "");
        }
        set
        {
            SetValue("OrderBy", value);
            srcFriends.OrderBy = value;
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
    /// Gets or sets selected columns.
    /// </summary>
    public string Columns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Columns"), "");
        }
        set
        {
            SetValue("Columns", value);
            srcFriends.SelectedColumns = value;
        }
    }

    #endregion


    #region "Basic repeater properties"

    /// <summary>
    /// Gets or sets AlternatingItemTemplate property.
    /// </summary>
    public string AlternatingItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternatingItemTransformationName"), "");
        }
        set
        {
            SetValue("AlternatingItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets FooterTemplate property.
    /// </summary>
    public string FooterTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FooterTransformationName"), "");
        }
        set
        {
            SetValue("FooterTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HeaderTemplate property.
    /// </summary>
    public string HeaderTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("HeaderTransformationName"), "");
        }
        set
        {
            SetValue("HeaderTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate property.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TransformationName"), "");
        }
        set
        {
            SetValue("TransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets SeparatorTemplate property.
    /// </summary>
    public string SeparatorTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SeparatorTransformationName"), "");
        }
        set
        {
            SetValue("SeparatorTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HideControlForZeroRows property.
    /// </summary>
    public bool HideControlForZeroRows
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideControlForZeroRows"), true);
        }
        set
        {
            SetValue("HideControlForZeroRows", value);
            repFriends.HideControlForZeroRows = value;
        }
    }


    /// <summary>
    /// Gets or sets ZeroRowsText property.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ZeroRowsText"), "");
        }
        set
        {
            SetValue("ZeroRowsText", value);
            repFriends.ZeroRowsText = value;
        }
    }

    #endregion


    #region "UniPager properties"

    /// <summary>
    /// Gets or sets the value that indicates whether pager should be hidden for single page.
    /// </summary>
    public bool HidePagerForSinglePage
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HidePagerForSinglePage"), pagerElem.HidePagerForSinglePage);
        }
        set
        {
            SetValue("HidePagerForSinglePage", value);
            pagerElem.HidePagerForSinglePage = value;
        }
    }


    /// <summary>
    /// Gets or sets the number of records to display on a page.
    /// </summary>
    public int PageSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PageSize"), pagerElem.PageSize);
        }
        set
        {
            SetValue("PageSize", value);
            pagerElem.PageSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the number of pages displayed for current page range.
    /// </summary>
    public int GroupSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("GroupSize"), pagerElem.GroupSize);
        }
        set
        {
            SetValue("GroupSize", value);
            pagerElem.GroupSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the pager mode ('querystring' or 'postback').
    /// </summary>
    public string PagingMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagingMode"), "querystring");
        }
        set
        {
            if (value != null)
            {
                SetValue("PagingMode", value);
                switch (value.ToLowerCSafe())
                {
                    case "postback":
                        pagerElem.PagerMode = UniPagerMode.PostBack;
                        break;
                    default:
                        pagerElem.PagerMode = UniPagerMode.Querystring;
                        break;
                }
            }
        }
    }


    /// <summary>
    /// Gets or sets the querysting parameter.
    /// </summary>
    public string QueryStringKey
    {
        get
        {
            return ValidationHelper.GetString(GetValue("QueryStringKey"), pagerElem.QueryStringKey);
        }
        set
        {
            SetValue("QueryStringKey", value);
            pagerElem.QueryStringKey = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public bool DisplayFirstLastAutomatically
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFirstLastAutomatically"), pagerElem.DisplayFirstLastAutomatically);
        }
        set
        {
            SetValue("DisplayFirstLastAutomatically", value);
            pagerElem.DisplayFirstLastAutomatically = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public bool DisplayPreviousNextAutomatically
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayPreviousNextAutomatically"), pagerElem.DisplayPreviousNextAutomatically);
        }
        set
        {
            SetValue("DisplayPreviousNextAutomatically", value);
            pagerElem.DisplayPreviousNextAutomatically = value;
        }
    }

    #endregion


    #region "UniPager Template properties"

    /// <summary>
    /// Gets or sets the pages template.
    /// </summary>
    public string PagesTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Pages"), "");
        }
        set
        {
            SetValue("Pages", value);
        }
    }


    /// <summary>
    /// Gets or sets the current page template.
    /// </summary>
    public string CurrentPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CurrentPage"), "");
        }
        set
        {
            SetValue("CurrentPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the separator template.
    /// </summary>
    public string SeparatorTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PageSeparator"), "");
        }
        set
        {
            SetValue("PageSeparator", value);
        }
    }


    /// <summary>
    /// Gets or sets the first page template.
    /// </summary>
    public string FirstPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FirstPage"), "");
        }
        set
        {
            SetValue("FirstPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the last page template.
    /// </summary>
    public string LastPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LastPage"), "");
        }
        set
        {
            SetValue("LastPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the previous page template.
    /// </summary>
    public string PreviousPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PreviousPage"), "");
        }
        set
        {
            SetValue("PreviousPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the next page template.
    /// </summary>
    public string NextPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NextPage"), "");
        }
        set
        {
            SetValue("NextPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the previous group template.
    /// </summary>
    public string PreviousGroupTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PreviousGroup"), "");
        }
        set
        {
            SetValue("PreviousGroup", value);
        }
    }


    /// <summary>
    /// Gets or sets the next group template.
    /// </summary>
    public string NextGroupTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NextGroup"), "");
        }
        set
        {
            SetValue("NextGroup", value);
        }
    }


    /// <summary>
    /// Gets or sets the layout template.
    /// </summary>
    public string LayoutTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagerLayout"), "");
        }
        set
        {
            SetValue("PagerLayout", value);
        }
    }


    /// <summary>
    /// Gets or sets the direct page template.
    /// </summary>
    public string DirectPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DirectPage"), "");
        }
        set
        {
            SetValue("DirectPage", value);
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
            srcFriends.StopProcessing = value;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();

        SetupControl();
    }


    /// <summary>
    /// OnPreRender override.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        if (!StopProcessing)
        {
            if (RequestHelper.IsPostBack() && (pagerElem.PagerMode == UniPagerMode.PostBack))
            {
                // Make sure that the pager (in postback mode) propagates the correct current page value
                pagerElem.RebindPager();
                repFriends.DataBind();
            }

            Visible &= repFriends.HasData() || !HideControlForZeroRows;
        }

        base.OnPreRender(e);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        repFriends.DataBindByDefault = false;
        pagerElem.PageControl = repFriends.ID;

        if (StopProcessing || (CurrentUser == null))
        {
            // Do nothing
            srcFriends.StopProcessing = true;
        }
        else
        {
            if (!String.IsNullOrEmpty(TransformationName))
            {
                // Basic control properties
                repFriends.HideControlForZeroRows = HideControlForZeroRows;
                repFriends.ZeroRowsText = ZeroRowsText;

                // Data source properties
                srcFriends.WhereCondition = WhereCondition;
                srcFriends.OrderBy = OrderBy;
                srcFriends.UserID = CurrentUser.UserID;
                srcFriends.SourceFilterName = FilterName;
                srcFriends.CacheItemName = CacheItemName;
                srcFriends.CacheDependencies = CacheDependencies;
                srcFriends.CacheMinutes = CacheMinutes;
                srcFriends.FriendStatus = FriendStatus;
                srcFriends.SelectedColumns = Columns;


                #region "Repeater template properties"

                // Apply transformations if they exist
                repFriends.ItemTemplate = TransformationHelper.LoadTransformation(this, TransformationName);

                if (!string.IsNullOrEmpty(AlternatingItemTransformationName))
                {
                    repFriends.AlternatingItemTemplate = TransformationHelper.LoadTransformation(this, AlternatingItemTransformationName);
                }
                if (!string.IsNullOrEmpty(FooterTransformationName))
                {
                    repFriends.FooterTemplate = TransformationHelper.LoadTransformation(this, FooterTransformationName);
                }
                if (!string.IsNullOrEmpty(HeaderTransformationName))
                {
                    repFriends.HeaderTemplate = TransformationHelper.LoadTransformation(this, HeaderTransformationName);
                }
                if (!string.IsNullOrEmpty(SeparatorTransformationName))
                {
                    repFriends.SeparatorTemplate = TransformationHelper.LoadTransformation(this, SeparatorTransformationName);
                }

                #endregion


                // UniPager properties
                pagerElem.PageSize = PageSize;
                pagerElem.GroupSize = GroupSize;
                pagerElem.QueryStringKey = QueryStringKey;
                pagerElem.DisplayFirstLastAutomatically = DisplayFirstLastAutomatically;
                pagerElem.DisplayPreviousNextAutomatically = DisplayPreviousNextAutomatically;
                pagerElem.HidePagerForSinglePage = HidePagerForSinglePage;

                switch (PagingMode.ToLowerCSafe())
                {
                    case "postback":
                        pagerElem.PagerMode = UniPagerMode.PostBack;
                        break;
                    default:
                        pagerElem.PagerMode = UniPagerMode.Querystring;
                        break;
                }


                #region "UniPager template properties"

                // UniPager template properties
                if (!string.IsNullOrEmpty(PagesTemplate))
                {
                    pagerElem.PageNumbersTemplate = TransformationHelper.LoadTransformation(pagerElem, PagesTemplate);
                }

                if (!string.IsNullOrEmpty(CurrentPageTemplate))
                {
                    pagerElem.CurrentPageTemplate = TransformationHelper.LoadTransformation(pagerElem, CurrentPageTemplate);
                }

                if (!string.IsNullOrEmpty(SeparatorTemplate))
                {
                    pagerElem.PageNumbersSeparatorTemplate = TransformationHelper.LoadTransformation(pagerElem, SeparatorTemplate);
                }

                if (!string.IsNullOrEmpty(FirstPageTemplate))
                {
                    pagerElem.FirstPageTemplate = TransformationHelper.LoadTransformation(pagerElem, FirstPageTemplate);
                }

                if (!string.IsNullOrEmpty(LastPageTemplate))
                {
                    pagerElem.LastPageTemplate = TransformationHelper.LoadTransformation(pagerElem, LastPageTemplate);
                }

                if (!string.IsNullOrEmpty(PreviousPageTemplate))
                {
                    pagerElem.PreviousPageTemplate = TransformationHelper.LoadTransformation(pagerElem, PreviousPageTemplate);
                }

                if (!string.IsNullOrEmpty(NextPageTemplate))
                {
                    pagerElem.NextPageTemplate = TransformationHelper.LoadTransformation(pagerElem, NextPageTemplate);
                }

                if (!string.IsNullOrEmpty(PreviousGroupTemplate))
                {
                    pagerElem.PreviousGroupTemplate = TransformationHelper.LoadTransformation(pagerElem, PreviousGroupTemplate);
                }

                if (!string.IsNullOrEmpty(NextGroupTemplate))
                {
                    pagerElem.NextGroupTemplate = TransformationHelper.LoadTransformation(pagerElem, NextGroupTemplate);
                }

                if (!String.IsNullOrEmpty(DirectPageTemplate))
                {
                    pagerElem.DirectPageTemplate = TransformationHelper.LoadTransformation(pagerElem, DirectPageTemplate);
                }

                if (!string.IsNullOrEmpty(LayoutTemplate))
                {
                    pagerElem.LayoutTemplate = TransformationHelper.LoadTransformation(pagerElem, LayoutTemplate);
                }

                #endregion


                // Connects repeater with data source
                repFriends.DataSource = srcFriends.DataSource;
            }

            pagerElem.RebindPager();
            repFriends.DataBind();
        }
    }

    #endregion


    #region "Overidden methods"

    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
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