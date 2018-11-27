using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.OnlineMarketing;
using CMS.OnlineMarketing.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;

using Newtonsoft.Json;

using Action = CMS.UIControls.UniGridConfig.Action;

/// <summary>
/// AB test Overview page, which shows graphs and listing for all variants for the test.
/// Shows data like conversion rate, conversion count, conversion value, and user can switch between cultures, conversions (because we log all conversions for all tests),
/// whether the graph will show data in cumulative or day-wise manner and other.
/// </summary>
/// <remarks>Saves state of selectors to a cookie</remarks>
[Security(Resource = "CMS.ABTest", UIElements = "Overview")]
[Security(Resource = "CMS.ABTest", UIElements = "Detail")]
[UIElement("CMS.ABTest", "Overview")]
public partial class CMSModules_OnlineMarketing_Pages_Content_ABTesting_ABTest_Overview : CMSABTestPage
{
    #region "Variables"

    /// <summary>
    /// Variants of the current AB test.
    /// </summary>
    private List<ABVariantInfo> mABVariants;


    /// <summary>
    /// Statistics data of the current AB test's variants.
    /// </summary>
    private Dictionary<string, ABVariantStatisticsData> mVariantsStatisticsData;


    /// <summary>
    /// Conversion rate intervals of the current AB test's variants.
    /// </summary>
    private Dictionary<string, ABConversionRateInterval> mABConversionRateIntervals;


    /// <summary>
    /// Original variant of the current AB test.
    /// </summary>
    private ABVariantInfo mOriginalVariant;


    /// <summary>
    /// AB Variant performance calculator.
    /// </summary>
    private IABVariantPerformanceCalculator mVariantPerformanceCalculator;


    /// <summary>
    /// The minimum lower bound of the conversion rate intervals for the test.
    /// </summary>
    private double mMinConversionRateLowerBound;


    /// <summary>
    /// The difference between the minimum lower bound and the maximum upper bound values for the test.
    /// </summary>
    private double mConversionRateRange;


    /// <summary>
    /// Class that writes info messages into the page.
    /// </summary>
    private ABTestMessagesWriter mMessagesWriter;


    /// <summary>
    /// Indicates whether the user is authorized to manage the test.
    /// </summary>
    private bool? mIsUserAuthorizedToManageTest;


    private string mDrpConversionsValue, mDrpCultureValue;


    private int mSamplingIndex, mGraphDataIndex, mDrpSuccessMetricIndex, mDrpCountingMethodologyIndex;


    private bool mAdvancedcontrolsVisible;

    #endregion


    #region "Constants"

    /// <summary>
    /// Max length of a link that is in the upper right box - long links can break that table.
    /// </summary>
    private const int MAX_LINK_LENGTH = 22;


    /// <summary>
    /// Minimum of conversions to mark a variant as winning.
    /// </summary>
    private const int WINNING_VARIANT_MIN_CONVERSIONS = 25;


    /// <summary>
    /// Minimum chance to beat to mark a variant as winning.
    /// </summary>
    private const double WINNING_VARIANT_MIN_CHANCETOBEAT = 0.95d;


    /// <summary>
    /// Relative path to show advanced filters image.
    /// </summary>
    protected const string SHOW_FILTERS_IMAGE = "Design/Controls/UniGrid/Actions/SortDown.png";


    /// <summary>
    /// Relative path to hide advanced filters image.
    /// </summary>
    protected const string HIDE_FILTERS_IMAGE = "Design/Controls/UniGrid/Actions/SortUp.png";


    /// <summary>
    /// Resource string of the show advanced filters text.
    /// </summary>
    protected const string SHOW_FILTERS_TEXT = "abtesting.showfilters";


    /// <summary>
    /// Resource string of the hide advanced filters text.
    /// </summary>
    protected const string HIDE_FILTERS_TEXT = "abtesting.hidefilters";


    /// <summary>
    /// Smart tip identifier. If this smart tip is collapsed, this ID is stored in DB.
    /// </summary>
    private const string SMART_TIP_IDENTIFIER = "howtovideo|abtest|overview";


    /// <summary>
    /// Names of sampling selector actions.
    /// </summary>
    protected string[] samplingActions = { "day", "week", "month" };


    /// <summary>
    /// Names of data format selector actions.
    /// </summary>
    protected string[] dataFormatActions = { "cumulative", "daywise" };

    #endregion


    #region "Properties"

    /// <summary>
    /// Current AB test.
    /// </summary>
    private ABTestInfo ABTest
    {
        get
        {
            return ABTestInfoProvider.GetABTestInfo(QueryHelper.GetInteger("objectid", 0));
        }
    }


    /// <summary>
    /// Status of the current test.
    /// </summary>
    private ABTestStatusEnum TestStatus
    {
        get
        {
            return ABTestStatusEvaluator.GetStatus(ABTest);
        }
    }


    /// <summary>
    /// Variants of the current AB test.
    /// </summary>
    private List<ABVariantInfo> ABVariants
    {
        get
        {
            return mABVariants ?? (mABVariants = ABCachedObjects.GetVariants(ABTest));
        }
    }


