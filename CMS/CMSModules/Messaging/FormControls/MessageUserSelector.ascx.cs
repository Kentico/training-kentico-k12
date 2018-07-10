using System;

using CMS.Base.Web.UI;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;


public partial class CMSModules_Messaging_FormControls_MessageUserSelector : FormEngineUserControl
{
    #region "Private variables"

    private bool? mVisisble = null;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets field value. You need to override this method to make the control work properly with the form.
    /// </summary>
    public override object Value
    {
        get
        {
            return SelectedUserID;
        }
        set
        {
            SelectedUserID = ValidationHelper.GetInteger(value, 0);
        }
    }


    /// <summary>
    /// Gets or sets the enabled state of the control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }
        set
        {
            base.Enabled = value;
            txtUser.Enabled = value;
            btnSelect.Enabled = value;
        }
    }


    /// <summary>
    /// Name of the selected user.
    /// </summary>
    public string SelectedUserName
    {
        get
        {
            UserInfo ui = GetUser();
            return ui != null ? ui.UserName : string.Empty;
        }
        set
        {
            UserInfo ui = UserInfoProvider.GetUserInfo(value);
            if (ui != null)
            {
                txtUser.Text = ui.UserName;
                hiddenField.Value = ui.UserName;
            }
        }
    }


    /// <summary>
    /// ID of the selected user.
    /// </summary>
    public int SelectedUserID
    {
        get
        {
            UserInfo ui = GetUser();
            return ui != null ? ui.UserID : 0;
        }
        set
        {
            UserInfo ui = UserInfoProvider.GetUserInfo(value);
            if (ui != null)
            {
                txtUser.Text = ui.UserName;
                hiddenField.Value = ui.UserName;
            }
            else
            {
                txtUser.Text = string.Empty;
                hiddenField.Value = string.Empty;
            }
        }
    }


    /// <summary>
    /// Visibility of control.
    /// </summary>
    public override bool Visible
    {
        get
        {
            if (mVisisble.HasValue)
            {
                return mVisisble.Value;
            }
            else
            {
                return base.Visible;
            }
        }
        set
        {
            mVisisble = value;
            base.Visible = value;
        }
    }


    /// <summary>
    /// Gets textbox with user name.
    /// </summary>
    public CMSTextBox UserNameTextBox
    {
        get
        {
            return txtUser;
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Returns UserInfo object from hidden field value (because readonly textbox).
    /// </summary>
    private UserInfo GetUser()
    {
        // Try to find by username
        string userName = txtUser.Text.Trim();
        if (!string.IsNullOrEmpty(userName))
        {
            return UserInfoProvider.GetUserInfo(txtUser.Text.Trim());
        }

        int userId = ValidationHelper.GetInteger(hiddenField.Value, 0);
        if (userId > 0)
        {
            return UserInfoProvider.GetUserInfo(userId);
        }

        return null;
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            btnSelect.Text = GetString("General.Select");

            ScriptHelper.RegisterDialogScript(Page);
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "fillScript", ScriptHelper.GetScript("function FillUserName(userId, mText, mId, mId2) {document.getElementById(mId).value = mText;document.getElementById(mId2).value = userId;}"));

            string showTab = MembershipContext.AuthenticatedUser.IsPublic() ? "Search" : "ContactList";
            string query = QueryHelper.BuildQueryWithHash("refresh", "false", "showtab", showTab, "hidid", hiddenField.ClientID, "mid", txtUser.ClientID);
            string url = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/Dialogs/MessageUserSelector_Frameset.aspx");
            if (IsLiveSite)
            {
                if (!MembershipContext.AuthenticatedUser.IsPublic())
                {
                    url = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/CMSPages/MessageUserSelector_Frameset.aspx");
                }
                else
                {
                    url = ApplicationUrlHelper.ResolveDialogUrl("~/CMSModules/Messaging/CMSPages/PublicMessageUserSelector.aspx");
                }
            }

            btnSelect.OnClientClick = "modalDialog('" + url + query + "','MessageUserSelector',700, 550); return false;";

            if (!RequestHelper.IsPostBack())
            {
                txtUser.Text = SelectedUserName;
            }
        }
    }

    #endregion
}