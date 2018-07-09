using System;

using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Navigation_cmssitemap : CMSAbstractWebPart
{
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
            smElem.CacheMinutes = value;
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
            smElem.CacheDependencies = value;
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
            smElem.CacheItemName = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether permissions are checked.
    /// </summary>
    public bool CheckPermissions
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CheckPermissions"), smElem.CheckPermissions);
        }
        set
        {
            SetValue("CheckPermissions", value);
            smElem.CheckPermissions = value;
        }
    }


    /// <summary>
    /// Gets or sets the class names.
    /// </summary>
    public string ClassNames
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("Classnames"), smElem.ClassNames), smElem.ClassNames);
        }
        set
        {
            SetValue("ClassNames", value);
            smElem.ClassNames = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether selected documents are combined with default culture.
    /// </summary>
    public bool CombineWithDefaultCulture
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CombineWithDefaultCulture"), smElem.CombineWithDefaultCulture);
        }
        set
        {
            SetValue("CombineWithDefaultCulture", value);
            smElem.CombineWithDefaultCulture = value;
        }
    }


    /// <summary>
    /// Gets or sets the culture code.
    /// </summary>
    public string CultureCode
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("CultureCode"), smElem.CultureCode), smElem.CultureCode);
        }
        set
        {
            SetValue("CultureCode", value);
            smElem.CultureCode = value;
        }
    }


    /// <summary>
    /// Gets or sets the maximal relative level.
    /// </summary>
    public int MaxRelativeLevel
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxRelativeLevel"), smElem.MaxRelativeLevel);
        }
        set
        {
            SetValue("MaxRelativeLevel", value);
            smElem.MaxRelativeLevel = value;
        }
    }


    /// <summary>
    /// Gets or sets the order by clause.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("OrderBy"), smElem.OrderBy), smElem.OrderBy);
        }
        set
        {
            SetValue("OrderBy", value);
            smElem.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets the nodes path.
    /// </summary>
    public string Path
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("Path"), smElem.Path);
        }
        set
        {
            SetValue("Path", value);
            smElem.Path = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether selected documents must be published.
    /// </summary>
    public bool SelectOnlyPublished
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyPublished"), smElem.SelectOnlyPublished);
        }
        set
        {
            SetValue("SelectOnlyPublished", value);
            smElem.SelectOnlyPublished = value;
        }
    }


    /// <summary>
    /// Gets or sets the site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("SiteName"), smElem.SiteName), smElem.SiteName);
        }
        set
        {
            SetValue("SiteName", value);
            smElem.SiteName = value;
        }
    }


    /// <summary>
    /// Gets or sets the where condition.
    /// </summary>
    public string WhereCondition
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("WhereCondition"), smElem.WhereCondition);
        }
        set
        {
            SetValue("WhereCondition", value);
            smElem.WhereCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether item ID will be rendered.
    /// </summary>
    public bool RenderLinkTitle
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("RenderLinkTitle"), smElem.RenderLinkTitle);
        }
        set
        {
            SetValue("RenderLinkTitle", value);
            smElem.RenderLinkTitle = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether text can be wrapped or space is replaced with non breakable space.
    /// </summary>
    public bool WordWrap
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("WordWrap"), smElem.WordWrap);
        }
        set
        {
            SetValue("WordWrap", value);
            smElem.WordWrap = value;
        }
    }


    /// <summary>
    /// Gets or sets the link URL target.
    /// </summary>
    public string UrlTarget
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("UrlTarget"), smElem.UrlTarget), smElem.UrlTarget);
        }
        set
        {
            SetValue("UrlTarget", value);
            smElem.UrlTarget = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether control should be hidden if no data found.
    /// </summary>
    public bool HideControlForZeroRows
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideControlForZeroRows"), smElem.HideControlForZeroRows);
        }
        set
        {
            SetValue("HideControlForZeroRows", value);
            smElem.HideControlForZeroRows = value;
        }
    }


    /// <summary>
    /// Gets or sets the text which is displayed for zero rows results.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ZeroRowsText"), smElem.ZeroRowsText);
        }
        set
        {
            SetValue("ZeroRowsText", value);
            smElem.ZeroRowsText = value;
        }
    }


    /// <summary>
    /// Gets or sets the filter name.
    /// </summary>
    public string FilterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterName"), smElem.FilterName);
        }
        set
        {
            SetValue("FilterName", value);
            smElem.FilterName = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the sitemap should apply menu inactivation flag.
    /// </summary>
    public bool ApplyMenuInactivation
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ApplyMenuInactivation"), true);
        }
        set
        {
            SetValue("ApplyMenuInactivation", value);
        }
    }


    /// <summary>
    /// Gets or sets property which indicates if menu caption should be HTML encoded.
    /// </summary>
    public bool EncodeMenuCaption
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("EncodeMenuCaption"), smElem.EncodeMenuCaption);
        }
        set
        {
            SetValue("EncodeMenuCaption", value);
            smElem.EncodeMenuCaption = value;
        }
    }


    /// <summary>
    /// Gets or sets the columns to be retrieved from database.
    /// </summary>  
    public string Columns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Columns"), smElem.Columns);
        }
        set
        {
            SetValue("Columns", value);
            smElem.Columns = value;
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
            smElem.StopProcessing = true;
        }
        else
        {
            smElem.ControlContext = ControlContext;

            // Set properties from Webpart form        
            smElem.CacheItemName = CacheItemName;
            smElem.CacheDependencies = CacheDependencies;
            smElem.CacheMinutes = CacheMinutes;
            smElem.CheckPermissions = CheckPermissions;
            smElem.ClassNames = ClassNames;
            smElem.CombineWithDefaultCulture = CombineWithDefaultCulture;
            smElem.CultureCode = CultureCode;
            smElem.MaxRelativeLevel = MaxRelativeLevel;
            smElem.OrderBy = OrderBy;
            smElem.Path = Path;
            smElem.SelectOnlyPublished = SelectOnlyPublished;
            smElem.SiteName = SiteName;
            smElem.UrlTarget = UrlTarget;
            smElem.WhereCondition = WhereCondition;
            smElem.RenderLinkTitle = RenderLinkTitle;
            smElem.WordWrap = WordWrap;
            smElem.ApplyMenuInactivation = ApplyMenuInactivation;
            smElem.HideControlForZeroRows = HideControlForZeroRows;
            smElem.ZeroRowsText = ZeroRowsText;
            smElem.FilterName = FilterName;
            smElem.EncodeMenuCaption = EncodeMenuCaption;
            smElem.Columns = Columns;
        }
    }


    /// <summary>
    /// OnPreRender override (Set visibility).
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        Visible = smElem.Visible;

        if (DataHelper.DataSourceIsEmpty(smElem.DataSource) && (smElem.HideControlForZeroRows))
        {
            Visible = false;
        }
    }
}