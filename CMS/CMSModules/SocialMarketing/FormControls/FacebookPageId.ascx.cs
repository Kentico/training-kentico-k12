using System;

using CMS.EventLog;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.SocialMarketing;


public partial class CMSModules_SocialMarketing_FormControls_FacebookPageId : FormEngineUserControl
{
    #region "Variables and private properties"

    private FacebookAccountInfo mEditedAccount;


    /// <summary>
    /// Gets edited facebook account.
    /// </summary>
    private FacebookAccountInfo EditedAccount
    {
        get
        {
            return mEditedAccount ?? (mEditedAccount = Form.Data as FacebookAccountInfo);
        }
    }


    /// <summary>
    /// Gets or sets page id in the viewstate.
    /// </summary>
    private string PageId
    {
        get
        {
            return (string) ViewState["PageId"] ?? String.Empty;
        }
        set
        {
            ViewState["PageId"] = value;
        }
    }



    /// <summary>
    /// Gets or sets url of the page in the viewstate. Id of this url is stored in the viewstate.
    /// </summary>
    private string PageUrl
    {
        get
        {
            return (string) ViewState["PageUrl"] ?? String.Empty;
        }
        set
        {
            ViewState["PageUrl"] = value;
        }
    }

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets enabled state.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return txtPageUrl.Enabled;
        }
        set
        {
            txtPageUrl.Enabled = value;
        }
    }


    /// <summary>
    /// Gets or sets the id of the <see cref="FacebookApplicationInfo"/> required for acquiring Facebook access token.
    /// </summary>
    public int FacebookApplicationId
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("FacebookApplicationId"), 0);
        }
        set
        {
            SetValue("FacebookApplicationId", value);
        }
    }


    /// <summary>
    /// Gets or sets the id of the page.
    /// </summary>
    public override object Value
    {
        get
        {
            if (String.IsNullOrWhiteSpace(txtPageUrl.Text))
            {
                PageId = String.Empty;
                PageUrl = String.Empty;
            }
            else if (String.IsNullOrEmpty(PageId) || (txtPageUrl.Text != PageUrl))
            {
                FacebookApplicationInfo appInfo = FacebookApplicationInfoProvider.GetFacebookApplicationInfo(FacebookApplicationId);
                if (appInfo == null)
                {
                    ShowError(GetString("sm.facebook.account.msg.appnotset"));
                }
                
                PageId = String.Empty;
                try
                {
                    string identifier = null;
                    PageId = FacebookHelper.TryGetFacebookPageId(txtPageUrl.Text, appInfo, out identifier) ? identifier : String.Empty;
                    PageUrl = txtPageUrl.Text;
                }
                catch (Exception ex)
                {
                    EventLogProvider.LogException("SocialMarketingFacebookAccount", "GETPAGEID", ex, SiteContext.CurrentSiteID, "Error occurred while getting Facebook page ID from its URL.");
                    ShowError(GetString("sm.facebook.account.msg.getpageidfail"));
                }
            }

            return PageId;
        }
        set
        {
            // This field si read only
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// On page init event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        CheckFieldEmptiness = false;
    }


    /// <summary>
    /// On page load event.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (Form != null)
        {
            if (!RequestHelper.IsPostBack() && (EditedAccount != null))
            {
                PageUrl = txtPageUrl.Text = EditedAccount.FacebookPageIdentity.PageUrl;
                PageId = EditedAccount.FacebookPageIdentity.PageId;
            }
        }
    }

    #endregion


    #region "Public methods"

    /// <summary>
    /// Returns true if entered data is valid. If data is invalid, it returns false and displays an error message.
    /// </summary>
    public override bool IsValid()
    {
        if (String.IsNullOrWhiteSpace(txtPageUrl.Text))
        {
            ValidationError = GetString("basicform.erroremptyvalue");

            return false;
        }
        ValidationError = GetString("sm.facebook.account.msg.invalidpageurl");
        
        return !String.IsNullOrEmpty((string) Value);
    }


    /// <summary>
    /// Loads the other fields values to the state of the form control
    /// </summary>
    public override void LoadOtherValues()
    {
        if (ContainsColumn("FacebookAccountPageUrl"))
        {
            txtPageUrl.Text = ValidationHelper.GetString(GetColumnValue("FacebookAccountPageUrl"), "");
        }
    }


    /// <summary>
    /// Returns an array of values of any other fields returned by the control.
    /// </summary>
    /// <remarks>It returns an array where first dimension is attribute name and the second dimension is its value.</remarks>
    public override object[,] GetOtherValues()
    {
        return new object[,] { { "FacebookAccountPageUrl", txtPageUrl.Text } };
    }
    #endregion
} 