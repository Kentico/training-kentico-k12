using System;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.UIControls;
using CMS.Newsletters.Web.UI;

public partial class CMSModules_Newsletters_EmailBuilder_EmailABVariants : CMSAdminControl
{
    protected const int VARIANT_NAME_SIZE = 200;


    private IEmailABTestService mEmailABTestService;
    private IssueInfo mOriginalVariant;


    /// <summary>
    /// Gets or sets the current issue.
    /// </summary>
    public IssueInfo Issue
    {
        get;
        set;
    }


    /// <summary>
    /// Indicates whether the control is enabled or is in a read-only mode.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }


    /// <summary>
    /// Returns ID of the parent issue using current <see cref="Issue"/> without querying database.
    /// </summary>
    private int ParentIssueId
    {
        get
        {
            if (Issue != null)
            {
                if (Issue.IssueVariantOfIssueID > 0)
                {
                    return Issue.IssueVariantOfIssueID;
                }

                return Issue.IssueID;
            }

            return 0;
        }
    }


    /// <summary>
    /// Gets the email variant which is considered as 'original'. Original variant is a clone of email which is A/B tested.
    /// </summary>
    private IssueInfo OriginalVariant
    {
        get
        {
            return mOriginalVariant ?? (mOriginalVariant = EmailABTestService.GetOriginalVariant(ParentIssueId));
        }
    }


    /// <summary>
    /// Gets the A/B test service.
    /// </summary>
    private IEmailABTestService EmailABTestService
    {
        get
        {
            if (mEmailABTestService == null)
            {
                mEmailABTestService = Service.Resolve<IEmailABTestService>();
            }

            return mEmailABTestService;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        txtVariantName.MaxLength = VARIANT_NAME_SIZE;

        gridVariants.OrderBy = "IssueVariantName, IssueID";
        gridVariants.WhereCondition = new WhereCondition().WhereEquals("IssueVariantOfIssueID", ParentIssueId).ToString(true);
        gridVariants.OnAction += GridVariants_OnAction;
        gridVariants.OnExternalDataBound += GridVariants_OnExternalDataBound;

        ucTitle.TitleText = GetString("newslettervariant.addvariant");
        btnCloseDialog.OnClientClick = $@"
            hideModalPopup('{variantDialog.ClientID}', '{variantDialog.ClientID}_background');
            return false;";

        if (Enabled && NewsletterHelper.IsABTestingAvailable())
        {
            gridVariants.ZeroRowsText = GetString("emailbuilder.variantlist.novariants");

            if (IsTrackingEnabled())
            {
                btnSaveDialog.Click += btnSaveDialog_Click;

                btnNewVariant.OnClientClick = $@"
                $cmsj('#{errorMessage.ClientID}').hide();
                var $txtVariantName = $cmsj('#{txtVariantName.ClientID}');
                $txtVariantName.val('');

                showModalPopup('{variantDialog.ClientID}', '{variantDialog.ClientID}_background');
                $txtVariantName.focus();
                return false;";
            }
            else
            {
                btnNewVariant.Enabled = false;
                btnNewVariant.ToolTip = GetString("emailbuilder.abtest.trackingdisabled");
            }
        }
        else
        {
            errorMessage.Text = GetString("emailbuilder.generalerror");
            btnNewVariant.Enabled = false;
            btnSaveDialog.Enabled = false;
        }
    }


    private bool IsTrackingEnabled()
    {
        var feed = NewsletterInfoProvider.GetNewsletterInfo(Issue.IssueNewsletterID);
        return (feed != null) ? feed.NewsletterTrackClickedLinks && feed.NewsletterTrackOpenEmails : false;
    }


    /// <summary>
    /// When dialog's OK button for new variant action is clicked.
    /// </summary>
    protected void btnSaveDialog_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrWhiteSpace(txtVariantName.Text))
        {
            errorMessage.Text = GetString("newsletter.variantdisplaynamemissing");
            return;
        }

        try
        {
            string variantName = txtVariantName.Text.Truncate(VARIANT_NAME_SIZE);
            IssueInfo newVariant = EmailABTestService.CreateVariant(variantName, Issue.IssueID);

            string navigateUrl = EmailBuilderHelper.GetNavigationUrl(Issue.IssueNewsletterID, newVariant.IssueID, 2);
            URLHelper.Redirect(navigateUrl);
        }
        catch (InvalidOperationException exception)
        {
            Service.Resolve<IEventLogService>().LogException("Newsletter", "ADDVARIANT", exception);
            errorMessage.Text = GetString("emailbuilder.generalerror");
        }
    }


    private void GridVariants_OnAction(string actionName, object actionArgument)
    {
        if (actionName.Equals("delete", StringComparison.OrdinalIgnoreCase))
        {
            int deletedIssueId = ValidationHelper.GetInteger(actionArgument, 0);
            EmailABTestService.DeleteVariant(deletedIssueId);

            var variants = IssueHelper.GetIssueVariants(ParentIssueId, additionalWhereCondition: null);
            int redirectToIssueId;

            // Handle the situation when deleted variant was the actually displayed variant or entire A/B test was deleted as well.
            if ((variants != null) && variants.Any())
            {
                if (Issue.IssueID != deletedIssueId)
                {
                    // Don't redirect, leave current location unchanged.
                    return;
                }

                // Switch to first available variant
                redirectToIssueId = variants.First().IssueID;
            }
            else
            {
                // Suppose the entire A/B test was removed so switch to the original issue
                redirectToIssueId = ParentIssueId;
            }

            string url = EmailBuilderHelper.GetNavigationUrl(Issue.IssueNewsletterID, redirectToIssueId, 2);
            URLHelper.Redirect(url);
        }
    }


    private object GridVariants_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        if (sourceName.Equals("variantName", StringComparison.OrdinalIgnoreCase))
        {
            var drv = UniGridFunctions.GetDataRowView(sender as DataControlFieldCell);
            var issue = new IssueInfo(drv.Row);

            string navigateUrl = EmailBuilderHelper.GetNavigationUrl(Issue.IssueNewsletterID, issue.IssueID, 2);

            return new HyperLink
            {
                NavigateUrl = navigateUrl,
                Text = HTMLHelper.HTMLEncode(issue.GetVariantName())
            };
        }

        if (sourceName.Equals("delete", StringComparison.OrdinalIgnoreCase))
        {
            var dr = UniGridFunctions.GetDataRowView(parameter);
            int issueId = Convert.ToInt32(dr["IssueID"]);
            var deleteButton = (CMSGridActionButton)sender;
            deleteButton.Enabled = (OriginalVariant.IssueID != issueId) && Enabled;
        }

        return parameter;
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        ScriptHelper.RegisterJQueryDialog(Page);
    }
}
