using System;
using System.Collections.Generic;
using System.Data;

using CMS.Base;

using System.Linq;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.OnlineMarketing;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;

using GridAction = CMS.UIControls.UniGridConfig.Action;

public partial class CMSModules_OnlineMarketing_Controls_UI_AbTest_List : CMSAdminListControl
{
    #region "Variables"

    private string mAliasPath = String.Empty;
    private bool mShowOriginalPageColumn = true;

    #endregion


    #region "Properties"

    /// <summary>
    /// Inner grid.
    /// </summary>
    public UniGrid Grid
    {
        get
        {
            return gridElem;
        }
    }


    /// <summary>
    /// Alias path of document to which this abtest belongs.
    /// </summary>
    public string AliasPath
    {
        get
        {
            return mAliasPath;
        }
        set
        {
            mAliasPath = value;
        }
    }


    /// <summary>
    /// Indicates if the control should perform the operations.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return base.StopProcessing;
        }
        set
        {
            base.StopProcessing = value;
            gridElem.StopProcessing = value;
        }
    }


    /// <summary>
    /// Indicates if the control is used on the live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            base.IsLiveSite = value;
            gridElem.IsLiveSite = value;
        }
    }


    /// <summary>
    /// NodeID.
    /// </summary>
    public int NodeID
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates if the column with original page should be displayed.
    /// </summary>
    public bool ShowOriginalPageColumn
    {
        get
        {
            return mShowOriginalPageColumn;
        }
        set
        {
            mShowOriginalPageColumn = value;
        }
    }

    #endregion


    #region "Events"

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadData();

        if (!mShowOriginalPageColumn)
        {
            HideGridColumns(new[]
            {
                "ABTestOriginalPage"
            });
        }

        // Set nice 'No data' message (message differs based on whether the user is in content tree or on on-line marketing tab
        gridElem.ZeroRowsText = GetString(NodeID > 0 ? "abtesting.abtest.nodataondocument" : "abtesting.abtest.nodata");
        
        string url = UIContextHelper.GetElementUrl("CMS.ABTest", "Detail", gridElem.EditInDialog);

        url = URLHelper.AddParameterToUrl(url, "objectid", "{0}");
        url = URLHelper.AddParameterToUrl(url, "aliasPath", AliasPath);

        if (NodeID > 0)
        {
            url = URLHelper.AddParameterToUrl(url, "NodeID", NodeID.ToString());
        }

        gridElem.EditActionUrl = url;
    }


    /// <summary>
    /// Handles Unigrid's OnAction event.
    /// </summary>
    protected void gridElem_OnOnAction(string actionname, object actionargument)
    {
        string argument = actionargument.ToString();

        switch (actionname)
        {
            case "delete":
                int testId = ValidationHelper.GetInteger(argument, 0);
                if (testId > 0)
                {
                    ABTestInfoProvider.DeleteABTestInfo(testId);
                    LoadData();
                }
                break;
        }
    }


    /// <summary>
    /// Handles Unigrid's OnExternalDataBound event.
    /// </summary>
    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        string param = parameter.ToString();

        switch (sourceName.ToLowerCSafe())
        {
            case "status":
                {
                    ABTestStatusEnum status;
                    if (Enum.TryParse(param, out status))
                    {
                        return ABTestStatusEvaluator.GetFormattedStatus(status);
                    }
                    break;
                }

            case "page":
                return new HyperLink
                {
                    NavigateUrl = DocumentURLProvider.GetUrl(param),
                    Text = HTMLHelper.HTMLEncode(param),
                    Target = "_blank"
                };

            case "visitors":
            case "conversions":
                {
                    string statisticsCodeName = (sourceName.ToLowerCSafe() == "visitors" ? "abvisitfirst" : "absessionconversionfirst");

                    ABTestInfo abTestInfo = ABTestInfoProvider.GetABTestInfo(param, SiteContext.CurrentSiteName);
                    if (abTestInfo != null)
                    {
                        return ValidationHelper.GetInteger(HitsInfoProvider.GetAllHitsInfo(SiteContext.CurrentSiteID, HitsIntervalEnum.Year, statisticsCodeName + ";" + abTestInfo.ABTestName + ";%", "SUM(HitsCount)", abTestInfo.ABTestCulture).Tables[0].Rows[0][0], 0);
                    }

                    return 0;
                }
        }

        return null;
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Loads data into Unigrid.
    /// </summary>
    private void LoadData()
    {
        var whereCondition = new WhereCondition();
        if (!String.IsNullOrEmpty(AliasPath))
        {
            whereCondition.WhereEquals("ABTestOriginalPage", AliasPath);
        }

        DataSet abTests = ABTestInfoProvider.GetABTests().OnSite(SiteContext.CurrentSiteID).Where(whereCondition);
        abTests.Tables[0].Columns.Add("ABTestStatus", typeof(int));

        foreach (DataRow abTestDataRow in abTests.Tables[0].Rows)
        {
            abTestDataRow["ABTestStatus"] = (int)ABTestStatusEvaluator.GetStatus(new ABTestInfo(abTestDataRow));
        }

        gridElem.DataSource = abTests;
    }


    /// <summary>
    /// Hides any grid column.
    /// </summary>
    /// <param name="columnSourceNames">Column source name</param>
    private void HideGridColumns(IEnumerable<string> columnSourceNames)
    {
        if ((gridElem.GridColumns != null) && (columnSourceNames != null) && (columnSourceNames.Any()))
        {
            var columns = gridElem.GridColumns.Columns.Where(t => columnSourceNames.Contains(t.Source));

            foreach (var column in columns)
            {
                column.Visible = false;
            }
        }
    }

    #endregion
}