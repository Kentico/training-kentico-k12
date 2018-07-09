using System;
using System.Linq;

using CMS.Core;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.SiteProvider;
using CMS.UIControls;


/// <summary>
/// Displays a table of clicked links.
/// </summary>
[UIElement(ModuleName.NEWSLETTER, "Newsletter.Issue.Reports.Clicks")]
public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_TrackedLinks : CMSDeskPage
{
    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.TitleText = GetString("newsletter.issue.clicks");
        var issueId = QueryHelper.GetInteger("objectid", 0);
        issueLinks.IssueID = issueId;
        issueLinks.NoDataText = GetString("newsletter.issue.noclicks");

        IssueInfo issue = IssueInfoProvider.GetIssueInfo(issueId);
        EditedObject = issue;

        if (issue.IssueSentEmails <= 0)
        {
            ShowInformation(GetString("newsletter.issue.overviewnotsentyet"));
            pnlContent.Visible = false;
            return;
        }

        // Prevent accessing issues from sites other than current site
        if (issue.IssueSiteID != SiteContext.CurrentSiteID)
        {
            RedirectToResourceNotAvailableOnSite("Issue with ID " + issueId);
        }
    }

    #endregion
}