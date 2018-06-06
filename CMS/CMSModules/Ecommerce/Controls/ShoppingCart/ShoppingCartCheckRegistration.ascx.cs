using System;
using System.Web;

using CMS.Activities.Loggers;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Protection;
using CMS.SiteProvider;
using CMS.WebAnalytics;


public partial class CMSModules_Ecommerce_Controls_ShoppingCart_ShoppingCartCheckRegistration : ShoppingCartStep
{
    // Variables
    private bool mDataLoaded = false;
    private bool mRequireOrgTaxRegIDs = false;
    private bool mShowTaxRegistrationIDField = false;
    private bool mShowOrganizationIDField = false;
    private IMembershipActivityLogger mMembershipActivityLogger;


    private IMembershipActivityLogger MembershipActivityLogger => mMembershipActivityLogger ?? (mMembershipActivityLogger = Service.Resolve<IMembershipActivityLogger>());


    protected override void OnPreRender(EventArgs e)
    {
        // Prepare script for showing/hiding form
        string script = null;
        if (radSignIn.Checked)
        {
            script = ScriptHelper.GetScript("showHideForm('tblSignIn','" + radSignIn.ClientID + "');");
        }
        if (radNewReg.Checked)
        {
            script = ScriptHelper.GetScript("showHideForm('tblRegistration','" + radNewReg.ClientID + "');");
        }
        if (radAnonymous.Checked)
        {
            script = ScriptHelper.GetScript("showHideForm('tblAnonymous','" + radAnonymous.ClientID + "');");
        }

        ScriptHelper.RegisterStartupScript(Page, typeof(string), "ShowHideFormInit", script);

        txtUsername.EnableAutoComplete = SecurityHelper.IsAutoCompleteEnabledForLogin(SiteContext.CurrentSiteName);

        base.OnPreRender(e);
    }


    /// <summary>
    /// On page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptHelper.RegisterClientScriptBlock(this, GetType(), "showHide", ScriptHelper.GetScript(@"
            /* Shows and hides tables with forms*/
            function showHideForm(obj, rad)
            {
                var tblSignInStat = '';
                var tblRegistrationStat = '';
                var tblAnonymousStat = '';
                if( obj != null && obj != '' && rad != null)
                {
                    switch(obj)
                    {
                        case 'tblSignIn':
                            tblSignInStat = '';
                            tblRegistrationStat = 'none';
                            tblAnonymousStat = 'none';
                            break;

                        case 'tblRegistration':
                            tblSignInStat = 'none';
                            tblRegistrationStat = '';
                            tblAnonymousStat = 'none';
                            break;

                        case 'tblAnonymous':
                            tblSignInStat = 'none';
                            tblRegistrationStat = 'none';
                            tblAnonymousStat = '';
                            break;                
                    }

                    if(document.getElementById('tblSignIn') != null)
                        document.getElementById('tblSignIn').style.display = tblSignInStat;
                    if(document.getElementById('tblRegistration') != null)
                        document.getElementById('tblRegistration').style.display = tblRegistrationStat;
                    if(document.getElementById('tblAnonymous') != null)
                        document.getElementById('tblAnonymous').style.display = tblAnonymousStat;
                    if(document.getElementById(rad) != null)
                        document.getElementById(rad).setAttribute('checked','true');
                }
            }
            function showElem(id)
            {
                style = document.getElementById(id).style;
                style.display = (style.display == 'block')?'none':'block';
                return false;
            }
            function showHideChk(id)
            {
                var elem = document.getElementById(id);
                if(elem.style.display == 'block')
                {
                    elem.style.display = 'none';
                }
                else
                {
                    elem.style.display = 'block';
                }
            }"));

        // Get settings for current site
        SiteInfo si = SiteContext.CurrentSite;
        if (si != null)
        {
            mRequireOrgTaxRegIDs = ECommerceSettings.RequireCompanyInfo(si.SiteName);
            mShowOrganizationIDField = ECommerceSettings.ShowOrganizationID(si.SiteName);
            mShowTaxRegistrationIDField = ECommerceSettings.ShowTaxRegistrationID(si.SiteName);
        }

        PreRender += CMSEcommerce_ShoppingCartCheckRegistration_PreRender;
        InitializeLabels();

        LoadStep(false);

