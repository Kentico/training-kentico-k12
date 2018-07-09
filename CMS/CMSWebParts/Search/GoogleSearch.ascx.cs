using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;

public partial class CMSWebParts_Search_GoogleSearch : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Search engine unique ID provided by Google.
    /// </summary>
    public string SearchEngineUniqueID
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchEngineUniqueID"), "");
        }
        set
        {
            SetValue("SearchEngineUniqueID", value);
        }
    }


    /// <summary>
    /// Search layout.
    /// </summary>
    public string Layout
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Layout"), "");
        }
        set
        {
            SetValue("Layout", value);
        }
    }


    /// <summary>
    /// Search layout style.
    /// </summary>
    public string LayoutStyle
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LayoutStyle"), "");
        }
        set
        {
            SetValue("LayoutStyle", value);
        }
    }


    /// <summary>
    /// Search results div element ID, in case that the search dialog and the results are divided.
    /// </summary>
    public string SearchResultsElementID
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchResultsElementID"), "cse");
        }
        set
        {
            SetValue("SearchResultsElementID", value);
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Loads the web part content.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }

    #endregion


    #region "Other methods"

    /// <summary>
    /// Sets up the control.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            if (PortalContext.ViewMode != ViewModeEnum.Design)
            {
                string culture = CultureHelper.GetShortCultureCode(DocumentContext.CurrentDocumentCulture.CultureCode);

                if (Layout == "dialog")
                {
                    ltlGoogleSearch.Text = "<div id=\"cse-search-form\" style=\"width: 100%;\">Loading</div><link rel=\"stylesheet\" href=\"http://www.google.com/cse/style/look/" + LayoutStyle + ".css\" type=\"text/css\" />";
                    ScriptHelper.RegisterStartupScript(Page, typeof(string), ClientID + "linkscript", ScriptHelper.GetScriptTag("http://www.google.com/jsapi"));
                    ScriptHelper.RegisterStartupScript(Page, typeof(string), ClientID + "linescript", ScriptHelper.GetScript("google.load('search', '1', {language : '" + culture + "'}); google.setOnLoadCallback(function() { var customSearchControl = new google.search.CustomSearchControl('" + SearchEngineUniqueID + "'); customSearchControl.setResultSetSize(google.search.Search.FILTERED_CSE_RESULTSET); var options = new google.search.DrawOptions(); options.setSearchFormRoot('cse-search-form'); customSearchControl.draw('" + SearchResultsElementID + "', options); }, true);"));
                }
                else if (Layout == "results")
                {
                    ltlGoogleSearch.Text = "<div id=\"" + SearchResultsElementID + "\" style=\"width:100%;\"></div>";
                }
                else
                {
                    ltlGoogleSearch.Text = "<div id=\"cse\" style=\"width: 100%;\">Loading</div><link rel=\"stylesheet\" href=\"http://www.google.com/cse/style/look/" + LayoutStyle + ".css\" type=\"text/css\" />";
                    ScriptHelper.RegisterStartupScript(Page, typeof(string), ClientID + "linkscript", ScriptHelper.GetScriptTag("http://www.google.com/jsapi"));
                    ScriptHelper.RegisterStartupScript(Page, typeof(string), ClientID + "linescript", ScriptHelper.GetScript("google.load('search', '1', {language : '" + culture + "'});google.setOnLoadCallback(function() {var customSearchControl = new google.search.CustomSearchControl('" + SearchEngineUniqueID + "');customSearchControl.setResultSetSize(google.search.Search." + Layout + ");customSearchControl.draw('cse');}, true);"));
                }
            }
        }
    }

    #endregion
}
