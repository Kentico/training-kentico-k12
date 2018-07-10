using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.Newsletters;
using CMS.Newsletters.Web.UI;
using CMS.SiteProvider;
using CMS.UIControls;

[Security(Resource = ModuleName.NEWSLETTER, Permission = "authorissues")]
[UIElement(ModuleName.NEWSLETTER, "Newsletter.Issue.Send")]
[EditedObject(IssueInfo.OBJECT_TYPE, "objectid")]
public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_Send : CMSNewsletterPage
{
    private const decimal PERCENTAGE_THRESHOLD = 0.9m;

    public const string SCHEDULE_ACTION_IDENTIFIER = "schedule";

    private NewsletterInfo mNewsletter;
    private IssueInfo mIssue;


    private bool IsIssueTemplateBased => mIssue.IssueTemplateID > 0;

    private Lazy<int> MarketableRecipientsCount => new Lazy<int>(GetMarketableRecipientsCount);
    private Lazy<int> LicenseMaxNumberOfRecipients => new Lazy<int>(GetLicenseMaxNumberOfRecipients);


    protected void Page_Load(object sender, EventArgs e)
    {
        MessagesPlaceHolder = plcMess;

        // Get newsletter issue and check its existence
        mIssue = EditedObject as IssueInfo;

        if (mIssue == null)
        {
            RedirectToAccessDenied(GetString("general.invalidparameters"));
        }

        if (!mIssue.CheckPermissions(PermissionsEnum.Read, CurrentSiteName, CurrentUser))
        {
            RedirectToAccessDenied(mIssue.TypeInfo.ModuleName, "AuthorIssues");
        }

        mNewsletter = NewsletterInfoProvider.GetNewsletterInfo(mIssue.IssueNewsletterID);

        string infoMessage;

        if (mIssue.IssueIsABTest)
        {
            InitSendVariant(mIssue);
            infoMessage = sendVariant.InfoMessage;
        }
        else
        {
            InitSendElem(mIssue);
            infoMessage = GetInfoMessage(mIssue.IssueStatus);
        }

        // Display additional information
        if (!String.IsNullOrEmpty(infoMessage))
        {
            ShowInformationInternal(infoMessage);
        }

        string scriptBlock = @"function RefreshPage() {{ document.location.replace(document.location); }}";
        ScriptHelper.RegisterClientScriptBlock(this, GetType(), "RefreshActions", scriptBlock, true);

        if (!RequestHelper.IsPostBack() && (QueryHelper.GetInteger("sent", 0) == 1))
        {
            ShowConfirmation(GetString("Newsletter_Send.SuccessfullySent"));
        }

        AddBrokenEmailUrlNotifier(mNewsletter, lblUrlWarning);
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        HandleBreadcrumbsScripts();
    }


    protected void sendVariant_OnChanged(object sender, EventArgs e)
    {
        ShowInformationInternal(sendVariant.InfoMessage);
    }


    /// <summary>
    /// Sends an issue.
    /// </summary>
    protected void Send()
    {
        CheckPermissions();

        string errMessage = null;

        if (sendElem.Visible)
        {
            errMessage = SendDynamicIssue();
        }
        else if (sendVariant.Visible)
        {
            errMessage = SendVariants();
        }
        else if (sendElem_TemplateBased.Visible)
        {
            errMessage = SendTemplateBasedIssue();
        }

        HandleActionResult(errMessage);
    }


    /// <summary>
    /// Saves current issue variant setting.
    /// </summary>
    protected void Save()
    {
        if (sendVariant.SaveIssue())
        {
            if (!String.IsNullOrEmpty(sendVariant.InfoMessage))
            {
                ShowInformationInternal(sendVariant.InfoMessage);
            }

            ShowChangesSaved();
        }
        else
        {
            ShowError(sendVariant.ErrorMessage);
        }
    }


    protected void hdrActions_ActionPerformed(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case ComponentEvents.SAVE:
                Save();
                break;
            case ComponentEvents.SUBMIT:
                Send();
                break;
            case SCHEDULE_ACTION_IDENTIFIER:
                ScheduleTemplateBasedIssue();
                break;
        }
    }


    private void InitSendVariant(IssueInfo issue)
    {
        sendVariant.StopProcessing = issue.IssueID <= 0;
        sendVariant.IssueID = issue.IssueID;
        sendVariant.OnChanged += sendVariant_OnChanged;
        sendVariant.ReloadData(!RequestHelper.IsPostBack());

        sendVariant.Visible = true;

        InitHeaderActions(issue, InitABTestHeaderActions);
    }


    private void InitABTestHeaderActions(IssueInfo issue, HeaderActions hdrActions)
    {
        if (IsEditable(issue))
        {
            hdrActions.ActionsList.Add(new SaveAction());
        }

        var sendingIssueAllowed = issue.IssueStatus == IssueStatusEnum.Idle;
        if (!sendingIssueAllowed)
        {
            return;
        }

        var variants = GetIssueVariants(issue);

        var variantNamesWithUnfilledRequiredWidgetProperties = GetVariantNamesWithUnfilledRequiredWidgetProperties(variants);
        var variantNamesWithMissingWidgetDefinition = GetVariantNamesWithMissingWidgetDefinition(variants);

        var isValidDefinition = !variantNamesWithUnfilledRequiredWidgetProperties.Any()
            && !variantNamesWithMissingWidgetDefinition.Any();

        AddSendHeaderAction(hdrActions, isValidDefinition, ButtonStyle.Default);

        if (!isValidDefinition)
        {
            var invalidVariantNames = variantNamesWithUnfilledRequiredWidgetProperties.Union(variantNamesWithMissingWidgetDefinition);
            plcMess.AddError(string.Format(GetString("newsletter.issue.send.variantwidgeterror"), string.Join(", ", invalidVariantNames)));
        }
    }


    private static bool IsEditable(IssueInfo issue)
    {
        return !issue.IssueStatus.Equals(IssueStatusEnum.Finished) && !(issue.IssueStatus.Equals(IssueStatusEnum.ReadyForSending) && AllVariantsAreSent(issue));
    }


    private static bool AllVariantsAreSent(IssueInfo issue)
    {
        var variants = GetIssueVariants(issue);

        return variants.All(variant => variant.IssueStatus.Equals(IssueStatusEnum.Finished));
    }


    private static IList<IssueInfo> GetIssueVariants(IssueInfo issue)
    {
        var originalIssue = IssueInfoProvider.GetOriginalIssue(issue.IssueID);

        return IssueInfoProvider.GetIssues()
                                .WhereEquals("IssueVariantOfIssueID", originalIssue.IssueID)
                                .ToList();
    }


    private IList<string> GetVariantNamesWithUnfilledRequiredWidgetProperties(IEnumerable<IssueInfo> variants)
    {
        return variants.Where(variant => variant.HasWidgetWithUnfilledRequiredProperty())
            .Select(v => v.GetVariantName())
            .ToList();
    }


    private IList<string> GetVariantNamesWithMissingWidgetDefinition(IEnumerable<IssueInfo> variants)
    {
        return variants.Where(variant => variant.HasWidgetWithMissingDefinition())
            .Select(v => v.GetVariantName())
            .ToList();
    }


    private void InitSendElem(IssueInfo issue)
    {
        if (IsIssueTemplateBased)
        {
            sendElem_TemplateBased.IssueID = issue.IssueID;
            sendElem_TemplateBased.Visible = true;
        }
        else
        {
            sendElem.IssueID = issue.IssueID;
            sendElem.NewsletterID = issue.IssueNewsletterID;
            sendElem.Visible = true;
        }

        InitHeaderActions(issue, InitIssueHeaderActions);
    }


    private void InitIssueHeaderActions(IssueInfo issue, HeaderActions hdrActions)
    {
        var sendingIssueAllowed = issue.IssueStatus == IssueStatusEnum.Idle || issue.IssueStatus == IssueStatusEnum.ReadyForSending;
        if (!sendingIssueAllowed)
        {
            return;
        }

        var recipientsCountAllowed = LicenseMaxNumberOfRecipients.Value == 0 || MarketableRecipientsCount.Value <= LicenseMaxNumberOfRecipients.Value;

        if (!recipientsCountAllowed)
        {
            plcMess.AddError(string.Format(GetString("newsletter.issue.send.subcriberlimiterror"), MarketableRecipientsCount.Value, LicenseMaxNumberOfRecipients.Value));
        }

        var hasWidgetWithUnfilledRequiredProperty = issue.HasWidgetWithUnfilledRequiredProperty();
        var hasWidgetWithMissingDefinition = issue.HasWidgetWithMissingDefinition();
        var isValidWidgetDefinition = !hasWidgetWithUnfilledRequiredProperty && !hasWidgetWithMissingDefinition;

        if (!isValidWidgetDefinition)
        {
            plcMess.AddError(GetString("newsletter.issue.send.widgeterror"));
        }

        if (IsIssueTemplateBased)
        {
            AddTemplateBasedHeaderActions(hdrActions, isValidWidgetDefinition && recipientsCountAllowed);
        }
        else
        {
            AddSendHeaderAction(hdrActions, isValidWidgetDefinition && recipientsCountAllowed);
        }
    }


    /// <summary>
    /// Initializes header action control.
    /// </summary>
    private void InitHeaderActions(IssueInfo issue, Action<IssueInfo, HeaderActions> initHeaderActions)
    {
        var hdrActions = CurrentMaster.HeaderActions;
        hdrActions.ActionsList.Clear();

        initHeaderActions(issue, hdrActions);

        hdrActions.ActionPerformed += hdrActions_ActionPerformed;
        hdrActions.ReloadData();

        CurrentMaster.DisplayActionsPanel = true;
    }


    /// <summary>
    /// Adds template-based header actions to <paramref name="hdrActions"/>.
    /// </summary>
    private static void AddTemplateBasedHeaderActions(HeaderActions hdrActions, bool enabled)
    {
        hdrActions.ActionsList.Add(new HeaderAction
        {
            CommandName = SCHEDULE_ACTION_IDENTIFIER,
            Text = GetString("newsletterissue_send.saveschedule"),
            Tooltip = GetString("newsletterissue_send.saveschedule"),
            Enabled = enabled
        });

        hdrActions.ActionsList.Add(new HeaderAction
        {
            CommandName = ComponentEvents.SUBMIT,
            Text = GetString("newsletterissue_send.sendnowbutton"),
            Tooltip = GetString("newsletterissue_send.sendnowbutton"),
            Enabled = enabled,
            OnClientClick = "return confirm('" + GetString("newsletterissue_send.confirmationdialog") + "');",
            ButtonStyle = ButtonStyle.Default
        });
    }


    /// <summary>
    /// Adds send header action to <paramref name="hdrActions"/>.
    /// </summary>
    private static void AddSendHeaderAction(HeaderActions hdrActions, bool enabled, ButtonStyle buttonStyle = ButtonStyle.Primary)
    {
        hdrActions.ActionsList.Add(new HeaderAction
        {
            CommandName = ComponentEvents.SUBMIT,
            Text = GetString("newsletterissue_send.send"),
            Tooltip = GetString("newsletterissue_send.send"),
            Enabled = enabled,
            ButtonStyle = buttonStyle
        });
    }


    /// <summary>
    /// Schedules a template-based issue.
    /// </summary>
    /// <remarks>It also checks permissions and handles possible error messages.</remarks>
    /// <returns>Error message on failure</returns>
    private void ScheduleTemplateBasedIssue()
    {
        CheckPermissions();

        string errorMessage = String.Empty;

        if (!sendElem_TemplateBased.SendScheduled())
        {
            errorMessage = sendElem_TemplateBased.ErrorMessage;
        }

        HandleActionResult(errorMessage);
    }


    /// <summary>
    /// Sends a template-based issue.
    /// </summary>
    /// <returns>Error message on failure</returns>
    private string SendTemplateBasedIssue()
    {
        if (!sendElem_TemplateBased.SendNow())
        {
            return sendElem_TemplateBased.ErrorMessage;
        }

        return String.Empty;
    }


    /// <summary>
    /// Sends a dynamic issue.
    /// </summary>
    /// <returns>Error message on failure</returns>
    private string SendDynamicIssue()
    {
        if (!sendElem.SendIssue())
        {
            return sendElem.ErrorMessage;
        }

        if (mNewsletter != null)
        {
            // Redirect to the issue list page
            ScriptHelper.RegisterStartupScript(this, typeof(string), "Newsletter_Issue_Send", "parent.location='" + ResolveUrl("~/CMSModules/Newsletters/Tools/Newsletters/Newsletter_Issue_List.aspx?newsletterid=" + mNewsletter.NewsletterID) + "';", true);
        }

        return String.Empty;
    }


    /// <summary>
    /// Sends issue with variants.
    /// </summary>
    /// <returns>Error message on failure</returns>
    private string SendVariants()
    {
        return !sendVariant.SendIssue() ? sendVariant.ErrorMessage : String.Empty;
    }


    /// <summary>
    /// Checks if user has permissions to access newsletters.
    /// </summary>
    private void CheckPermissions()
    {
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("cms.newsletter", "authorissues"))
        {
            RedirectToAccessDenied("cms.newsletter", "authorissues");
        }
    }


    /// <summary>
    /// Depending on <paramref name="errorMessage"/> it either shows error or if there is no error it redirects to confirmation url.
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    private void HandleActionResult(string errorMessage)
    {
        if (String.IsNullOrEmpty(errorMessage))
        {
            string url = URLHelper.AddParameterToUrl(RequestContext.CurrentURL, "sent", "1");
            URLHelper.Redirect(url);
        }
        else
        {
            ShowError(errorMessage);
        }
    }


    /// <summary>
    /// Gets info message depending on issue status.
    /// </summary>
    /// <param name="issueStatus">Issue status</param>
    /// <returns>Info message</returns>
    private string GetInfoMessage(IssueStatusEnum issueStatus)
    {
        switch (issueStatus)
        {
            case IssueStatusEnum.Finished:
                return GetString("Newsletter_Issue_Header.AlreadySent");

            case IssueStatusEnum.ReadyForSending:
                {
                    var message = GetString("Newsletter_Issue_Header.AlreadyScheduled");
                    return AppendReachingLimitMessage(message);
                }

            case IssueStatusEnum.Idle:
                {
                    var message = GetString("Newsletter_Issue_Header.NotSentYet");
                    return AppendReachingLimitMessage(message);
                }
        }

        return GetString("Newsletter_Issue_Header.NotSentYet");
    }


    private string AppendReachingLimitMessage(string message)
    {
        if (MarketableRecipientsNearLicenseLimitation(MarketableRecipientsCount.Value, LicenseMaxNumberOfRecipients.Value))
        {
            return $"{message} {string.Format(GetString("Newsletter_Issue_Header.LicenseLimitationWarning"), MarketableRecipientsCount.Value, LicenseMaxNumberOfRecipients.Value)}";
        }

        return message;
    }


    private static bool MarketableRecipientsNearLicenseLimitation(int marketableRecipients, int maxNumberOfRecipients)
    {
        return maxNumberOfRecipients != 0  && marketableRecipients >= maxNumberOfRecipients * PERCENTAGE_THRESHOLD && marketableRecipients <= maxNumberOfRecipients;
    }


    private int GetLicenseMaxNumberOfRecipients()
    {
        var site = SiteInfoProvider.GetSiteInfo(mIssue.IssueSiteID);
        return LicenseKeyInfoProvider.VersionLimitations(site.DomainName, FeatureEnum.SimpleContactManagement, false);
    }


    private int GetMarketableRecipientsCount()
    {
        return mIssue
            .GetRecipientsProvider()
            .GetMarketableRecipients()
            .Count;
    }
    

    /// <summary>
    /// Shows user friendly message in ordinary label.
    /// </summary>
    /// <param name="message">Message to be shown</param>
    private void ShowInformationInternal(string message)
    {
        plcMess.ShowInformation(message);
    }


    /// <summary>
    /// Handles manual rendering of breadcrumbs.
    /// On this page the breadcrumbs needs to be hard-coded in order to be able to access single email via link and ensure consistency of breadcrumbs.
    /// </summary>
    private void HandleBreadcrumbsScripts()
    {
        ScriptHelper.RegisterRequireJs(Page);

        ControlsHelper.RegisterClientScriptBlock(this, Page, typeof(string), "BreadcrumbsOverwriting", ScriptHelper.GetScript(@"
        cmsrequire(['CMS/EventHub'], function(hub) {
              hub.publish('OverwriteBreadcrumbs', " + IssueHelper.GetBreadcrumbsData((IssueInfo)EditedObject, (NewsletterInfo)EditedObjectParent) + @");
        });"));
    }
}
