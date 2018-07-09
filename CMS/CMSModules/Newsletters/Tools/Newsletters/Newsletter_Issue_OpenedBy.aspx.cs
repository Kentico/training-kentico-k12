using System;

using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.SiteProvider;
using CMS.UIControls;


/// <summary>
/// Displays a table of issue openers (subscribers who have opened the email with the specified issue).
/// </summary>
[UIElement(ModuleName.NEWSLETTER, "Newsletter.Issue.Reports.Opens")]
[EditedObject(IssueInfo.OBJECT_TYPE, "objectid")]
public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_OpenedBy : CMSDeskPage
{
    #region "Variables"

    // Default page size 15
    private const int PAGESIZE = 15;
    private bool isMainABTestIssue;
    private int issueID;
    private IssueInfo winnerIssue;

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.TitleText = GetString("newsletter_issue_openedby.title");

        var issue = (IssueInfo)EditedObject;
        issueID = issue.IssueID;

        // Do not show page if reports are not available
        if (issue.IssueSentEmails <= 0)
        {
            ShowInformation(GetString("newsletter.issue.overviewnotsentyet"));
            UniGrid.Visible = false;
            fltOpenedBy.Visible = false;
            return;
        }

        // Prevent accessing issues from sites other than current site
        if (issue.IssueSiteID != SiteContext.CurrentSiteID)
        {
            RedirectToResourceNotAvailableOnSite("Issue with ID " + issueID);
        }

        // Issue is the main A/B test issue
        isMainABTestIssue = issue.IssueIsABTest && !issue.IssueIsVariant;
        if (isMainABTestIssue)
        {
            // Initialize variant selector in the filter
            fltOpenedBy.IssueId = issue.IssueID;

            if (RequestHelper.IsPostBack())
            {
                // Get issue ID from variant selector
                issueID = fltOpenedBy.IssueId;
            }

            // Reset ID for main issue, grid will show data from main and winner variant issues
            if (issueID == issue.IssueID)
            {
                issueID = 0;
            }
        }

        var whereCondition = new WhereCondition(fltOpenedBy.WhereCondition);

        if (issueID > 0)
        {
            whereCondition.And(w => w.WhereEquals("OpenedEmailIssueID", issueID));
        }

        UniGrid.WhereCondition = whereCondition.ToString(true);
        UniGrid.Pager.DefaultPageSize = PAGESIZE;
        UniGrid.Pager.ShowPageSize = false;
        UniGrid.FilterLimit = 1;
        UniGrid.OnExternalDataBound += UniGrid_OnExternalDataBound;
        UniGrid.OnBeforeDataReload += UniGrid_OnBeforeDataReload;

        UniGrid.ZeroRowsText = GetString("newsletter.issue.noopenmails");
    }

    #endregion


    #region "Other methods"

    protected void UniGrid_OnBeforeDataReload()
    {
        // Display 'Variant name' column if data from all variants should be retrieved
        UniGrid.NamedColumns["variants"].Visible = isMainABTestIssue && (issueID < 0);
    }


    protected object UniGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName)
        {
            case "variantname":
                if (!isMainABTestIssue)
                {
                    return null;
                }

                IssueInfo issue = IssueInfoProvider.GetIssueInfo(ValidationHelper.GetInteger(parameter, 0));
                string variantName = null;

                if (issue != null)
                {
                    if (!issue.IssueIsVariant)
                    {
                        // Get variant name from the winner issue
                        if (winnerIssue == null)
                        {
                            ABTestInfo test = ABTestInfoProvider.GetABTestInfoForIssue(issue.IssueID);
                            if (test != null)
                            {
                                // Get A/B test winner variant
                                winnerIssue = IssueInfoProvider.GetIssueInfo(test.TestWinnerIssueID);
                            }
                        }

                        if (winnerIssue != null)
                        {
                            // Get variant name
                            variantName = winnerIssue.GetVariantName();
                        }
                    }
                    else
                    {
                        // Get variant name
                        variantName = issue.GetVariantName();
                    }
                }

                return HTMLHelper.HTMLEncode(variantName);

            default:
                return parameter;
        }
    }

    #endregion
}