    /// <summary>
    /// Original variant of the current AB test.
    /// </summary>
    private ABVariantInfo OriginalVariant
    {
        get
        {
            if (mOriginalVariant == null)
            {
                if (ABVariants != null)
                {
                    foreach (var variant in ABVariants)
                    {
                        if (variant.ABVariantPath == ABTest.ABTestOriginalPage)
                        {
                            mOriginalVariant = variant;
                            break;
                        }
                    }
                }
            }

            return mOriginalVariant;
        }
    }


    /// <summary>
    /// Instance of IABVariantPerformanceCalculator implementation.
    /// </summary>
    private IABVariantPerformanceCalculator VariantPerformanceCalculator
    {
        get
        {
            if (mVariantPerformanceCalculator == null)
            {
                if (VariantsStatisticsData.ContainsKey(OriginalVariant.ABVariantName))
                {
                    var variantData = VariantsStatisticsData[OriginalVariant.ABVariantName];

                    if ((variantData.Visits > 0) && (variantData.ConversionsCount >= 0))
                    {
                        mVariantPerformanceCalculator = ABVariantPerformanceCalculatorFactory.GetImplementation(variantData.ConversionsCount, variantData.Visits);
                    }
                }
            }

            return mVariantPerformanceCalculator;
        }
    }


    /// <summary>
    /// Gets dictionary that holds statistics data of variants.
    /// </summary>
    private Dictionary<string, ABVariantStatisticsData> VariantsStatisticsData
    {
        get
        {
            return mVariantsStatisticsData ?? (mVariantsStatisticsData = new Dictionary<string, ABVariantStatisticsData>());
        }
    }


    /// <summary>
    /// Gets dictionary that holds conversion rate intervals of variants.
    /// </summary>
    private Dictionary<string, ABConversionRateInterval> ABConversionRateIntervals
    {
        get
        {
            return mABConversionRateIntervals ?? (mABConversionRateIntervals = new Dictionary<string, ABConversionRateInterval>());
        }
    }


    /// <summary>
    /// Gets class that writes info messages into the page.
    /// </summary>
    private ABTestMessagesWriter MessagesWriter
    {
        get
        {
            return mMessagesWriter ?? (mMessagesWriter = new ABTestMessagesWriter(ShowMessage));
        }
    }


    /// <summary>
    /// Gets key of the cookie saving important selectors' state.
    /// </summary>
    protected string SelectorsCookieKey
    {
        get
        {
            return CookieName.ABSelectorStatePrefix + ABTest.ABTestName;
        }
    }


    /// <summary>
    /// Indicates whether the user is authorized to finish the test.
    /// </summary>
    private bool IsUserAuthorizedToManageTest
    {
        get
        {
            if (!mIsUserAuthorizedToManageTest.HasValue)
            {
                SiteInfo site = SiteInfoProvider.GetSiteInfo(ABTest.ABTestSiteID);
                mIsUserAuthorizedToManageTest = CurrentUser.IsAuthorizedPerResource("CMS.ABTest", "Manage", site.SiteName);
            }

            return mIsUserAuthorizedToManageTest.Value;
        }
    }

    #endregion


    #region "Events"

    protected void Page_Init(object sender, EventArgs e)
    {
        if (ABTest == null)
        {
            RedirectToInformation(GetString("general.incorrectURLparams"));
        }

        InitRadioPanelButtons();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        HeaderActions.ActionPerformed += HeaderActions_ActionPerformed;

        MessagesWriter.ShowABTestScheduleInformation(ABTest, TestStatus);
        MessagesWriter.ShowMissingVariantsTranslationsWarning(ABTest);

        ScriptHelper.RegisterDialogScript(Page);
        InitializeSelectors();
        InitSmartTip();

        // Hide summary and table if the test has not been started yet
        if ((ABTest.ABTestOpenFrom > DateTime.Now) || (ABTest.ABTestOpenFrom == DateTimeHelper.ZERO_TIME))
        {
            Summary.Visible = false;
            gridElem.Visible = false;
            return;
        }

        // Display test winner if there is one
        var winner = GetTestWinner();
        if (winner != null)
        {
            DisplayWinnerInformation(winner);
            SetWinnerTooltip();
        }

        EnsureVariantsStatisticsData();
        if (DataAvailable())
        {
            // Add class to the report because graph with data requires special positioning
            // Show all information after graph do postback
            if (RequestHelper.IsPostBack())
            {
                Summary.Visible = true;
                gridElem.Visible = true;
                gridElem.GridView.AddCssClass("rows-middle-vertical-align");

                // Hide NoDataFound panel
                pnlNoData.Visible = false;
            }
        }
        else
        {
            // Hide summary and table
            Summary.Visible = false;
            gridElem.Visible = false;

            // Show NoDataFound panel
            pnlNoData.Visible = true;
            return;
        }

        LoadSummaryBox();
        InitializeGraph();
        InitializeGrid();
        SetImprovementColumnCaption();
        ShowInvalidFilterCombinationImage();
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Reload conversions list to show only conversions with data
        drpConversions.ReloadData(false);

        // Pre-select first conversion, load selector state might change it later
        // It has to be in PreRender, because items in DropDownSingleSelect are not yet loaded in PageLoad event
        if (drpConversions.UniSelector.DropDownSingleSelect.Items.Count > 1)
        {
            drpConversions.Value = drpConversions.UniSelector.DropDownSingleSelect.Items[1].Value;
        }

        SetSelectorValues();

        // Select test culture in the selector
        if (!String.IsNullOrEmpty(ABTest.ABTestCulture))
        {
            drpCulture.Value = ABTest.ABTestCulture;
            drpCulture.Enabled = false;
        }

        // OnPreRender because test status can be changed later, not only in the Load event
        InitHeaderActions();
    }


    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        if (!RequestHelper.IsPostBack())
        {
            return string.Empty;
        }

