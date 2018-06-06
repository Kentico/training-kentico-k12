using System;
using System.Globalization;
using System.Linq;
using System.Text;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Newsletters;
using CMS.Newsletters.Internal;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;

public partial class CMSModules_Newsletters_EmailBuilder_EmailProperties : CMSAdminControl
{
    #region "Variables"

    private int mTemplateID;
    private const string DEFAULT_UTM_MEDIUM = "email";
    private NewsletterInfo mNewsletter;
    private bool mAreCampaignsAvailable;
    private CMSTextBox mUTMCampaignTextBox;

    #endregion


    #region "Properties"

    /// <summary>
    /// ID of newsletter issue that should be edited, required when editing an issue.
    /// </summary>
    public int IssueID
    {
        get;
        set;
    }


    /// <summary>
    /// Newsletter ID, required when creating new issue.
    /// </summary>
    public int NewsletterID
    {
        get;
        set;
    }


    /// <summary>
    /// ID of newsletter template that should be used for new issue.
    /// If not set, template from newsletter configuration is used.
    /// </summary>
    private int TemplateID
    {
        get
        {
            if (mTemplateID == 0)
            {
                // Try to get value from selector
                mTemplateID = ValidationHelper.GetInteger(issueTemplate.Value, 0);
            }

            return mTemplateID;
        }
        set
        {
            mTemplateID = value;
        }
    }


    /// <summary>
    /// Gets or sets value that indicates whether control is enabled.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }


    /// <summary>
    /// Gets the newsletter.
    /// </summary>
    private NewsletterInfo Newsletter
    {
        get
        {
            return mNewsletter ?? (mNewsletter = NewsletterInfoProvider.GetNewsletterInfo(NewsletterID));
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Email properties save event.
    /// </summary>
    public event EventHandler<EmailBuilderEventArgs> Save;

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing || ((NewsletterID <= 0) && (IssueID <= 0)))
        {
            return;
        }

        mAreCampaignsAvailable = LicenseKeyInfoProvider.IsFeatureAvailable(RequestContext.CurrentDomain, FeatureEnum.CampaignAndConversions);
        ToggleUTMCampaignInput();
        mUTMCampaignTextBox = GetUTMCampaignTextBox();

        // Add shadow below header actions
        ScriptHelper.RegisterModule(Page, "CMS/HeaderShadow");

        LoadForm();
    }


    /// <summary>
    /// Shows or hides UTM parameters settings.
    /// </summary>
    protected void chkIssueUseUTM_CheckedChanged(object sender, EventArgs e)
    {
        pnlUTMParameters.Visible = chkIssueUseUTM.Checked;
    }

    #endregion


    #region "Protected methods"

    /// <summary>
    /// Radio buttons for UTM campaign checked changed.
    /// </summary>
    protected void radUTMCampaign_OnCheckedChanged(object sender, EventArgs e)
    {
        txtIssueUTMCampaign.Enabled = radUTMCampaignNew.Checked;
        selectorUTMCampaign.Enabled = radUTMCampaignExisting.Checked;
    }


