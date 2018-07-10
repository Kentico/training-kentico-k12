using System;
using System.Web.UI;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Membership_Controls_Subscriptions : CMSAdminControl
{
    #region "Private variables"

    private bool mShowBlogs = true;
    private bool mShowNewsletters = true;
    private bool mShowMessageBoards = true;
    private bool mShowForums = true;
    private bool mShowReports = true;
    private bool mSendConfirmationMail = true;
    private int mUserId = 0;
    private int mSiteId = 0;

    private CMSAdminControl ucBlogs = null;
    private CMSAdminControl ucNewsletters = null;
    private CMSAdminControl ucBoards = null;
    private CMSAdminControl ucForums = null;
    private CMSAdminControl ucReports = null;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Indicates whether blog post subscriptions are shown.
    /// </summary>
    public bool ShowBlogs
    {
        get
        {
            return mShowBlogs;
        }
        set
        {
            mShowBlogs = value;
        }
    }


    /// <summary>
    /// Indicates whether message boards subscriptions are shown.
    /// </summary>
    public bool ShowMessageBoards
    {
        get
        {
            return mShowMessageBoards;
        }
        set
        {
            mShowMessageBoards = value;
        }
    }


    /// <summary>
    /// Indicates whether forums subscriptions are shown.
    /// </summary>
    public bool ShowForums
    {
        get
        {
            return mShowForums;
        }
        set
        {
            mShowForums = value;
        }
    }


    /// <summary>
    /// Indicates whether newsletters subscriptions are shown.
    /// </summary>
    public bool ShowNewsletters
    {
        get
        {
            return mShowNewsletters;
        }
        set
        {
            mShowNewsletters = value;
        }
    }


    /// <summary>
    /// Indicates whether reporting subscriptions are shown.
    /// </summary>
    public bool ShowReports
    {
        get
        {
            return mShowReports;
        }
        set
        {
            mShowReports = value;
        }
    }


    /// <summary>
    /// Indicates whether send email when (un)subscribed.
    /// </summary>
    public bool SendConfirmationMail
    {
        get
        {
            return mSendConfirmationMail;
        }
        set
        {
            mSendConfirmationMail = value;
        }
    }


    /// <summary>
    /// Gets or sets user ID.
    /// </summary>
    public int UserID
    {
        get
        {
            return mUserId;
        }
        set
        {
            mUserId = value;
        }
    }


    /// <summary>
    /// Gets or sets site ID.
    /// </summary>
    public int SiteID
    {
        get
        {
            return mSiteId;
        }
        set
        {
            mSiteId = value;
            if (ucNewsletters != null)
            {
                ucNewsletters.SetValue("siteid", value);
            }
            if (ucBlogs != null)
            {
                ucBlogs.SetValue("siteid", value);
            }
            if (ucBoards != null)
            {
                ucBoards.SetValue("siteid", value);
            }
        }
    }

    #endregion


    protected override void CreateChildControls()
    {
        base.CreateChildControls();

        // Get current user if UserID is not defined
        if (UserID <= 0)
        {
            UserID = MembershipContext.AuthenticatedUser.UserID;
        }

        // Use current site if SiteID is not defined
        if (SiteID <= 0)
        {
            SiteID = SiteContext.CurrentSiteID;
        }
        string siteName = SiteInfoProvider.GetSiteName(SiteID);

        bool firstInserted = false;

        // Try to init newsletters subscriptions
        if (ShowNewsletters && ModuleManager.IsModuleLoaded(ModuleName.NEWSLETTER))
        {
            ucNewsletters = Page.LoadUserControl("~/CMSModules/Newsletters/Controls/MySubscriptions.ascx") as CMSAdminControl;
            if (ucNewsletters != null)
            {
                headNewsletters.Visible = true;
                pnlNewsletters.Visible = true;
                ucNewsletters.ID = "ucNewsletters";
                ucNewsletters.SetValue("externaluse", true);
                ucNewsletters.SetValue("forcedvisible", true);
                ucNewsletters.SetValue("userid", UserID);
                ucNewsletters.SetValue("siteid", SiteID);
                ucNewsletters.SetValue("sendconfirmationemail", SendConfirmationMail);
                ucNewsletters.StopProcessing = StopProcessing;
                ucNewsletters.IsLiveSite = IsLiveSite;

                pnlNewsletters.Controls.Clear();
                pnlNewsletters.Controls.Add(new LiteralControl("<div class=\"SubscriptionsGroup\">"));
                pnlNewsletters.Controls.Add(ucNewsletters);
                pnlNewsletters.Controls.Add(new LiteralControl("</div>"));


                firstInserted = true;

                ucNewsletters.OnCheckPermissions += ucNewsletters_OnCheckPermissions;
            }
        }

        // Try to init blog post subscriptions
        if (ShowBlogs && ModuleManager.IsModuleLoaded(ModuleName.BLOGS) && ResourceSiteInfoProvider.IsResourceOnSite(ModuleName.BLOGS, siteName))
        {
            ucBlogs = Page.LoadUserControl("~/CMSModules/Blogs/Controls/BlogPostSubscriptions.ascx") as CMSAdminControl;
            if (ucBlogs != null)
            {
                headBlogs.Visible = true;
                pnlBlogs.Visible = true;
                ucBlogs.ID = "ucBlogs";
                ucBlogs.SetValue("userid", UserID);
                ucBlogs.SetValue("siteid", SiteID);
                ucBlogs.StopProcessing = StopProcessing;
                ucBlogs.OnCheckPermissions += ucBlogs_OnCheckPermissions;
                ucBlogs.IsLiveSite = IsLiveSite;

                if (firstInserted)
                {
                    pnlBlogs.Attributes.Add("class", "SubscriptionsPanel");
                }

                pnlBlogs.Controls.Clear();
                pnlBlogs.Controls.Add(new LiteralControl("<div class=\"SubscriptionsGroup\">"));
                pnlBlogs.Controls.Add(ucBlogs);
                pnlBlogs.Controls.Add(new LiteralControl("</div>"));

                firstInserted = true;
            }
        }

        // Try to init message board subscriptions
        if (ShowMessageBoards && ModuleManager.IsModuleLoaded(ModuleName.MESSAGEBOARD) && ResourceSiteInfoProvider.IsResourceOnSite(ModuleName.MESSAGEBOARD, siteName))
        {
            ucBoards = Page.LoadUserControl("~/CMSModules/MessageBoards/Controls/Boards/BoardUserSubscriptions.ascx") as CMSAdminControl;
            if (ucBoards != null)
            {
                headBoards.Visible = true;
                pnlBoards.Visible = true;
                ucBoards.ID = "ucBoards";
                ucBoards.SetValue("userid", UserID);
                ucBoards.SetValue("siteid", SiteID);
                ucBoards.StopProcessing = StopProcessing;
                ucBoards.OnCheckPermissions += ucBoards_OnCheckPermissions;
                ucBoards.IsLiveSite = IsLiveSite;

                if (firstInserted)
                {
                    pnlBoards.Attributes.Add("class", "SubscriptionsPanel");
                }
                pnlBoards.Controls.Clear();
                pnlBoards.Controls.Add(new LiteralControl("<div class=\"SubscriptionsGroup\">"));
                pnlBoards.Controls.Add(ucBoards);
                pnlBoards.Controls.Add(new LiteralControl("</div>"));
            }
        }

        // Try to init forums subscriptions
        if (ShowForums && ModuleManager.IsModuleLoaded(ModuleName.FORUMS) && ResourceSiteInfoProvider.IsResourceOnSite(ModuleName.FORUMS, siteName))
        {
            ucForums = Page.LoadUserControl("~/CMSModules/Forums/Controls/Subscriptions/UserSubscriptionList.ascx") as CMSAdminControl;
            if (ucForums != null)
            {
                headForums.Visible = true;
                pnlForums.Visible = true;
                ucForums.ID = "ucForums";
                ucForums.SetValue("userid", UserID);
                ucForums.StopProcessing = StopProcessing;
                ucForums.OnCheckPermissions += ucForums_OnCheckPermissions;
                ucForums.IsLiveSite = IsLiveSite;

                if (firstInserted)
                {
                    pnlForums.Attributes.Add("class", "SubscriptionsPanel");
                }
                pnlForums.Controls.Clear();
                pnlForums.Controls.Add(new LiteralControl("<div class=\"SubscriptionsGroup\">"));
                pnlForums.Controls.Add(ucForums);
                pnlForums.Controls.Add(new LiteralControl("</div>"));
            }
        }

        // Try to init reports subscriptions
        if (ShowReports && ModuleManager.IsModuleLoaded(ModuleName.REPORTING) && ResourceSiteInfoProvider.IsResourceOnSite(ModuleName.REPORTING, siteName))
        {
            ucReports = Page.LoadUserControl("~/CMSModules/Reporting/Controls/UserSubscriptions.ascx") as CMSAdminControl;
            if (ucReports != null)
            {
                headReports.Visible = true;
                pnlReports.Visible = true;
                ucReports.ID = "ucReports";
                ucReports.SetValue("userid", UserID);
                ucReports.SetValue("siteid", SiteID);
                ucReports.StopProcessing = StopProcessing;
                ucReports.OnCheckPermissions += ucReports_OnCheckPermissions;
                ucReports.IsLiveSite = IsLiveSite;

                if (firstInserted)
                {
                    pnlReports.Attributes.Add("class", "SubscriptionsPanel");
                }
                pnlReports.Controls.Clear();
                pnlReports.Controls.Add(new LiteralControl("<div class=\"SubscriptionsGroup\">"));
                pnlReports.Controls.Add(ucReports);
                pnlReports.Controls.Add(new LiteralControl("</div>"));
            }
        }
    }


    #region "Security"

    protected void ucNewsletters_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        RaiseOnCheckPermissions(permissionType, sender);
    }


    protected void ucBlogs_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        RaiseOnCheckPermissions(permissionType, sender);
    }


    protected void ucBoards_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        RaiseOnCheckPermissions(permissionType, sender);
    }


    protected void ucForums_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        RaiseOnCheckPermissions(permissionType, sender);
    }

    protected void ucReports_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        RaiseOnCheckPermissions(permissionType, sender);
    }


    #endregion


    public override bool SetValue(string propertyName, object value)
    {
        base.SetValue(propertyName, value);
        switch (propertyName.ToLowerCSafe())
        {
            case "userid":
                UserID = ValidationHelper.GetInteger(value, 0);
                break;

            case "siteid":
                SiteID = ValidationHelper.GetInteger(value, 0);
                break;

            case "showblogs":
                ShowBlogs = ValidationHelper.GetBoolean(value, true);
                break;

            case "showmessageboards":
                ShowMessageBoards = ValidationHelper.GetBoolean(value, true);
                break;

            case "shownewsletters":
                ShowNewsletters = ValidationHelper.GetBoolean(value, true);
                break;

            case "showforums":
                ShowForums = ValidationHelper.GetBoolean(value, true);
                break;

            case "showreports":
                ShowReports = ValidationHelper.GetBoolean(value, true);
                break;

            case "sendconfirmationemail":
                SendConfirmationMail = ValidationHelper.GetBoolean(value, true);
                break;
        }

        return true;
    }
} 