using System;
using System.Web;
using System.Web.UI;
using System.Text;
using CMS.IO;
using System.Globalization;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine;

public partial class CMSWebParts_Navigation_xmlsitemap : CMSAbstractWebPart
{
    #region "Sitemap properties"

    /// <summary>
    /// Indicates whether sitemap index transformations should be used
    /// </summary>
    public bool IsSiteMapIndex
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("IsSiteMapIndex"), ucSiteMap.IsSiteMapIndex);
        }
        set
        {
            SetValue("IsSiteMapIndex", value);
            ucSiteMap.IsSiteMapIndex = value;
        }
    }


    /// <summary>
    /// Indicates whether exclude from search option should be ignored for sitemap
    /// </summary>
    public bool IgnoreExcludeFromSearch
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("IgnoreExcludeFromSearch"), ucSiteMap.IgnoreExcludeFromSearch);
        }
        set
        {
            SetValue("IgnoreExcludeFromSearch", value);
            ucSiteMap.IgnoreExcludeFromSearch = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether children should be hidden if parent is not accessible. False by default.
    /// </summary>
    public bool HideChildrenForHiddenParent
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideChildrenForHiddenParent"), ucSiteMap.HideChildrenForHiddenParent);
        }
        set
        {
            SetValue("HideChildrenForHiddenParent", value);
            ucSiteMap.HideChildrenForHiddenParent = value;
        }
    }


    /// <summary>
    /// Property to set and get name of transformation for displaying results. If none is set, default transformation is used.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TransformationName"), String.Empty);
        }
        set
        {
            SetValue("TransformationName", value);
            ucSiteMap.TransformationName = value;
        }
    }

    #endregion


    #region "Document properties"

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
            ucSiteMap.CacheMinutes = value;
        }
    }


    /// <summary>
    /// Gets or sets the cache item dependencies.
    /// </summary>
    public override string CacheDependencies
    {
        get
        {
            return base.CacheDependencies;
        }
        set
        {
            base.CacheDependencies = value;
            ucSiteMap.CacheDependencies = value;
        }
    }


    /// <summary>
    /// Gets or sets the name of the cache item. If not explicitly specified, the name is automatically 
    /// created based on the control unique ID
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
            ucSiteMap.CacheItemName = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether permissions are checked.
    /// </summary>
    public bool CheckPermissions
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CheckPermissions"), ucSiteMap.CheckPermissions);
        }
        set
        {
            SetValue("CheckPermissions", value);
            ucSiteMap.CheckPermissions = value;
        }
    }


    /// <summary>
    /// Gets or sets the class names.
    /// </summary>
    public string ClassNames
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("Classnames"), ucSiteMap.ClassNames), ucSiteMap.ClassNames);
        }
        set
        {
            SetValue("ClassNames", value);
            ucSiteMap.ClassNames = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether selected documents are combined with default culture.
    /// </summary>
    public bool CombineWithDefaultCulture
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CombineWithDefaultCulture"), ucSiteMap.CombineWithDefaultCulture);
        }
        set
        {
            SetValue("CombineWithDefaultCulture", value);
            ucSiteMap.CombineWithDefaultCulture = value;
        }
    }


    /// <summary>
    /// Gets or sets the culture code.
    /// </summary>
    public string CultureCode
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("CultureCode"), ucSiteMap.CultureCode), ucSiteMap.CultureCode);
        }
        set
        {
            SetValue("CultureCode", value);
            ucSiteMap.CultureCode = value;
        }
    }


    /// <summary>
    /// Gets or sets the maximal relative level.
    /// </summary>
    public int MaxRelativeLevel
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxRelativeLevel"), ucSiteMap.MaxRelativeLevel);
        }
        set
        {
            SetValue("MaxRelativeLevel", value);
            ucSiteMap.MaxRelativeLevel = value;
        }
    }


    /// <summary>
    /// Gets or sets the order by clause.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("OrderBy"), ucSiteMap.OrderBy), ucSiteMap.OrderBy);
        }
        set
        {
            SetValue("OrderBy", value);
            ucSiteMap.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets the nodes path.
    /// </summary>
    public string Path
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("Path"), ucSiteMap.Path);
        }
        set
        {
            SetValue("Path", value);
            ucSiteMap.Path = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether selected documents must be published.
    /// </summary>
    public bool SelectOnlyPublished
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyPublished"), ucSiteMap.SelectOnlyPublished);
        }
        set
        {
            SetValue("SelectOnlyPublished", value);
            ucSiteMap.SelectOnlyPublished = value;
        }
    }


    /// <summary>
    /// Gets or sets the site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("SiteName"), ucSiteMap.SiteName), ucSiteMap.SiteName);
        }
        set
        {
            SetValue("SiteName", value);
            ucSiteMap.SiteName = value;
        }
    }


    /// <summary>
    /// Gets or sets the where condition.
    /// </summary>
    public string WhereCondition
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("WhereCondition"), ucSiteMap.WhereCondition);
        }
        set
        {
            SetValue("WhereCondition", value);
            ucSiteMap.WhereCondition = value;
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
        if (StopProcessing)
        {
            // Do nothing
            ucSiteMap.StopProcessing = true;
        }
        else
        {
            // Switch by view mode
            switch (PortalContext.ViewMode)
            {

                case ViewModeEnum.LiveSite:

                    // Set properties
                    ucSiteMap.IgnoreExcludeFromSearch = IgnoreExcludeFromSearch;
                    ucSiteMap.HideChildrenForHiddenParent = HideChildrenForHiddenParent;
                    ucSiteMap.IsSiteMapIndex = IsSiteMapIndex;

                    ucSiteMap.TransformationName = TransformationName;

                    ucSiteMap.CacheMinutes = CacheMinutes;
                    ucSiteMap.CacheDependencies = CacheDependencies;
                    ucSiteMap.CacheItemName = CacheItemName;
                    ucSiteMap.CheckPermissions = CheckPermissions;

                    ucSiteMap.ClassNames = ClassNames;
                    ucSiteMap.CombineWithDefaultCulture = CombineWithDefaultCulture;
                    ucSiteMap.CultureCode = CultureCode;
                    ucSiteMap.MaxRelativeLevel = MaxRelativeLevel;
                    ucSiteMap.OrderBy = OrderBy;
                    ucSiteMap.Path = Path;
                    ucSiteMap.SelectOnlyPublished = SelectOnlyPublished;
                    ucSiteMap.SiteName = SiteName;
                    ucSiteMap.WhereCondition = WhereCondition;

                    // Reload data
                    ucSiteMap.ReloadData(true);

                    // Keep current response
                    HttpResponse response = Context.Response;

                    // Render XML
                    response.Clear();
                    response.ClearContent();
                    response.ContentType = "text/xml";
                    response.ContentEncoding = Encoding.UTF8;

                    // Render control
                    StringBuilder stringBuilder = new StringBuilder();
                    using (StringWriter textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
                    {
                        using (HtmlTextWriter writer = new HtmlTextWriter(textWriter))
                        {
                            RenderChildren(writer);
                        }
                    }

                    // Add to the response an end response
                    response.Write(stringBuilder.ToString());
                    RequestHelper.EndResponse();
                    break;

                // For other view modes do nothing and display message
                default:
                    ucSiteMap.StopProcessing = true;
                    lbSiteMap.Visible = true;
                    lbSiteMap.Text = GetString("xmlsitemap.nodisplay");
                    break;
            }
        }
    }


    /// <summary>
    /// Reloads the control data
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }

    #endregion
}