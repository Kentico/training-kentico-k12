using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.IO;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.Search;
using CMS.SiteProvider;
using CMS.WebAnalytics;

/// <summary>
/// Displays a simple text box that allows users to enter a search expression and redirects to a page with the search results.
/// The search indexes that should be used are specified by the web part displaying the results.
/// </summary>
public partial class CMSWebParts_SmartSearch_SearchBox : CMSAbstractWebPart, ICallbackEventHandler
{
    #region "Variables"

    /// <summary>
    /// Result page URL
    /// </summary>
    protected string mResultPageUrl = RequestContext.CurrentURL;

    /// <summary>
    /// Stores predictive search results to transfer the information between the two ICallbackEventHanlder methods.
    /// </summary>
    private string mPredictiveSearchResult;


    /// <summary>
    /// Service to log pages related activities.
    /// </summary>
    private IPagesActivityLogger mPagesActivityLogger = Service.Resolve<IPagesActivityLogger>();

    #endregion


    #region "Public properties"

    #region "Search box properties"

    /// <summary>
    /// The text to show when the control has no value.
    /// </summary>
    public string WatermarkText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WatermarkText"), txtWord.WatermarkText);
        }
        set
        {
            SetValue("WatermarkText", value);
            txtWord.WatermarkText = value;
        }
    }


    /// <summary>
    /// The CSS class to apply to the TextBox when it has no value (e.g. the watermark text is shown).
    /// </summary>
    public string WatermarkCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WatermarkCssClass"), txtWord.WatermarkCssClass);
        }
        set
        {
            SetValue("WatermarkCssClass", value);
            txtWord.WatermarkCssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether image button is displayed instead of regular button.
    /// </summary>
    public bool ShowImageButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowImageButton"), false);
        }
        set
        {
            SetValue("ShowImageButton", value);
            btnSearch.Visible = !value;
            btnImageButton.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets an Image button URL.
    /// </summary>
    public string ImageUrl
    {
        get
        {
            return ResolveUrl(ValidationHelper.GetString(GetValue("ImageUrl"), btnImageButton.ImageUrl));
        }
        set
        {
            SetValue("ImageUrl", value);
            btnImageButton.ImageUrl = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether search label is displayed.
    /// </summary>
    public bool ShowSearchLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowSearchLabel"), lblSearch.Visible);
        }
        set
        {
            SetValue("ShowSearchLabel", value);
            lblSearch.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the search label text.
    /// </summary>
    public string SearchLabelText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchLabelText"), GetString("srch.searchbox.searchfor"));
        }
        set
        {
            SetValue("SearchLabelText", value);
            lblSearch.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the search button text.
    /// </summary>
    public string SearchButtonText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchButtonText"), GetString("general.search"));
        }
        set
        {
            SetValue("SearchButtonText", value);
            btnSearch.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the search label Css class.
    /// </summary>
    public string SearchLabelCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchLabelCssClass"), "");
        }
        set
        {
            SetValue("SearchLabelCssClass", value);
            lblSearch.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets search text box CSS class.
    /// </summary>
    public string SearchTextboxCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchTextboxCssClass"), "");
        }
        set
        {
            SetValue("SearchTextboxCssClass", value);
            txtWord.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the search button CSS class.
    /// </summary>
    public string SearchButtonCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchButtonCssClass"), "");
        }
        set
        {
            SetValue("SearchButtonCssClass", value);
            btnSearch.CssClass = value;
            btnImageButton.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the search results page URL.
    /// </summary>
    public string SearchResultsPageUrl
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchResultsPageUrl"), mResultPageUrl);
        }
        set
        {
            SetValue("SearchResultsPageUrl", value);
            mResultPageUrl = value;
        }
    }


    /// <summary>
    ///  Gets or sets the Search mode.
    /// </summary>
    public SearchModeEnum SearchMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchMode"), "").ToEnum<SearchModeEnum>();
        }
        set
        {
            SetValue("SearchMode", value.ToStringRepresentation());
        }
    }


    /// <summary>
    /// If enabled, user is not redirected to search results page when search text is empty. When validation fails the same page is displayed.
    /// </summary>
    public bool SearchTextRequired
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SearchTextRequired"), true);
        }
        set
        {
            SetValue("SearchTextRequired", value);
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
    /// Sets the text that is displayed when  search text validations fails. This text is displayed as tooltip.
    /// </summary>
    public string SearchTextValidationFailedText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchTextValidationFailedText"), "");
        }
        set
        {
            SetValue("SearchTextValidationFailedText", value);
        }
    }

    #endregion

    #region "Predictive search properties"

    /// <summary>
    /// Enables predictive search functionality.
    /// </summary>
    public bool PredictiveSearchEnabled
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchEnabled"), false);
        }
        set
        {
            SetValue("PredictiveSearchEnabled", value);
        }
    }


    /// <summary>
    /// Sets the default search mode. The options are searching for results that contain all words in the entered expression, any of the words or only the exact phrase.
    /// </summary>
    public SearchModeEnum PredictiveSearchMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchMode"), "").ToEnum<SearchModeEnum>();
        }
        set
        {
            SetValue("PredictiveSearchModeSearchMode", value.ToString());
        }
    }


    /// <summary>
    /// Sets the culture code of the documents that should be included in searches. If empty, the current culture is searched.
    /// The ##ALL## macro can be used to search in documents of all cultures.
    /// </summary>
    public string PredictiveSearchCultureCode
    {
        get
        {
            string cultureCode = ValidationHelper.GetString(GetValue("PredictiveSearchCultureCode"), LocalizationContext.PreferredCultureCode);
            // Empty string
            return DataHelper.GetNotEmpty(cultureCode, LocalizationContext.PreferredCultureCode);
        }
        set
        {
            SetValue("PredictiveSearchCultureCode", value);
        }
    }


    /// <summary>
    /// If enabled, only search expressions that contain at least one standard content keyword will be allowed. With this property disabled,
    /// it is also possible to submit field search expressions composed of nothing but special query syntax, such as: DocumentNodeID:(int)17.
    /// </summary>
    public bool PredictiveSearchBlockFieldOnlySearch
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchBlockFieldOnlySearch"), true);
        }
        set
        {
            SetValue("PredictiveSearchBlockFieldOnlySearch", value);
        }
    }


    /// <summary>
    /// Allows the selection of one or more smart search indexes that should be used for searching.
    /// </summary>
    public string PredictiveSearchIndexes
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchIndexes"), null);
        }
        set
        {
            SetValue("PredictiveSearchIndexes", value);
        }
    }


    /// <summary>
    /// Can be used to limit the path that is searched by document searches.
    /// </summary>
    public string PredictiveSearchPath
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchPath"), null);
        }
        set
        {
            SetValue("PredictiveSearchPath", value);
        }
    }


    /// <summary>
    /// Allows document searches to be restricted to specific document types.
    /// Document types must be entered using their code names and separated by semicolons (;).
    /// If empty, all document types specified by the index are searched.
    /// </summary>
    public string PredictiveSearchDocumentTypes
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchDocumentTypes"), null);
        }
        set
        {
            SetValue("PredictiveSearchDocumentTypes", value);
        }
    }


    /// <summary>
    /// Sets the level of syntax that is allowed in search expressions:
    /// Basic - users are allowed to input special syntax, but cannot search specific fields(field:value).
    /// None - users can only enter text, everything is processed as a part of the search expression.
    /// Full - all search options can be used, including field searching.
    /// </summary>
    public SearchOptionsEnum PredictiveSearchOptions
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchOptions"), "").ToEnum<SearchOptionsEnum>();
        }
        set
        {
            SetValue("PredictiveSearchOptions", value.ToStringRepresentation());
        }
    }


    /// <summary>
    /// Indicates whether search results should be retrieved from documents in the default culture in addition to the culture that is currently selected.
    /// </summary>
    public bool PredictiveSearchCombineWithDefaultCulture
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchCombineWithDefaultCulture"), SiteInfoProvider.CombineWithDefaultCulture(SiteContext.CurrentSiteName));
        }
        set
        {
            SetValue("PredictiveSearchCombineWithDefaultCulture", value);
        }
    }


    /// <summary>
    /// Sets a search condition that is added to any conditions specified in the search expression.
    /// You can use special characters (+ -) and field conditions (e.g. +documentnodeid:(int)255).
    /// </summary>
    public string PredictiveSearchCondition
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchCondition"), null);
        }
        set
        {
            SetValue("PredictiveSearchCondition", value);
        }
    }


    /// <summary>
    /// Defines the order in which search results are displayed.
    /// You can specify one or more document fields (separated by commas) according to which the results will be sorted.
    /// The ##SCORE## macro can be used to order results by their score (relevance).
    /// The default order is ascending, you can change this using the DESC keyword (e.g. articleid DESC).
    /// </summary>
    public string PredictiveSearchSort
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchSort"), null);
        }
        set
        {
            SetValue("PredictiveSearchSort", value);
        }
    }


    /// <summary>
    /// Sets the maximum number of search results that can be displayed.
    /// </summary>
    public int PredictiveSearchMaxResults
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PredictiveSearchMaxResults"), 10);
        }
        set
        {
            SetValue("PredictiveSearchMaxResults", value);
        }
    }


    /// <summary>
    /// Indicates whether the web part should filter out documents for which the current user does not have the "read" permission from search results.
    /// </summary>
    public bool PredictiveSearchCheckPermissions
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchCheckPermissions"), false);
        }
        set
        {
            SetValue("PredictiveSearchCheckPermissions", value);
        }
    }


    /// <summary>
    /// Sets the transformation according to which the search results will be displayed.
    /// </summary>
    public string PredictiveSearchResultItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchResultItemTransformationName"), "CMS.Root.SmartSearchPredictiveResults");
        }
        set
        {
            SetValue("PredictiveSearchResultItemTransformationName", value);
        }
    }


    /// <summary>
    /// Amount of characters user has to type to trigger the predictive search.
    /// </summary>
    public int PredictiveSearchMinCharacters
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PredictiveSearchMinCharacters"), 1);
        }
        set
        {
            SetValue("PredictiveSearchMinCharacters", value);
        }
    }


    /// <summary>
    /// HTML content displayed when search produces no results.
    /// </summary>
    public string PredictiveSearchNoResultsContent
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchNoResultsContent"), "");
        }
        set
        {
            SetValue("PredictiveSearchNoResultsContent", value);
        }
    }


    /// <summary>
    /// HTML content placed after the last search result when max. results limit has been reached.
    /// Use {0} formatting expression as a placeholder for the result page link.
    /// </summary>
    public string PredictiveSearchMoreResultsContent
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PredictiveSearchMoreResultsContent"), "");
        }
        set
        {
            SetValue("PredictiveSearchMoreResultsContent", value);
        }
    }


    /// <summary>
    /// Indicates whether the search results should be groupped into categories.
    /// Category is an index to which the search result belongs. Index display name is used for displaying category.
    /// </summary>
    public bool PredictiveSearchDisplayCategories
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchDisplayCategories"), false);
        }
        set
        {
            SetValue("PredictiveSearchDisplayCategories", value);
        }
    }


    /// <summary>
    /// Assign a CSS class to the element containing the results.
    /// </summary>
    public string PredictiveSearchResultsCssClass
    {
        get
        {
            string css = ValidationHelper.GetString(GetValue("PredictiveSearchResultsCssClass"), "predictiveSearchResults");
            // Empty string
            return DataHelper.GetNotEmpty(css, "predictiveSearchResults");
        }
        set
        {
            SetValue("PredictiveSearchResultsCssClass", value);
        }
    }


    /// <summary>
    /// Specifies the name of the CSS class which will be applied to selected search result element.
    /// </summary>
    public string PredictiveSearchSelectedCssClass
    {
        get
        {
            string css = ValidationHelper.GetString(GetValue("PredictiveSearchSelectedCssClass"), "selectedResult");
            // Empty string
            return DataHelper.GetNotEmpty(css, "selectedResult");
        }
        set
        {
            SetValue("PredictiveSearchSelectedCssClass", value);
        }
    }


    /// <summary>
    /// If enabled, user can use mouse and arrow keys to select a search result.
    /// When search is triggered with a selected result, web part searches the result for "a href" element.
    /// If found user is redirected to result's href URL instead of search results page URL.
    /// </summary>
    public bool PredictiveSearchEnableSelection
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchEnableSelection"), true);
        }
        set
        {
            SetValue("PredictiveSearchEnableSelection", value);
        }
    }


    /// <summary>
    /// Enables logging of internal search contact activity for predictive search requests.
    /// </summary>
    public bool PredictiveSearchLogSearchActivity
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchLogSearchActivity"), true);
        }
        set
        {
            SetValue("PredictiveSearchLogSearchActivity", value);
        }
    }


    /// <summary>
    /// Enables logging of web analytics on-site search keywords for predictive search requests.
    /// </summary>
    public bool PredictiveSearchTrackSearchKeywords
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("PredictiveSearchTrackSearchKeywords"), true);
        }
        set
        {
            SetValue("PredictiveSearchTrackSearchKeywords", value);
        }
    }

    #endregion

    #endregion


    #region "Events"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Button search handler.
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchButtonClicked();
    }


    /// <summary>
    /// Image button search handler.
    /// </summary>
    protected void btnImageButton_Click(object sender, ImageClickEventArgs e)
    {
        SearchButtonClicked();
    }


    /// <summary>
    /// Excecutes actions after search button was clicked.
    /// </summary>
    protected void SearchButtonClicked()
    {
        if (!SearchTextRequired || !String.IsNullOrEmpty(txtWord.Text))
        {
            Search();
        }
        else
        {
            pnlSearch.AddCssClass(SearchTextValidationFailedCssClass);
            txtWord.ToolTip = SearchTextValidationFailedText;
        }
    }


    /// <summary>
    /// OnLoad event handler.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (PredictiveSearchEnabled)
        {
            ControlsHelper.RegisterPostbackControl(this);
        }
    }


    /// <summary>
    /// OnPreRender event handler.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (PredictiveSearchEnabled)
        {
            RegisterScrips();
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            return;
        }
        else
        {
            btnSearch.Visible = !ShowImageButton;
            btnImageButton.Visible = ShowImageButton;
            pnlSearch.DefaultButton = btnSearch.Visible ? btnSearch.ID : btnImageButton.ID;

            btnImageButton.ImageUrl = ImageUrl;
            btnImageButton.AlternateText = GetString("General.search");

            // Set label visibility according to WAI standards
            if (!ShowSearchLabel)
            {
                lblSearch.Style.Add("display", "none");
            }

            // Set text properties
            lblSearch.Text = SearchLabelText;

            btnSearch.Text = SearchButtonText;

            // Set class properties
            lblSearch.CssClass = SearchLabelCssClass;
            txtWord.CssClass = SearchTextboxCssClass;
            btnSearch.CssClass = SearchButtonCssClass;
            btnImageButton.CssClass = SearchButtonCssClass;

            // Watermark
            txtWord.WatermarkText = WatermarkText;
            txtWord.WatermarkCssClass = WatermarkCssClass;

            // Set result page
            mResultPageUrl = SearchResultsPageUrl;
        }
    }


    /// <summary>
    /// Runs the search.
    /// </summary>
    private void Search()
    {
        string url = SearchResultsPageUrl;

        if (url.StartsWithCSafe("~"))
        {
            url = ResolveUrl(url.Trim());
        }

        url = URLHelper.UpdateParameterInUrl(url, "searchtext", HttpUtility.UrlEncode(txtWord.Text));
        url = URLHelper.UpdateParameterInUrl(url, "searchmode", SearchMode.ToStringRepresentation());

        // Log "internal search" activity
        mPagesActivityLogger.LogInternalSearch(txtWord.Text, DocumentContext.CurrentDocument);

        // Try remove paging parameter
        url = URLHelper.RemoveParameterFromUrl(url, "page");

        URLHelper.Redirect(UrlResolver.ResolveUrl(url.Trim()));
    }


    /// <summary>
    /// Performs search.
    /// </summary>
    private SearchResult PredictiveSearch(string searchText)
    {
        // Prepare search text
        var docCondition = new DocumentSearchCondition(PredictiveSearchDocumentTypes, PredictiveSearchCultureCode, CultureHelper.GetDefaultCultureCode(SiteContext.CurrentSiteName), PredictiveSearchCombineWithDefaultCulture);
        var condition = new SearchCondition(PredictiveSearchCondition, PredictiveSearchMode, PredictiveSearchOptions, docCondition);

        string searchCondition = SearchSyntaxHelper.CombineSearchCondition(searchText, condition);

        // Prepare parameters
        SearchParameters parameters = new SearchParameters()
        {
            SearchFor = searchCondition,
            SearchSort = PredictiveSearchSort,
            Path = PredictiveSearchPath,
            ClassNames = PredictiveSearchDocumentTypes,
            CurrentCulture = PredictiveSearchCultureCode,
            DefaultCulture = CultureHelper.GetDefaultCultureCode(SiteContext.CurrentSiteName),
            CombineWithDefaultCulture = PredictiveSearchCombineWithDefaultCulture,
            CheckPermissions = PredictiveSearchCheckPermissions,
            SearchInAttachments = false,
            User = MembershipContext.AuthenticatedUser,
            SearchIndexes = PredictiveSearchIndexes,
            StartingPosition = 0,
            DisplayResults = PredictiveSearchMaxResults,
            NumberOfProcessedResults = 100 > PredictiveSearchMaxResults ? PredictiveSearchMaxResults : 100,
            NumberOfResults = 0,
            AttachmentWhere = null,
            AttachmentOrderBy = null,
            BlockFieldOnlySearch = PredictiveSearchBlockFieldOnlySearch,
        };

        var results = SearchHelper.Search(parameters);

        return results;
    }


    /// <summary>
    /// Renders search results into HTML string.
    /// </summary>
    private string RenderResults(List<SearchResultItem> results, string searchText)
    {
        if (results == null || results.Count == 0)
        {
            // No results
            return String.IsNullOrEmpty(PredictiveSearchNoResultsContent) ? "" : "<div class='nonSelectable'>" + PredictiveSearchNoResultsContent + "</div>";
        }
        else
        {
            UIRepeater repSearchResults = new UIRepeater();
            var indexCategories = new Dictionary<string, IEnumerable<SearchResultItem>>(StringComparer.InvariantCultureIgnoreCase);
            StringWriter stringWriter = new StringWriter();

            // Display categories - create DataView for each index
            if (PredictiveSearchDisplayCategories)
            {
                foreach (SearchResultItem resultItem in results)
                {
                    string index = resultItem.Index;

                    if (!indexCategories.ContainsKey(index))
                    {
                        indexCategories.Add(index, results.Where(item => String.Equals(item.Index, index, StringComparison.InvariantCultureIgnoreCase)));
                    }
                }
            }
            // Do not display categories - create DataView of whole table
            else
            {
                indexCategories.Add("results", results);
            }


            // Render each index category
            foreach (var categories in indexCategories)
            {
                // Display categories
                if (PredictiveSearchDisplayCategories)
                {
                    SearchIndexInfo indexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(categories.Key);
                    string categoryName = indexInfo == null ? String.Empty : indexInfo.IndexDisplayName;
                    repSearchResults.HeaderTemplate = new TextTransformationTemplate("<div class='predictiveSearchCategory nonSelectable'>" + categoryName + "</div>");
                }

                // Fill repeater with results
                repSearchResults.ItemTemplate = TransformationHelper.LoadTransformation(this, PredictiveSearchResultItemTransformationName);
                repSearchResults.DataSource = categories.Value;
                repSearchResults.DataBind();
                repSearchResults.RenderControl(new HtmlTextWriter(stringWriter));
            }

            // More results
            if (PredictiveSearchMaxResults == results.Count)
            {
                stringWriter.Write(String.Format(PredictiveSearchMoreResultsContent, CreateSearchUrl(searchText)));
            }

            return stringWriter.ToString();
        }
    }


    /// <summary>
    /// Returns URL to search page with search query.
    /// </summary>
    private string CreateSearchUrl(string searchText)
    {
        string url = SearchResultsPageUrl;

        if (url.StartsWithCSafe("~"))
        {
            url = ResolveUrl(url.Trim());
        }

        url = URLHelper.UpdateParameterInUrl(url, "searchtext", HttpUtility.UrlEncode(searchText));
        url = URLHelper.UpdateParameterInUrl(url, "searchmode", PredictiveSearchMode.ToStringRepresentation());

        return url.Trim();
    }


    /// <summary>
    /// Renders scrips into the page.
    /// </summary>
    private void RegisterScrips()
    {
        if (!RequestHelper.IsCallback())
        {
            ScriptHelper.RegisterJQuery(this.Page);
            ScriptHelper.RegisterScriptFile(Page, "~/CMSWebParts/SmartSearch/SearchBox_files/PredictiveSearch.js");

            string predictiveSearchCallback = Page.ClientScript.GetCallbackEventReference(this, "arg", "predictiveSearchObject_" + ClientID + ".RecieveSearchResults", "context");

            string script =
                "var predictiveSearchObject_" + ClientID + " = new PredictiveSearchExtender('" + ClientID + "', '" + txtWord.ClientID + "', '" + pnlPredictiveResultsHolder.ClientID +
                "', " + PredictiveSearchMinCharacters + ", " + (PredictiveSearchEnableSelection ? "true" : "false") + ", '" + PredictiveSearchSelectedCssClass + "', '" + PredictiveSearchResultsCssClass + "');\n" +
                "predictiveSearchObject_" + ClientID + ".CallPredictiveSearch = function(arg, context) { " + predictiveSearchCallback + "; }\n";

            ScriptHelper.RegisterStartupScript(this, typeof(string), "PredictiveSearch_" + ClientID, ScriptHelper.GetScript(script));
        }
    }

    /// <summary>
    /// Logs "internal search" activity and web analytics "on-site" search keyword.
    /// </summary>
    private void LogPredictiveSearch(string keywords)
    {
        if (PredictiveSearchLogSearchActivity)
        {
            mPagesActivityLogger.LogInternalSearch(keywords, DocumentContext.CurrentDocument);
        }

        if (PredictiveSearchTrackSearchKeywords)
        {
            AnalyticsHelper.LogOnSiteSearchKeywords(SiteContext.CurrentSiteName, DocumentContext.CurrentAliasPath, DocumentContext.CurrentDocumentCulture.CultureCode, keywords, 0, 1);
        }
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        SetupControl();
        base.ReloadData();
    }

    #endregion


    #region "ICallbackEventHandler methods"

    /// <summary>
    /// Returns predictive search rendered results.
    /// </summary>
    public string GetCallbackResult()
    {
        return mPredictiveSearchResult;
    }


    /// <summary>
    /// Performs the search and saves rendered results.
    /// </summary>
    public void RaiseCallbackEvent(string eventArgument)
    {
        if (!String.IsNullOrEmpty(eventArgument) && PredictiveSearchEnabled)
        {
            LogPredictiveSearch(eventArgument);

            var searchResults = PredictiveSearch(eventArgument);
            mPredictiveSearchResult = RenderResults(searchResults.Items, eventArgument);
        }
    }

    #endregion
}