        // Initialize onclick events
        radSignIn.Attributes.Add("onclick", "showHideForm('tblSignIn','" + radSignIn.ClientID + "');");
        radNewReg.Attributes.Add("onclick", "showHideForm('tblRegistration','" + radNewReg.ClientID + "');");
        radAnonymous.Attributes.Add("onclick", "showHideForm('tblAnonymous','" + radAnonymous.ClientID + "');");
        lnkPasswdRetrieval.Attributes.Add("onclick", "return showElem('" + pnlPasswdRetrieval.ClientID + "');");
    }


    private void CMSEcommerce_ShoppingCartCheckRegistration_PreRender(object sender, EventArgs e)
    {
        if (!mDataLoaded && !ShoppingCartControl.IsCurrentStepPostBack)
        {
            LoadData();
        }
    }


    protected void LoadData()
    {
        mDataLoaded = true;
        // If user ID specified, load the given user ID
        if (!ShoppingCartControl.UserInfo.IsPublic())
        {
            // Get the customer data
            CustomerInfo ci = CustomerInfoProvider.GetCustomerInfoByUserID(ShoppingCartControl.UserInfo.UserID);

            // Set the fields
            if (ci != null)
            {
                txtEditCompany.Text = ci.CustomerCompany;
                txtEditEmail.Text = ci.CustomerEmail;
                txtEditFirst.Text = ci.CustomerFirstName;
                txtEditLast.Text = ci.CustomerLastName;
                txtEditOrgID.Text = ci.CustomerOrganizationID;
                txtEditTaxRegID.Text = ci.CustomerTaxRegistrationID;

                if (ci.CustomerHasCompanyInfo)
                {
                    chkEditCorpBody.Checked = true;
                    pnlCompanyAccount2.Visible = true;
                }
            }
            else
            {
                txtEditFirst.Text = ShoppingCartControl.UserInfo.FirstName;
                txtEditLast.Text = ShoppingCartControl.UserInfo.LastName;
                txtEditEmail.Text = ShoppingCartControl.UserInfo.Email;
            }
        }
    }


    /// <summary>
    /// Loads anonymous customer data from view state.
    /// </summary>
    protected void LoadAnonymousCustomerData()
    {
        if (ShoppingCart.Customer != null)
        {
            txtFirstName2.Text = ShoppingCart.Customer.CustomerFirstName;
            txtLastName2.Text = ShoppingCart.Customer.CustomerLastName;
            txtEmail3.Text = ShoppingCart.Customer.CustomerEmail;
            txtCompany2.Text = ShoppingCart.Customer.CustomerCompany;
            txtOrganizationID2.Text = ShoppingCart.Customer.CustomerOrganizationID;
            txtTaxRegistrationID2.Text = ShoppingCart.Customer.CustomerTaxRegistrationID;

            if (!string.IsNullOrEmpty(txtCompany2.Text))
            {
                chkCorporateBody2.Checked = true;
                plcCompanyAccount3.Visible = true;
            }
        }
    }


    protected void LoadStep(bool loadData)
    {
        // If user logged in, edit the customer data
        if (!ShoppingCartControl.UserInfo.IsPublic())
        {
            plcEditCustomer.Visible = true;
            plcEditOrgID.Visible = mShowOrganizationIDField;
            plcEditTaxRegID.Visible = mShowTaxRegistrationIDField;
            plcAccount.Visible = false;

            if (loadData)
            {
                LoadData();
            }
        }
        else
        {
            // Display/Hide the form for anonymous customer
            if (SiteContext.CurrentSite != null)
            {
                plhAnonymous.Visible = ECommerceSettings.AllowAnonymousCustomers(SiteContext.CurrentSite.SiteName);
            }

            if (!ShoppingCartControl.IsCurrentStepPostBack)
            {
                // If anonymous customer data were already saved -> display them
                if ((plhAnonymous.Visible) && (ShoppingCart.ShoppingCartCustomerID > 0))
                {
                    // Mark 'Continue as anonymous customer' radio button
                    radAnonymous.Checked = true;

                    LoadAnonymousCustomerData();
                }
                else
                {
                    // Mark 'Sign in using your existing account' radio button
                    radSignIn.Checked = true;
                }
            }

            plcEditCustomer.Visible = false;
            plcAccount.Visible = true;

            plcTaxRegistrationID.Visible = mShowTaxRegistrationIDField;
            plcOrganizationID.Visible = mShowOrganizationIDField;

            plcTaxRegistrationID2.Visible = mShowTaxRegistrationIDField;
            plcOrganizationID2.Visible = mShowOrganizationIDField;

            // Set strings
            lnkPasswdRetrieval.Text = GetString("LogonForm.lnkPasswordRetrieval");
            lblPasswdRetrieval.Text = GetString("LogonForm.lblPasswordRetrieval");
            btnPasswdRetrieval.Text = GetString("LogonForm.btnPasswordRetrieval");

            lnkPasswdRetrieval.Visible = ShoppingCartControl.EnablePasswordRetrieval;
            btnPasswdRetrieval.Click += btnPasswdRetrieval_Click;

            pnlPasswdRetrieval.Attributes.Add("style", "display:none;");
        }
    }


    /// <summary>
    /// Retrieve the user password.
    /// </summary>
    private void btnPasswdRetrieval_Click(object sender, EventArgs e)
    {
        string value = txtPasswordRetrieval.Text.Trim();
        if (!String.IsNullOrEmpty(value) && ValidationHelper.IsEmail(value) && (SiteContext.CurrentSite != null))
        {
            AuthenticationHelper.ForgottenEmailRequest(value, SiteContext.CurrentSiteName, "ECOMMERCE", ECommerceSettings.SendEmailsFrom(SiteContext.CurrentSite.SiteName), null, AuthenticationHelper.GetResetPasswordUrl(SiteContext.CurrentSiteName));

            lblResult.Text = String.Format(GetString("LogonForm.EmailSent"), value);
            plcResult.Visible = true;
            plcErrorResult.Visible = false;

            pnlPasswdRetrieval.Attributes.Add("style", "display:block;");
        }
        else
        {
            lblErrorResult.Text = String.Format(GetString("LogonForm.EmailNotValid"), value);
            plcErrorResult.Visible = true;
            plcResult.Visible = false;

            pnlPasswdRetrieval.Attributes.Add("style", "display:block;");
        }
    }


    /// <summary>
    /// Initialization of labels.
    /// </summary>
    protected void InitializeLabels()
    {
        if (mRequireOrgTaxRegIDs)
        {
            lblOrganizationID.Text = GetString("ShoppingCartCheckRegistration.lblOrganizationIDRequired");
            lblTaxRegistrationID.Text = GetString("ShoppingCartCheckRegistration.lblTaxRegistrationIDRequired");
            // Show required field marks
            lblMark15.Visible = true;
            lblMark16.Visible = true;
            lblMark17.Visible = true;
            lblMark21.Visible = true;
            lblMark22.Visible = true;
            lblMark23.Visible = true;

            lblEditCompany.Text = GetString("ShoppingCartCheckRegistration.CompanyRequired");
            lblEditOrgID.Text = lblOrganizationID.Text;
            lblEditTaxRegID.Text = lblTaxRegistrationID.Text;
            // Show required field marks
            lblMark18.Visible = true;
            lblMark19.Visible = true;
            lblMark20.Visible = true;
        }
        else
        {
            lblOrganizationID.Text = GetString("ShoppingCartCheckRegistration.lblOrganizationID");
            lblTaxRegistrationID.Text = GetString("ShoppingCartCheckRegistration.lblTaxRegistrationID");
            // Show required field marks
            lblMark15.Visible = false;
            lblMark16.Visible = false;
            lblMark17.Visible = false;

            lblEditCompany.Text = GetString("ShoppingCartCheckRegistration.CompanyRequired");
            lblEditOrgID.Text = lblOrganizationID.Text;
            lblEditTaxRegID.Text = lblTaxRegistrationID.Text;
            // Show required field marks
            lblMark18.Visible = false;
            lblMark19.Visible = false;
            lblMark20.Visible = false;
        }

        radSignIn.Text = GetString("ShoppingCartCheckRegistration.SignIn");
        lblUsername.Text = GetString("ShoppingCartCheckRegistration.Username");
        lblPsswd1.Text = GetString("ShoppingCartCheckRegistration.Psswd");
        radNewReg.Text = GetString("ShoppingCartCheckRegistration.NewReg");
        lblFirstName1.Text = GetString("ShoppingCartCheckRegistration.FirstName");
        lblLastName1.Text = GetString("ShoppingCartCheckRegistration.LastName");
        lblEmail2.Text = GetString("ShoppingCartCheckRegistration.EmailUsername");
        lblPsswd2.Text = lblPsswd1.Text;
        lblConfirmPsswd.Text = GetString("ShoppingCartCheckRegistration.ConfirmPsswd");
        radAnonymous.Text = GetString("ShoppingCartCheckRegistration.Anonymous");
        lblFirstName2.Text = lblFirstName1.Text;
        lblLastName2.Text = lblLastName1.Text;

        lblEditFirst.Text = lblFirstName1.Text;
        lblEditLast.Text = lblLastName1.Text;

        lblTaxRegistrationID2.Text = lblTaxRegistrationID.Text;

        lblOrganizationID2.Text = lblOrganizationID.Text;

        lblCorporateBody.Text = GetString("ShoppingCartCheckRegistration.lblCorporateBody");
        lblEditCorpBody.Text = lblCorporateBody.Text;

        // Mark required fields
        if (ShoppingCartControl.RequiredFieldsMark != "")
        {
            string mark = ShoppingCartControl.RequiredFieldsMark;
            lblMark1.Text = mark;
            lblMark2.Text = mark;
            lblMark3.Text = mark;
            lblMark4.Text = mark;
            lblMark5.Text = mark;
            lblMark6.Text = mark;
            passStrength.RequiredFieldMark = mark;
            lblMark8.Text = mark;
            lblMark9.Text = mark;
            lblMark10.Text = mark;
            lblMark11.Text = mark;
            lblMark12.Text = mark;
            lblMark13.Text = mark;
            lblMark14.Text = mark;
            lblMark15.Text = mark;
            lblMark16.Text = mark;
            lblMark17.Text = mark;
            lblMark18.Text = mark;
            lblMark19.Text = mark;
            lblMark20.Text = mark;
            lblMark21.Text = mark;
            lblMark22.Text = mark;
            lblMark23.Text = mark;
        }
    }


    /// <summary>
    /// On chkCorporateBody checkbox checked changed.
    /// </summary>
    protected void chkCorporateBody_CheckChanged(object sender, EventArgs e)
    {
        pnlCompanyAccount1.Visible = chkCorporateBody.Checked;
    }


    /// <summary>
    /// On chkCorporateBody2 checkbox checked changed.
    /// </summary>
    protected void chkCorporateBody2_CheckChanged(object sender, EventArgs e)
    {
        plcCompanyAccount3.Visible = chkCorporateBody2.Checked;
    }


    /// <summary>
    /// On chkEditCorpBody checkbox checked changed.
    /// </summary>
    protected void chkEditCorpBody_CheckChanged(object sender, EventArgs e)
    {
        pnlCompanyAccount2.Visible = chkEditCorpBody.Checked;
    }


    /// <summary>
    /// Validate values in textboxes.
    /// </summary>
    public override bool IsValid()
    {
        Validator val = new Validator();
        string result = null;

        if (plcAccount.Visible)
        {
            // Validate registration data
            if (radSignIn.Checked)
            {
                ScriptHelper.RegisterStartupScript(this, GetType(), "checkSignIn", ScriptHelper.GetScript("showHideForm('tblSignIn','" + radSignIn.ClientID + "');"));

                // Check banned IP
                if (!BannedIPInfoProvider.IsAllowed(SiteContext.CurrentSiteName, BanControlEnum.Login))
                {
                    result = GetString("banip.ipisbannedlogin");
                }

                // Check user name
                if (string.IsNullOrEmpty(result))
                {
                    result = val.NotEmpty(txtUsername.Text.Trim(), GetString("ShoppingCartCheckRegistration.ErrorMissingUsername")).Result;
                }

                if (!string.IsNullOrEmpty(result))
                {
                    lblError.Text = result;
                    lblError.Visible = true;
                    return false;
                }
            }
            // Check 'New registration' section
            else if (radNewReg.Checked)
            {
                ScriptHelper.RegisterStartupScript(this, GetType(), "checkRegistration", ScriptHelper.GetScript("showHideForm('tblRegistration','" + radNewReg.ClientID + "');"));

                // Check banned IP
                if (!BannedIPInfoProvider.IsAllowed(SiteContext.CurrentSiteName, BanControlEnum.Registration))
                {
                    result = GetString("banip.ipisbannedregistration");
                }

                if (string.IsNullOrEmpty(result) && !BannedIPInfoProvider.IsAllowed(SiteContext.CurrentSiteName, BanControlEnum.Login))
                {
                    result = GetString("banip.ipisbannedlogin");
                }

                // Check registration form
                if (string.IsNullOrEmpty(result))
                {
                    result = val.NotEmpty(txtFirstName1.Text.Trim(), GetString("ShoppingCartCheckRegistration.FirstNameErr"))
                        .NotEmpty(txtLastName1.Text.Trim(), GetString("ShoppingCartCheckRegistration.LastNameErr"))
                        .NotEmpty(txtEmail2.Text.Trim(), GetString("ShoppingCartCheckRegistration.EmailErr"))
                        .NotEmpty(passStrength.Text.Trim(), GetString("ShoppingCartCheckRegistration.PsswdErr")).Result;
                }

                // Check company properties
                if (string.IsNullOrEmpty(result) && mRequireOrgTaxRegIDs && chkCorporateBody.Checked)
                {
                    result = val.NotEmpty(txtCompany1.Text.Trim(), GetString("ShoppingCartCheckRegistration.CompanyErr")).Result;
                    if ((result == "") && plcOrganizationID.Visible)
                    {
                        result = val.NotEmpty(txtOrganizationID.Text.Trim(), GetString("ShoppingCartCheckRegistration.OrganizationIDErr")).Result;
                    }

                    if ((result == "") && plcTaxRegistrationID.Visible)
                    {
                        result = val.NotEmpty(txtTaxRegistrationID.Text.Trim(), GetString("ShoppingCartCheckRegistration.TaxRegistrationIDErr")).Result;
                    }
                }
                if (result == "")
                {
                    if (!ValidationHelper.IsEmail(txtEmail2.Text.Trim(), true))
                    {
                        lblEmail2Err.Text = GetString("ShoppingCartCheckRegistration.EmailErr");
                        lblEmail2Err.Visible = true;
                    }
                    // Password and confirmed password must be same
                    if (passStrength.Text != txtConfirmPsswd.Text)
                    {
                        lblPsswdErr.Text = GetString("ShoppingCartCheckRegistration.DifferentPsswds");
                        lblPsswdErr.Visible = true;
                    }

                    // Check policy
                    if (!passStrength.IsValid())
                    {
                        lblPsswdErr.Text = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName);
                        lblPsswdErr.Visible = true;
                    }


                    if ((!DataHelper.IsEmpty(lblEmail2Err.Text.Trim())) || (!DataHelper.IsEmpty(lblPsswdErr.Text.Trim())))
                    {
                        return false;
                    }
                }
                else
                {
                    lblError.Text = result;
                    lblError.Visible = true;
                    return false;
                }
            }
            // Check 'Continue as anonymous customer' section
            else if (radAnonymous.Checked)
            {
                ScriptHelper.RegisterStartupScript(this, GetType(), "checkAnonymous", ScriptHelper.GetScript("showHideForm('tblAnonymous','" + radAnonymous.ClientID + "');"));

                result = val.NotEmpty(txtFirstName2.Text.Trim(), GetString("ShoppingCartCheckRegistration.FirstNameErr"))
                    .NotEmpty(txtLastName2.Text.Trim(), GetString("ShoppingCartCheckRegistration.LastNameErr"))
                    .NotEmpty(txtEmail3.Text.Trim(), GetString("ShoppingCartCheckRegistration.EmailErr")).Result;

                if (result == "" && mRequireOrgTaxRegIDs && chkCorporateBody2.Checked)
                {
                    result = val.NotEmpty(txtCompany2.Text.Trim(), ResHelper.GetString("ShoppingCartCheckRegistration.CompanyErr")).Result;
                    // Check organization ID only if visible
                    if ((result == "") && plcOrganizationID2.Visible)
                    {
                        result = val.NotEmpty(txtOrganizationID2.Text.Trim(), ResHelper.GetString("ShoppingCartCheckRegistration.OrganizationIDErr")).Result;
                    }
                    // Check tax ID only if visible
                    if ((result == "") && plcTaxRegistrationID2.Visible)
                    {
                        result = val.NotEmpty(txtTaxRegistrationID2.Text.Trim(), ResHelper.GetString("ShoppingCartCheckRegistration.TaxRegistrationIDErr")).Result;
                    }
                }

                if (result == "")
                {
                    if (!ValidationHelper.IsEmail(txtEmail3.Text.Trim(), true))
                    {
                        lblEmail3Err.Text = GetString("ShoppingCartCheckRegistration.EmailErr");
                        lblEmail3Err.Visible = true;
                        return false;
                    }
                }
                else
                {
                    lblError.Text = result;
                    lblError.Visible = true;
                    return false;
                }
            }
        }
        else
        {
            // Validate customer data
            result = val.NotEmpty(txtEditFirst.Text.Trim(), GetString("ShoppingCartCheckRegistration.FirstNameErr"))
                .NotEmpty(txtEditLast.Text.Trim(), GetString("ShoppingCartCheckRegistration.LastNameErr"))
                .IsEmail(txtEditEmail.Text.Trim(), GetString("ShoppingCartCheckRegistration.EmailErr"), true).Result;

            if (result == "" && mRequireOrgTaxRegIDs && chkEditCorpBody.Checked)
            {
                result = val.NotEmpty(txtEditCompany.Text.Trim(), GetString("ShoppingCartCheckRegistration.CompanyErr")).Result;
                // Check organization id only if visible
                if ((result == "") && plcEditOrgID.Visible)
                {
                    result = val.NotEmpty(txtEditOrgID.Text.Trim(), GetString("ShoppingCartCheckRegistration.OrganizationIDErr")).Result;
                }
                // Check tax id only if visible
                if ((result == "") && plcEditTaxRegID.Visible)
                {
                    result = val.NotEmpty(txtEditTaxRegID.Text.Trim(), GetString("ShoppingCartCheckRegistration.TaxRegistrationIDErr")).Result;
                }
            }
            if (result == "")
            {
                return true;
            }
            else
            {
                lblError.Text = result;
                lblError.Visible = true;
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// Process valid values of this step.
    /// </summary>
    public override bool ProcessStep()
    {
        if (plcAccount.Visible)
        {
            string siteName = SiteContext.CurrentSiteName;

            // Existing account
            if (radSignIn.Checked)
            {
                // Authenticate user
                UserInfo ui = AuthenticationHelper.AuthenticateUser(txtUsername.Text.Trim(), txtPsswd1.Text, SiteContext.CurrentSiteName, false);
                if (ui == null)
                {
                    lblError.Text = GetString("ShoppingCartCheckRegistration.LoginFailed");
                    lblError.Visible = true;
                    return false;
                }

                // Sign in customer with existing account
                AuthenticationHelper.AuthenticateUser(ui.UserName, false);

                // Registered user has already started shopping as anonymous user -> Drop his stored shopping cart
                ShoppingCartInfoProvider.DeleteShoppingCartInfo(ui.UserID, siteName);

                // Assign current user to the current shopping cart
                ShoppingCart.User = ui;

                // Save changes to database
                if (!ShoppingCartControl.IsInternalOrder)
                {
                    ShoppingCartInfoProvider.SetShoppingCartInfo(ShoppingCart);
                }

                // Log "login" activity
                MembershipActivityLogger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);

                LoadStep(true);

                // Return false to get to Edit customer page
                return false;
            }
            // New registration
            else if (radNewReg.Checked)
            {
                txtEmail2.Text = txtEmail2.Text.Trim();
                pnlCompanyAccount1.Visible = chkCorporateBody.Checked;

                string[] siteList = { siteName };

                // If AssignToSites field set
                if (!String.IsNullOrEmpty(ShoppingCartControl.AssignToSites))
                {
                    siteList = ShoppingCartControl.AssignToSites.Split(';');
                }

                // Check if user exists
                UserInfo ui = UserInfoProvider.GetUserInfo(txtEmail2.Text);
                if (ui != null)
                {
                    lblError.Visible = true;
                    lblError.Text = GetString("ShoppingCartUserRegistration.ErrorUserExists");
                    return false;
                }

                // Check all sites where user will be assigned
                if (!UserInfoProvider.IsEmailUnique(txtEmail2.Text.Trim(), siteList, 0))
                {
                    lblError.Visible = true;
                    lblError.Text = GetString("UserInfo.EmailAlreadyExist");
                    return false;
                }

                // Create new customer and user account and sign in
                // User
                ui = new UserInfo();
                ui.UserName = txtEmail2.Text.Trim();
                ui.Email = txtEmail2.Text.Trim();
                ui.FirstName = txtFirstName1.Text.Trim();
                ui.LastName = txtLastName1.Text.Trim();
                ui.FullName = ui.FirstName + " " + ui.LastName;
                ui.Enabled = true;
                ui.SiteIndependentPrivilegeLevel = UserPrivilegeLevelEnum.None;
                ui.UserURLReferrer = CookieHelper.GetValue(CookieName.UrlReferrer);
                ui.UserCampaign = Service.Resolve<ICampaignService>().CampaignCode;
                ui.UserSettings.UserRegistrationInfo.IPAddress = RequestContext.UserHostAddress;
                ui.UserSettings.UserRegistrationInfo.Agent = HttpContext.Current.Request.UserAgent;

                try
                {
                    UserInfoProvider.SetPassword(ui, passStrength.Text);

                    foreach (string site in siteList)
                    {
                        UserInfoProvider.AddUserToSite(ui.UserName, site);

                        // Add user to roles
                        if (ShoppingCartControl.AssignToRoles != "")
                        {
                            AssignUserToRoles(ui.UserName, ShoppingCartControl.AssignToRoles, site);
                        }
                    }

                    // Log registered user
                    AnalyticsHelper.LogRegisteredUser(siteName, ui);

                    MembershipActivityLogger.LogRegistration(ui.UserName, DocumentContext.CurrentDocument);
                }
                catch (Exception ex)
                {
                    lblError.Visible = true;
                    lblError.Text = ex.Message;
                    return false;
                }

                // Customer
                CustomerInfo ci = new CustomerInfo();
                ci.CustomerFirstName = txtFirstName1.Text.Trim();
                ci.CustomerLastName = txtLastName1.Text.Trim();
                ci.CustomerEmail = txtEmail2.Text.Trim();

                ci.CustomerCompany = "";
                ci.CustomerOrganizationID = "";
                ci.CustomerTaxRegistrationID = "";
                if (chkCorporateBody.Checked)
                {
                    ci.CustomerCompany = txtCompany1.Text.Trim();
                    if (mShowOrganizationIDField)
                    {
                        ci.CustomerOrganizationID = txtOrganizationID.Text.Trim();
                    }
                    if (mShowTaxRegistrationIDField)
                    {
                        ci.CustomerTaxRegistrationID = txtTaxRegistrationID.Text.Trim();
                    }
                }

                ci.CustomerUserID = ui.UserID;
                ci.CustomerSiteID = 0;
                ci.CustomerCreated = DateTime.Now;
                CustomerInfoProvider.SetCustomerInfo(ci);

                // Track successful registration conversion
                string name = ShoppingCartControl.RegistrationTrackConversionName;
                ECommerceHelper.TrackRegistrationConversion(ShoppingCart.SiteName, name);

                CreateContactRelation(ci);

                // Sign in
                if (ui.Enabled)
                {
                    AuthenticationHelper.AuthenticateUser(ui.UserName, false);
                    ShoppingCart.User = ui;

                    MembershipActivityLogger.LogLogin(ui.UserName, DocumentContext.CurrentDocument);
                }

                ShoppingCart.ShoppingCartCustomerID = ci.CustomerID;

                // Send new registration notification email
                if (ShoppingCartControl.SendNewRegistrationNotificationToAddress != "")
                {
                    SendRegistrationNotification(ui);
                }
            }
            // Anonymous customer
            else if (radAnonymous.Checked)
            {
                CustomerInfo ci = null;
                if (ShoppingCart.ShoppingCartCustomerID > 0)
                {
                    // Update existing customer account
                    ci = CustomerInfoProvider.GetCustomerInfo(ShoppingCart.ShoppingCartCustomerID);
                }
                if (ci == null)
                {
                    // Create new customer account 
                    ci = new CustomerInfo();
                }

                ci.CustomerFirstName = txtFirstName2.Text.Trim();
                ci.CustomerLastName = txtLastName2.Text.Trim();
                ci.CustomerEmail = txtEmail3.Text.Trim();

                ci.CustomerCompany = "";
                ci.CustomerOrganizationID = "";
                ci.CustomerTaxRegistrationID = "";

                if (chkCorporateBody2.Checked)
                {
                    ci.CustomerCompany = txtCompany2.Text.Trim();
                    if (mShowOrganizationIDField)
                    {
                        ci.CustomerOrganizationID = txtOrganizationID2.Text.Trim();
                    }
                    if (mShowTaxRegistrationIDField)
                    {
                        ci.CustomerTaxRegistrationID = txtTaxRegistrationID2.Text.Trim();
                    }
                }

                ci.CustomerCreated = DateTime.Now;
                ci.CustomerSiteID = SiteContext.CurrentSiteID;
                CustomerInfoProvider.SetCustomerInfo(ci);

                CreateContactRelation(ci);

                // Assign customer to shoppingcart
                ShoppingCart.ShoppingCartCustomerID = ci.CustomerID;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Save the customer data
            bool newCustomer = false;
            CustomerInfo ci = CustomerInfoProvider.GetCustomerInfoByUserID(ShoppingCartControl.UserInfo.UserID);
            if (ci == null)
            {
                ci = new CustomerInfo();
                ci.CustomerUserID = ShoppingCartControl.UserInfo.UserID;
                ci.CustomerSiteID = 0;
                newCustomer = true;
            }

            // Old email address
            string oldEmail = ci.CustomerEmail.ToLowerCSafe();

            ci.CustomerFirstName = txtEditFirst.Text.Trim();
            ci.CustomerLastName = txtEditLast.Text.Trim();
            ci.CustomerEmail = txtEditEmail.Text.Trim();

            pnlCompanyAccount2.Visible = chkEditCorpBody.Checked;

            ci.CustomerCompany = "";
            ci.CustomerOrganizationID = "";
            ci.CustomerTaxRegistrationID = "";
            if (chkEditCorpBody.Checked)
            {
                ci.CustomerCompany = txtEditCompany.Text.Trim();
                if (mShowOrganizationIDField)
                {
                    ci.CustomerOrganizationID = txtEditOrgID.Text.Trim();
                }
                if (mShowTaxRegistrationIDField)
                {
                    ci.CustomerTaxRegistrationID = txtEditTaxRegID.Text.Trim();
                }
            }

            // Update customer data
            CustomerInfoProvider.SetCustomerInfo(ci);

            // Update corresponding user email when required
            if (oldEmail != ci.CustomerEmail.ToLowerCSafe())
            {
                UserInfo user = UserInfoProvider.GetUserInfo(ci.CustomerUserID);
                if (user != null)
                {
                    user.Email = ci.CustomerEmail;
                    UserInfoProvider.SetUserInfo(user);
                }
            }

            if (newCustomer)
            {
                CreateContactRelation(ci);
            }

            // Set the shopping cart customer ID
            ShoppingCart.ShoppingCartCustomerID = ci.CustomerID;
        }

        try
        {
            if (!ShoppingCartControl.IsInternalOrder)
            {
                ShoppingCartInfoProvider.SetShoppingCartInfo(ShoppingCart);
            }

            ShoppingCart.Evaluate();
            return true;
        }
        catch
        {
            return false;
        }
    }





    /// <summary>
    /// Adds user to role
    /// <param name="userName">User name</param>
    /// <param name="roles">Role names the user should be assign to. Role names are separated by the char of ';'</param>
    /// <param name="siteName">Site name</param>
    /// </summary>
    private void AssignUserToRoles(string userName, string roles, string siteName)
    {
        if (string.IsNullOrEmpty(siteName))
        {
            return;
        }

        var roleList = roles.Split(';');
        foreach (string roleName in roleList)
        {
            String sn = roleName.StartsWithCSafe(".") ? "" : siteName;

            if (RoleInfoProvider.RoleExists(roleName, sn))
            {
                UserInfoProvider.AddUserToRole(userName, roleName, sn);
            }
        }
    }


    /// <summary>
    /// Sends new registration notification e-mail to administrator.
    /// </summary>
    private void SendRegistrationNotification(UserInfo ui)
    {
        var currentSiteName = SiteContext.CurrentSiteName;

        // Notify administrator
        if ((ui != null) && !String.IsNullOrEmpty(currentSiteName) && (ShoppingCartControl.SendNewRegistrationNotificationToAddress != ""))
        {
            EmailTemplateInfo mEmailTemplate = null;
            MacroResolver resolver = MembershipResolvers.GetRegistrationResolver(ui);
            if (SettingsKeyInfoProvider.GetBoolValue(currentSiteName + ".CMSRegistrationAdministratorApproval"))
            {
                mEmailTemplate = EmailTemplateProvider.GetEmailTemplate("Registration.Approve", currentSiteName);
            }
            else
            {
                mEmailTemplate = EmailTemplateProvider.GetEmailTemplate("Registration.New", currentSiteName);
            }

            if (mEmailTemplate == null)
            {
                // Email template not exist
                EventLogProvider.LogEvent(EventType.ERROR, "RegistrationForm", "GetEmailTemplate", eventUrl: RequestContext.RawURL);
            }
            else
            {
                // Initialize email message
                EmailMessage message = new EmailMessage();
                message.EmailFormat = EmailFormatEnum.Default;

                message.From = EmailHelper.GetSender(mEmailTemplate, ECommerceSettings.SendEmailsFrom(currentSiteName));
                message.Subject = GetString("RegistrationForm.EmailSubject");

                message.Recipients = ShoppingCartControl.SendNewRegistrationNotificationToAddress;
                message.Body = mEmailTemplate.TemplateText;

                try
                {
                    // Add template metafiles to e-mail
                    EmailHelper.ResolveMetaFileImages(message, mEmailTemplate.TemplateID, EmailTemplateInfo.OBJECT_TYPE, ObjectAttachmentsCategories.TEMPLATE);
                    // Send e-mail
                    EmailSender.SendEmailWithTemplateText(currentSiteName, message, mEmailTemplate, resolver, false);
                }
                catch
                {
                    // Email sending failed
                    EventLogProvider.LogEvent(EventType.ERROR, "Membership", "RegistrationEmail");
                }
            }
        }
    }


    /// <summary>
    /// Merge contact with customer and creates relation between these two.
    /// </summary>
    /// <param name="customerInfo">Registered customer to merge with proper contact</param>
    private void CreateContactRelation(CustomerInfo customerInfo)
    {
        int contactId = ModuleCommands.OnlineMarketingGetCurrentContactID();
        ModuleCommands.OnlineMarketingCreateRelation(customerInfo.CustomerID, MembershipType.ECOMMERCE_CUSTOMER, contactId);
        ContactInfoProvider.UpdateContactFromExternalData(
                customerInfo,
                DataClassInfoProvider.GetDataClassInfo(CustomerInfo.TYPEINFO.ObjectClassName).ClassContactOverwriteEnabled,
                contactId);
    }
}