        // Handle the grid action first because it doesn't require access to VariantsStatisticsData
        if (sourceName == "selectwinner")
        {
            var gridViewRow = parameter as GridViewRow;
            if (gridViewRow != null)
            {
                var dataRowView = gridViewRow.DataItem as DataRowView;
                if (dataRowView != null)
                {
                    var img = sender as CMSGridActionButton;
                    if (img != null)
                    {
                        // Check permissions to select winner
                        if (!IsUserAuthorizedToManageTest)
                        {
                            img.Enabled = false;
                            img.ToolTip = GetString("abtesting.selectwinner.permission.tooltip");
                        }
                        else
                        {
                            var winner = GetTestWinner();
                            if (winner != null)
                            {
                                string variantName = (ValidationHelper.GetString(dataRowView["ABVariantName"], ""));
                                if (variantName == winner.ABVariantName)
                                {
                                    // Disable action image for the winning variant
                                    img.Enabled = false;
                                }
                                else
                                {
                                    // Hide action image for other variants
                                    img.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        string currentVariantName = parameter.ToString();

        if (String.IsNullOrEmpty(currentVariantName) || (OriginalVariant == null) || !VariantsStatisticsData.ContainsKey(currentVariantName))
        {
            return string.Empty;
        }

        var variantData = VariantsStatisticsData[currentVariantName];

        switch (sourceName)
        {
            case "name":
                var variant = ABVariants.FirstOrDefault(v => v.ABVariantName == currentVariantName);
                if (variant != null)
                {
                    var link = new HtmlAnchor();
                    link.InnerText = ResHelper.LocalizeString(variant.ABVariantDisplayName);
                    link.HRef = DocumentURLProvider.GetUrl(variant.ABVariantPath);
                    link.Target = "_blank";
                    return link;
                }
                break;

            case "conversionsovervisits":
                return variantData.ConversionsCount + " / " + variantData.Visits;

            case "chancetobeatoriginal":
                if ((currentVariantName != OriginalVariant.ABVariantName) && (VariantPerformanceCalculator != null) && (variantData.Visits > 0))
                {
                    double chanceToBeatOriginal = VariantPerformanceCalculator.GetChanceToBeatOriginal(variantData.ConversionsCount, variantData.Visits);

                    // Check whether the variant is most probably winning already and mark the row green
                    if ((chanceToBeatOriginal >= WINNING_VARIANT_MIN_CHANCETOBEAT) && (variantData.ConversionsCount >= WINNING_VARIANT_MIN_CONVERSIONS))
                    {
                        AddCSSToParentControl(sender as WebControl, "winning-variant-row");
                    }

                    return String.Format("{0:P2}", chanceToBeatOriginal);
                }
                break;

            case "conversionrate":
                if ((VariantPerformanceCalculator != null) && (variantData.Visits > 0)
                    && ABConversionRateIntervals.ContainsKey(currentVariantName) && ABConversionRateIntervals.ContainsKey(OriginalVariant.ABVariantName))
                {
                    // Render the picture representing how the challenger variant is performing against the original variant
                    return new ABConversionRateIntervalVisualizer(
                        mMinConversionRateLowerBound, mConversionRateRange, ABConversionRateIntervals[currentVariantName], ABConversionRateIntervals[OriginalVariant.ABVariantName]);
                }
                break;

            case "conversionvalue":
                return variantData.ConversionsValue;

            case "averageconversionvalue":
                return String.Format("{0:#.##}", variantData.AverageConversionValue);

            case "improvement":
                if ((currentVariantName != OriginalVariant.ABVariantName) && VariantsStatisticsData.ContainsKey(OriginalVariant.ABVariantName))
                {
                    var originalData = VariantsStatisticsData[OriginalVariant.ABVariantName];
                    switch (drpSuccessMetric.SelectedValue)
                    {
                        case "conversioncount":
                            if (!originalData.ConversionsCount.Equals(0))
                            {
                                return GetPercentageImprovementPanel((variantData.ConversionsCount / (double)originalData.ConversionsCount) - 1);
                            }
                            break;

                        case "conversionvalue":
                            if (!originalData.ConversionsValue.Equals(0))
                            {
                                return GetPercentageImprovementPanel((variantData.ConversionsValue / originalData.ConversionsValue) - 1);
                            }
                            break;

                        case "conversionrate":
                            if (!originalData.ConversionRate.Equals(0))
                            {
                                return GetPercentageImprovementPanel((variantData.ConversionRate / originalData.ConversionRate) - 1);
                            }
                            break;

                        case "averageconversionvalue":
                            if (!originalData.AverageConversionValue.Equals(0))
                            {
                                return GetPercentageImprovementPanel((variantData.AverageConversionValue / originalData.AverageConversionValue) - 1);
                            }
                            break;
                    }
                }
                break;
        }

        return string.Empty;
    }


    protected void gridElem_OnAction(string actionName, object actionArgument)
    {
        if (!IsUserAuthorizedToManageTest)
        {
            RedirectToAccessDenied("cms.abtest", "Manage");
        }

        switch (actionName)
        {
            case "selectwinner":
                FinishTestAndSelectVariantAsWinner(actionArgument as string);
                break;
        }
    }


    protected void HeaderActions_ActionPerformed(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "selectwinner":
                FinishTestAndSelectVariantAsWinner(e.CommandArgument as string);
                break;
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Initializes selectors.
    /// </summary>
    private void InitializeSelectors()
    {
        // Turn off autocomplete to allow only predefined options (conversions) to be selected
        drpConversions.UniSelector.UseUniSelectorAutocomplete = false;

        var testConversions = ABTest.ABTestConversions.ToList();
        if (testConversions.Any())
        {
            drpConversions.WhereCondition = SqlHelper.GetWhereCondition("ConversionName", testConversions);
        }
        drpConversions.AllRecordValue = "";
        drpConversions.ABTestName = ABTest.ABTestName;

        // Turn on autopostback for the culture selector
        drpCulture.DropDownSingleSelect.AutoPostBack = true;

        // Load ShowAdvancedFilters content
        spnShowAdvancedFilters.InnerText = GetString(SHOW_FILTERS_TEXT);

        ParseSelectorCookie();
    }


    /// <summary>
    /// Initializes unigrid.
    /// </summary>
    private void InitializeGrid()
    {
        gridElem.DataSource = new InfoDataSet<ABVariantInfo>(ABVariants.ToArray());

        if (drpSuccessMetric.SelectedValue == "conversionvalue")
        {
            columnConversionValue.Visible = true;

            // Hide chance to beat original and conversion rate interval columns as they are related to conversion rate
            HideChanceToBeatAndRateColumns();
        }
        else if (drpSuccessMetric.SelectedValue == "averageconversionvalue")
        {
            columnAvgConversionValue.Visible = true;

            // Hide chance to beat original and conversion rate interval columns as they are related to conversion rate
            HideChanceToBeatAndRateColumns();
        }

        if (ValidationHelper.GetString(drpConversions.Value, "") == "")
        {
            // Hide chance to beat original and conversion rate interval columns as they don't provide relevant data when all conversions are selected
            HideChanceToBeatAndRateColumns();

            if (drpSuccessMetric.SelectedValue == "conversionrate")
            {
                columnImprovement.Visible = false;
            }
        }

        // If Visitors conversion methodology selected, use "Visitors" instead of "Visits" in unigrid
        if (drpCountingMethodology.SelectedValue == "absessionconversionfirst")
        {
            columnConversionsOverVisits.Caption = GetString("abtesting.conversionsovervisitors");
        }
    }


    /// <summary>
    /// Hides chance to beat original and conversion rate columns.
    /// </summary>
    private void HideChanceToBeatAndRateColumns()
    {
        columnChanceToBeatOriginal.Visible = false;
        columnConversionRateInterval.Visible = false;
    }


    /// <summary>
    /// Initializes graph.
    /// </summary>
    private void InitializeGraph()
    {
        DataTable data = new DataTable();
        data.Columns.Add("FromDate", typeof(DateTime));

        // Show graph data to the finish date or today if the test is still running
        data.Columns.Add("ToDate", typeof(DateTime));

        data.Columns.Add("TestName", typeof(string));

        // Set conversion name either to selected one, or to all conversions set in settings tab
        data.Columns.Add("ConversionName", typeof(string));
        data.Columns.Add("GraphType", typeof(string));
        data.Columns.Add("ABTestID", typeof(int));
        data.Columns.Add("VariationName", typeof(string));
        data.Columns.Add("ABTestCulture", typeof(string));
        data.Columns.Add("ConversionType", typeof(string));

        string conversionName = (string)(String.IsNullOrEmpty(drpConversions.Value as string) ? ABTest.ABTestConversions.Join(";") : drpConversions.Value);

        object[] parameters =
        {
            ABTest.ABTestOpenFrom,
            GetFinishDateOrToday(),
            ABTest.ABTestName,
            conversionName,
            graphDataElem.SelectedActionName,
            ABTest.ABTestID,
            string.Empty,
            drpCulture.Value,
            drpCountingMethodology.SelectedValue
        };

        data.Rows.Add(parameters);
        data.AcceptChanges();

        displayReport.Colors = ABVariantColorAssigner.GetColors(ABTest);
        displayReport.LoadFormParameters = false;
        displayReport.DisplayFilter = false;
        displayReport.GraphImageWidth = 100;
        displayReport.IgnoreWasInit = true;
        displayReport.UseExternalReload = true;
        displayReport.UseProgressIndicator = true;
        displayReport.SelectedInterval = samplingElem.SelectedActionName;
        displayReport.ReportParameters = data.Rows[0];
        displayReport.ReportName = "abtest" + drpSuccessMetric.SelectedValue + "." + samplingElem.SelectedActionName + "report";
    }


    /// <summary>
    /// Gets winning variant of the test.
    /// </summary>
    private ABVariantInfo GetTestWinner()
    {
        return ABVariants.FirstOrDefault(v => v.ABVariantGUID == ABTest.ABTestWinnerGUID);
    }


    /// <summary>
    /// Finishes test and selects variant as winner.
    /// </summary>
    /// <param name="winnerName">Winner variant name</param>
    private void FinishTestAndSelectVariantAsWinner(string winnerName)
    {
        if (String.IsNullOrEmpty(winnerName) || (OriginalVariant == null))
        {
            ShowError(GetString("abtesting.pushwinner.error.general"));
            return;
        }

        var variant = ABVariants.FirstOrDefault(v => v.ABVariantName == winnerName);
        if (variant == null)
        {
            ShowError(GetString("abtesting.pushwinner.error.general"));
            return;
        }

        // Set winner GUID to ABTest
        ABTest.ABTestWinnerGUID = variant.ABVariantGUID;

        // If the test is running, finish it
        if (TestStatus == ABTestStatusEnum.Running)
        {
            ABTest.ABTestOpenTo = DateTime.Now;
        }

        ABTestInfoProvider.SetABTestInfo(ABTest);

        DisplayWinnerInformation(variant);
        SetWinnerTooltip();

        // Reload data because HeaderActions_ActionPerformed event is too late to change action tooltip
        gridElem.ReloadData();

        // Reload summary box based on new status
        LoadSummaryBox();
    }


    /// <summary>
    /// Displays info label about the winner of the test.
    /// </summary>
    /// <param name="winner">AB test winner</param>
    private void DisplayWinnerInformation(ABVariantInfo winner)
    {
        ShowInformation(String.Format(GetString("abtesting.winningvariantselected"), HTMLHelper.HTMLEncode(ResHelper.LocalizeString(winner.ABVariantDisplayName))));
    }


    /// <summary>
    /// Updates tooltip of the grid action to reflect that winner has been selected.
    /// </summary>
    private void SetWinnerTooltip()
    {
        var action = gridElem.GridActions.Actions.Cast<Action>().FirstOrDefault(t => t.Name.ToLowerCSafe() == "selectwinner");
        if (action != null)
        {
            action.Caption = "$abtesting.winningvariantselected.tooltip$";
        }
    }


    /// <summary>
    /// Sets CSS class of a parent of given control.
    /// </summary>
    /// <param name="control">Child control that has a parent that's being changed</param>
    /// <param name="cssClass">CSS class to change parent to</param>
    private static void AddCSSToParentControl(Control control, string cssClass)
    {
        if (control != null)
        {
            var row = control.Parent as WebControl;
            if (row != null)
            {
                row.AddCssClass(cssClass);
            }
        }
    }


    /// <summary>
    /// Initializes header action control.
    /// </summary>
    private void InitHeaderActions()
    {
        switch (TestStatus)
        {
            case ABTestStatusEnum.NotStarted:
            case ABTestStatusEnum.Scheduled:
                AddStartTestButton();
                break;

            case ABTestStatusEnum.Running:
                AddFinishAndSelectAsWinnerButton();
                break;

            case ABTestStatusEnum.Finished:
                if (ABTest.ABTestWinnerGUID == Guid.Empty)
                {
                    AddSelectAsWinnerButton();
                }
                break;
        }
    }


    /// <summary>
    /// Adds the "Finish test" with alternative actions button.
    /// </summary>
    private void AddFinishAndSelectAsWinnerButton()
    {
        var actions = new List<HeaderAction>();

        AddSelectAsWinnerOptions(actions);

        string testFinishUrl = ResolveUrl("~/CMSModules/OnlineMarketing/Pages/Content/ABTesting/ABTest/FinishABTest.aspx?testid=" + ABTest.ABTestID);
        HeaderActions.AddAction(new HeaderAction
        {
            Tooltip = GetString("abtesting.finishtest.tooltip"),
            Text = GetString("abtesting.finishtest"),
            AlternativeActions = actions,
            OnClientClick = "modalDialog('" + testFinishUrl + @"', '', 670, 320);",
            Enabled = IsUserAuthorizedToManageTest
        });
    }


    /// <summary>
    /// Adds the "Select as winner" button.
    /// </summary>
    private void AddSelectAsWinnerButton()
    {
        var actions = new List<HeaderAction>();
        AddSelectAsWinnerOptions(actions);

        HeaderActions.AddAction(new HeaderAction
        {
            Tooltip = GetString("abtesting.selectwinner.tooltip"),
            Text = GetString("abtesting.selectwinner"),
            Inactive = true,
            AlternativeActions = actions,
            Enabled = IsUserAuthorizedToManageTest
        });
    }


    /// <summary>
    /// Adds the "Select as winner" option for each variant of the test.
    /// </summary>
    /// <param name="actions">Header actions list to be filled</param>
    private void AddSelectAsWinnerOptions(List<HeaderAction> actions)
    {
        foreach (var variant in ABVariants)
        {
            actions.Add(new HeaderAction
            {
                Text = String.Format(GetString("abtesting.selectvariantaswinner"), HTMLHelper.HTMLEncode(ResHelper.LocalizeString(variant.ABVariantDisplayName))),
                Tooltip = String.Format(GetString("abtesting.selectvariantaswinner.tooltip"), HTMLHelper.HTMLEncode(ResHelper.LocalizeString(variant.ABVariantDisplayName))),
                CommandName = "selectwinner",
                CommandArgument = variant.ABVariantName,
                OnClientClick = "return confirm('" + String.Format(GetString("abtesting.winnerselectionconfirmation"), ScriptHelper.GetString(ResHelper.LocalizeString(variant.ABVariantDisplayName), false)) + "');",
                Enabled = IsUserAuthorizedToManageTest
            });
        }
    }


    /// <summary>
    /// Adds the "Start test" button.
    /// </summary>
    private void AddStartTestButton()
    {
        string testStartUrl = ResolveUrl("~/CMSModules/OnlineMarketing/Pages/Content/ABTesting/ABTest/StartABTest.aspx?testid=" + ABTest.ABTestID);
        var btnStartTest = new HeaderAction
        {
            Tooltip = GetString("abtesting.starttest.tooltip"),
            Text = GetString("abtesting.starttest"),
            OnClientClick = "modalDialog('" + testStartUrl + @"', '', 670, 320);",
            Enabled = IsUserAuthorizedToManageTest
        };
        HeaderActions.AddAction(btnStartTest);
    }


    /// <summary>
    /// Gets finish date of the test or today if the date is empty.
    /// </summary>
    private DateTime GetFinishDateOrToday()
    {
        if ((ABTest.ABTestOpenTo < DateTime.Now) && (ABTest.ABTestOpenTo != DateTimeHelper.ZERO_TIME))
        {
            return ABTest.ABTestOpenTo;
        }
        return DateTime.Now;
    }


    /// <summary>
    /// Gets selector values from the cookie.
    /// </summary>
    private void ParseSelectorCookie()
    {
        // Get cookie value
        string json = CookieHelper.GetValue(SelectorsCookieKey);

        if (json == null)
        {
            return;
        }

        try
        {
            // Deserialize cookie value
            var selectorsState = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            // Parse cookie value
            mSamplingIndex = ValidationHelper.GetInteger(selectorsState[samplingElem.ID], 0);
            mDrpSuccessMetricIndex = ValidationHelper.GetInteger(selectorsState[drpSuccessMetric.ID], 0);
            mDrpCountingMethodologyIndex = ValidationHelper.GetInteger(selectorsState[drpCountingMethodology.ID], 0);
            mGraphDataIndex = ValidationHelper.GetInteger(selectorsState[graphDataElem.ID], 0);
            mDrpCultureValue = selectorsState[drpCulture.ID];
            mDrpConversionsValue = selectorsState[drpConversions.ID];

            // Load ShowAdvancedFilters content
            mAdvancedcontrolsVisible = ValidationHelper.GetBoolean(selectorsState[AdvancedControls.ID], false);
        }
        catch
        {
            // If something failed, delete the cookie by setting its expiration to the past
            CookieHelper.SetValue(SelectorsCookieKey, string.Empty, DateTime.Now.AddDays(-1));
        }
    }


    /// <summary>
    /// Sets selector values to the values used in the previous page visit.
    /// </summary>
    private void SetSelectorValues()
    {
        samplingElem.SelectedActionName = samplingActions[mSamplingIndex];
        drpSuccessMetric.SelectedIndex = mDrpSuccessMetricIndex;
        drpCountingMethodology.SelectedIndex = mDrpCountingMethodologyIndex;
        graphDataElem.SelectedActionName = dataFormatActions[mGraphDataIndex];
        drpCulture.Value = mDrpCultureValue;
        drpConversions.Value = mDrpConversionsValue;

        // Load ShowAdvancedFilters content
        spnShowAdvancedFilters.InnerText = mAdvancedcontrolsVisible ? GetString(HIDE_FILTERS_TEXT) : GetString(SHOW_FILTERS_TEXT);
        AdvancedControls.Style[HtmlTextWriterStyle.Display] = mAdvancedcontrolsVisible ? "block" : "none";
    }


    /// <summary>
    /// Loads summary box in the right upper corner.
    /// </summary>
    private void LoadSummaryBox()
    {
        lnkTest.HRef = DocumentURLProvider.GetUrl(ABTest.ABTestOriginalPage);
        lnkTest.InnerText = ShortenUrl(ABTest.ABTestOriginalPage, MAX_LINK_LENGTH);
        lnkTest.Target = "_blank";

        // If Visitors conversion methodology selected, use "Visitors" instead of "Visits"
        if (drpCountingMethodology.SelectedValue == "absessionconversionfirst")
        {
            lblVisits.ResourceString = "abtesting.overview.summary.visitors";
        }

        lblStatus.Text = ABTestStatusEvaluator.GetFormattedStatus(TestStatus).ToString();
        int visits = VariantsStatisticsData.Sum(d => d.Value.Visits);
        int conversions = VariantsStatisticsData.Sum(d => d.Value.ConversionsCount);
        lblTotalVisitors.Text = String.Format("{0:N0}", visits);
        lblTotalConversions.Text = String.Format("{0:N0}", conversions);

        if (TestStatus == ABTestStatusEnum.Finished)
        {
            txtDuration.ResourceString = "abtesting.daysrun";
        }

        DateTime start = ABTest.ABTestOpenFrom;
        DateTime finish = GetFinishDateOrToday();
        lblDuration.Text = (finish - start).Days.ToString();
    }


    /// <summary>
    /// Shortens URL, adds dots and rounds URL length to the nearest slash occurrence.
    /// </summary>
    /// <returns>Unchanged URL, if <paramref name="maxLength"/> doesn't exceed length of <paramref name="url"/> or shortened URL in format '../something/something'</returns>
    private string ShortenUrl(string url, int maxLength)
    {
        // No need to shorten
        if (url.Length <= maxLength)
        {
            return url;
        }

        // Get last index that can be kept
        int maxIndex = url.Length - maxLength;

        // Get index of last slash, so that URL will start with that
        var indexOfSlash = url.IndexOf('/', maxIndex);
        if (indexOfSlash != -1)
        {
            return ".." + url.Substring(indexOfSlash);
        }

        // There wasn't any slash found, so return just shortened URL
        return ".." + url.Substring(maxIndex);
    }


    /// <summary>
    /// Ensures statistics data for AB variants.
    /// </summary>
    private void EnsureVariantsStatisticsData()
    {
        if (OriginalVariant == null)
        {
            return;
        }

        // Ensure data for the original variant so that challenger variants can be compared to it
        EnsureVariantStatisticsData(OriginalVariant.ABVariantName);

        foreach (var variant in ABVariants)
        {
            EnsureVariantStatisticsData(variant.ABVariantName);
        }

        // Calculate minimum lower bound and rate range for interval visualization
        if (ABConversionRateIntervals.Count > 0)
        {
            double minConversionRateLowerBound = ABConversionRateIntervals.Values.Min(i => i.ConversionRateLowerBound);
            double maxConversionRateUpperBound = ABConversionRateIntervals.Values.Max(i => i.ConversionRateUpperBound);
            mConversionRateRange = maxConversionRateUpperBound - minConversionRateLowerBound;
            mMinConversionRateLowerBound = minConversionRateLowerBound;
        }
    }


    /// <summary>
    /// Ensures statistics data for specific AB variant.
    /// </summary>
    /// <param name="variantName">Variant name</param>
    private void EnsureVariantStatisticsData(string variantName)
    {
        if (!VariantsStatisticsData.ContainsKey(variantName))
        {
            // Select both abvisitfirst and abvisitreturn by default
            string visitType = "abvisit%";

            // If counting methodology is set to visitor conversion, select abvisitfirst only
            string countingMethodology = drpCountingMethodology.Items[mDrpCountingMethodologyIndex].Value;
            if (countingMethodology == "absessionconversionfirst")
            {
                visitType = "abvisitfirst";
            }

            string conversionsCodename = countingMethodology + ";" + ABTest.ABTestName + ";" + variantName;
            string visitsCodename = visitType + ";" + ABTest.ABTestName + ";" + variantName;

            // Get conversions count and value
            DataRow conversions = GetHits(conversionsCodename, "Sum(HitsCount), Sum(HitsValue)", mDrpCultureValue, GetConversionCondition());
            int conversionsCount = ValidationHelper.GetInteger(conversions[0], 0);
            double conversionsValue = ValidationHelper.GetDouble(conversions[1], 0);

            // Get visits count
            int visits = ValidationHelper.GetInteger(GetHits(visitsCodename, "Sum(HitsCount)", mDrpCultureValue)[0], 0);

            // Add statistics data
            VariantsStatisticsData.Add(variantName, new ABVariantStatisticsData(conversionsCount, conversionsValue, visits));

            // Add conversion rate intervals
            if ((VariantPerformanceCalculator != null) && (visits > 0) && (conversionsCount <= visits))
            {
                ABConversionRateIntervals.Add(variantName, VariantPerformanceCalculator.GetConversionRateInterval(conversionsCount, visits));
            }
        }
    }


    /// <summary>
    /// Checks whether there is test data for filters specified by the user.
    /// </summary>
    private bool DataAvailable()
    {
        bool dataAvailable = false;

        // Search data for all AB variants
        foreach (var data in VariantsStatisticsData.Values)
        {
            // If we have visits we have data for conversion rate
            if (drpSuccessMetric.SelectedValue == "conversionrate")
            {
                if (data.Visits > 0)
                {
                    dataAvailable = true;
                    break;
                }
            }
            // For other success metrics we need conversions
            else if (data.ConversionsCount > 0)
            {
                dataAvailable = true;
                break;
            }
        }
        return dataAvailable;
    }


    /// <summary>
    /// Returns panel with information about improvement.
    /// </summary>
    /// <param name="improvement">Improvement</param>
    private Panel GetPercentageImprovementPanel(double improvement)
    {
        var panel = new Panel();

        if (!improvement.Equals(0))
        {
            // Add picture representing improvement
            string iconClass = (improvement > 0 ? "green-arrow" : "red-arrow");
            string tooltip = (improvement > 0 ? "abtesting.overview.increase" : "abtesting.overview.decrease");

            panel.Controls.Add(new LiteralControl(UIHelper.GetAccessibleIconTag(iconClass, GetString(tooltip))));
        }

        // Add text representing improvement
        panel.Controls.Add(new Label
        {
            Text = String.Format(" {0:P2}", improvement),
        });

        return panel;
    }


    /// <summary>
    /// Returns hits for specified codename.
    /// </summary>
    /// <param name="codename">Statistics codename</param>
    /// <param name="columns">Selected columns</param>
    /// <param name="culture">Culture</param>
    /// <param name="where">Additional where condition</param>
    private DataRow GetHits(string codename, string columns, string culture, string where = null)
    {
        return HitsInfoProvider.GetAllHitsInfo(SiteContext.CurrentSiteID, HitsIntervalEnum.Year, codename, columns, culture, where).Tables[0].Rows[0];
    }


    /// <summary>
    /// Returns where condition which specifies selected conversion.
    /// </summary>
    private string GetConversionCondition()
    {
        string conversion = ValidationHelper.GetString(drpConversions.Value, string.Empty);
        if (!String.IsNullOrEmpty(conversion))
        {
            return "StatisticsObjectName = N'" + SqlHelper.EscapeQuotes(conversion) + "'";
        }
        // User has selected 'all' in conversion selector. Try to select only those conversions, that belong to the test (that are set in settings tab)
        var testConversions = ABTest.ABTestConversions.ToList();
        if (testConversions.Any())
        {
            return SqlHelper.GetWhereCondition("StatisticsObjectName", testConversions);
        }
        return null;
    }


    /// <summary>
    /// Sets caption of the percentage improvement column.
    /// </summary>
    private void SetImprovementColumnCaption()
    {
        string caption = string.Empty;

        // Set improvement caption according to the selected success metric
        switch (drpSuccessMetric.SelectedValue)
        {
            case "conversioncount":
                caption = "$abtesting.improvement.conversioncount$";
                break;

            case "conversionvalue":
                caption = "$abtesting.improvement.conversionvalue$";
                break;

            case "conversionrate":
                caption = "$abtesting.improvement.conversionrate$";
                break;

            case "averageconversionvalue":
                caption = "$abtesting.improvement.averageconversionvalue$";
                break;
        }

        // Set caption to improvement column
        columnImprovement.Caption = caption;
    }


    /// <summary>
    /// Shows warning image on invalid filter combination.
    /// Invalid filter combination is "All conversion goals" and "Conversion rate".
    /// </summary>
    private void ShowInvalidFilterCombinationImage()
    {
        // Do not show on first load
        if (!RequestHelper.IsPostBack())
        {
            return;
        }

        if ((ValidationHelper.GetString(drpConversions.Value, "") == "") && (drpSuccessMetric.SelectedValue == "conversionrate"))
        {
            ShowWarning(GetString("abtesting.invalidfiltercombination"));
        }
    }


    /// <summary>
    /// Initialzes button groups in radio button panel
    /// </summary>
    private void InitRadioPanelButtons()
    {
        // Sampling buttons
        samplingElem.Actions.Add(new CMSButtonGroupAction
        {
            Name = samplingActions[0],
            Text = GetString("general.day"),
            OnClientClick = "ABOverview.saveSelectorStateSamplingClick(this);"
        });

        samplingElem.Actions.Add(new CMSButtonGroupAction
        {
            Name = samplingActions[1],
            Text = GetString("general.week"),
            OnClientClick = "ABOverview.saveSelectorStateSamplingClick(this);"
        });

        samplingElem.Actions.Add(new CMSButtonGroupAction
        {
            Name = samplingActions[2],
            Text = GetString("general.month"),
            OnClientClick = "ABOverview.saveSelectorStateSamplingClick(this);"
        });

        // Graph data buttons
        graphDataElem.Actions.Add(new CMSButtonGroupAction
        {
            Name = dataFormatActions[0],
            Text = GetString("abtesting.rate.cumulative"),
            OnClientClick = "ABOverview.saveSelectorStateGraphDataClick(this);"
        });

        graphDataElem.Actions.Add(new CMSButtonGroupAction
        {
            Name = dataFormatActions[1],
            Text = GetString("abtesting.rate.daywise"),
            OnClientClick = "ABOverview.saveSelectorStateGraphDataClick(this);"
        });
    }


    /// <summary>
    /// Initializes the smart tip with the how to video.
    /// </summary>
    private void InitSmartTip()
    {
        var linkBuilder = new MagnificPopupYouTubeLinkBuilder();
        var linkID = Guid.NewGuid().ToString();
        var link = linkBuilder.GetLink("EGzYxXggueM", linkID, GetString("abtesting.howto.howtoevaluateabtest.link"));

        new MagnificPopupYouTubeJavaScriptRegistrator().RegisterMagnificPopupElement(this, linkID);

        tipHowToOverview.Content = string.Format("{0} {1}", GetString("abtesting.howto.howtoevaluateabtest.overview.text"), link);
        tipHowToOverview.CollapsedStateIdentifier = SMART_TIP_IDENTIFIER;
        tipHowToOverview.ExpandedHeader = GetString("abtesting.howto.howtoevaluateabtest.title");
    }

    #endregion
}