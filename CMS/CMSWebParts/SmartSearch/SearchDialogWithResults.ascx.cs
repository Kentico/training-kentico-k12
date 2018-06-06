using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.Search;

public partial class CMSWebParts_SmartSearch_SearchDialogWithResults : CMSAbstractWebPart, ISearchFilterable, IUniPageable
{
    #region "Variables"

    private string mFilterSuffix = "diarespsfx";

    #endregion


    #region "Dialog properties"

    /// <summary>
    /// Gets or sets the label search for text.
    /// </summary>
    public string SearchForLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchForLabel"), srchDialog.SearchForLabel);
        }
        set
        {
            SetValue("SearchForLabel", value);
            srchDialog.SearchForLabel = value;
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
    /// Gets or sets the search button text.
    /// </summary>
    public string SearchButton
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchButton"), srchDialog.SearchButton);
        }
        set
        {
            SetValue("SearchButton", value);
            srchDialog.SearchButton = value;
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
    /// Gets or sets the search mode label text.
    /// </summary>
    public string SearchModeLabel
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchModeLabel"), srchDialog.SearchModeLabel);
        }
        set
        {
            SetValue("SearchModeLabel", value);
            srchDialog.SearchModeLabel = value;
        }
    }

    #endregion


    #region "Result properties"

    /// <summary>
    /// Gets or sets sorting of search.
    /// </summary>
    public string SearchSort
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchSort"), srchResults.SearchSort);
        }
        set
        {
            SetValue("SearchSort", value);
            srchResults.SearchSort = value;
        }
    }


    /// <summary>
    /// Gets or sets sorting of search.
    /// </summary>
    public string SearchCondition
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchCondition"), srchResults.SearchCondition);
        }
        set
        {
            SetValue("SearchCondition", value);
            srchResults.SearchCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets indexes.
    /// </summary>
    public string Indexes
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Indexes"), srchResults.Indexes);
        }
        set
        {
            SetValue("Indexes", value);
            srchResults.Indexes = value;
        }
    }


    /// <summary>
    /// Gets or sets path.
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
    /// Gets or sets document types.
    /// </summary>
    public string DocumentTypes
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DocumentTypes"), srchResults.DocumentTypes);
        }
        set
        {
            SetValue("DocumentTypes", value);
            srchResults.DocumentTypes = value;
        }
    }


    /// <summary>
    /// Gets or sets check permissions.
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
            srchResults.CheckPermissions = false;
        }
    }


    /// <summary>
    /// Gets or sets search option.
    /// </summary>
    public SearchOptionsEnum SearchOptions
    {
        get
        {
            string searchOptions = ValidationHelper.GetString(GetValue("SearchOptions"), "");
            if (String.IsNullOrEmpty(searchOptions))
            {
                return srchResults.SearchOptions;
            }
            else
            {
                return searchOptions.ToEnum<SearchOptionsEnum>();
            }
        }
        set
        {
            SetValue("SearchOptions", value.ToStringRepresentation());
            srchResults.SearchOptions = value;
        }
    }


    /// <summary>
    /// Gets or sets transformation name.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TransformationName"), srchResults.TransformationName);
        }
        set
        {
            SetValue("TransformationName", value);
            srchResults.TransformationName = value;
        }
    }


    /// <summary>
    /// Gets or sets culture code.
    /// </summary>
    public string CultureCode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CultureCode"), srchResults.CultureCode);
        }
        set
        {
            SetValue("CultureCode", value);
            srchResults.CultureCode = value;
        }
    }


    /// <summary>
    /// Gets or sets combine with default culture.
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
    /// Gets or sets search in attachments.
    /// </summary>
    public bool SearchInAttachments
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SearchInAttachments"), srchResults.SearchInAttachments);
        }
        set
        {
            SetValue("SearchInAttachments", value);
            srchResults.SearchInAttachments = value;
        }
    }


    /// <summary>
    /// Gets or sets attachments where.
    /// </summary>
    public string AttachmentsWhere
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AttachmentsWhere"), srchResults.AttachmentsWhere);
        }
        set
        {
            SetValue("AttachmentsWhere", value);
            srchResults.AttachmentsWhere = value;
        }
    }


    /// <summary>
    /// Gets or sets attachment order by.
    /// </summary>
    public string AttachmentsOrderBy
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AttachmentsOrderBy"), srchResults.AttachmentsOrderBy);
        }
        set
        {
            SetValue("AttachmentsOrderBy", value);
            srchResults.AttachmentsOrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets results text.
    /// </summary>
    public string NoResultsText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NoResultsText"), srchResults.NoResultsText);
        }
        set
        {
            SetValue("NoResultsText", value);
            srchResults.NoResultsText = value;
        }
    }


    /// <summary>
    /// Indicates if search errors should be showed if occurs.
    /// </summary>
    public bool ShowParsingErrors
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowParsingErrors"), srchResults.ShowParsingErrors);
        }
        set
        {
            SetValue("ShowParsingErrors", value);
            srchResults.ShowParsingErrors = value;
        }
    }


    /// <summary>
    /// Indicates if search errors should be showed if occurs.
    /// </summary>
    public bool SearchTextRequired
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SearchTextRequired"), srchResults.SearchTextRequired);
        }
        set
        {
            SetValue("SearchTextRequired", value);
            srchResults.SearchTextRequired = value;
        }
    }


    /// <summary>
    /// Sets the text that is displayed when search text is empty and required.
    /// </summary>
    public string SearchTextValidationFailedText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchTextValidationFailedText"), srchResults.SearchTextValidationFailedText);
        }
        set
        {
            SetValue("SearchTextValidationFailedText", value);
            srchResults.SearchTextValidationFailedText = value;
        }
    }


    /// <summary>
    /// CSS class that will be assigned to the web part when search text validation fails and user is not redirected to results page.
    /// </summary>
    public string SearchTextValidationFailedCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchTextValidationFailedCssClass"), "");
        }
        set
        {
            SetValue("SearchTextValidationFailedCssClass", value);
        }
    }


    /// <summary>
    /// Indicates if performs the search only when the content part of the search expression is present.
    /// </summary>
    public bool BlockFieldOnlySearch
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("BlockFieldOnlySearch"), srchResults.BlockFieldOnlySearch);
        }
        set
        {
            SetValue("BlockFieldOnlySearch", value);
            srchResults.BlockFieldOnlySearch = value;
        }
    }


    /// <summary>
    /// Gets or sets the maximal displayed results.
    /// </summary>
    public int MaxResults
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxResults"), srchResults.MaxResults);
        }
        set
        {
            SetValue("MaxResults", value);
            srchResults.MaxResults = value;
        }
    }


    /// <summary>
    /// Gets or sets if filter is applied immediately.
    /// </summary>
    public bool SearchOnEachPageLoad
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SearchOnEachPageLoad"), srchResults.SearchOnEachPageLoad);
        }
        set
        {
            SetValue("SearchOnEachPageLoad", value);
            srchResults.SearchOnEachPageLoad = value;
        }
    }


    /// <summary>
    /// The text to show when the control has no value.
    /// </summary>
    public string WatermarkText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WatermarkText"), srchDialog.WatermarkText);
        }
        set
        {
            SetValue("WatermarkText", value);
            srchDialog.WatermarkText = value;
        }
    }


    /// <summary>
    /// The CSS class to apply to the TextBox when it has no value (e.g. the watermark text is shown).
    /// </summary>
    public string WatermarkCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WatermarkCssClass"), srchDialog.WatermarkCssClass);
        }
        set
        {
            SetValue("WatermarkCssClass", value);
            srchDialog.WatermarkCssClass = value;
        }
    }


    /// <summary>
    /// If true, fuzzy search (typo tolerant search) is performed.
    /// </summary>
    public bool DoFuzzySearch
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DoFuzzySearch"), srchResults.DoFuzzySearch);
        }
        set
        {
            SetValue("DoFuzzySearch", value);
            srchResults.DoFuzzySearch = value;
        }
    }

    #endregion


    #region "UniPager properties"

    /// <summary>
    /// Gets or sets the value that indicates whether scroll position should be cleared after post back paging
    /// </summary>
    public bool ResetScrollPositionOnPostBack
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ResetScrollPositionOnPostBack"), srchResults.ResetScrollPositionOnPostBack);
        }
        set
        {
            SetValue("ResetScrollPositionOnPostBack", value);
            srchResults.ResetScrollPositionOnPostBack = value;
        }
    }


    /// <summary>
    /// Gets or sets page size.
    /// </summary>
    public int PageSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PageSize"), srchResults.PageSize);
        }
        set
        {
            SetValue("PageSize", value);
            srchResults.PageSize = value;
        }
    }


    /// <summary>
    /// Gets or sets search option.
    /// </summary>
    public string PagingMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagingMode"), srchResults.PagingMode.ToString());
        }
        set
        {
            SetValue("PagingMode", value);

            srchResults.PagingMode = UniPagerMode.Querystring;
            if (value == "postback")
            {
                srchResults.PagingMode = UniPagerMode.PostBack;
            }
        }
    }


    /// <summary>
    /// Gets or sets query string key.
    /// </summary>
    public string QueryStringKey
    {
        get
        {
            return ValidationHelper.GetString(GetValue("QueryStringKey"), srchResults.QueryStringKey);
        }
        set
        {
            SetValue("QueryStringKey", value);
            srchResults.QueryStringKey = value;
        }
    }


    /// <summary>
    /// Gets or sets group size.
    /// </summary>
    public int GroupSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("GroupSize"), srchResults.PageSize);
        }
        set
        {
            SetValue("GroupSize", value);
            srchResults.PageSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public bool DisplayFirstLastAutomatically
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFirstLastAutomatically"), srchResults.DisplayFirstLastAutomatically);
        }
        set
        {
            SetValue("DisplayFirstLastAutomatically", value);
            srchResults.DisplayFirstLastAutomatically = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public bool DisplayPreviousNextAutomatically
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayPreviousNextAutomatically"), srchResults.DisplayPreviousNextAutomatically);
        }
        set
        {
            SetValue("DisplayPreviousNextAutomatically", value);
            srchResults.DisplayPreviousNextAutomatically = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether pager should be hidden for single page.
    /// </summary>
    public bool HidePagerForSinglePage
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HidePagerForSinglePage"), srchResults.HidePagerForSinglePage);
        }
        set
        {
            SetValue("HidePagerForSinglePage", value);
            srchResults.HidePagerForSinglePage = value;
        }
    }


    /// <summary>
    /// Gets or sets the max. pages.
    /// </summary>
    public int MaxPages
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaxPages"), srchResults.MaxPages);
        }
        set
        {
            SetValue("MaxPages", value);
            srchResults.MaxPages = value;
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
            return ValidationHelper.GetString(GetValue("Pages"), srchResults.PagesTemplateName);
        }
        set
        {
            SetValue("Pages", value);
            srchResults.PagesTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the current page template.
    /// </summary>
    public string CurrentPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CurrentPage"), srchResults.CurrentPageTemplateName);
        }
        set
        {
            SetValue("CurrentPage", value);
            srchResults.CurrentPageTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the separator template.
    /// </summary>
    public string SeparatorTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PageSeparator"), srchResults.SeparatorTemplateName);
        }
        set
        {
            SetValue("PageSeparator", value);
            srchResults.SeparatorTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the first page template.
    /// </summary>
    public string FirstPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FirstPage"), srchResults.FirstPageTemplateName);
        }
        set
        {
            SetValue("FirstPage", value);
            srchResults.FirstPageTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the last page template.
    /// </summary>
    public string LastPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LastPage"), srchResults.LastPageTemplateName);
        }
        set
        {
            SetValue("LastPage", value);
            srchResults.LastPageTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the previous page template.
    /// </summary>
    public string PreviousPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PreviousPage"), srchResults.PreviousPageTemplateName);
        }
        set
        {
            SetValue("PreviousPage", value);
            srchResults.PreviousPageTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the next page template.
    /// </summary>
    public string NextPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NextPage"), srchResults.NextPageTemplateName);
        }
        set
        {
            SetValue("NextPage", value);
            srchResults.NextPageTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the previous group template.
    /// </summary>
    public string PreviousGroupTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PreviousGroup"), srchResults.PreviousGroupTemplateName);
        }
        set
        {
            SetValue("PreviousGroup", value);
            srchResults.NextPageTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the next group template.
    /// </summary>
    public string NextGroupTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NextGroup"), srchResults.NextGroupTemplateName);
        }
        set
        {
            SetValue("NextGroup", value);
            srchResults.NextGroupTemplateName = value;
        }
    }


    /// <summary>
    /// Gets or sets the layout template.
    /// </summary>
    public string LayoutTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagerLayout"), srchResults.LayoutTemplateName);
        }
        set
        {
            SetValue("PagerLayout", value);
            srchResults.LayoutTemplateName = value;
        }
    }

    #endregion


    #region "Methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!StopProcessing)
        {
            string webpartID = ValidationHelper.GetString(GetValue("WebPartControlID"), ID);
            
            // Set filter for results object
            CMSControlsHelper.SetFilter(webpartID, this);

            srchDialog.ResultWebpartID = webpartID + mFilterSuffix;
            srchResults.FilterID = webpartID + mFilterSuffix;
            srchResults.LoadData();
        }
    }


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
            srchDialog.StopProcessing = true;
            srchResults.StopProcessing = true;
        }
        else
        {
            // Set settings to search dialog
            srchDialog.SearchForLabel = SearchForLabel;
            srchDialog.SearchModeLabel = SearchModeLabel;
            srchDialog.SearchButton = SearchButton;
            srchDialog.WatermarkCssClass = WatermarkCssClass;
            srchDialog.WatermarkText = WatermarkText;

            srchDialog.SearchMode = SearchMode;
            srchDialog.ShowSearchMode = ShowSearchMode;
            
            // Get unipage mode
            UniPagerMode mode = UniPagerMode.Querystring;
            if (PagingMode == "postback")
            {
                mode = UniPagerMode.PostBack;
            }

            // Search results properties
            srchResults.SearchSort = SearchSort;
            srchResults.Indexes = Indexes;
            srchResults.Path = Path;
            srchResults.DocumentTypes = DocumentTypes;
            srchResults.CheckPermissions = CheckPermissions;
            srchResults.SearchOptions = SearchOptions;
            srchResults.TransformationName = TransformationName;
            srchResults.CultureCode = CultureCode;
            srchResults.CombineWithDefaultCulture = CombineWithDefaultCulture;
            srchResults.SearchInAttachments = SearchInAttachments;
            srchResults.AttachmentsOrderBy = AttachmentsOrderBy;
            srchResults.AttachmentsWhere = AttachmentsWhere;
            srchResults.NoResultsText = NoResultsText;
            srchResults.SearchCondition = SearchCondition;
            srchResults.ShowParsingErrors = ShowParsingErrors;
            srchResults.BlockFieldOnlySearch = BlockFieldOnlySearch;
            srchResults.SearchOnEachPageLoad = SearchOnEachPageLoad;
            srchResults.SearchTextRequired = SearchTextRequired;
            srchResults.SearchTextValidationFailedText = SearchTextValidationFailedText;
            srchResults.SearchTextValidationFailedCssClass = SearchTextValidationFailedCssClass;
            srchResults.DoFuzzySearch = DoFuzzySearch;
            srchResults.LoadData();

            // UniPager properties
            srchResults.PageSize = PageSize;
            srchResults.GroupSize = GroupSize;
            srchResults.QueryStringKey = QueryStringKey;
            srchResults.DisplayFirstLastAutomatically = DisplayFirstLastAutomatically;
            srchResults.DisplayPreviousNextAutomatically = DisplayPreviousNextAutomatically;
            srchResults.HidePagerForSinglePage = HidePagerForSinglePage;
            srchResults.PagingMode = mode;
            srchResults.MaxPages = MaxPages;
            srchResults.MaxResults = MaxResults;
            srchResults.ResetScrollPositionOnPostBack = ResetScrollPositionOnPostBack;

            // Unipager template properties
            srchResults.PagesTemplateName = PagesTemplate;
            srchResults.CurrentPageTemplateName = CurrentPageTemplate;
            srchResults.SeparatorTemplateName = SeparatorTemplate;
            srchResults.FirstPageTemplateName = FirstPageTemplate;
            srchResults.LastPageTemplateName = LastPageTemplate;
            srchResults.PreviousPageTemplateName = PreviousPageTemplate;
            srchResults.NextPageTemplateName = NextPageTemplate;
            srchResults.PreviousGroupTemplateName = PreviousGroupTemplate;
            srchResults.NextGroupTemplateName = NextGroupTemplate;
            srchResults.LayoutTemplateName = LayoutTemplate;

            // Handle external pager
            srchResults.OnPageBinding += new EventHandler<EventArgs>(srchResults_OnPageBinding);
        }
    }


    /// <summary>
    /// Ensure external external pager value
    /// </summary>
    void srchResults_OnPageBinding(object sender, EventArgs e)
    {
        this.PagerForceNumberOfResults = srchResults.PagerForceNumberOfResults;
        if (OnPageBinding != null)
        {
            OnPageBinding(sender, e);
        }
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        SetupControl();
        base.ReloadData();
    }

    #endregion


    #region ISearchFilterable Members

    /// <summary>
    /// Gets or sets filter condition - not implemented.
    /// </summary>
    public string FilterSearchCondition
    {
        get
        {
            return srchDialog.FilterSearchCondition;
        }
        set
        {
            srchDialog.FilterSearchCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets filter search sort - not implemented.
    /// </summary>
    public string FilterSearchSort
    {
        get
        {
            return srchDialog.FilterSearchSort;
        }
        set
        {
            srchDialog.FilterSearchSort = value;
        }
    }


    /// <summary>
    /// Applies filter.
    /// </summary>
    /// <param name="searchCondition">Search condition</param>
    /// <param name="filterPostback">If true filter caused the postback which means that filter condition has been changed.</param>
    public void ApplyFilter(string searchCondition, string searchSort, bool filterPostback)
    {
        srchDialog.ApplyFilter(searchCondition, searchSort, filterPostback);
    }


    /// <summary>
    /// Adds filter option to url.
    /// </summary>
    /// <param name="searchWebpartID">Web part id</param>
    /// <param name="options">Options</param>
    public void AddFilterOptionsToUrl(string searchWebpartID, string options)
    {
        srchDialog.AddFilterOptionsToUrl(searchWebpartID, options);
    }

    #endregion


    #region "IUniPageable Members"

    /// <summary>
    /// Pager data item object.
    /// </summary>
    public object PagerDataItem
    {
        get;
        set;
    }


    /// <summary>
    /// Pager control.
    /// </summary>
    public UniPager UniPagerControl
    {
        get
        {
            return srchResults.UniPagerControl;
        }
        set
        {
            srchResults.UniPagerControl = value;
        }
    }


    /// <summary>
    /// Gets or sets the number of result. Enables proceed "fake" datasets, where number 
    /// of results in the dataset is not correspondent to the real number of results
    /// This property must be equal -1 if should be disabled
    /// </summary>
    public int PagerForceNumberOfResults
    {
        get
        {
            return srchResults.PagerForceNumberOfResults;
        }
        set
        {
            srchResults.PagerForceNumberOfResults = value;
        }
    }


    /// <summary>
    /// Occurs when the control bind data.
    /// </summary>
    public event EventHandler<EventArgs> OnPageBinding;

    // Do not display warning for not-used event handler required by interface
#pragma warning disable 67

    /// <summary>
    /// Occurs when the pager change the page and current mode is postback => reload data
    /// </summary>
    public event EventHandler<EventArgs> OnPageChanged;

#pragma warning restore 67

    /// <summary>
    /// Evokes control databind.
    /// </summary>
    public void ReBind()
    {
        srchResults.ReBind();
    }

    #endregion
}