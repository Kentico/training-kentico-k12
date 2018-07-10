using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;

using CMS.EventLog;
using CMS.Helpers;
using CMS.IO;
using CMS.Localization;
using CMS.PortalEngine.Web.UI;

using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using CMS.Search;

public partial class CMSWebParts_Search_DidYouMean : CMSAbstractWebPart
{
    private const string startTag = "##START_SUGG##";
    private const string endTag = "##END_SUGG##";
    private const string searchUrlParameter = "searchtext";


    #region "Properties"

    /// <summary>
    /// Gets or sets value of language.
    /// </summary>
    public string Language
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Language"), "");
        }
        set
        {
            SetValue("Language", value);
        }
    }


    /// <summary>
    /// Gets or sets value of text message.
    /// </summary>
    public string Text
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Text"), "");
        }
        set
        {
            SetValue("Text", value);
        }
    }


    /// <summary>
    /// Gets or sets value of starting tag.
    /// </summary>
    public string StartTag
    {
        get
        {
            return ValidationHelper.GetString(GetValue("StartTag"), "<strong>");
        }
        set
        {
            SetValue("StartTag", value);
        }
    }


    /// <summary>
    /// Gets or sets value of ending tag.
    /// </summary>
    public string EndTag
    {
        get
        {
            return ValidationHelper.GetString(GetValue("EndTag"), "</strong>");
        }
        set
        {
            SetValue("EndTag", value);
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
        }
        else
        {
            // Get search terms
            string searchtext = QueryHelper.GetString(searchUrlParameter, String.Empty);

            // Use try/catch block. Search text can be un-parsered for query parser
            try
            {
                // Get search clauses -> searched words
                SearchQueryClauses clauses = SearchManager.GetQueryClauses(searchtext);
                if (clauses != null)
                {
                    // Get collection of highlights
                    clauses.GetQuery(false, true);
                    List<string> searchTerms = clauses.HighlightedWords;

                    string currentCulture = String.IsNullOrEmpty(Language.Trim()) ? LocalizationContext.PreferredCultureCode : Language;

                    // Get suggestions
                    string dicFileName = currentCulture + ".dic";
                    string suggestion = DidYouMean(dicFileName, searchtext, searchTerms);

                    // Show only if there is something to suggest
                    if (suggestion != String.Empty)
                    {
                        string queryText = HTMLHelper.HTMLEncode(suggestion).Replace(startTag, String.Empty).Replace(endTag, String.Empty);
                        string visibleText = HTMLHelper.HTMLEncode(suggestion).Replace(startTag, StartTag).Replace(endTag, EndTag);

                        // Change value of search parameter
                        string url = URLHelper.RemoveParameterFromUrl(RequestContext.CurrentURL, searchUrlParameter);
                        url = URLHelper.AddParameterToUrl(url, searchUrlParameter, queryText);

                        ltrText.Text = Text;
                        lnkSearch.NavigateUrl = url;
                        lnkSearch.Text = visibleText;
                    }
                    else
                    {
                        Visible = false;
                    }
                }
                else
                {
                    Visible = false;
                }
            }
            catch
            {
                Visible = false;
            }
        }
    }


    /// <summary>
    /// Did you mean suggestion.
    /// </summary>
    private static string DidYouMean(string dictionaryFile, string searchQuery, IEnumerable<string> searchTerms)
    {
        if (searchTerms != null)
        {
            Spelling SpellChecker = null;
            WordDictionary WordDictionary = null;


            #region "Word dictionary"

            // If not in cache, create new
            WordDictionary = new WordDictionary();
            WordDictionary.EnableUserFile = false;

            // Getting folder for dictionaries
            string folderName = HttpContext.Current.Request.MapPath("~/App_Data/Dictionaries/");

            // Check if dictionary file exists
            string fileName = Path.Combine(folderName, dictionaryFile);
            if (!File.Exists(fileName))
            {
                EventLogProvider.LogEvent(EventType.ERROR, "DidYouMean webpart", "Dictionary file not found!");

                return String.Empty;
            }

            WordDictionary.DictionaryFolder = folderName;
            WordDictionary.DictionaryFile = dictionaryFile;

            // Load and initialize the dictionary
            WordDictionary.Initialize();

            #endregion


            #region "SpellCheck"

            // Prepare spellchecker
            SpellChecker = new Spelling();
            SpellChecker.Dictionary = WordDictionary;
            SpellChecker.SuggestionMode = Spelling.SuggestionEnum.NearMiss;

            bool suggest = false;

            // Check all searched terms
            foreach (string term in searchTerms)
            {
                if (term.Length > 2)
                {
                    SpellChecker.Suggest(term);
                    ArrayList al = SpellChecker.Suggestions;

                    // If there are some suggestions
                    if ((al != null) && (al.Count > 0))
                    {
                        suggest = true;

                        // Expression to find term
                        Regex regex = RegexHelper.GetRegex("([\\+\\-\\s\\(]|^)" + term + "([\\s\\)]|$)");

                        // Change term in original search query
                        string suggestion = "$1" + startTag + al[0] + endTag + "$2";
                        searchQuery = regex.Replace(searchQuery, suggestion);
                    }
                }
            }

            #endregion


            if (suggest)
            {
                return searchQuery;
            }
        }

        return String.Empty;
    }


    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }

    #endregion
}