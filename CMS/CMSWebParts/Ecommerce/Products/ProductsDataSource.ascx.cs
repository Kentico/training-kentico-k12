using System;

using CMS.Helpers;
using CMS.Localization;
using CMS.MacroEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;


public partial class CMSWebParts_Ecommerce_Products_ProductsDataSource : CMSAbstractWebPart
{
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
            srcProducts.WhereCondition = value;
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
            srcProducts.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets top N selected documents.
    /// </summary>
    public int SelectTopN
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("SelectTopN"), 0);
        }
        set
        {
            SetValue("SelectTopN", value);
            srcProducts.TopN = value;
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
            srcProducts.SourceFilterName = value;
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
            srcProducts.CacheItemName = value;
        }
    }


    /// <summary>
    /// Cache dependencies, each cache dependency on a new line.
    /// </summary>
    public override string CacheDependencies
    {
        get
        {
            return ValidationHelper.GetString(base.CacheDependencies, srcProducts.CacheDependencies);
        }
        set
        {
            base.CacheDependencies = value;
            srcProducts.CacheDependencies = value;
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
            srcProducts.CacheMinutes = value;
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
            srcProducts.SelectedColumns = value;
        }
    }

    #endregion


    #region "Document filter properties"

    /// <summary>
    /// Indicates if the comments should be retrieved according to document filter settings.
    /// </summary>
    public bool UseDocumentFilter
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseDocumentFilter"), false);
        }
        set
        {
            SetValue("UseDocumentFilter", value);
            srcProducts.UseDocumentFilter = value;
        }
    }


    /// <summary>
    /// Gets or sets the site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SiteName"), "");
        }
        set
        {
            SetValue("SiteName", value);
            srcProducts.SiteName = value;
        }
    }


    /// <summary>
    /// Gets or sets the where condition for blog posts.
    /// </summary>
    public string DocumentsWhereCondition
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DocumentsWhereCondition"), "");
        }
        set
        {
            SetValue("DocumentsWhereCondition", value);
            srcProducts.DocumentsWhereCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether documents are combined with default culture version.
    /// </summary>
    public bool CombineWithDefaultCulture
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CombineWithDefaultCulture"), false);
        }
        set
        {
            SetValue("CombineWithDefaultCulture", value);
            srcProducts.CombineWithDefaultCulture = value;
        }
    }


    /// <summary>
    /// Gets or sets the culture code of the documents.
    /// </summary>
    public string CultureCode
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("CultureCode"), LocalizationContext.PreferredCultureCode);
        }
        set
        {
            SetValue("CultureCode", value);
            srcProducts.CultureCode = value;
        }
    }


    /// <summary>
    /// Gets or sets the maximal relative level of the documents to be shown.
    /// </summary>
    public int MaxRelativeLevel
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxRelativeLevel"), -1);
        }
        set
        {
            SetValue("MaxRelativeLevel", value);
            srcProducts.MaxRelativeLevel = value;
        }
    }


    /// <summary>
    /// Gets or sets the path of the documents.
    /// </summary>
    public string Path
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Path"), srcProducts.Path);
        }
        set
        {
            SetValue("Path", value);
            srcProducts.Path = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether only published documents are selected.
    /// </summary>
    public bool SelectOnlyPublished
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyPublished"), false);
        }
        set
        {
            SetValue("SelectOnlyPublished", value);
            srcProducts.SelectOnlyPublished = value;
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
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Properties
            srcProducts.WhereCondition = WhereCondition;
            srcProducts.OrderBy = OrderBy;
            srcProducts.TopN = SelectTopN;
            srcProducts.FilterName = ValidationHelper.GetString(GetValue("WebPartControlID"), ID);
            srcProducts.SourceFilterName = FilterName;
            srcProducts.CacheItemName = CacheItemName;
            srcProducts.CacheDependencies = CacheDependencies;
            srcProducts.CacheMinutes = CacheMinutes;
            srcProducts.SelectedColumns = Columns;

            // Prepare alias path
            string aliasPath = Path;
            if (String.IsNullOrEmpty(aliasPath))
            {
                aliasPath = "/%";
            }
            aliasPath = MacroResolver.ResolveCurrentPath(aliasPath);

            // Prepare site name
            string siteName = SiteName;
            if (String.IsNullOrEmpty(siteName))
            {
                siteName = SiteContext.CurrentSiteName;
            }

            // Prepare culture code
            string cultureCode = CultureCode;
            if (String.IsNullOrEmpty(cultureCode))
            {
                cultureCode = LocalizationContext.PreferredCultureCode;
            }

            // Document filter properties
            srcProducts.SiteName = siteName;
            srcProducts.UseDocumentFilter = UseDocumentFilter;
            srcProducts.DocumentsWhereCondition = DocumentsWhereCondition;
            srcProducts.CombineWithDefaultCulture = CombineWithDefaultCulture;
            srcProducts.CultureCode = cultureCode;
            srcProducts.SelectOnlyPublished = SelectOnlyPublished;
            srcProducts.MaxRelativeLevel = MaxRelativeLevel;
            srcProducts.Path = aliasPath;
        }
    }


    /// <summary>
    /// Clears cache.
    /// </summary>
    public override void ClearCache()
    {
        srcProducts.ClearCache();
    }
}