using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Search;

public partial class CMSWebParts_SmartSearch_SearchDialog : CMSAbstractWebPart
{
    #region "Properties"

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
    /// Gets or sets the value that indicates whether only search button should be displayed.
    /// </summary>
    public bool ShowOnlySearchButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowOnlySearchButton"), srchDialog.ShowOnlySearchButton);
        }
        set
        {
            SetValue("ShowOnlySearchButton", value);
            srchDialog.ShowOnlySearchButton = value;
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


    /// <summary>
    /// Gets or sets the result webpart id.
    /// </summary>
    public string ResultWebpartID
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ResultWebpartID"), "");
        }
        set
        {
            SetValue("ResultWebpartID", value);
            srchDialog.ResultWebpartID = value;
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

    #endregion


    #region "Methods"

    /// <summary>
    /// On init event.
    /// </summary>
    /// <param name="e">Params</param>
    protected override void OnInit(EventArgs e)
    {
        srchDialog.FilterID = ValidationHelper.GetString(GetValue("WebPartControlID"), ID);
        srchDialog.LoadData();
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
        // Check stop processing
        if (StopProcessing)
        {
            srchDialog.StopProcessing = true;
            return;
        }
        else
        {
            // Set settings to search dialog
            srchDialog.SearchForLabel = SearchForLabel;
            srchDialog.SearchModeLabel = SearchModeLabel;
            srchDialog.SearchButton = SearchButton;
            srchDialog.SearchMode = SearchMode;
            srchDialog.ShowSearchMode = ShowSearchMode;
            srchDialog.ShowOnlySearchButton = ShowOnlySearchButton;
            srchDialog.ResultWebpartID = ResultWebpartID;
            srchDialog.WatermarkCssClass = WatermarkCssClass;
            srchDialog.WatermarkText = WatermarkText;
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
}