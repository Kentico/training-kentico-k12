using System;
using System.Data;
using System.Web;
using System.Web.UI;

using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Search;

public partial class CMSWebParts_Search_cmssearchresults : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates whether permissions should be checked.
    /// </summary>
    public bool CheckPermissions
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CheckPermissions"), srchResults.CheckPermissions);
        }
        set
        {
            SetValue("CheckPermissions", value);
            srchResults.CheckPermissions = value;
        }
    }


    /// <summary>
    /// Gets or sets the class names which should be displayed.
    /// </summary>
    public string ClassNames
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("Classnames"), srchResults.ClassNames), srchResults.ClassNames);
        }
        set
        {
            SetValue("Classnames", value);
            srchResults.ClassNames = value;
        }
    }


    /// <summary>
    /// Gets or sets the ID of the search dialog.
    /// </summary>
    public string CMSSearchDialogID
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("CMSSearchDialogID"), srchResults.CMSSearchDialogID), srchResults.CMSSearchDialogID);
        }
        set
        {
            SetValue("CMSSearchDialogID", value);
            srchResults.CMSSearchDialogID = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether documents are combined with default culture version.
    /// </summary>
    public bool CombineWithDefaultCulture
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("CombineWithDefaultCulture"), srchResults.CombineWithDefaultCulture);
        }
        set
        {
            SetValue("CombineWithDefaultCulture", value);
            srchResults.CombineWithDefaultCulture = value;
        }
    }


    /// <summary>
    /// Gets or sets the culture code of the documents.
    /// </summary>
    public string CultureCode
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("CultureCode"), srchResults.CultureCode), srchResults.CultureCode);
        }
        set
        {
            SetValue("CultureCode", value);
            srchResults.CultureCode = value;
        }
    }


    /// <summary>
    /// Gets or sets the path of the documents.
    /// </summary>
    public string Path
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("Path"), srchResults.Path);
        }
        set
        {
            SetValue("Path", value);
            srchResults.Path = value;
        }
    }


    /// <summary>
    /// Gets or sets the search mode.
    /// </summary>
    public SearchModeEnum SearchMode
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchMode"), srchResults.SearchMode.ToStringRepresentation()).ToEnum<SearchModeEnum>();
        }
        set
        {
            SetValue("SearchMode", value.ToStringRepresentation());
            srchResults.SearchMode = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether only published documents are selected.
    /// </summary>
    public bool SelectOnlyPublished
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyPublished"), srchResults.SelectOnlyPublished);
        }
        set
        {
            SetValue("SelectOnlyPublished", value);
            srchResults.SelectOnlyPublished = value;
        }
    }


    /// <summary>
    /// Gets or sets the site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("SiteName"), srchResults.SiteName), srchResults.SiteName);
        }
        set
        {
            SetValue("SiteName", value);
            srchResults.SiteName = value;
        }
    }


    /// <summary>
    /// Gets or sets the where condition.
    /// </summary>
    public string WhereCondition
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("WhereCondition"), srchResults.WhereCondition), srchResults.WhereCondition);
        }
        set
        {
            SetValue("WhereCondition", value);
            srchResults.WhereCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets the order by.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("OrderBy"), srchResults.OrderBy), srchResults.OrderBy);
        }
        set
        {
            SetValue("OrderBy", value);
            srchResults.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets the text to be displayed if no results has been found.
    /// </summary>
    public string NoResultText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("NoResultText"), ResHelper.LocalizeString("{$CMSSearchResults.NoDocumentFound$}"));
        }
        set
        {
            SetValue("NoResultText", value);
            srchResults.NoResultsLabel.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the name of the transforamtion which is used for displaying the results.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("TransformationName"), srchResults.TransformationName);
        }
        set
        {
            SetValue("TransformationName", value);
            srchResults.TransformationName = value;
        }
    }


    /// <summary>
    /// Gets or sets the name of the query string.
    /// </summary>
    public string QueryStringKey
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("QueryStringKey"), srchResults.QueryStringKey);
        }
        set
        {
            SetValue("QueryStringKey", value);
            srchResults.QueryStringKey = value;
        }
    }


    /// <summary>
    /// Gets or sets the number of the search results on each sigle page.
    /// </summary>
    public int PageSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PageSize"), srchResults.PagerControl.PageSize);
        }
        set
        {
            SetValue("PageSize", value);
            srchResults.PagerControl.PageSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the position of the pager.
    /// </summary>
    public PagingPlaceTypeEnum PagerPosition
    {
        get
        {
            return srchResults.PagerControl.GetPagerPosition(DataHelper.GetNotEmpty(GetValue("PagerPosition"), srchResults.PagerControl.PagerPosition.ToString()));
        }
        set
        {
            SetValue("PagerPosition", value);
            srchResults.PagerControl.PagerPosition = value;
        }
    }


    /// <summary>
    /// Gets or sets the paging mode.
    /// </summary>
    public PagingModeTypeEnum PagingMode
    {
        get
        {
            return srchResults.PagerControl.GetPagingMode(DataHelper.GetNotEmpty(GetValue("PagingMode"), srchResults.PagerControl.PagingMode.ToString()));
        }
        set
        {
            SetValue("PagingMode", value);
            srchResults.PagerControl.PagingMode = value;
        }
    }


    /// <summary>
    /// Gets or sets the navigation mode.
    /// </summary>
    public BackNextLocationTypeEnum BackNextLocation
    {
        get
        {
            return srchResults.PagerControl.GetBackNextLocation(DataHelper.GetNotEmpty(GetValue("BackNextLocation"), srchResults.PagerControl.BackNextLocation.ToString()));
        }
        set
        {
            SetValue("BackNextLocation", value.ToString());
            srchResults.PagerControl.BackNextLocation = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether  first and last page is shown if paging is allowed.
    /// </summary>
    public bool ShowFirstLast
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowFirstLast"), srchResults.PagerControl.ShowFirstLast);
        }
        set
        {
            SetValue("ShowFirstLast", value);
            srchResults.PagerControl.ShowFirstLast = value;
        }
    }


    /// <summary>
    /// Gets or sets the html before pager.
    /// </summary>
    public string PagerHTMLBefore
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagerHTMLBefore"), srchResults.PagerControl.PagerHTMLBefore);
        }
        set
        {
            SetValue("PagerHTMLBefore", value);
            srchResults.PagerControl.PagerHTMLBefore = value;
        }
    }


    /// <summary>
    /// Gets or sets the html after pager.
    /// </summary>
    public string PagerHTMLAfter
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagerHTMLAfter"), srchResults.PagerControl.PagerHTMLAfter);
        }
        set
        {
            SetValue("PagerHTMLAfter", value);
            srchResults.PagerControl.PagerHTMLAfter = value;
        }
    }


    /// <summary>
    /// Gets or sets the results position.
    /// </summary>
    public ResultsLocationTypeEnum ResultsPosition
    {
        get
        {
            return srchResults.PagerControl.GetResultPosition(ValidationHelper.GetString(GetValue("ResultsPosition"), ""));
        }
        set
        {
            SetValue("ResultsPosition", value);
            srchResults.PagerControl.ResultsLocation = value;
        }
    }


    /// <summary>
    /// Gets or sets the page numbers separator.
    /// </summary>
    public string PageNumbersSeparator
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PageNumbersSeparator"), srchResults.PagerControl.PageNumbersSeparator);
        }
        set
        {
            SetValue("PageNumbersSeparator", value);
            srchResults.PagerControl.PageNumbersSeparator = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether paging is enabled.
    /// </summary>
    public bool EnablePaging
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("EnablePaging"), srchResults.EnablePaging);
        }
        set
        {
            SetValue("EnablePaging", value);
            srchResults.EnablePaging = value;
        }
    }


    /// <summary>
    /// Filter name.
    /// </summary>
    public string FilterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterName"), srchResults.FilterName);
        }
        set
        {
            SetValue("FilterName", value);
            srchResults.FilterName = value;
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
            srchResults.StopProcessing = true;
        }
        else
        {
            // Setup the control
            srchResults.CheckPermissions = CheckPermissions;
            srchResults.ClassNames = ClassNames;
            srchResults.CMSSearchDialogID = CMSSearchDialogID;
            srchResults.CombineWithDefaultCulture = CombineWithDefaultCulture;
            srchResults.CultureCode = CultureCode;
            srchResults.Path = Path;

            srchResults.SearchMode = SearchMode;

            srchResults.SelectOnlyPublished = SelectOnlyPublished;
            srchResults.SiteName = SiteName;
            // Set 'no results' text
            srchResults.NoResultsLabel.Text = NoResultText;

            srchResults.TransformationName = TransformationName;

            // This Query string key isn't property of DataPager ! This property set DataPager QueryStrinKey with search query parameters
            srchResults.EnablePaging = EnablePaging;
            srchResults.QueryStringKey = QueryStringKey;
            srchResults.PagerControl.PageSize = PageSize;
            srchResults.PagerControl.PagerPosition = PagerPosition;
            srchResults.PagerControl.PagingMode = PagingMode;
            srchResults.PagerControl.BackNextLocation = BackNextLocation;
            srchResults.PagerControl.ShowFirstLast = ShowFirstLast;
            srchResults.PagerControl.PagerHTMLBefore = PagerHTMLBefore;
            srchResults.PagerControl.PagerHTMLAfter = PagerHTMLAfter;
            srchResults.PagerControl.ResultsLocation = ResultsPosition;
            srchResults.PagerControl.PageNumbersSeparator = PageNumbersSeparator;

            srchResults.WhereCondition = WhereCondition;
            srchResults.OrderBy = OrderBy;
            srchResults.FilterName = FilterName;

            // Event
            srchResults.OnSearchCompleted += srchResults_OnSearchCompleted;
            Visible = false;
        }
    }


    protected void srchResults_OnSearchCompleted(bool noResults, bool renderIt)
    {
        if (renderIt)
        {
            Visible = true;
        }
        else
        {
            if (srchResults.NoResultsLabel != null)
            {
                if (!string.IsNullOrEmpty(srchResults.NoResultsLabel.Text))
                {
                    Visible = true && noResults;
                }
            }
        }
    }
}