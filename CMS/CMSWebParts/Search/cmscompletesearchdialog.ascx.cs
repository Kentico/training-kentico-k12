using System.Web.UI;

using CMS.Core;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Search;
using CMS.WebAnalytics;

public partial class CMSWebParts_Search_cmscompletesearchdialog : CMSAbstractWebPart
{
    #region Private fields

    private IPagesActivityLogger mPagesActivityLogger = Service.Resolve<IPagesActivityLogger>();

    #endregion


    #region "Search dialog properties"

    /// <summary>
    /// Gets or sets the label search for text.
    /// </summary>
    public string SearchForLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchForLabel"), ResHelper.LocalizeString("{$CMSSearchDialog.SearchFor$}"));
        }
        set
        {
            SetValue("SearchForLabel", value);
            srchDialog.SearchForLabel.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether search mode settings should be displayed.
    /// </summary>
    public bool ShowSearchMode
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowSearchMode"), srchDialog.ShowSearchMode);
        }
        set
        {
            SetValue("ShowSearchMode", value);
            srchDialog.ShowSearchMode = value;
        }
    }


    /// <summary>
    /// Gets or sets search button text.
    /// </summary>
    public string SearchButtonText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchButtonText"), ResHelper.LocalizeString("{$CMSSearchDialog.Go$}"));
        }
        set
        {
            SetValue("SearchButtonText", value);
            srchDialog.SearchButton.Text = value;
        }
    }


    /// <summary>
    ///  Gets or sets the search mode.
    /// </summary>
    public SearchModeEnum SearchMode
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchMode"), srchDialog.SearchMode.ToStringRepresentation()).ToEnum<SearchModeEnum>();
        }
        set
        {
            SetValue("SearchMode", value.ToStringRepresentation());
            srchDialog.SearchMode = value;
        }
    }


    /// <summary>
    /// Gets or sets the search mode label.
    /// </summary>
    public string SearchModeLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchModeLabel"), ResHelper.LocalizeString("{$CMSSearchDialog.SearchMode$}"));
        }
        set
        {
            SetValue("SearchModeLabel", value);
            srchDialog.SearchModeLabel.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether search scope drop down is diplayed.
    /// </summary>
    public bool ShowSearchScope
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowSearchScope"), srchDialog.ShowSearchScope);
        }
        set
        {
            SetValue("ShowSearchScope", value);
            srchDialog.ShowSearchScope = value;
        }
    }


    /// <summary>
    /// Gets or sets the search scope.
    /// </summary>
    public SearchScopeEnum SearchScope
    {
        get
        {
            return CMSSearchDialog.GetSearchScope(DataHelper.GetNotEmpty(GetValue("SearchScope"), srchDialog.SearchScope.ToString()));
        }
        set
        {
            SetValue("SearchScope", value.ToString());
            srchDialog.SearchScope = value;
        }
    }


    /// <summary>
    /// Gets or sets the search scope label.
    /// </summary>
    public string SearchScopeLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchScopeLabel"), ResHelper.LocalizeString("{$CMSSearchDialog.SearchScope$}"));
        }
        set
        {
            SetValue("SearchScopeLabel", value);
            srchDialog.SearchScopeLabel.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the Skin ID.
    /// </summary>
    public override string SkinID
    {
        get
        {
            return base.SkinID;
        }
        set
        {
            base.SkinID = value;
            srchDialog.SkinID = value;
        }
    }

    #endregion


    #region "Document properties"

    /// <summary>
    /// Gets or sets the value that indidcates whether permissions are checked.
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
    /// Gets or sets the class names.
    /// </summary>
    public string ClassNames
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("Classnames"), srchResults.ClassNames), srchResults.ClassNames);
        }
        set
        {
            SetValue("ClassNames", value);
            srchResults.ClassNames = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indidcates whether current culture will be combined with default culture.
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
    /// Gets or sets the culture code.
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
    /// Gets or sets the nodes path.
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
    /// Gets or sets the value that indicates whether only published nodes will be selected.
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

    #endregion


    #region "Pager properties"

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
    /// Gets or sets the pager position.
    /// </summary>
    public PagingPlaceTypeEnum PagerPosition
    {
        get
        {
            return srchResults.PagerControl.GetPagerPosition(DataHelper.GetNotEmpty(GetValue("PagerPosition"), srchResults.PagerControl.PagerPosition.ToString()));
        }
        set
        {
            SetValue("PagerPosition", value.ToString());
            srchResults.PagerControl.PagerPosition = value;
        }
    }


    /// <summary>
    /// Gets or sets the page size.
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
    /// Gets or sets the pager query string key.
    /// </summary>
    public string QueryStringKey
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("QueryStringKey"), srchResults.PagerControl.QueryStringKey), srchResults.PagerControl.QueryStringKey);
        }
        set
        {
            SetValue("QueryStringKey", value);
            srchResults.PagerControl.QueryStringKey = value;
        }
    }


    /// <summary>
    /// Gets or sets the paging mode (QueryString, PostBack).
    /// </summary>
    public PagingModeTypeEnum PagingMode
    {
        get
        {
            return srchResults.PagerControl.GetPagingMode(DataHelper.GetNotEmpty(GetValue("PagingMode"), srchResults.PagerControl.PagingMode.ToString()));
        }
        set
        {
            SetValue("PagingMode", value.ToString());
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
    /// Gets or sets the value that indicates whether first and last links will be displayed.
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
    /// Gets or sets the html which will be displayed before pager.
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
    /// Gets or sets the html which will be displayed after pager.
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

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the name of the transforamtion which is used for displaying the results.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("TransformationName"), srchResults.TransformationName), srchResults.TransformationName);
        }
        set
        {
            SetValue("TransformationName", value);
            srchResults.TransformationName = value;
        }
    }


    /// <summary>
    /// Gets or sets the text which will be displayed if no result were found.
    /// </summary>
    public string NoResultsLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("NoResults"), ResHelper.LocalizeString("{$CMSSearchResults.NoDocumentFound$}"));
        }
        set
        {
            SetValue("NoResults", value);
            srchResults.NoResultsLabel.Text = value;
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
            srchDialog.StopProcessing = true;
            srchResults.StopProcessing = true;
        }
        else
        {
            //Search dialog
            srchDialog.ID = ID + "_CMSSearchDialog";

            srchDialog.SearchForLabel.Text = SearchForLabel;
            srchDialog.SearchButton.Text = SearchButtonText;

            srchDialog.ShowSearchMode = ShowSearchMode;
            srchDialog.SearchMode = SearchMode;
            srchDialog.SearchModeLabel.Text = SearchModeLabel;

            srchDialog.ShowSearchScope = ShowSearchScope;
            srchDialog.SearchScope = SearchScope;
            srchDialog.SearchScopeLabel.Text = SearchScopeLabel;
            srchDialog.DoSearch += new CMSSearchDialog.DoSearchEventHandler(srchDialog_DoSearch);

            // Document properties
            srchResults.CheckPermissions = CheckPermissions;
            srchResults.ClassNames = ClassNames;
            srchResults.CombineWithDefaultCulture = CombineWithDefaultCulture;
            srchResults.CultureCode = CultureCode;
            srchResults.Path = Path;
            srchResults.SelectOnlyPublished = SelectOnlyPublished;
            srchResults.SiteName = SiteName;

            srchResults.CMSSearchDialogID = ID + "_CMSSearchDialog";

            srchResults.SearchMode = SearchMode;
            srchResults.TransformationName = TransformationName;
            srchResults.NoResultsLabel.Text = NoResultsLabel;

            // Pager
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

            // Set SkinID property
            if (!StandAlone && (PageCycle < PageCycleEnum.Initialized) && (ValidationHelper.GetString(Page.StyleSheetTheme, "") == ""))
            {
                srchDialog.SkinID = SkinID;
                srchResults.SkinID = SkinID;
            }
        }
    }


    protected void srchDialog_DoSearch()
    {
        // Log "internal search" activity
        mPagesActivityLogger.LogInternalSearch(srchDialog.SearchForTextBox.Text, DocumentContext.CurrentDocument);
    }


    /// <summary>
    /// Applies given stylesheet skin.
    /// </summary>
    public override void ApplyStyleSheetSkin(Page page)
    {
        string skinId = SkinID;
        srchDialog.SkinID = skinId;
        srchResults.SkinID = skinId;

        base.ApplyStyleSheetSkin(page);
    }
}