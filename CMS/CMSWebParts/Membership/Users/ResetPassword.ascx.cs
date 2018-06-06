using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Membership_Users_ResetPassword : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Text shown if request hash isn't found.
    /// </summary>
    public string InvalidRequestText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("InvalidRequestText"), "");
        }
        set
        {
            SetValue("InvalidRequestText", value);
        }
    }


    /// <summary>
    /// Url on which is user redirected after successful password reset.
    /// </summary>
    public string RedirectUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("RedirectUrl"), "");
        }
        set
        {
            SetValue("RedirectUrl", value);
        }
    }


    /// <summary>
    /// E-mail address from which e-mail is sent.
    /// </summary>
    public string SendEmailFrom
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SendEmailFrom"), "");
        }
        set
        {
            SetValue("SendEmailFrom", value);
        }
    }


    /// <summary>
    /// If interval for action confirmation is exceeded this text is shown.
    /// </summary>
    public string ExceededIntervalText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ExceededIntervalText"), "");
        }
        set
        {
            SetValue("ExceededIntervalText", value);
        }
    }


    /// <summary>
    /// Text shown when password reset was succesful. 
    /// </summary>
    public string SuccessText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SuccessText"), "");
        }
        set
        {
            SetValue("SuccessText", value);
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// On content loaded method.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Setup control.
    /// </summary>
    protected void SetupControl()
    {
        Visible = QueryHelper.Contains("hash");

        if (StopProcessing || !Visible)
        {
            resetPassitem.StopProcessing = true;
        }
        else
        {
            // Set properties to inner control
            resetPassitem.InvalidRequestText = InvalidRequestText;
            resetPassitem.RedirectUrl = RedirectUrl;
            resetPassitem.SendEmailFrom = SendEmailFrom;
            resetPassitem.ExceededIntervalText = ExceededIntervalText;
            resetPassitem.SuccessText = SuccessText;
        }
    }

    #endregion
}