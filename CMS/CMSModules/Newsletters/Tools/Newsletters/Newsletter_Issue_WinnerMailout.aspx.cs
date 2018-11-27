using System;

using CMS.Base.Web.UI;
using CMS.Core;

using CMS.Helpers;
using CMS.Newsletters;
using CMS.Newsletters.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;


[EditedObject("newsletter.issue", "objectid")]
[UIElement("CMS.Newsletter", "Newsletters")]
public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_WinnerMailout : CMSNewsletterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.TitleText = GetString("newsletter_winnermailout.title");
        PageTitle.ShowFullScreenButton = false;

        RegisterModalPageScripts();
        RegisterEscScript();
        // Register script for refreshing parent page
        ScriptHelper.RegisterStartupScript(Page, GetType(), "RefreshParent", "function RefreshPage() {if((wopener)&&(wopener.RefreshPage!=null)){wopener.RefreshPage();}}", true);

        btnSend.Click += btnSend_Click;

        // Show variant name in info message
        string variantName = GetString("general.na");
        IssueInfo winner = (IssueInfo)EditedObject;

        if (winner != null)
        {
            // Prevent accessing issues from sites other than current site
            if (winner.IssueSiteID != SiteContext.CurrentSiteID)
            {
                RedirectToResourceNotAvailableOnSite("Issue with ID " + winner.IssueID);
            }

            variantName = HTMLHelper.HTMLEncode(winner.GetVariantName());
        }
        lblInfo.Text = String.Format(GetString("newsletter_winnermailout.question"), variantName);
    }


    protected void btnSend_Click(object sender, EventArgs e)
    {
        // Validate date/time
        if (dtpMailout.SelectedDateTime == DateTimeHelper.ZERO_TIME)
        {
            ShowError(GetString("newsletterissue_send.invaliddatetime"));
            return;
        }

        IssueInfo winner = (IssueInfo)EditedObject;
        int parentIssueId = winner.IssueVariantOfIssueID;

        // Get A/B test info
        ABTestInfo abi = ABTestInfoProvider.GetABTestInfoForIssue(parentIssueId);
        if (abi != null)
        {
            IssueInfo issue = IssueInfoProvider.GetIssueInfo(parentIssueId);

            // Check if winner was selected and sent
            if (abi.TestWinnerIssueID != 0)
            {
                if ((issue.IssueStatus == IssueStatusEnum.Finished) || (issue.IssueStatus == IssueStatusEnum.Sending))
                {
                    // Winner was already sent
                    CloseDialogAndRefreshParent();
                    return;
                }
            }

            // Update A/B test info and winner selection task (if exist)
            abi.TestWinnerOption = ABTestWinnerSelectionEnum.Manual;
            NewsletterTasksManager.EnsureWinnerSelectionTask(abi, issue, false, DateTime.Now);

            abi.TestSelectWinnerAfter = 0;
            abi.TestWinnerSelected = DateTime.Now;
            abi.TestWinnerIssueID = winner.IssueID;
            ABTestInfoProvider.SetABTestInfo(abi);

            if ((issue != null) && (winner != null))
            {
                var parentIssue = IssueInfoProvider.GetIssueInfo(parentIssueId);
                NewsletterSendingStatusModifier.ResetAllEmailsInQueueForIssue(parentIssue.IssueID);

                // Copy data from winner to parent
                IssueHelper.CopyWinningVariantIssueProperties(winner, issue);
                IssueInfoProvider.SetIssueInfo(issue);

                // Remove previous scheduled task of this issue
                NewsletterTasksManager.DeleteMailoutTask(issue.IssueGUID, issue.IssueSiteID);

                DateTime mailoutTime = dtpMailout.SelectedDateTime;
                Service.Resolve<IIssueScheduler>().ScheduleIssue(parentIssue, mailoutTime);
            }
        }

        // Close dialog and refresh parent page
        CloseDialogAndRefreshParent();
    }


    private void CloseDialogAndRefreshParent()
    {
        ScriptHelper.RegisterStartupScript(this, GetType(), "ClosePage", "RefreshPage(); setTimeout('CloseDialog()',200);", true);
    }    
}
