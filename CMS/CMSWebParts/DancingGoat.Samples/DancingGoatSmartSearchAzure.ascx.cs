using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Search;
using CMS.Search.Azure;

using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Azure;

using SearchParameters = Microsoft.Azure.Search.Models.SearchParameters;
using SearchResult = Microsoft.Azure.Search.Models.SearchResult;

public partial class CMSWebParts_DancingGoat_Samples_DancingGoatSmartSearchAzure : CMSAbstractWebPart
{
    private const string CATEGORY_COUNTRY = "coffeecountry";
    private const string CATEGORY_PROCESSING = "coffeeprocessing";
    private const string CATEGORY_ALTITUDE = "coffeealtitude";


    private readonly IList<string> Facets = new List<string>
    {
        CATEGORY_COUNTRY,
        CATEGORY_PROCESSING,
        $"{CATEGORY_ALTITUDE},values:4000|4700|5400|6100"
    };


    /// <summary>
    /// Resets all facets and search filters.
    /// </summary>
    protected void ButtonResetClick(object sender, EventArgs e)
    {
        chklstFilterAltitude.ClearSelection();
        chklstFilterCountry.ClearSelection();
        chklstFilterProcessing.ClearSelection();
        drplstOrderByAscDesc.ClearSelection();
        drplstOrderBy.ClearSelection();
        txtbox.Text = string.Empty;

        Page_Load(sender, e);
    }


    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        var searchParams = GetSearchParameters(string.Join(" and ", GetFilterConditions()));
        var searchString = GetQueryEncode(txtbox.Text);

        DocumentSearchResult result;

        try
        {
            ISearchIndexClient searchIndexClient = GetSearchClient();

            if (searchIndexClient == null)
            {
                labelPanel.Text = "The Azure page index is missing. Please go to <strong>/Special-pages/Generator</strong> page and set up Azure Smart search demo.";
                return;
            }

            result = searchIndexClient.Documents.Search(searchString, searchParams);
        }
        catch (ArgumentException)
        {
            labelPanel.Text = "The Azure search credentials were not in the correct format. Please go to <strong>/Special-pages/Generator</strong> page and set up Azure Smart search demo.";
            return;
        }
        catch (HttpRequestException)
        {
            labelPanel.Text = "The Azure search service with given name does not exist. Please go to <strong>/Special-pages/Generator</strong> page and set up Azure Smart search demo.";
            return;
        }
        catch (CloudException)
        {
            labelPanel.Text = "The Azure search index Api key is incorrect or rebuild task is still in progress. Please go to the <strong>Smart Search -> Azure index -> Sample Dancing Goat - Coffee index -> Edit</strong> and verify whether all settings are filled correctly or visit <strong>/Special-pages/Generator</strong> page and set up Azure Smart search demo.";
            return;
        }
        catch
        {
            labelPanel.Text = "Unexpected error. Please go to <strong>/Special-pages/Generator</strong> page and set up Azure Smart search demo.";
            return;
        }

        if (result.Facets != null)
        {
            if (IsPostBack)
            {
                UpdateCountListItemForFacet(result, chklstFilterCountry, CATEGORY_COUNTRY, UpdateCountValueListItem);
                UpdateCountListItemForFacet(result, chklstFilterProcessing, CATEGORY_PROCESSING, UpdateCountValueListItem);
                UpdateCountListItemForFacet(result, chklstFilterAltitude, CATEGORY_ALTITUDE, UpdateCountRangeListItem);
            }
            else
            {
                GenerateFilterFacets(result.Facets);
            }
        }

