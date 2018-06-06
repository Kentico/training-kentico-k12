using System;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;


public partial class CMSWebParts_Ecommerce_Products_TopNProductsBySales : CMSAbstractWebPart
{
    #region "Document properties"

    /// <summary>
    /// Site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SiteName"), lstElem.SiteName);
        }
        set
        {
            SetValue("SiteName", value);
            lstElem.SiteName = value;
        }
    }


    /// <summary>
    /// Gets or sets path where random products will be find.
    /// </summary>
    public string Path
    {
        get
        {
            string path = ValidationHelper.GetString(GetValue("Path"), String.Empty);
            if (String.IsNullOrEmpty(path))
            {
                return "/%";
            }

            return MacroContext.CurrentResolver.ResolvePath(path);
        }

        set
        {
            SetValue("Path", value);
            lstElem.Path = value;
        }
    }


    /// <summary>
    /// Maximal relative level.
    /// </summary>
    public int MaxRelativeLevel
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxRelativeLevel"), lstElem.MaxRelativeLevel);
        }
        set
        {
            SetValue("MaxRelativeLevel", value);
            lstElem.MaxRelativeLevel = value;
        }
    }


    /// <summary>
    /// Class names.
    /// </summary>
    public string ClassNames
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("Classnames"), lstElem.ClassNames);
        }
        set
        {
            SetValue("ClassNames", value);
            lstElem.ClassNames = value;
        }
    }


    /// <summary>
    /// Select top N items.
    /// </summary>
    public int SelectTopN
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("SelectTopN"), lstElem.SelectTopN);
        }
        set
        {
            SetValue("SelectTopN", value);
            lstElem.SelectTopN = value;
        }
    }


    /// <summary>
    /// Where condition.
    /// </summary>
    public string WhereCondition
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("WhereCondition"), GetWhereCondition(String.Empty).ToString(true));
        }
        set
        {
            SetValue("WhereCondition", value);
            lstElem.WhereCondition = GetWhereCondition(value).ToString(true);
        }
    }


    /// <summary>
    /// Order by clause.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("OrderBy"), GetOrderByExpression(String.Empty));
        }
        set
        {
            SetValue("OrderBy", value);
            lstElem.OrderBy = GetOrderByExpression(value);
        }
    }


    /// <summary>
    /// Select only published nodes.
    /// </summary>
    public bool SelectOnlyPublished
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyPublished"), lstElem.SelectOnlyPublished);
        }
        set
        {
            SetValue("SelectOnlyPublished", value);
            lstElem.SelectOnlyPublished = value;
        }
    }


    /// <summary>
    /// Culture code.
    /// </summary>
    public string CultureCode
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("CultureCode"), lstElem.CultureCode);
        }
        set
        {
            SetValue("CultureCode", value);
            lstElem.CultureCode = value;
        }
    }


    /// <summary>
    /// Combine with default culture.
    /// </summary>
    public bool CombineWithDefaultCulture
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CombineWithDefaultCulture"), lstElem.CombineWithDefaultCulture);
        }
        set
        {
            SetValue("CombineWithDefaultCulture", value);
            lstElem.CombineWithDefaultCulture = value;
        }
    }

    #endregion


    #region "System settings"

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
            lstElem.CacheItemName = value;
        }
    }


    /// <summary>
    /// Cache dependencies, each cache dependency on a new line.
    /// </summary>
    public override string CacheDependencies
    {
        get
        {
            return ValidationHelper.GetString(base.CacheDependencies, lstElem.CacheDependencies);
        }
        set
        {
            base.CacheDependencies = value;
            lstElem.CacheDependencies = value;
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
            lstElem.CacheMinutes = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether permissions should be checked.
    /// </summary>
    public bool CheckPermissions
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CheckPermissions"), lstElem.CheckPermissions);
        }
        set
        {
            SetValue("CheckPermissions", value);
            lstElem.CheckPermissions = value;
        }
    }

    #endregion


    #region "Layout"

    /// <summary>
    /// Repeat columns.
    /// </summary>
    public int RepeatColumns
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("RepeatColumns"), lstElem.RepeatColumns);
        }
        set
        {
            SetValue("RepeatColumns", value);
            lstElem.RepeatColumns = value;
        }
    }


    /// <summary>
    /// Repeat layout.
    /// </summary>
    public RepeatLayout RepeatLayout
    {
        get
        {
            return CMSDataList.GetRepeatLayout(DataHelper.GetNotEmpty(GetValue("RepeatLayout"), lstElem.RepeatLayout.ToString()));
        }
        set
        {
            SetValue("RepeatLayout", value.ToString());
            lstElem.RepeatLayout = value;
        }
    }


    /// <summary>
    /// Repeat Direction.
    /// </summary>
    public RepeatDirection RepeatDirection
    {
        get
        {
            return CMSDataList.GetRepeatDirection(DataHelper.GetNotEmpty(GetValue("RepeatDirection"), lstElem.RepeatDirection.ToString()));
        }
        set
        {
            SetValue("RepeatDirection", value.ToString());
            lstElem.RepeatDirection = value;
        }
    }

    #endregion


    #region "Transformation properties"

    /// <summary>
    /// Gets or sets the name of the transforamtion which is used for displaying the results.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("TransformationName"), lstElem.TransformationName);
        }
        set
        {
            SetValue("TransformationName", value);
            lstElem.TransformationName = value;
        }
    }


    /// <summary>
    /// Gets or sets the name of the transforamtion which is used for displaying the alternate results.
    /// </summary>
    public string AlternatingTransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("AlternatingTransformationName"), lstElem.AlternatingTransformationName);
        }
        set
        {
            SetValue("AlternatingTransformationName", value);
            lstElem.AlternatingTransformationName = value;
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Hide control for zero rows.
    /// </summary>
    public bool HideControlForZeroRows
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideControlForZeroRows"), lstElem.HideControlForZeroRows);
        }
        set
        {
            SetValue("HideControlForZeroRows", value);
            lstElem.HideControlForZeroRows = value;
        }
    }


    /// <summary>
    /// Zero rows text.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ZeroRowsText"), lstElem.ZeroRowsText);
        }
        set
        {
            SetValue("ZeroRowsText", value);
            lstElem.ZeroRowsText = value;
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        ReloadData();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            lstElem.StopProcessing = true;
        }
        else
        {
            lstElem.ControlContext = ControlContext;

            // System settings
            lstElem.CacheItemName = CacheItemName;
            lstElem.CacheDependencies = CacheDependencies;
            lstElem.CacheMinutes = CacheMinutes;
            lstElem.CheckPermissions = CheckPermissions;

            // Document properties
            lstElem.SiteName = SiteName;
            lstElem.ClassNames = ClassNames;
            lstElem.Path = Path;
            lstElem.MaxRelativeLevel = MaxRelativeLevel;
            lstElem.SelectOnlyPublished = SelectOnlyPublished;
            lstElem.CombineWithDefaultCulture = CombineWithDefaultCulture;
            lstElem.CultureCode = CultureCode;

            lstElem.SelectTopN = SelectTopN;
            lstElem.WhereCondition = WhereCondition;
            lstElem.OrderBy = GetOrderByExpression(OrderBy);

            // Layout
            lstElem.RepeatColumns = RepeatColumns;
            lstElem.RepeatDirection = RepeatDirection;
            lstElem.RepeatLayout = RepeatLayout;

            // Transformations            
            lstElem.AlternatingTransformationName = AlternatingTransformationName;
            lstElem.TransformationName = TransformationName;

            //// Public
            lstElem.HideControlForZeroRows = HideControlForZeroRows;
            lstElem.ZeroRowsText = ZeroRowsText;
        }
    }


    /// <summary>
    /// Reloads the data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
        lstElem.ReloadData(true);
    }


    /// <summary>
    /// Returns where condition.
    /// </summary>
    /// <param name="customWhere">Custom WHERE condition</param>
    private WhereCondition GetWhereCondition(string customWhere)
    {
        SiteInfo si;
        var where = new WhereCondition();

        // Get required site data
        if (!String.IsNullOrEmpty(SiteName))
        {
            si = SiteInfoProvider.GetSiteInfo(SiteName);
        }
        else
        {
            si = SiteContext.CurrentSite;
        }

        if (si != null)
        {
            // Build where condition
            var classWhere = new WhereCondition();

            // Get documents of the specified class only - without coupled data !!!            
            if (!String.IsNullOrEmpty(ClassNames))
            {
                string[] classNames = ClassNames.Trim(';').Split(';');
                foreach (string className in classNames)
                {
                    classWhere.WhereEquals("ClassName", className).Or();
                }
            }

            where.WhereIn("NodeSKUID", new IDQuery<OrderItemInfo>("OrderItemSKUID").Source(s => s.Join<SKUInfo>("OrderItemSKUID", "SKUID")
                                                                                   .Join<OrderInfo>("COM_OrderItem.OrderItemOrderID", "OrderID"))
                                                                                   .WhereTrue("SKUEnabled")
                                                                                   .WhereNull("SKUOptionCategoryID")
                                                                                   .WhereEquals("OrderSiteID", si.SiteID)
                                                                                   .Where(classWhere));

            // Add custom WHERE condition
            if (!String.IsNullOrEmpty(customWhere))
            {
                where.Where(customWhere);
            }
        }

        return where;
    }


    /// <summary>
    /// Returns ORDER BY expression.
    /// </summary>
    ///<param name="customOrderBy">Custom order by expression</param>
    private string GetOrderByExpression(string customOrderBy)
    {
        // Required "ORDER BY" expression
        string orderBy = "(SELECT Count(OrderItemSKUID) FROM COM_OrderItem WHERE COM_OrderItem.OrderItemSKUID = NodeSKUID GROUP BY OrderItemSKUID) DESC";

        // Add cutom "ORDER BY" expression
        if (!String.IsNullOrEmpty(customOrderBy))
        {
            orderBy += ", " + customOrderBy;
        }

        return orderBy;
    }


    /// <summary>
    /// OnPrerender override (Set visibility).
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        Visible = lstElem.Visible;

        if ((HideControlForZeroRows) && (DataHelper.DataSourceIsEmpty(lstElem.DataSource)))
        {
            Visible = false;
        }
    }


    /// <summary>
    /// Clears cache.
    /// </summary>
    public override void ClearCache()
    {
        lstElem.ClearCache();
    }
}