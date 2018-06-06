using System.Web.UI;

using CMS.Core;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Search;
using CMS.WebAnalytics;

public partial class CMSWebParts_Search_cmssearchdialog : CMSAbstractWebPart
{
    #region "Variables"

    /// <summary>
    /// Flag indicates whether DoSearch event has been called.
    /// </summary>
    private bool doSearch = false;
    private IPagesActivityLogger mPagesActivityLogger = Service.Resolve<IPagesActivityLogger>();

    #endregion


    #region "Public properties"

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
    /// Gets or sets the search button text.
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
            return CMSSearchDialog.GetSearchMode(DataHelper.GetNotEmpty(GetValue("SearchMode"), srchDialog.SearchMode.ToStringRepresentation()));
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
            return DataHelper.GetNotEmpty(GetValue("SearchModeLabel"), ResHelper.LocalizeString("{$CMSSearchDialog.SearchMode$}"));
        }
        set
        {
            SetValue("SearchModeLabel", value);
            srchDialog.SearchModeLabel.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether search scope drop down is displayed.
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
    /// Gets or sets the search scope label text.
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
        }
        else
        {
            srchDialog.ID = ID + "_CMSSearchDialog";

            // Set label text
            srchDialog.SearchForLabel.Text = SearchForLabel;
            // Set button text
            srchDialog.SearchButton.Text = SearchButtonText;
            srchDialog.DoSearch += new CMSSearchDialog.DoSearchEventHandler(srchDialog_DoSearch);
            
            if (srchDialog.ShowSearchMode = ShowSearchMode)
            {
                // Set search mode
                srchDialog.SearchMode = SearchMode;
                // Set mode label text
                srchDialog.SearchModeLabel.Text = SearchModeLabel;
            }

            if (srchDialog.ShowSearchScope = ShowSearchScope)
            {
                // Set search scope
                srchDialog.SearchScope = SearchScope;
                // Set scope label text
                srchDialog.SearchScopeLabel.Text = SearchScopeLabel;
            }

            if (!StandAlone && (PageCycle < PageCycleEnum.Initialized) && (ValidationHelper.GetString(Page.StyleSheetTheme, "") == ""))
            {
                // Set SkinID property
                srchDialog.SkinID = SkinID;
            }
        }
    }


    protected void srchDialog_DoSearch()
    {
        if (!doSearch)
        {
            doSearch = true;
            // Log "internal search" activity
            mPagesActivityLogger.LogInternalSearch(srchDialog.SearchForTextBox.Text, DocumentContext.CurrentDocument);
        }
    }


    /// <summary>
    /// Applies given stylesheet skin.
    /// </summary>
    public override void ApplyStyleSheetSkin(Page page)
    {
        srchDialog.SkinID = SkinID;
        base.ApplyStyleSheetSkin(page);
    }
}