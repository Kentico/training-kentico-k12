using CMS.Activities;
using CMS.Activities.Web.UI;
using CMS.Newsletters;


public partial class CMSModules_Activities_Controls_UI_ActivityDetails_NewsletterClickThrough : ActivityDetail
{
    #region "Methods"

    public override bool LoadData(ActivityInfo ai)
    {
        if ((ai == null) || (ai.ActivityType != PredefinedActivityType.NEWSLETTER_CLICKTHROUGH))
        {
            return false;
        }

        // Get newsletter name
        int newsletterId = ai.ActivityItemID;
        NewsletterInfo newsletterInfo = NewsletterInfoProvider.GetNewsletterInfo(newsletterId);
        if (newsletterInfo != null)
        {
            ucDetails.AddRow("om.activitydetails.newsletter", newsletterInfo.NewsletterDisplayName);
        }

        // Get issue subject
        int issueId = ai.ActivityItemDetailID;
        IssueInfo issueInfo = IssueInfoProvider.GetIssueInfo(issueId);
        if (issueInfo != null)
        {
            ucDetails.AddRow("om.activitydetails.newsletterissue", issueInfo.IssueDisplayName);
        }

        string targetLink = ai.ActivityURL;
        ucDetails.AddRow("om.activitydetails.newstargetlink", GetLink(targetLink, targetLink), false);

        return ucDetails.IsDataLoaded;
    }

    #endregion
}