        GenerateResultView(result);
    }


    /// <summary>
    /// Adds all items to the related controls.
    /// </summary>
    private void GenerateFilterFacets(FacetResults facets)
    {
        foreach (var facetCategory in facets)
        {
            foreach (FacetResult facet in facetCategory.Value)
            {
                AddItemToCheckBoxListControl(facet, facetCategory.Key);
            }
        }

        // List of items which will the search results be ordered by.
        drplstOrderBy.Items.AddRange(new[]
        {
            new ListItem("Name", "skuname"),
            new ListItem("Coffee region", CATEGORY_COUNTRY),
            new ListItem("Altitude", CATEGORY_ALTITUDE)
        });

        drplstOrderByAscDesc.Items.Add(new ListItem("Ascending", "asc"));
        drplstOrderByAscDesc.Items.Add(new ListItem("Descending", "desc"));
    }


    /// <summary>
    /// Returns search parameters based on given <paramref name="filter"/>.
    /// </summary>
    /// <param name="filter">Query used to filter indexed items.</param>
    private SearchParameters GetSearchParameters(string filter)
    {
        var searchparams = new SearchParameters
        {
            Facets = Facets,
            Filter = filter,
            HighlightFields = new List<string>
            {
                "skuname"
            },
            HighlightPreTag = "<strong>",
            HighlightPostTag = "</strong>",
            IncludeTotalResultCount = true
        };

        if (drplstOrderBy.SelectedItem != null && drplstOrderByAscDesc.SelectedItem != null)
        {
            searchparams.OrderBy = new[] { $"{drplstOrderBy.SelectedValue} {drplstOrderByAscDesc.SelectedValue}" };
        }
        else
        {
            searchparams.OrderBy = new[] { "skuname asc" };
        }

        return searchparams;
    }


    /// <summary>
    /// Returns query for filtering results based on faceted search.
    /// </summary>
    /// <remarks>Also specifies page category.</remarks>
    private IEnumerable<string> GetFilterConditions()
    {
        var filter = new List<string>
        {
            GetEqualFilterQuery("classname", "dancinggoat.coffee")
        };

        filter.AddRange(GetFilterQueryForCategory(chklstFilterCountry.GetSelectedItems(), CATEGORY_COUNTRY, GetEqualFilterQuery));
        filter.AddRange(GetFilterQueryForCategory(chklstFilterProcessing.GetSelectedItems(), CATEGORY_PROCESSING, GetEqualFilterQuery));
        filter.AddRange(GetFilterQueryForCategory(chklstFilterAltitude.GetSelectedItems(), CATEGORY_ALTITUDE, GetRangeFilterQuery));

        return filter;
    }


    /// <summary>
    /// Returns filter query based on <paramref name="selectedFacets"/>.
    /// </summary>
    /// <param name="selectedFacets">Facets selected by user.</param>
    /// <param name="columnName">Name of the facet category on which query will be created.</param>
    /// <param name="getFilterQuery">Delegate returning filter query.</param>
    private IEnumerable<string> GetFilterQueryForCategory(IEnumerable<ListItem> selectedFacets, string columnName,
        Func<string, string, string> getFilterQuery)
    {
        if (selectedFacets.Any())
        {
            var queries = selectedFacets.Select(facet => getFilterQuery(columnName, facet.Value));
            yield return $"({String.Join(" or ", queries)})";
        }
    }


    /// <summary>
    /// Returns filter query for range facet.
    /// </summary>
    private string GetRangeFilterQuery(string columnName, string value)
    {
        var rangeValues = GetQueryEncode(value).Split(';');

        return rangeValues.Any(String.IsNullOrWhiteSpace) ?
            $"({columnName} ge {rangeValues[0]})" :
            $"({columnName} ge {rangeValues[0]} and {columnName} le {rangeValues[1]})";
    }


    /// <summary>
    /// Returns equality filter query.
    /// </summary>
    private string GetEqualFilterQuery(string columnName, string value)
    {
        return $"{columnName} eq '{GetQueryEncode(value)}'";
    }


    /// <summary>
    /// Generates all filtered results with highlighting if available.
    /// </summary>
    private void GenerateResultView(DocumentSearchResult searchResult)
    {
        if (!searchResult.Results.Any())
        {
            labelPanel.Text = "No results were found.";
            totalCountLabel.Text = "Results: 0";

            return;
        }

        var productsViewResult = new StringBuilder();

        foreach (SearchResult result in searchResult.Results)
        {
            var skuInfo = SKUInfoProvider.GetSKUInfo(Guid.Parse(result.Document["skuguid"].ToString()));

            productsViewResult.Append(
$@"<div class='col-md-6 col-lg-4'>
    <article class='product-tile'>
        <a href='~{result.Document["nodealiaspath"]}'>
            <h1 class='product-heading'>{GetHighlightedNameResultView(result)}</h1>
            {GetStatusResultView(result)}
            {GetImageForSearchResultView(result, skuInfo)}
            {GetRatingResultView(result)}
            {GetPriceResultView(result, skuInfo)}
        </a>
    </article>
</div>"
            );
        }

        labelPanel.Text = productsViewResult.ToString();
        totalCountLabel.Text = $"Results: {searchResult.Count}";
    }


    /// <summary>
    /// Returns name of the product with highlighted text if any.
    /// </summary>
    private string GetHighlightedNameResultView(SearchResult result)
    {
        const string name = "skuname";

        return result.Highlights?.FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase)).Value?.First()
            ?? result.Document[name].ToString();
    }


    /// <summary>
    /// Returns tags with current status of the product if any.
    /// </summary>
    private string GetStatusResultView(SearchResult result)
    {
        var status = result.Document["skupublicstatusid"]?.ToString();

        int skuStatus;
        int.TryParse(status, out skuStatus);

        PublicStatusInfo publicStatus = PublicStatusInfoProvider.GetPublicStatusInfo(skuStatus);

        if (status == null || publicStatus == null)
        {
            return String.Empty;
        }

        var name = HTMLHelper.HTMLEncode(ResHelper.LocalizeString(publicStatus.PublicStatusDisplayName));
        var statusClassName = HTMLHelper.HTMLEncode(publicStatus.PublicStatusName).ToLowerInvariant();

        return $"<span class='product-tile-status {statusClassName}'>{name}</span>";
    }


    /// <summary>
    /// Returns tags with actual price and standard price if any.
    /// </summary>
    private string GetPriceResultView(SearchResult result, SKUInfo skuInfo)
    {
        ProductCatalogPrices skuPrice = GetProductCatalogPrices(skuInfo);

        if(skuPrice == null)
        {
            return $"<div class='product-tile-info'><span class='product-tile-price'>${result.Document["skuprice"]}</span>";
        }

        var tagResult = new StringBuilder();
        tagResult.AppendFormat("<div class='product-tile-info'><span class='product-tile-price'>${0}</span>", skuPrice.Price);

        if (skuPrice.Discounts.Any())
        {
            tagResult.AppendFormat("<span class='product-tile-list-price'>${0}</span>", skuPrice.StandardPrice);
        }

        tagResult.Append("</div>");

        return tagResult.ToString();
    }


    /// <summary>
    /// Returns tags with related image and out of stock message if any.
    /// </summary>
    private string GetImageForSearchResultView(SearchResult result, SKUInfo skuInfo)
    {
        var tagResult = new StringBuilder();
        bool isInStock = SKUInfoProvider.IsSKUInStock(skuInfo);

        tagResult.AppendFormat(
            "<figure class='product-tile-image {0}'><img src='{2}' alt='{3}'/>{1}</figure>",
            isInStock ? string.Empty : "notavailable",
            isInStock ? string.Empty : "<span class=\'product-tile-stock notavailable\'> OUT OF STOCK </span>",
            result.Document["skuimagepath"],
            result.Document["skuname"]
    );

        return tagResult.ToString();
    }


    /// <summary>
    /// Returns tags with rating.
    /// </summary>
    private string GetRatingResultView(SearchResult result)
    {
        double documentRatingValue;
        double documentRatings;

        Double.TryParse(result.Document["documentratingvalue"]?.ToString() ?? "0", out documentRatingValue);
        Double.TryParse(result.Document["documentratings"]?.ToString() ?? "1", out documentRatings);

        var rating = documentRatingValue / ((Math.Abs(documentRatings) < 0) ? 1 : documentRatings);

        var tagResult = new StringBuilder();
        tagResult.Append("<div class='product-rating-content'>");

        for (int i = 0; i < 5; ++i)
        {
            tagResult.AppendFormat("<span class='rating-star cms-icon-80 {0}'></span>", ((rating > (i / 5.0)) ? "icon-star-full" : "icon-star-empty"));
        }

        tagResult.Append("</div>");

        return tagResult.ToString();
    }


    /// <summary>
    /// Updates count status for each facet.
    /// </summary>
    private void UpdateCountListItemForFacet(DocumentSearchResult result, ListControl facetControl, string facetName,
        Action<IEnumerable<FacetResult>, ListItem> updateCountListItem)
    {
        foreach (ListItem item in facetControl.Items)
        {
            updateCountListItem(result.Facets[facetName], item);
        }
    }


    /// <summary>
    /// Updates sum of filtered value results in <paramref name="listItem"/> according to <paramref name="facetResults"/>.
    /// </summary>
    private void UpdateCountValueListItem(IEnumerable<FacetResult> facetResults, ListItem listItem)
    {
        long? count = facetResults
            .FirstOrDefault(x => (x.Type == FacetType.Value) && listItem.Value.Equals(x.Value.ToString(), StringComparison.OrdinalIgnoreCase))?
            .Count ?? 0;

        listItem.Text = $"{listItem.Value} ({count})";
    }


    /// <summary>
    /// Updates sum of filtered range results in <paramref name="listItem"/> according to <paramref name="facetResults"/>.
    /// </summary>
    private void UpdateCountRangeListItem(IEnumerable<FacetResult> facetResults, ListItem listItem)
    {
        var listItemValues = listItem.Value.Split(';');
        int from = ValidationHelper.GetInteger(listItemValues[0], 0);
        int to = ValidationHelper.GetInteger(listItemValues[1], int.MaxValue);

        var count = facetResults
            .FirstOrDefault(facetResult =>
            {
                int fromFacet = ValidationHelper.GetInteger(facetResult.From, 0);
                int toFacet = ValidationHelper.GetInteger(facetResult.To, int.MaxValue);

                return (facetResult.Type == FacetType.Range) && (fromFacet == from) && (toFacet == to);
            })?
            .Count ?? 0;

        string toFinalResult = (to == int.MaxValue) ? string.Empty : to.ToString();

        listItem.Text = $"{from}-{toFinalResult} ({count})";
    }


    /// <summary>
    /// Adds <paramref name="facetResult"/> to <paramref name="categoryName"/> category.
    /// </summary>
    private void AddItemToCheckBoxListControl(FacetResult facetResult, string categoryName)
    {
        switch (categoryName)
        {
            case CATEGORY_COUNTRY:
                chklstFilterCountry.Items.Add(GetValueListItem(facetResult));
                break;
            case CATEGORY_PROCESSING:
                chklstFilterProcessing.Items.Add(GetValueListItem(facetResult));
                break;
            case CATEGORY_ALTITUDE:
                chklstFilterAltitude.Items.Add(GetRangeListItem(facetResult));
                break;
        }
    }


    /// <summary>
    /// Returns list item with range values.
    /// </summary>
    private ListItem GetRangeListItem(FacetResult facetResult)
    {
        string from = facetResult.From?.ToString() ?? "0";
        string to = facetResult.To?.ToString() ?? string.Empty;

        return new ListItem($"{from}-{to} ({facetResult.Count})", $"{from};{to}");
    }


    /// <summary>
    /// Returns list item with exact value from <paramref name="facetResult"/>.
    /// </summary>
    private ListItem GetValueListItem(FacetResult facetResult)
    {
        return new ListItem($"{facetResult.Value} ({facetResult.Count})", facetResult.Value.ToString());
    }


    /// <summary>
    /// Returns all catalog prices of given <paramref name="sku"/>.
    /// </summary>
    private ProductCatalogPrices GetProductCatalogPrices(SKUInfo sku)
    {
        var cart = ECommerceContext.CurrentShoppingCart;
        if (cart == null)
        {
            return null;
        }

        return Service.Resolve<ICatalogPriceCalculatorFactory>()
            .GetCalculator(cart.ShoppingCartSiteID)
            .GetPrices(sku, null, cart);
    }


    /// <summary>
    /// Returns a string converted into query-encoded string.
    /// </summary>
    private string GetQueryEncode(string inputText)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            return string.Empty;
        }

        return inputText.Replace("'", "''");
    }


    /// <summary>
    /// Returns initialized <see cref="SearchServiceClient"/> based on index name "sample-dancinggoat-coffee-azure".
    /// </summary>
    private ISearchIndexClient GetSearchClient()
    {
        var indexName = NamingHelper.GetValidIndexName("sample-dancinggoat-coffee-azure");
        var indexInfo = SearchIndexInfoProvider.GetSearchIndexInfo(indexName);

        if (indexInfo == null)
        {
            return null;
        }

        var serviveClient = new SearchServiceClient(indexInfo.IndexSearchServiceName, new SearchCredentials(indexInfo.IndexAdminKey));

        return serviveClient.Indexes.GetClient(indexName);
    }
}
