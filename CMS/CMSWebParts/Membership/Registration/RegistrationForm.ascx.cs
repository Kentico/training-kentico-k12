using System;
using System.Web;

using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.PortalEngine.Web.UI;
using CMS.Protection;
using CMS.SiteProvider;
using CMS.WebAnalytics;


public partial class CMSWebParts_Membership_Registration_RegistrationForm : CMSAbstractWebPart
{
    #region "Text properties and constants"

    private const int EMAIL_MAX_LENGTH = 100;


    /// <summary>
    /// Gets or sets the Skin ID.
    /// </summary>
    public override string SkinID
    {
        get
        {
            return base.SkinID;
        }
        set
        {
            base.SkinID = value;
            SetSkinID(value);
        }
    }


    /// <summary>
    /// Gets or sets the first name text.
    /// </summary>
    public string FirstNameText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("FirstNameText"), ResHelper.LocalizeString("{$Webparts_Membership_RegistrationForm.FirstName$}"));
        }
        set
        {
            SetValue("FirstNameText", value);
            lblFirstName.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the last name text.
    /// </summary>
    public string LastNameText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("LastNameText"), ResHelper.LocalizeString("{$Webparts_Membership_RegistrationForm.LastName$}"));
        }
        set
        {
            SetValue("LastNameText", value);
            lblLastName.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the e-mail text.
    /// </summary>
    public string EmailText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("EmailText"), ResHelper.LocalizeString("{$Webparts_Membership_RegistrationForm.Email$}"));
        }
        set
        {
            SetValue("EmailText", value);
            lblEmail.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the password text.
    /// </summary>
    public string PasswordText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("PasswordText"), ResHelper.LocalizeString("{$Webparts_Membership_RegistrationForm.Password$}"));
        }
        set
        {
            SetValue("PasswordText", value);
            lblPassword.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the confirmation password text.
    /// </summary>
    public string ConfirmPasswordText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ConfirmPasswordText"), ResHelper.LocalizeString("{$Webparts_Membership_RegistrationForm.ConfirmPassword$}"));
        }
        set
        {
            SetValue("ConfirmPasswordText", value);
            lblConfirmPassword.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the button text.
    /// </summary>
    public string ButtonText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ButtonText"), ResHelper.LocalizeString("{$Webparts_Membership_RegistrationForm.Button$}"));
        }
        set
        {
            SetValue("ButtonText", value);
            btnOk.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the captcha label text.
    /// </summary>
    public string CaptchaText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("CaptchaText"), ResHelper.LocalizeString("{$Webparts_Membership_RegistrationForm.Captcha$}"));
        }
        set
        {
            SetValue("CaptchaText", value);
            lblCaptcha.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets registration approval page URL.
    /// </summary>
    public string ApprovalPage
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ApprovalPage"), "");
        }
        set
        {
            SetValue("ApprovalPage", value);
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
            return ValidationHelper.GetString(GetValue("AssignToRoles"), "");
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
            return ValidationHelper.GetString(GetValue("AssignToSites"), "");
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
            return ValidationHelper.GetString(GetValue("DisplayMessage"), "");
        }
        set
        {
            SetValue("DisplayMessage", value);
        }
    }


    /// <summary>
    /// Gets or set the url where is user redirected after successful registration.
    /// </summary>
    public string RedirectToURL
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RedirectToURL"), "");
        }
        set
        {
            SetValue("RedirectToURL", value);
        }
    }


    /// <summary>
    /// Gets or sets value that indicates whether the captcha image should be displayed.
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
            plcCaptcha.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the default starting alias path for newly registered user.
    /// </summary>
    public string StartingAliasPath
    {
        get
        {
            return ValidationHelper.GetString(GetValue("StartingAliasPath"), "");
        }
        set
        {
            SetValue("StartingAliasPath", value);
        }
    }


    /// <summary>
    /// Gets or sets the password minimal length.
    /// </summary>
    public int PasswordMinLength
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PasswordMinLength"), 0);
        }
        set
        {
            SetValue("PasswordMinLength", value);
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
            return ValidationHelper.GetString(GetValue("TrackConversionName"), "");
        }
        set
        {
            if (value.Length > 400)
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
            // Do not process
            rfvFirstName.Enabled = false;
            rfvEmail.Enabled = false;
            rfvConfirmPassword.Enabled = false;
            rfvLastName.Enabled = false;
        }
        else
        {
            // Set default visibility
            pnlForm.Visible = true;
            lblText.Visible = false;

            // Set texts
            lblFirstName.Text = FirstNameText;
            lblLastName.Text = LastNameText;
            lblEmail.Text = EmailText;
            lblPassword.Text = PasswordText;
            lblConfirmPassword.Text = ConfirmPasswordText;
            btnOk.Text = ButtonText;
            lblCaptcha.Text = CaptchaText;

            if (MFAuthenticationHelper.IsMultiFactorAuthEnabled && !MFAuthenticationHelper.IsMultiFactorAuthRequired)
            {
                plcMFIsRequired.Visible = true;
                chkUseMultiFactorAutentization.ToolTip = GetString("webparts_membership_registrationform.mfexplanationtext");
            }
            // Set required field validators texts
            rfvFirstName.ErrorMessage = GetString("Webparts_Membership_RegistrationForm.rfvFirstName");
            rfvLastName.ErrorMessage = GetString("Webparts_Membership_RegistrationForm.rfvLastName");
            rfvEmail.ErrorMessage = GetString("Webparts_Membership_RegistrationForm.rfvEmail");
            rfvConfirmPassword.ErrorMessage = GetString("Webparts_Membership_RegistrationForm.rfvConfirmPassword");

            // Add unique validation form
            rfvFirstName.ValidationGroup = ClientID + "_registration";
            rfvLastName.ValidationGroup = ClientID + "_registration";
            rfvEmail.ValidationGroup = ClientID + "_registration";
            passStrength.ValidationGroup = ClientID + "_registration";
            rfvConfirmPassword.ValidationGroup = ClientID + "_registration";
            btnOk.ValidationGroup = ClientID + "_registration";


            // Set SkinID
            if (!StandAlone && (PageCycle < PageCycleEnum.Initialized))
            {
                SetSkinID(SkinID);
            }

            plcCaptcha.Visible = DisplayCaptcha;

            // WAI validation
            lblPassword.AssociatedControlClientID = passStrength.InputClientID;
        }

        // Register client side validation
        txtEmail.RegisterCustomValidator(rfvEmail);
    }


    /// <summary>
    /// Sets SkinID.
    /// </summary>
    private void SetSkinID(string skinId)
    {
        if (skinId != "")
        {
            lblFirstName.SkinID = skinId;
            lblLastName.SkinID = skinId;
            lblEmail.SkinID = skinId;
            lblPassword.SkinID = skinId;
            lblConfirmPassword.SkinID = skinId;
            txtFirstName.SkinID = skinId;
            txtLastName.SkinID = skinId;
            txtEmail.SkinID = skinId;
            passStrength.SkinID = skinId;
            txtConfirmPassword.SkinID = skinId;
            btnOk.SkinID = skinId;
        }
    }


    /// <summary>
    /// OK click handler (Proceed registration).
    /// </summary>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (PortalContext.IsDesignMode(PortalContext.ViewMode) || (HideOnCurrentPage) || (!IsVisible))
        {
            // Do not process
        }
        else
        {
            String siteName = SiteContext.CurrentSiteName;


            #region "Banned IPs"

            // Ban IP addresses which are blocked for registration
            if (!BannedIPInfoProvider.IsAllowed(siteName, BanControlEnum.Registration))
            {
                ShowError(GetString("banip.ipisbannedregistration"));
                return;
            }

            #endregion


            #region "Check Email & password"

            string[] siteList = { siteName };

            // If AssignToSites field set
            if (!String.IsNullOrEmpty(AssignToSites))
            {
                siteList = AssignToSites.Split(';');
            }

            // Check whether another user with this user name (which is effectively email) does not exist
            UserInfo ui = UserInfoProvider.GetUserInfo(txtEmail.Text);
            SiteInfo si = SiteContext.CurrentSite;
            UserInfo siteui = UserInfoProvider.GetUserInfo(UserInfoProvider.EnsureSitePrefixUserName(txtEmail.Text, si));

            if ((ui != null) || (siteui != null))
            {
                ShowError(GetString("Webparts_Membership_RegistrationForm.UserAlreadyExists").Replace("%%name%%", HTMLHelper.HTMLEncode(txtEmail.Text)));
                return;
            }

            // Check whether password is same
            if (passStrength.Text != txtConfirmPassword.Text)
            {
                ShowError(GetString("Webparts_Membership_RegistrationForm.PassworDoNotMatch"));
                return;
            }

            if ((PasswordMinLength > 0) && (passStrength.Text.Length < PasswordMinLength))
            {
                ShowError(String.Format(GetString("Webparts_Membership_RegistrationForm.PasswordMinLength"), PasswordMinLength));
                return;
            }

            if (!passStrength.IsValid())
            {
                ShowError(AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName));
                return;
            }

            if ((!txtEmail.IsValid()) || (txtEmail.Text.Length > EMAIL_MAX_LENGTH))
            {
                ShowError(String.Format(GetString("Webparts_Membership_RegistrationForm.EmailIsNotValid"), EMAIL_MAX_LENGTH));
                return;
            }

            #endregion


            #region "Captcha"

            // Check if captcha is required and verify captcha text
            if (DisplayCaptcha && !scCaptcha.IsValid())
            {
                // Display error message if catcha text is not valid
                ShowError(GetString("Webparts_Membership_RegistrationForm.captchaError"));
                return;
            }

            #endregion


            #region "User properties"

            var userEmail = txtEmail.Text.Trim();

            ui = new UserInfo();
            ui.PreferredCultureCode = "";
            ui.Email = userEmail;
            ui.FirstName = txtFirstName.Text.Trim();
            ui.LastName = txtLastName.Text.Trim();
            ui.FullName = UserInfoProvider.GetFullName(ui.FirstName, String.Empty, ui.LastName);
            ui.MiddleName = "";
            ui.UserMFRequired = chkUseMultiFactorAutentization.Checked;

            // User name as put by user (no site prefix included)
            var plainUserName = userEmail;
            ui.UserName = plainUserName;

            // Check if the given email can be used as user name
            if (!ValidationHelper.IsUserName(plainUserName))
            {
                ShowError(String.Format(GetString("Webparts_Membership_RegistrationForm.UserNameNotValid"), HTMLHelper.HTMLEncode(plainUserName)));
                return;
            }

            // Ensure site prefixes
            if (UserInfoProvider.UserNameSitePrefixEnabled(siteName))
            {
                ui.UserName = UserInfoProvider.EnsureSitePrefixUserName(plainUserName, si);
            }

            ui.Enabled = EnableUserAfterRegistration;
            ui.UserURLReferrer = CookieHelper.GetValue(CookieName.UrlReferrer);
            ui.UserCampaign = Service.Resolve<ICampaignService>().CampaignCode;

            ui.SiteIndependentPrivilegeLevel = UserPrivilegeLevelEnum.None;

            ui.UserSettings.UserRegistrationInfo.IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            ui.UserSettings.UserRegistrationInfo.Agent = HttpContext.Current.Request.UserAgent;

            // Check whether confirmation is required
            bool requiresConfirmation = SettingsKeyInfoProvider.GetBoolValue(siteName + ".CMSRegistrationEmailConfirmation");
            bool requiresAdminApprove = false;

            if (!requiresConfirmation)
            {
                // If confirmation is not required check whether administration approval is required
                requiresAdminApprove = SettingsKeyInfoProvider.GetBoolValue(siteName + ".CMSRegistrationAdministratorApproval");
                if (requiresAdminApprove)
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

            #endregion


            #region "Reserved names"

            // Check for reserved user names like administrator, sysadmin, ...
            if (UserInfoProvider.NameIsReserved(siteName, plainUserName))
            {
                ShowError(GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(ui.UserName, true))));
                return;
            }

            if (UserInfoProvider.NameIsReserved(siteName, ui.UserNickName))
            {
                ShowError(GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(ui.UserNickName)));
                return;
            }

            #endregion


            #region "License limitations"

            string errorMessage = String.Empty;
            UserInfoProvider.CheckLicenseLimitation(ui, ref errorMessage);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                ShowError(errorMessage);
                return;
            }

            #endregion


            // Check whether email is unique if it is required
            if (!UserInfoProvider.IsEmailUnique(userEmail, siteList, 0))
            {
                ShowError(GetString("UserInfo.EmailAlreadyExist"));
                return;
            }

            // Set password
            UserInfoProvider.SetPassword(ui, passStrength.Text);

            #region "Welcome Emails (confirmation, waiting for approval)"

            bool error = false;
            EmailTemplateInfo template = null;

            string emailSubject = null;
            // Send welcome message with username and password, with confirmation link, user must confirm registration
            if (requiresConfirmation)
            {
                template = EmailTemplateProvider.GetEmailTemplate("RegistrationConfirmation", siteName);
                emailSubject = EmailHelper.GetSubject(template, GetString("RegistrationForm.RegistrationConfirmationEmailSubject"));
            }
            // Send welcome message with username and password, with information that user must be approved by administrator
            else if (SendWelcomeEmail)
            {
                if (requiresAdminApprove)
                {
                    template = EmailTemplateProvider.GetEmailTemplate("Membership.RegistrationWaitingForApproval", siteName);
                    emailSubject = EmailHelper.GetSubject(template, GetString("RegistrationForm.RegistrationWaitingForApprovalSubject"));
                }
                // Send welcome message with username and password, user can logon directly
                else
                {
                    template = EmailTemplateProvider.GetEmailTemplate("Membership.Registration", siteName);
                    emailSubject = EmailHelper.GetSubject(template, GetString("RegistrationForm.RegistrationSubject"));
                }
            }

            if (template != null)
            {
                // Create relation between contact and user. This ensures that contact will be correctly recognized when user approves registration (if approval is required)
                // New contact with user data is created in case it is not disabled by Data protection settings, otherwise an anonymous contact is created. Moreover an another
                // anonymous contact could be created during approval process when a new browser is used for approval step (when contact isn't in browser cookies).
                int contactId = ModuleCommands.OnlineMarketingGetCurrentContactID();
                if (contactId > 0)
                {
                    Service.Resolve<IContactRelationAssigner>().Assign(ui.UserID, MembershipType.CMS_USER, contactId, new UserContactDataPropagationChecker());
                }

                // Email message
                EmailMessage email = new EmailMessage();
                email.EmailFormat = EmailFormatEnum.Default;
                email.Recipients = ui.Email;
                email.From = SettingsKeyInfoProvider.GetValue(siteName + ".CMSNoreplyEmailAddress");
                email.Subject = emailSubject;

                try
                {
                    var resolver = MembershipResolvers.GetMembershipRegistrationResolver(ui, AuthenticationHelper.GetRegistrationApprovalUrl(ApprovalPage, ui.UserGUID, siteName, NotifyAdministrator));
                    EmailSender.SendEmailWithTemplateText(siteName, email, template, resolver, true);
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
                return;
            }

            #endregion


            #region "Administrator notification email"

            // Notify administrator if enabled and e-mail confirmation is not required
            if (!requiresConfirmation && NotifyAdministrator && (FromAddress != String.Empty) && (ToAddress != String.Empty))
            {
                EmailTemplateInfo mEmailTemplate;
                if (SettingsKeyInfoProvider.GetBoolValue(siteName + ".CMSRegistrationAdministratorApproval"))
                {
                    mEmailTemplate = EmailTemplateProvider.GetEmailTemplate("Registration.Approve", siteName);
                }
                else
                {
                    mEmailTemplate = EmailTemplateProvider.GetEmailTemplate("Registration.New", siteName);
                }

                if (mEmailTemplate == null)
                {
                    // Log missing e-mail template
                    EventLogProvider.LogEvent(EventType.ERROR, "RegistrationForm", "GetEmailTemplate", eventUrl: RequestContext.RawURL);
                }
                else
                {
                    EmailMessage message = new EmailMessage();
                    message.EmailFormat = EmailFormatEnum.Default;
                    message.From = EmailHelper.GetSender(mEmailTemplate, FromAddress);
                    message.Recipients = ToAddress;
                    message.Subject = GetString("RegistrationForm.EmailSubject");

                    try
                    {
                        MacroResolver resolver = MembershipResolvers.GetRegistrationResolver(ui);
                        EmailSender.SendEmailWithTemplateText(siteName, message, mEmailTemplate, resolver, false);
                    }
                    catch
                    {
                        EventLogProvider.LogEvent(EventType.ERROR, "Membership", "RegistrationEmail");
                    }
                }
            }

            #endregion


            #region "Web analytics"

            // Track successful registration conversion
            if (TrackConversionName != String.Empty)
            {
                if (AnalyticsHelper.AnalyticsEnabled(siteName) && Service.Resolve<IAnalyticsConsentProvider>().HasConsentForLogging()
                    && !AnalyticsHelper.IsIPExcluded(siteName, RequestContext.UserHostAddress)
                    && SystemContext.IsCMSRunningAsMainApplication)
                {
                    // Log conversion
                    HitLogProvider.LogConversions(siteName, LocalizationContext.PreferredCultureCode, TrackConversionName, 0, ConversionValue);
                }
            }

            // Log registered user if confirmation is not required
            if (!requiresConfirmation)
            {
                AnalyticsHelper.LogRegisteredUser(siteName, ui);
            }

            #endregion


            #region "On-line marketing - activity"

            // Log registered user if confirmation is not required
            if (!requiresConfirmation)
            {
                IMembershipActivityLogger logger = Service.Resolve<IMembershipActivityLogger>();
                logger.LogRegistration(ui.UserName, DocumentContext.CurrentDocument);
                // Log login activity
                if (ui.Enabled)
                {
                    logger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);
                }
            }

            #endregion


            #region "Roles & authentication"

            string[] roleList = AssignRoles.Split(';');

            foreach (string sn in siteList)
            {
                // Add new user to the current site
                UserInfoProvider.AddUserToSite(ui.UserName, sn);
                foreach (string roleName in roleList)
                {
                    if (!String.IsNullOrEmpty(roleName))
                    {
                        String s = roleName.StartsWith(".", StringComparison.Ordinal) ? "" : sn;

                        // Add user to desired roles
                        if (RoleInfoProvider.RoleExists(roleName, s))
                        {
                            UserInfoProvider.AddUserToRole(ui.UserName, roleName, s);
                        }
                    }
                }
            }

            if (DisplayMessage.Trim() != String.Empty)
            {
                pnlForm.Visible = false;
                lblText.Visible = true;
                lblText.Text = DisplayMessage;
            }
            else
            {
                if (ui.Enabled)
                {
                    AuthenticationHelper.AuthenticateUser(ui.UserName, true);
                }

                if (RedirectToURL != String.Empty)
                {
                    URLHelper.Redirect(UrlResolver.ResolveUrl(RedirectToURL));
                }

                else if (QueryHelper.GetString("ReturnURL", "") != String.Empty)
                {
                    string url = QueryHelper.GetString("ReturnURL", "");

                    // Do url decode
                    url = Server.UrlDecode(url);

                    // Check that url is relative path or hash is ok
                    if (url.StartsWith("~", StringComparison.Ordinal) || url.StartsWith("/", StringComparison.Ordinal) || QueryHelper.ValidateHash("hash", "aliaspath"))
                    {
                        URLHelper.Redirect(UrlResolver.ResolveUrl(url));
                    }
                    // Absolute path with wrong hash
                    else
                    {
                        URLHelper.Redirect(AdministrationUrlHelper.GetErrorPageUrl("dialogs.badhashtitle", "dialogs.badhashtext"));
                    }
                }
            }

            #endregion

            lblError.Visible = false;
        }
    }


    /// <summary>
    /// Shows the specified error message.
    /// </summary>
    private void ShowError(string errorMessage)
    {
        lblError.Visible = true;
        lblError.Text = errorMessage;
    }

    #endregion
}