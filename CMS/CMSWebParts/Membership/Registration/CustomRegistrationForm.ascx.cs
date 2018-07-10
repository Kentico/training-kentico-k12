using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Localization;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.Protection;
using CMS.SiteProvider;
using CMS.WebAnalytics;

public partial class CMSWebParts_Membership_Registration_CustomRegistrationForm : CMSAbstractWebPart
{
    private string mSiteName;
    private bool? mConfirmationRequired;
    private bool? mAdminApprovalRequired;
    private IEnumerable<string> mSiteList;

    #region "Layout properties"

    /// <summary>
    /// Full alternative form name ('classname.formname') for UserSettingsInfo.
    /// Default value is cms.user.RegistrationForm
    /// </summary>
    public string AlternativeForm
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternativeForm"), "cms.user.RegistrationForm");
        }
        set
        {
            SetValue("AlternativeForm", value);
        }
    }


    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Info label
    /// </summary>
    public Label InfoLabel
    {
        get
        {
            return MessagesPlaceHolder.InfoLabel;
        }
    }


    /// <summary>
    /// Error label
    /// </summary>
    public Label ErrorLabel
    {
        get
        {
            return MessagesPlaceHolder.ErrorLabel;
        }
    }

    #endregion


    #region "Text properties"

    /// <summary>
    /// Gets or sets submit button text.
    /// </summary>
    public string ButtonText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ButtonText"), GetString("Webparts_Membership_RegistrationForm.Button"));
        }

        set
        {
            SetValue("ButtonText", value);
            btnRegister.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets registration approval page URL.
    /// </summary>
    public string ApprovalPage
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ApprovalPage"), String.Empty);
        }
        set
        {
            SetValue("ApprovalPage", value);
        }
    }


    /// <summary>
    /// Gets or sets colons visibility in form labels. Default value is true. 
    /// </summary>
    public bool DisplayColons
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayColons"), true);
        }
        set
        {
            SetValue("DisplayColons", value);
        }
    }


    /// <summary>
    /// Gets or sets form layout. 
    /// </summary>
    public string FormLayout
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FormLayout"), FormLayoutEnum.SingleTable.ToStringRepresentation());
        }
        set
        {
            SetValue("FormLayout", value);
        }
    }


    /// <summary>
    /// Gets or sets field layout. 
    /// </summary>
    public string FieldLayout
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FieldLayout"), FieldLayoutEnum.Default.ToStringRepresentation());
        }
        set
        {
            SetValue("FieldLayout", value);
        }
    }


    /// <summary>
    /// Gets or sets CSS class to style the submit button.
    /// </summary>
    public string ButtonCSS
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ButtonCSS"), string.Empty);
        }
        set
        {
            SetValue("ButtonCSS", value);
        }
    }

    #endregion


    #region "Registration properties"

    /// <summary>
    /// Gets or sets the value that indicates whether email to user should be sent.
    /// </summary>
    public bool SendWelcomeEmail
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SendWelcomeEmail"), true);
        }
        set
        {
            SetValue("SendWelcomeEmail", value);
        }
    }


    /// <summary>
    /// Determines whether the captcha image should be displayed.
    /// </summary>
    public bool DisplayCaptcha
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayCaptcha"), false);
        }

        set
        {
            SetValue("DisplayCaptcha", value);
        }
    }


    /// <summary>
    /// Gets or sets the message which is displayed after registration failed.
    /// </summary>
    public string RegistrationErrorMessage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RegistrationErrorMessage"), String.Empty);
        }
        set
        {
            SetValue("RegistrationErrorMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether user is enabled after registration.
    /// </summary>
    public bool EnableUserAfterRegistration
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("EnableUserAfterRegistration"), true);
        }
        set
        {
            SetValue("EnableUserAfterRegistration", value);
        }
    }


    /// <summary>
    /// Gets or sets the sender email (from).
    /// </summary>
    public string FromAddress
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("FromAddress"), SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSNoreplyEmailAddress"));
        }
        set
        {
            SetValue("FromAddress", value);
        }
    }


    /// <summary>
    /// Gets or sets the recipient email (to).
    /// </summary>
    public string ToAddress
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ToAddress"), SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSAdminEmailAddress"));
        }
        set
        {
            SetValue("ToAddress", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether after successful registration is 
    /// notification email sent to the administrator 
    /// </summary>
    public bool NotifyAdministrator
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("NotifyAdministrator"), false);
        }
        set
        {
            SetValue("NotifyAdministrator", value);
        }
    }


    /// <summary>
    /// Gets or sets the roles where is user assigned after successful registration.
    /// </summary>
    public string AssignRoles
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AssignToRoles"), String.Empty);
        }
        set
        {
            SetValue("AssignToRoles", value);
        }
    }


    /// <summary>
    /// Gets or sets the sites where is user assigned after successful registration.
    /// </summary>
    public string AssignToSites
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AssignToSites"), String.Empty);
        }
        set
        {
            SetValue("AssignToSites", value);
        }
    }


    /// <summary>
    /// Gets or sets the message which is displayed after successful registration.
    /// </summary>
    public string DisplayMessage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DisplayMessage"), String.Empty);
        }
        set
        {
            SetValue("DisplayMessage", value);
        }
    }


    /// <summary>
    /// Gets or set the URL where is user redirected after successful registration.
    /// </summary>
    public string RedirectToURL
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RedirectToURL"), String.Empty);
        }
        set
        {
            SetValue("RedirectToURL", value);
        }
    }


    /// <summary>
    /// Gets or sets the default starting alias path for newly registered user.
    /// </summary>
    public string StartingAliasPath
    {
        get
        {
            return ValidationHelper.GetString(GetValue("StartingAliasPath"), String.Empty);
        }
        set
        {
            SetValue("StartingAliasPath", value);
        }
    }

    #endregion


    #region "Conversion properties"

    /// <summary>
    /// Gets or sets the conversion track name used after successful registration.
    /// </summary>
    public string TrackConversionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TrackConversionName"), String.Empty);
        }
        set
        {
            if ((value != null) && (value.Length > 400))
            {
                value = value.Substring(0, 400);
            }
            SetValue("TrackConversionName", value);
        }
    }


    /// <summary>
    /// Gets or sets the conversion value used after successful registration.
    /// </summary>
    public double ConversionValue
    {
        get
        {
            return ValidationHelper.GetDoubleSystem(GetValue("ConversionValue"), 0);
        }
        set
        {
            SetValue("ConversionValue", value);
        }
    }

    #endregion


    /// <summary>
    /// Name of the current site
    /// </summary>
    public override string CurrentSiteName
    {
        get
        {
            return mSiteName ?? (mSiteName = SiteContext.CurrentSiteName);
        }
    }


    /// <summary>
    /// True if email confirmation is required
    /// </summary>
    private bool ConfirmationRequired
    {
        get
        {
            if (mConfirmationRequired == null)
            {
                mConfirmationRequired = SettingsKeyInfoProvider.GetBoolValue(CurrentSiteName + ".CMSRegistrationEmailConfirmation");
            }
            return mConfirmationRequired.Value;
        }
    }


    /// <summary>
    /// True if administrator approval is required
    /// </summary>
    private bool AdminApprovalRequired
    {
        get
        {
            if (mAdminApprovalRequired == null)
            {
                mAdminApprovalRequired = SettingsKeyInfoProvider.GetBoolValue(CurrentSiteName + ".CMSRegistrationAdministratorApproval");
            }
            return mAdminApprovalRequired.Value;
        }
    }


    /// <summary>
    /// List of sites the user will be assigned to
    /// </summary>
    private IEnumerable<string> SiteList
    {
        get
        {
            return mSiteList ?? (mSiteList = !String.IsNullOrEmpty(AssignToSites) ? AssignToSites.Split(';') : new[] { CurrentSiteName });
        }
    }

    #region "Methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Set default visibility
            pnlRegForm.Visible = true;

            lblCaptcha.ResourceString = "webparts_membership_registrationform.captcha";

            // WAI validation
            lblCaptcha.AssociatedControlClientID = captchaElem.InputClientID;

            // Get alternative form info
            AlternativeFormInfo afi = AlternativeFormInfoProvider.GetAlternativeFormInfo(AlternativeForm);
            if (afi != null)
            {
                formUser.Data = new UserInfo();

                formUser.FormInformation = FormHelper.GetFormInfo(AlternativeForm, true);
                formUser.AltFormInformation = afi;
                formUser.Visible = true;
                formUser.ValidationErrorMessage = RegistrationErrorMessage;
                formUser.IsLiveSite = true;
                formUser.UseColonBehindLabel = DisplayColons;
                formUser.DefaultFormLayout = FormLayout.ToEnum<FormLayoutEnum>();
                formUser.DefaultFieldLayout = FieldLayout.ToEnum<FieldLayoutEnum>();

                formUser.MessagesPlaceHolder = plcMess;
                formUser.InfoLabel = plcMess.InfoLabel;
                formUser.ErrorLabel = plcMess.ErrorLabel;

                formUser.OnAfterSave += formUser_OnAfterSave;

                // Reload form if not in PortalEngine environment and if post back
                if (StandAlone)
                {
                    formUser.ReloadData();
                }

                captchaElem.Visible = DisplayCaptcha;
                lblCaptcha.Visible = DisplayCaptcha;
                plcCaptcha.Visible = DisplayCaptcha;

                btnRegister.Text = ButtonText;
                btnRegister.AddCssClass(ButtonCSS);
                btnRegister.Click += btnRegister_Click;

                InfoLabel.CssClass = "EditingFormInfoLabel";
                ErrorLabel.CssClass = "EditingFormErrorLabel ErrorLabel";

                if (formUser != null)
                {
                    // Set the live site context
                    formUser.ControlContext.ContextName = CMS.Base.Web.UI.ControlContext.LIVE_SITE;
                }
            }
            else
            {
                ShowError(String.Format(GetString("altform.formdoesntexists"), AlternativeForm));
                pnlRegForm.Visible = false;
            }
        }
    }


    /// <summary>
    /// Page pre-render event handler.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        // Hide default form submit button
        if (formUser != null)
        {
            formUser.SubmitButton.Visible = false;
        }
    }


    /// <summary>
    /// OK click handler (Proceed registration).
    /// </summary>
    private void btnRegister_Click(object sender, EventArgs e)
    {
        if ((PageManager.ViewMode == ViewModeEnum.Design) || (HideOnCurrentPage) || (!IsVisible))
        {
            // Do not process
            return;
        }
        // Ban IP addresses which are blocked for registration
        if (!BannedIPInfoProvider.IsAllowed(CurrentSiteName, BanControlEnum.Registration))
        {
            ShowError(GetString("banip.ipisbannedregistration"));
            return;
        }

        // Check if captcha is required and verify captcha text
        if (DisplayCaptcha && !captchaElem.IsValid())
        {
            // Display error message if captcha text is not valid
            ShowError(GetString("Webparts_Membership_RegistrationForm.captchaError"));
            return;
        }

        string userName = String.Empty;
        string nickName = String.Empty;
        string emailValue = String.Empty;

        // Check duplicate user
        // 1. Find appropriate control and get its value (i.e. user name)
        // 2. Try to find user info
        FormEngineUserControl txtUserName = formUser.FieldControls["UserName"];
        if (txtUserName != null)
        {
            userName = ValidationHelper.GetString(txtUserName.Value, String.Empty);
        }

        FormEngineUserControl txtEmail = formUser.FieldControls["Email"];
        if (txtEmail != null)
        {
            emailValue = ValidationHelper.GetString(txtEmail.Value, String.Empty);
        }

        // If user name and e-mail aren't filled stop processing and display error.
        if (string.IsNullOrEmpty(userName))
        {
            userName = emailValue;
            if (String.IsNullOrEmpty(emailValue))
            {
                formUser.StopProcessing = true;
                formUser.DisplayErrorLabel("Email", GetString("customregistrationform.usernameandemail"));
                return;
            }

            // Set username after data retrieval in case the username control is hidden (visible field hidden in custom layout)
            formUser.OnBeforeSave += (s, args) => formUser.Data.SetValue("UserName", userName);
        }

        FormEngineUserControl txtNickName = formUser.FieldControls["UserNickName"];
        if (txtNickName != null)
        {
            nickName = ValidationHelper.GetString(txtNickName.Value, String.Empty);
        }

        // Test if "global" or "site" user exists. 
        SiteInfo si = SiteContext.CurrentSite;
        UserInfo siteui = UserInfoProvider.GetUserInfo(UserInfoProvider.EnsureSitePrefixUserName(userName, si));
        if ((UserInfoProvider.GetUserInfo(userName) != null) || (siteui != null))
        {
            ShowError(GetString("Webparts_Membership_RegistrationForm.UserAlreadyExists").Replace("%%name%%", HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(userName, true))));
            return;
        }

        // Check for reserved user names like administrator, sysadmin, ...
        if (UserInfoProvider.NameIsReserved(CurrentSiteName, userName))
        {
            ShowError(GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(userName, true))));
            return;
        }

        if (UserInfoProvider.NameIsReserved(CurrentSiteName, nickName))
        {
            ShowError(GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(nickName)));
            return;
        }

        // Check limitations for site members
        if (!UserInfoProvider.LicenseVersionCheck(RequestContext.CurrentDomain, FeatureEnum.SiteMembers, ObjectActionEnum.Insert, false))
        {
            ShowError(GetString("License.MaxItemsReachedSiteMember"));
            return;
        }

        // Check whether email is unique if it is required
        if (!UserInfoProvider.IsEmailUnique(emailValue, SiteList, 0))
        {
            formUser.DisplayErrorLabel("Email", GetString("UserInfo.EmailAlreadyExist"));
            return;
        }

        formUser.SaveData(null, String.IsNullOrEmpty(DisplayMessage.Trim()));
    }


    void formUser_OnAfterSave(object sender, EventArgs e)
    {
        // Get user info from form
        UserInfo ui = (UserInfo)formUser.Data;

        // Add user prefix if settings is on
        // Ensure site prefixes
        if (UserInfoProvider.UserNameSitePrefixEnabled(CurrentSiteName))
        {
            ui.UserName = UserInfoProvider.EnsureSitePrefixUserName(ui.UserName, SiteContext.CurrentSite);
        }

        ui.Enabled = EnableUserAfterRegistration;
        ui.UserURLReferrer = CookieHelper.GetValue(CookieName.UrlReferrer);
        ui.UserCampaign = Service.Resolve<ICampaignService>().CampaignCode;

        ui.SiteIndependentPrivilegeLevel = UserPrivilegeLevelEnum.None;

        // Fill optionally full user name
        if (String.IsNullOrEmpty(ui.FullName))
        {
            ui.FullName = UserInfoProvider.GetFullName(ui.FirstName, ui.MiddleName, ui.LastName);
        }

        // Ensure nick name
        if (ui.UserNickName.Trim() == String.Empty)
        {
            ui.UserNickName = Functions.GetFormattedUserName(ui.UserName, true);
        }

        ui.UserSettings.UserRegistrationInfo.IPAddress = RequestContext.UserHostAddress;
        ui.UserSettings.UserRegistrationInfo.Agent = HttpContext.Current.Request.UserAgent;
        ui.UserSettings.UserLogActivities = true;
        ui.UserSettings.UserShowIntroductionTile = true;

        // Check whether confirmation is required
        if (!ConfirmationRequired)
        {
            // If confirmation is not required check whether administration approval is required
            if (AdminApprovalRequired)
            {
                ui.Enabled = false;
                ui.UserSettings.UserWaitingForApproval = true;
            }
        }
        else
        {
            // EnableUserAfterRegistration is overridden by requiresConfirmation - user needs to be confirmed before enable
            ui.Enabled = false;
        }

        // Set user's starting alias path
        if (!String.IsNullOrEmpty(StartingAliasPath))
        {
            ui.UserStartingAliasPath = MacroResolver.ResolveCurrentPath(StartingAliasPath);
        }

        // Get user password and save it in appropriate format after form save
        string password = ValidationHelper.GetString(ui.GetValue("UserPassword"), String.Empty);
        UserInfoProvider.SetPassword(ui, password);

        if (!ConfirmationRequired)
        {
            SendAdminNotification(ui);
            LogOMActivity(ui);
        }

        SendRegistrationEmail(ui);
        LogWebAnalytics(ui);
        AssignToRoles(ui);

        if (ui.Enabled)
        {
            // Authenticate currently created user
            AuthenticationHelper.AuthenticateUser(ui.UserName, true);
        }

        var displayMessage = DisplayMessage.Trim();

        if (!String.IsNullOrEmpty(displayMessage))
        {
            ShowInformation(displayMessage);
        }
        else
        {
            if (RedirectToURL != String.Empty)
            {
                URLHelper.Redirect(UrlResolver.ResolveUrl(RedirectToURL));
            }

            string returnUrl = QueryHelper.GetString("ReturnURL", String.Empty);
            if (!String.IsNullOrEmpty(returnUrl) && (returnUrl.StartsWith("~", StringComparison.Ordinal) || returnUrl.StartsWith("/", StringComparison.Ordinal) || QueryHelper.ValidateHash("hash", "aliaspath")))
            {
                URLHelper.Redirect(UrlResolver.ResolveUrl(HttpUtility.UrlDecode(returnUrl)));
            }
        }

        // Hide registration form
        pnlRegForm.Visible = false;
    }


    /// <summary>
    /// Logs web analytics data.
    /// </summary>
    private void LogWebAnalytics(UserInfo ui)
    {
        // Track successful registration conversion
        if (TrackConversionName != String.Empty)
        {
            if (AnalyticsHelper.AnalyticsEnabled(CurrentSiteName) && Service.Resolve<IAnalyticsConsentProvider>().HasConsentForLogging() && !AnalyticsHelper.IsIPExcluded(CurrentSiteName, RequestContext.UserHostAddress))
            {
                HitLogProvider.LogConversions(CurrentSiteName, LocalizationContext.PreferredCultureCode, TrackConversionName, 0, ConversionValue);
            }
        }

        // Log registered user if confirmation is not required
        if (!ConfirmationRequired)
        {
            AnalyticsHelper.LogRegisteredUser(CurrentSiteName, ui);
        }
    }


    /// <summary>
    /// Logs online marketing activities.
    /// </summary>
    private void LogOMActivity(UserInfo ui)
    {
        IMembershipActivityLogger logger = Service.Resolve<IMembershipActivityLogger>();
        logger.LogRegistration(ui.UserName, DocumentContext.CurrentDocument);

        // Log login activity
        if (ui.Enabled)
        {
            // Log activity
            logger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);
        }
    }


    /// <summary>
    /// Sends notification email to the administrator.
    /// </summary>
    private void SendAdminNotification(UserInfo ui)
    {
        if (NotifyAdministrator && (FromAddress != String.Empty) && (ToAddress != String.Empty))
        {
            var resolver = MembershipResolvers.GetRegistrationResolver(ui);
            var emailTemplate = EmailTemplateProvider.GetEmailTemplate(AdminApprovalRequired ? "Registration.Approve" : "Registration.New", CurrentSiteName);

            if (emailTemplate == null)
            {
                EventLogProvider.LogEvent(EventType.ERROR, "RegistrationForm", "GetEmailTemplate", eventUrl: RequestContext.RawURL);
            }
            else
            {
                EmailMessage message = new EmailMessage();
                message.EmailFormat = EmailFormatEnum.Default;
                message.From = FromAddress;
                message.Recipients = ToAddress;
                message.Subject = GetString("RegistrationForm.EmailSubject");

                try
                {
                    EmailSender.SendEmailWithTemplateText(CurrentSiteName, message, emailTemplate, resolver, false);
                }
                catch
                {
                    EventLogProvider.LogEvent(EventType.ERROR, "Membership", "RegistrationEmail");
                }
            }
        }
    }


    /// <summary>
    /// Sends confirmation or welcome email.
    /// </summary>
    private void SendRegistrationEmail(UserInfo ui)
    {
        bool error = false;
        EmailTemplateInfo template = null;

        // Email message
        EmailMessage emailMessage = new EmailMessage();
        emailMessage.EmailFormat = EmailFormatEnum.Default;
        emailMessage.Recipients = ui.Email;
        emailMessage.From = SettingsKeyInfoProvider.GetValue(CurrentSiteName + ".CMSNoreplyEmailAddress");

        // Send welcome message with username and password, with confirmation link, user must confirm registration
        if (ConfirmationRequired)
        {
            template = EmailTemplateProvider.GetEmailTemplate("RegistrationConfirmation", CurrentSiteName);
            emailMessage.Subject = GetString("RegistrationForm.RegistrationConfirmationEmailSubject");
        }
        // Send welcome message with username and password, with information that user must be approved by administrator
        else if (SendWelcomeEmail)
        {
            if (AdminApprovalRequired)
            {
                template = EmailTemplateProvider.GetEmailTemplate("Membership.RegistrationWaitingForApproval", CurrentSiteName);
                emailMessage.Subject = GetString("RegistrationForm.RegistrationWaitingForApprovalSubject");
            }
            // Send welcome message with username and password, user can logon directly
            else
            {
                template = EmailTemplateProvider.GetEmailTemplate("Membership.Registration", CurrentSiteName);
                emailMessage.Subject = GetString("RegistrationForm.RegistrationSubject");
            }
        }

        if (template != null)
        {
            // Create relation between contact and user. This ensures that contact will be correctly recognized when user approves registration (if approval is required)
            int contactId = ModuleCommands.OnlineMarketingGetCurrentContactID();
            if (contactId > 0)
            {
                var checker = new UserContactDataPropagationChecker();
                Service.Resolve<IContactRelationAssigner>().Assign(ui.UserID, MembershipType.CMS_USER, contactId, checker);
            }

            try
            {
                // Prepare resolver for notification and welcome emails
                MacroResolver resolver = MembershipResolvers.GetMembershipRegistrationResolver(ui, AuthenticationHelper.GetRegistrationApprovalUrl(ApprovalPage, ui.UserGUID, CurrentSiteName, NotifyAdministrator));
                EmailSender.SendEmailWithTemplateText(CurrentSiteName, emailMessage, template, resolver, true);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("E", "RegistrationForm - SendEmail", ex);
                error = true;
            }
        }

        // If there was some error, user must be deleted
        if (error)
        {
            ShowError(GetString("RegistrationForm.UserWasNotCreated"));

            // Email was not send, user can't be approved - delete it
            UserInfoProvider.DeleteUser(ui);
        }
    }


    /// <summary>
    /// Assigns user to roles defined in AssignRoles property.
    /// </summary>
    private void AssignToRoles(UserInfo ui)
    {
        string[] roleList = AssignRoles.Split(';');

        foreach (string siteName in SiteList)
        {
            // Add new user to the current site
            UserInfoProvider.AddUserToSite(ui.UserName, siteName);
            foreach (string roleName in roleList)
            {
                if (!String.IsNullOrEmpty(roleName))
                {
                    String sn = roleName.StartsWith(".", StringComparison.Ordinal) ? String.Empty : siteName;

                    // Add user to desired roles
                    if (RoleInfoProvider.RoleExists(roleName, sn))
                    {
                        UserInfoProvider.AddUserToRole(ui.UserName, roleName, sn);
                    }
                }
            }
        }
    }

    #endregion
}