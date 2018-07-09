using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;

public partial class CMSWebParts_Membership_PasswordExpiration : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Password expiration text before link
    /// </summary>
    public string PasswordExpirationTextBefore
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("PasswordExpirationTextBefore"), "");
        }
        set
        {
            this.SetValue("PasswordExpirationTextBefore", value);
        }
    }


    /// <summary>
    /// Password expiration text after link
    /// </summary>
    public string PasswordExpirationTextAfter
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("PasswordExpirationTextAfter"), "");
        }
        set
        {
            this.SetValue("PasswordExpirationTextAfter", value);
        }
    }


    /// <summary>
    /// Password warning text before link
    /// </summary>
    public string PasswordWarningTextBefore
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("PasswordWarningTextBefore"), "");
        }
        set
        {
            this.SetValue("PasswordWarningTextBefore", value);
        }
    }


    /// <summary>
    /// Password expiration warning text after link
    /// </summary>
    public string PasswordWarningTextAfter
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("PasswordWarningTextAfter"), "");
        }
        set
        {
            this.SetValue("PasswordWarningTextAfter", value);
        }
    }


    /// <summary>
    /// Show change password link
    /// </summary>
    public bool ShowChangePasswordLink
    {
        get
        {
            return ValidationHelper.GetBoolean(this.GetValue("ShowChangePasswordLink"), true);
        }
        set
        {
            this.SetValue("ShowChangePasswordLink", value);
        }
    }


    /// <summary>
    /// Change password link text
    /// </summary>
    public string ChangePasswordLinkText
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("ChangePasswordLinkText"), "");
        }
        set
        {
            this.SetValue("ChangePasswordLinkText", value);
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Content loaded event handler
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties
    /// </summary>
    protected void SetupControl()
    {
        if (this.StopProcessing)
        {
            // Do not process
            pwdExp.StopProcessing = true;
        }
        else
        {
            pwdExp.ExpirationTextBefore = PasswordExpirationTextBefore;
            pwdExp.ExpirationTextAfter = PasswordExpirationTextAfter;
            pwdExp.ExpirationWarningTextBefore = PasswordWarningTextBefore;
            pwdExp.ExpirationWarningTextAfter = PasswordWarningTextAfter;
            pwdExp.ChangePasswordLinkText = ChangePasswordLinkText;
            pwdExp.ShowChangePasswordLink = ShowChangePasswordLink;
        }
    }


    /// <summary>
    /// Reloads the control data
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }

    #endregion
}