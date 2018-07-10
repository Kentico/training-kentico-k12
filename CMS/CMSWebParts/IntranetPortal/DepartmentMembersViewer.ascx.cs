using System;
using System.Collections.Generic;
using System.Data;

using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.SiteProvider;

using TreeNode = CMS.DocumentEngine.TreeNode;
using CMS.DataEngine;

public partial class CMSWebParts_IntranetPortal_DepartmentMembersViewer : CMSAbstractWebPart
{
    #region "Constants"

    private const string DEPARTMENT_CLASS_NAME = "intranetportal.department";

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets path to department document.
    /// </summary>
    public string Path
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Path"), "");
        }
        set
        {
            SetValue("Path", value);
        }
    }


    /// <summary>
    /// Gets or sets whether use filter control.
    /// </summary>
    public bool ShowFilterControl
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowFilterControl"), true);
        }
        set
        {
            SetValue("ShowFilterControl", value);
        }
    }


    /// <summary>
    /// Gets or sets WHERE condition.
    /// </summary>
    public string WhereCondition
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WhereCondition"), srcUsers.WhereCondition);
        }
        set
        {
            SetValue("WhereCondition", value);
            srcUsers.WhereCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets ORDER BY condition.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderBy"), srcUsers.OrderBy);
        }
        set
        {
            SetValue("OrderBy", value);
            srcUsers.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets top N selected documents.
    /// </summary>
    public int SelectTopN
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("SelectTopN"), srcUsers.TopN);
        }
        set
        {
            SetValue("SelectTopN", value);
            srcUsers.TopN = value;
        }
    }


    /// <summary>
    /// Gets or sets selected columns.
    /// </summary>
    public string Columns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Columns"), srcUsers.SelectedColumns);
        }
        set
        {
            SetValue("Columns", value);
            srcUsers.SelectedColumns = value;
        }
    }


    /// <summary>
    /// Gets or sets the source filter name.
    /// </summary>
    public string FilterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterName"), srcUsers.SourceFilterName);
        }
        set
        {
            SetValue("FilterName", value);
            srcUsers.SourceFilterName = value;
        }
    }


    /// <summary>
    /// Gets or sets site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SiteName"), SiteContext.CurrentSiteName);
        }
        set
        {
            SetValue("SiteName", value);
            srcUsers.SiteName = value;
        }
    }


    /// <summary>
    /// Gets or sets the select only approved users.
    /// </summary>
    public bool SelectOnlyApproved
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyApproved"), srcUsers.SelectOnlyApproved);
        }
        set
        {
            SetValue("SelectOnlyApproved", value);
            srcUsers.SelectOnlyApproved = value;
        }
    }


    /// <summary>
    /// Gets or sets the select hidden users.
    /// </summary>
    public bool SelectHidden
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectHidden"), srcUsers.SelectHidden);
        }
        set
        {
            SetValue("SelectHidden", value);
            srcUsers.SelectHidden = value;
        }
    }


    /// <summary>
    /// Gest or sest the cache item name.
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
            srcUsers.CacheItemName = value;
        }
    }


    /// <summary>
    /// Cache dependencies, each cache dependency on a new line.
    /// </summary>
    public override string CacheDependencies
    {
        get
        {
            return ValidationHelper.GetString(base.CacheDependencies, srcUsers.CacheDependencies);
        }
        set
        {
            base.CacheDependencies = value;
            srcUsers.CacheDependencies = value;
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
            srcUsers.CacheMinutes = value;
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
            return ValidationHelper.GetBoolean(GetValue("HideControlForZeroRows"), false);
        }
        set
        {
            SetValue("HideControlForZeroRows", value);
            repUsers.HideControlForZeroRows = value;
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
            repUsers.ZeroRowsText = value;
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
            return ValidationHelper.GetString(GetValue("PagingMode"), "postback");
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


    /// <summary>
    /// Gets or sets the value that indicates whether paging is enabled.
    /// </summary>
    public bool EnablePaging
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("EnablePaging"), true);
        }
        set
        {
            SetValue("EnablePaging", value);
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();

        SetupControl();
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// OnFilterChanged - init data properties and rebind repeater.
    /// </summary>
    protected void filterUsers_OnFilterChanged()
    {
        filterUsers.InitDataProperties(srcUsers);

        // Connects repeater with data source
        repUsers.DataSource = srcUsers.DataSource;
        repUsers.DataBind();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        // Set general properties
        repUsers.DataBindByDefault = false;
        pagerElem.PageControl = repUsers.ID;

        if (StopProcessing)
        {
            // Do nothing
            filterUsers.StopProcessing = true;
            srcUsers.StopProcessing = true;
        }
        else
        {
            filterUsers.Visible = ShowFilterControl;
            filterUsers.OnFilterChanged += filterUsers_OnFilterChanged;
            srcUsers.OnFilterChanged += filterUsers_OnFilterChanged;

            // Basic control properties
            repUsers.HideControlForZeroRows = HideControlForZeroRows;
            repUsers.ZeroRowsText = ZeroRowsText;


            TreeNode node = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            // Check if path is set
            if (String.IsNullOrEmpty(Path))
            {
                TreeNode curDoc = DocumentContext.CurrentDocument;
                // Check if current document is department
                if ((curDoc != null) && (curDoc.NodeClassName.ToLowerCSafe() == DEPARTMENT_CLASS_NAME))
                {
                    node = DocumentContext.CurrentDocument;
                }
            }
            else
            {
                // Obtain document from specified path
                node = tree.SelectSingleNode(SiteName, Path, LocalizationContext.PreferredCultureCode, true, DEPARTMENT_CLASS_NAME, false, false, false);
            }

            // If department document exists and has own ACL continue with initializing controls
            if ((node != null) && node.NodeIsACLOwner)
            {
                // Get users and roles with read permission for department document
                int aclId = ValidationHelper.GetInteger(node.GetValue("NodeACLID"), 0);
                DataSet dsRoles = AclItemInfoProvider.GetAllowedRoles(aclId, NodePermissionsEnum.Read, "RoleID");
                DataSet dsUsers = AclItemInfoProvider.GetAllowedUsers(aclId, NodePermissionsEnum.Read, "UserID");

                string where = null;

                // Process users dataset to where condition
                if (!DataHelper.DataSourceIsEmpty(dsUsers))
                {
                    // Get allowed users ids
                    IList<string> users = DataHelper.GetStringValues(dsUsers.Tables[0], "UserID");
                    string userIds = TextHelper.Join(", ", users);

                    // Populate where condition with user condition                
                    where = SqlHelper.AddWhereCondition("UserID IN (" + userIds + ")", where);
                }

                // Process roles dataset to where condition
                if (!DataHelper.DataSourceIsEmpty(dsRoles))
                {
                    // Get allowed roles ids
                    IList<string> roles = DataHelper.GetStringValues(dsRoles.Tables[0], "RoleID");
                    string roleIds = TextHelper.Join(", ", roles);

                    // Populate where condition with role condition
                    where = SqlHelper.AddWhereCondition("UserID IN (SELECT UserID FROM View_CMS_UserRole_MembershipRole_ValidOnly_Joined WHERE RoleID IN (" + roleIds + "))", where, "OR");
                }


                if (!String.IsNullOrEmpty(where))
                {
                    // Check if exist where condition and add it to current where condition
                    where = SqlHelper.AddWhereCondition(WhereCondition, where);

                    // Data source properties
                    srcUsers.WhereCondition = where;
                    srcUsers.OrderBy = OrderBy;
                    srcUsers.TopN = SelectTopN;
                    srcUsers.SelectedColumns = Columns;
                    srcUsers.SiteName = SiteName;
                    srcUsers.FilterName = filterUsers.ID;
                    srcUsers.SourceFilterName = FilterName;
                    srcUsers.CacheItemName = CacheItemName;
                    srcUsers.CacheDependencies = CacheDependencies;
                    srcUsers.CacheMinutes = CacheMinutes;
                    srcUsers.SelectOnlyApproved = SelectOnlyApproved;
                    srcUsers.SelectHidden = SelectHidden;

                    // Init data properties
                    filterUsers.InitDataProperties(srcUsers);


                    #region "Repeater template properties"

                    // Apply transformations if they exist
                    if (!String.IsNullOrEmpty(TransformationName))
                    {
                        repUsers.ItemTemplate = TransformationHelper.LoadTransformation(this, TransformationName);
                    }
                    if (!String.IsNullOrEmpty(AlternatingItemTransformationName))
                    {
                        repUsers.AlternatingItemTemplate = TransformationHelper.LoadTransformation(this, AlternatingItemTransformationName);
                    }
                    if (!String.IsNullOrEmpty(FooterTransformationName))
                    {
                        repUsers.FooterTemplate = TransformationHelper.LoadTransformation(this, FooterTransformationName);
                    }
                    if (!String.IsNullOrEmpty(HeaderTransformationName))
                    {
                        repUsers.HeaderTemplate = TransformationHelper.LoadTransformation(this, HeaderTransformationName);
                    }
                    if (!String.IsNullOrEmpty(SeparatorTransformationName))
                    {
                        repUsers.SeparatorTemplate = TransformationHelper.LoadTransformation(this, SeparatorTransformationName);
                    }

                    #endregion


                    // UniPager properties
                    pagerElem.PageSize = PageSize;
                    pagerElem.GroupSize = GroupSize;
                    pagerElem.QueryStringKey = QueryStringKey;
                    pagerElem.DisplayFirstLastAutomatically = DisplayFirstLastAutomatically;
                    pagerElem.DisplayPreviousNextAutomatically = DisplayPreviousNextAutomatically;
                    pagerElem.HidePagerForSinglePage = HidePagerForSinglePage;
                    pagerElem.Enabled = EnablePaging;

                    switch (PagingMode.ToLowerCSafe())
                    {
                        case "querystring":
                            pagerElem.PagerMode = UniPagerMode.Querystring;
                            break;

                        default:
                            pagerElem.PagerMode = UniPagerMode.PostBack;
                            break;
                    }


                    #region "UniPager template properties"

                    // UniPager template properties
                    if (!String.IsNullOrEmpty(PagesTemplate))
                    {
                        pagerElem.PageNumbersTemplate = TransformationHelper.LoadTransformation(pagerElem, PagesTemplate);
                    }

                    if (!String.IsNullOrEmpty(CurrentPageTemplate))
                    {
                        pagerElem.CurrentPageTemplate = TransformationHelper.LoadTransformation(pagerElem, CurrentPageTemplate);
                    }

                    if (!String.IsNullOrEmpty(SeparatorTemplate))
                    {
                        pagerElem.PageNumbersSeparatorTemplate = TransformationHelper.LoadTransformation(pagerElem, SeparatorTemplate);
                    }

                    if (!String.IsNullOrEmpty(FirstPageTemplate))
                    {
                        pagerElem.FirstPageTemplate = TransformationHelper.LoadTransformation(pagerElem, FirstPageTemplate);
                    }

                    if (!String.IsNullOrEmpty(LastPageTemplate))
                    {
                        pagerElem.LastPageTemplate = TransformationHelper.LoadTransformation(pagerElem, LastPageTemplate);
                    }

                    if (!String.IsNullOrEmpty(PreviousPageTemplate))
                    {
                        pagerElem.PreviousPageTemplate = TransformationHelper.LoadTransformation(pagerElem, PreviousPageTemplate);
                    }

                    if (!String.IsNullOrEmpty(NextPageTemplate))
                    {
                        pagerElem.NextPageTemplate = TransformationHelper.LoadTransformation(pagerElem, NextPageTemplate);
                    }

                    if (!String.IsNullOrEmpty(PreviousGroupTemplate))
                    {
                        pagerElem.PreviousGroupTemplate = TransformationHelper.LoadTransformation(pagerElem, PreviousGroupTemplate);
                    }

                    if (!String.IsNullOrEmpty(NextGroupTemplate))
                    {
                        pagerElem.NextGroupTemplate = TransformationHelper.LoadTransformation(pagerElem, NextGroupTemplate);
                    }

                    if (!String.IsNullOrEmpty(DirectPageTemplate))
                    {
                        pagerElem.DirectPageTemplate = TransformationHelper.LoadTransformation(pagerElem, DirectPageTemplate);
                    }

                    if (!String.IsNullOrEmpty(LayoutTemplate))
                    {
                        pagerElem.LayoutTemplate = TransformationHelper.LoadTransformation(pagerElem, LayoutTemplate);
                    }

                    #endregion


                    // Connects repeater with data source
                    repUsers.DataSource = srcUsers.DataSource;
                }
                else
                {
                    // Disable datasource
                    srcUsers.StopProcessing = true;
                }
            }
            else
            {
                // Disable datasource
                srcUsers.StopProcessing = true;
            }

            pagerElem.RebindPager();
            repUsers.DataBind();
        }
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
                repUsers.DataBind();
            }

            Visible &= repUsers.HasData() || !HideControlForZeroRows;
        }

        base.OnPreRender(e);
    }


    /// <summary>
    /// Clears cache.
    /// </summary>
    public override void ClearCache()
    {
        srcUsers.ClearCache();
    }
}