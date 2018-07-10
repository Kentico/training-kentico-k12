using System;
using System.Linq;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Core;
using CMS.DataEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Modules;
using CMS.Newsletters;
using CMS.Newsletters.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;

using MessageTypeEnum = CMS.Base.Web.UI.MessageTypeEnum;


[Security(ModuleName.NEWSLETTER, "AuthorIssues", "Newsletters;Newsletter;EditNewsletterProperties;Newsletter.Issues;NewIssue")]
public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_New : CMSNewsletterPage
{
    /// <summary>
    /// Messages placeholder.
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder => plcMessages;


    protected void Page_Load(object sender, EventArgs e)
    {
        InitializeHeaderActions();
        InitilalizeTemplateSelector();
    }


    private void InitializeHeaderActions()
    {
        CurrentMaster.HeaderActions.AddAction(
            new SaveAction
            {
                Text = GetString("general.create")
            });

        CurrentMaster.HeaderActions.ActionPerformed += HeaderActions_ActionPerformed;
    }


    private void InitilalizeTemplateSelector()
    {
        InitializeTemplateSelectorDataSource();
    }


    private void InitializeTemplateSelectorDataSource()
    {
        var templates = LoadAvailableEmailTemplates();

        ucTemplateSelector.DataSource = templates;

        EnsureFirstTemplateSelection(templates.FirstOrDefault());

        ucTemplateSelector.DataBind();
    }


    private void EnsureFirstTemplateSelection(EmailTemplateInfo template)
    {
        if (ucTemplateSelector.SelectedId.Equals(0))
        {
            ucTemplateSelector.SelectedId = template?.TemplateID ?? 0;
        }
    }


    private static ObjectQuery<EmailTemplateInfo> LoadAvailableEmailTemplates()
    {
        return EmailTemplateInfoProvider.GetEmailTemplates()
                                        .Columns("TemplateID", "TemplateDisplayName", "TemplateDescription", "TemplateThumbnailGUID", "TemplateIconClass")
                                        .WhereEquals("TemplateType", EmailTemplateTypeEnum.Issue.ToStringRepresentation())
                                        .Where(GetAssignedTemplatesWhere());
    }


    private static WhereCondition GetAssignedTemplatesWhere()
    {
        var newsletterId = QueryHelper.GetInteger("parentobjectid", 0);
        var newsletterTemplates = EmailTemplateNewsletterInfoProvider.GetEmailTemplateNewsletters()
            .Columns("TemplateID")
            .WhereEquals("NewsletterID", newsletterId);

        return new WhereCondition().WhereIn("TemplateID", newsletterTemplates);
    }


    /// <summary>
    /// Actions handler.
    /// </summary>
    private void HeaderActions_ActionPerformed(object sender, CommandEventArgs e)
    {
        if (e.CommandName.Equals(ComponentEvents.SAVE, StringComparison.OrdinalIgnoreCase))
        {
            ValidateInputAndCreateNewIssue();
        }
    }


    private void ValidateInputAndCreateNewIssue()
    {
        if (!InputIsValid())
        {
            return;
        }

        var issueInfo = CreateIssueInfo();

        if (SetIssueInfo(issueInfo))
        {
            RedirectToEmailBuilder(issueInfo.IssueID);
        }
    }


    private bool InputIsValid()
    {
        return IssueNameIsSpecified() && EmailTemplateIsSelected();
    }


    private bool IssueNameIsSpecified()
    {
        var isValid = !string.IsNullOrEmpty(txtDisplayName.Text);

        if (!isValid)
        {
            AddMessage(MessageTypeEnum.Error, GetString("newslettertemplateselect.error.emptyname"));
        }

        return isValid;
    }


    private bool EmailTemplateIsSelected()
    {
        var isValid = ucTemplateSelector.SelectedId > 0;

        if (!isValid)
        {
            AddMessage(MessageTypeEnum.Error, GetString("newslettertemplateselect.error.emptytemplate"));
        }

        return isValid;
    }


    private IssueInfo CreateIssueInfo()
    {
        var newsletterId = QueryHelper.GetInteger("parentobjectid", 0);
        var newsletter = NewsletterInfoProvider.GetNewsletterInfo(newsletterId);

        return new IssueInfo
        {
            IssueDisplayName = txtDisplayName.Text,
            IssueSubject = txtDisplayName.Text,
            IssueNewsletterID = newsletter.NewsletterID,
            IssueSenderName = newsletter.NewsletterSenderName,
            IssueSenderEmail = newsletter.NewsletterSenderEmail,
            IssueTemplateID = ucTemplateSelector.SelectedId,
            IssueSiteID = SiteContext.CurrentSiteID,
            IssueText = string.Empty,
            IssueUseUTM = false
        };
    }


    private bool SetIssueInfo(IssueInfo issueInfo)
    {
        try
        {
            IssueInfoProvider.SetIssueInfo(issueInfo);
            return true;
        }
        catch (Exception ex)
        {
            LogAndShowError("NEWSLETTER", "Save", ex);
            return false;
        }
    }


    private static void RedirectToEmailBuilder(int issueId)
    {
        var url = GetRedirectUrl(issueId);
        URLHelper.Redirect(url);
    }


    private static string GetRedirectUrl(int issueId)
    {
        var url = UIContextHelper.GetElementUrl(ModuleName.NEWSLETTER, "EditIssueProperties");
        url = URLHelper.AddParameterToUrl(url, "tabname", EmailBuilderHelper.EMAIL_BUILDER_UI_ELEMENT);
        url = URLHelper.AddParameterToUrl(url, "objectid", issueId.ToString());
        url = URLHelper.PropagateUrlParameters(url, "parentobjectid");
        url = ApplicationUrlHelper.AppendDialogHash(url);

        return url;
    }
}