    /// <summary>
    /// Creates new or updates existing newsletter issue.
    /// </summary>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsValid())
        {
            IssueInfo issue = IssueInfoProvider.GetIssueInfo(IssueID);
            if ((issue != null) && Enabled)
            {
                if (IsDisplayNameFieldEnabled(issue))
                {
                    issue.IssueDisplayName = txtDisplayName.Text.Trim();
                }

                issue.IssueTemplateID = TemplateID;
                issue.IssueSenderName = txtSenderName.Text.Trim();
                issue.IssueSenderEmail = txtSenderEmail.Text.Trim();
                issue.IssuePreheader = txtPreheader.Text.Trim();
                issue.IssueUseUTM = chkIssueUseUTM.Checked;
                issue.IssueUTMSource = Normalize(txtIssueUTMSource.Text.Trim());

                if (radUTMCampaignNew.Checked)
                {
                    var normalizedUtmCampaign = Normalize(mUTMCampaignTextBox.Text.Trim());
                    if (string.IsNullOrEmpty(normalizedUtmCampaign))
                    {
                        normalizedUtmCampaign = Normalize(Newsletter.NewsletterName);
                    }
                    mUTMCampaignTextBox.Text = issue.IssueUTMCampaign = normalizedUtmCampaign;
                }
                else
                {
                    issue.IssueUTMCampaign = selectorUTMCampaign.Value.ToString().ToLower(CultureInfo.CurrentCulture);
                }

                PostProcessVariants(issue);

                // Remove '#' from macros if included
                txtSubject.Text = txtSubject.Text.Trim().Replace("#%}", "%}");

                // Sign macros if included in the subject
                issue.IssueSubject = MacroSecurityProcessor.AddSecurityParameters(txtSubject.Text, MacroIdentityOption.FromUserInfo(MembershipContext.AuthenticatedUser), null);

                // Save issue
                IssueInfoProvider.SetIssueInfo(issue);

                // Update IssueID
                IssueID = issue.IssueID;

                OnSave(new EmailBuilderEventArgs(NewsletterID, IssueID));
            }
        }
    }


    private void PostProcessVariants(IssueInfo issue)
    {
        if (issue.IssueIsABTest)
        {
            // Select variants including the parent issue
            var issues = IssueInfoProvider.GetIssues()
                                .WhereNotEquals("IssueID", issue.IssueID)
                                .And(w => w.WhereEquals("IssueVariantOfIssueID", issue.IssueVariantOfIssueID)
                                           .Or()
                                           .WhereEquals("IssueID", issue.IssueVariantOfIssueID))
                                .ToList();

            foreach (var variant in issues)
            {
                // Synchronize issue data between all A/B test variants (including parent issue).

                variant.IssueDisplayName = issue.IssueDisplayName;

                // UTM parameters
                variant.IssueUTMCampaign = issue.IssueUTMCampaign;
                variant.IssueUTMSource = issue.IssueUTMSource;
                variant.IssueUseUTM = issue.IssueUseUTM;

                IssueInfoProvider.SetIssueInfo(variant);
            }
        }
    }


    protected virtual void OnSave(EmailBuilderEventArgs eventArgs)
    {
        Save?.Invoke(this, eventArgs);
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Loads form data.
    /// </summary>
    private void LoadForm()
    {
        var isABTest = false;
        IssueInfo issue = null;
        if (IssueID > 0)
        {
            // Get issue object
            issue = IssueInfoProvider.GetIssueInfo(IssueID);
            if (issue != null)
            {
                if (NewsletterID == 0)
                {
                    // Set newsletter ID
                    NewsletterID = issue.IssueNewsletterID;
                }

                if (!RequestHelper.IsPostBack())
                {
                    txtDisplayName.Text = issue.IssueDisplayName;
                    txtSubject.Text = issue.IssueSubject;
                    chkIssueUseUTM.Checked = issue.IssueUseUTM;
                }

                isABTest = issue.IssueIsABTest;
            }
        }

        // Get newsletter object
        if (Newsletter != null)
        {
            issueTemplate.WhereCondition = GetAvailableEmailTemplatesWhere(issue);

            if (TemplateID > 0)
            {
                // Set selected value
                issueTemplate.Value = TemplateID;
            }

            if ((TemplateID <= 0) && (issue != null) && (issue.IssueTemplateID != TemplateID))
            {
                // Change selected value
                issueTemplate.Value = TemplateID = issue.IssueTemplateID;
                issueTemplate.Reload(false);
            }

            // Prevent selecting none value in campaign selector if there is no campaign
            if (mAreCampaignsAvailable && CampaignInfoProvider.GetCampaigns().OnSite(SiteContext.CurrentSiteID).Count == 0)
            {
                radUTMCampaignExisting.Checked = false;
                radUTMCampaignExisting.Enabled = false;
                selectorUTMCampaign.Enabled = false;
                radUTMCampaignNew.Checked = true;
                mUTMCampaignTextBox.Enabled = true;
            }

            // Initialize inputs and content controls
            if (!RequestHelper.IsPostBack())
            {
                txtSenderName.Text = issue != null ? issue.IssueSenderName : Newsletter.NewsletterSenderName;
                txtSenderEmail.Text = issue != null ? issue.IssueSenderEmail : Newsletter.NewsletterSenderEmail;
                txtIssueUTMSource.Text = issue != null ? issue.IssueUTMSource : string.Empty;
                txtPreheader.Text = issue != null ? issue.IssuePreheader : string.Empty;

                if (issue != null)
                {
                    if (mAreCampaignsAvailable && (CampaignInfoProvider.GetCampaignByUTMCode(issue.IssueUTMCampaign, SiteContext.CurrentSiteName) != null))
                    {
                        selectorUTMCampaign.Value = issue.IssueUTMCampaign;
                        selectorUTMCampaign.Reload(false);
                        selectorUTMCampaign.Enabled = true;

                        radUTMCampaignExisting.Checked = true;
                        radUTMCampaignNew.Checked = false;
                        mUTMCampaignTextBox.Enabled = false;
                    }
                    else
                    {
                        mUTMCampaignTextBox.Text = issue.IssueUTMCampaign;
                        mUTMCampaignTextBox.Enabled = true;

                        radUTMCampaignExisting.Checked = false;
                        radUTMCampaignNew.Checked = true;
                        selectorUTMCampaign.Enabled = false;
                    }
                }
            }
            else
            {
                if (issue != null && !pnlUTMParameters.Visible)
                {
                    if (string.IsNullOrEmpty(txtIssueUTMSource.Text.Trim()))
                    {
                        txtIssueUTMSource.Text = Normalize(Newsletter.NewsletterName + "_" + txtSubject.Text.Trim());
                    }

                    if (string.IsNullOrEmpty(mUTMCampaignTextBox.Text.Trim()))
                    {
                        mUTMCampaignTextBox.Text = Newsletter.NewsletterName.ToLower(CultureInfo.CurrentCulture);
                    }
                }
            }

            mUTMCampaignTextBox.Attributes["placeholder"] = Newsletter.NewsletterName.ToLower(CultureInfo.CurrentCulture);
        }

        txtIssueUTMMedium.Text = DEFAULT_UTM_MEDIUM;
        btnSubmit.Enabled = Enabled;
        txtDisplayName.Enabled = Enabled && IsDisplayNameFieldEnabled(issue);
        txtPreheader.Enabled = Enabled;
        chkIssueUseUTM.Enabled = pnlIssueUTMCampaign.Enabled = pnlIssueUTMMedium.Enabled = pnlIssueUTMSource.Enabled = Enabled;
        txtSubject.Enabled = txtSenderEmail.Enabled = txtSenderName.Enabled = issueTemplate.Enabled = Enabled;
        pnlUTMParameters.Visible = chkIssueUseUTM.Checked;

        InitTooltips(isABTest);
    }


    /// <summary>
    /// Indicates whether display name field should be enabled.
    /// </summary>
    /// <remarks>If the issue is an A/B test, the display name should be editable from original variant only.</remarks>
    /// <param name="issue">Issue for which the state should be returned.</param>
    private bool IsDisplayNameFieldEnabled(IssueInfo issue)
    {
        // Disable email name for email variants which are not the original variant
        return !issue.IssueIsABTest || issue.IsOriginalVariant();
    }


    private void InitTooltips(bool isABTest)
    {
        pnlIssueUTMSource.ToolTip = lblIssueUTMSource.ToolTip = lblScreenReaderIssueUTMSource.Text = iconHelpIssueUTMSource.ToolTip = GetString("newsletterissue.utm.source.description");
        pnlIssueUTMMedium.ToolTip = lblIssueUTMMedium.ToolTip = lblScreenReaderIssueUTMMedium.Text = iconHelpIssueUTMMedium.ToolTip = GetString("newsletterissue.utm.medium.description");
        pnlIssueUTMCampaign.ToolTip = lblIssueUTMCampaign.ToolTip = GetString("newsletterissue.utm.campaign.description");
        lblDisplayName.ToolTip = GetString("newsletterissue.displayname.description");
        lblSubject.ToolTip = GetString("newsletterissue.subject.description");
        pnlIssueSenderName.ToolTip = lblSenderName.ToolTip = GetString("newsletterissue.sender.name.description");
        pnlIssueSenderEmail.ToolTip = lblSenderEmail.ToolTip = GetString("newsletterissue.sender.email.description");
        pnlIssuePreheader.ToolTip = lblPreheader.ToolTip = GetString("newsletterissue.preheader.description");
        pnlIssueTemplate.ToolTip = lblTemplate.ToolTip = GetString("newsletterissue.template.description");

        var useUTMTooltipText = GetString("newsletterissue.utm.use.description") + (isABTest ? " " + GetString("newsletterissue.utm.use.description.ab") : "");
        pnlIssueUseUTM.ToolTip = lblIssueUseUTM.ToolTip = lblScreenReaderIssueUseUTM.Text = iconHelpIssueUseUTM.ToolTip = useUTMTooltipText;

        InitPreheaderTooltip();

        ScriptHelper.RegisterBootstrapTooltip(Page, ".info-icon > i");
    }


    private void InitPreheaderTooltip()
    {
        var template = EmailTemplateInfoProvider.GetEmailTemplateInfo(TemplateID);
        if (!template.ContainsPreheaderMacro())
        {
            iconHelpPreheader.Visible = lblScreenReaderPreheader.Visible = true;
            iconHelpPreheader.ToolTip = lblScreenReaderPreheader.Text = GetString("newsletterissue.preheader.tooltip");
        }
    }


    /// <summary>
    /// Returns string that can be later used in URL and it is safe for the analytics.
    /// </summary>
    private string Normalize(string input)
    {
        input = input.Replace(' ', '_');
        return ValidationHelper.GetCodeName(input, "", 200, useUnicode: false).ToLower(CultureInfo.CurrentCulture);
    }


    /// <summary>
    /// Validates the input condition <paramref name="isValid"/> and appends
    /// the <paramref name="validationErrorMessage"/> to the <paramref name="stringBuilder"/>
    /// if the input condition is <c>false</c>.
    /// </summary>
    /// <param name="isValid">Input condition to validate.</param>
    /// <param name="validationErrorMessage">Error message used when input is not valid.</param>
    /// <returns>True if the input condition is valid, false otherwise.</returns>
    private bool ValidateInput(bool isValid, string validationErrorMessage, StringBuilder stringBuilder)
    {
        if (!isValid)
        {
            if (stringBuilder.Length != 0)
            {
                stringBuilder.AppendLine("<br>");
            }
            stringBuilder.AppendLine(validationErrorMessage);
        }

        return !isValid;
    }


    /// <summary>
    /// Toggles form control for selecting the UTM campaign code of the issue.
    /// </summary>
    /// <remarks>
    /// If campaigns are available for the current license, advanced control for selecting from existing campaigns is be used.
    /// Otherwise, simple textbox is displayed.
    /// </remarks>
    private void ToggleUTMCampaignInput()
    {
        pnlIssueUTMCampaignTextBox.Visible = !mAreCampaignsAvailable;
        pnlIssueUTMCampaign.Visible = mAreCampaignsAvailable;
    }


    /// <summary>
    /// Gets reference to the textbox specifying the issue UTM campaign code. 
    /// </summary>
    /// <remarks>
    /// There are two candidates for the textbox depending on whether campaigns are available for the current license or not.
    /// </remarks>
    /// <returns>Reference to the appropriate textbox</returns>
    private CMSTextBox GetUTMCampaignTextBox()
    {
        if (mAreCampaignsAvailable)
        {
            return txtIssueUTMCampaign;
        }

        return txtIssueUTMCampaignTextBox;
    }


    private bool IsValid()
    {
        var stringBuilder = new StringBuilder();
        string emailName = txtDisplayName.Text.Trim();

        if (!ValidateInput(!String.IsNullOrEmpty(emailName), GetString("NewsletterContentEditor.DisplayNameRequired"), stringBuilder))
        {
            ValidateInput(emailName.Length <= 200, GetString("Newsletter_Edit.ErrorEmailDisplayNameLength"), stringBuilder);
        }
        ValidateInput(!String.IsNullOrEmpty(txtSubject.Text.Trim()), GetString("NewsletterContentEditor.SubjectRequired"), stringBuilder);
        ValidateInput(txtSenderEmail.IsValid(), GetString("Newsletter_Edit.ErrorEmailFormat"), stringBuilder);

        if (chkIssueUseUTM.Checked)
        {
            ValidateInput(!String.IsNullOrEmpty(txtIssueUTMSource.Text.Trim()), GetString("NewsletterContentEditor.UTMSourceRequired"), stringBuilder);
        }

        alErroMsg.Text = stringBuilder.ToString();

        return String.IsNullOrEmpty(alErroMsg.Text);
    }


    private string GetAvailableEmailTemplatesWhere(IssueInfo issue)
    {
        return new WhereCondition().WhereEquals("TemplateType", EmailTemplateTypeEnum.Issue.ToStringRepresentation())
                                   .Where(GetAssignedTemplatesWhere(issue))
                                   .ToString(true);
    }


    private WhereCondition GetAssignedTemplatesWhere(IssueInfo issue)
    {
        var templatesCondition = EmailTemplateNewsletterInfoProvider.GetEmailTemplateNewsletters()
            .Column("TemplateID")
            .WhereEquals("NewsletterID", NewsletterID);

        var assignedTemplatesWhere = new WhereCondition().WhereIn("TemplateID", templatesCondition);

        //Add template assigned to issue
        if (issue != null)
        {
            assignedTemplatesWhere = AddIssueAssignedTemplate(assignedTemplatesWhere, issue.IssueID);
        }

        return assignedTemplatesWhere;
    }

    private WhereCondition AddIssueAssignedTemplate(WhereCondition assignedTemplatesWhere, int issueID)
    {
        var issueTemplateCondition = IssueInfoProvider.GetIssues()
            .Column("IssueTemplateID")
            .WhereEquals("IssueID", issueID);

        return assignedTemplatesWhere.Or().WhereIn("TemplateID", issueTemplateCondition);
    }

    #endregion
}