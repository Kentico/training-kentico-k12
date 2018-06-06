using System;
using System.Data;
using System.Web;
using System.Web.UI;

using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Membership_Registration_RegistrationConfirmation : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Gets or sets the value that indicates whether administrator should be informed about new user.
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
            registrationApproval.NotifyAdministrator = value;
        }
    }


    /// <summary>
    /// Gets or sets email address of sender.
    /// </summary>
    public string FromAddress
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FromAddress"), "");
        }
        set
        {
            SetValue("FromAddress", value);
            registrationApproval.FromAddress = value;
        }
    }


    /// <summary>
    /// Gets or sets email address of sender.
    /// </summary>
    public string AdministratorEmail
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AdministratorEmail"), "");
        }
        set
        {
            SetValue("AdministratorEmail", value);
            registrationApproval.AdministratorEmail = value;
        }
    }


    /// <summary>
    /// Gets or sets successful approval text.
    /// </summary>
    public string SuccessfulApprovalText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SuccessfulApprovalText"), "");
        }
        set
        {
            SetValue("SuccessfulApprovalText", value);
            registrationApproval.SuccessfulApprovalText = value;
        }
    }


    /// <summary>
    /// Gets or sets unsuccesfull approval text.
    /// </summary>
    public string UnsuccessfulApprovalText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("UnsuccessfulApprovalText"), "");
        }
        set
        {
            SetValue("UnsuccessfulApprovalText", value);
            registrationApproval.UnsuccessfulApprovalText = value;
        }
    }


    /// <summary>
    /// Gets or sets unsuccesfull approval text.
    /// </summary>
    public string UserDeletedText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("UserDeletedText"), "");
        }
        set
        {
            SetValue("UserDeletedText", value);
            registrationApproval.UserDeletedText = value;
        }
    }


    /// <summary>
    /// Gets or sets confirmation button text.
    /// </summary>
    public string ConfirmationButtonText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ConfirmationButtonText"), "");
        }
        set
        {
            SetValue("ConfirmationButtonText", value);
            registrationApproval.ConfirmationButtonText = value;
        }
    }


    /// <summary>
    /// Gets or sets confirmation button CSS class.
    /// </summary>
    public string ConfirmationButtonCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ConfirmationButtonCssClass"), "");
        }
        set
        {
            SetValue("ConfirmationButtonCssClass", value);
            registrationApproval.ConfirmationButtonCssClass = value;
        }
    }

    #endregion


    /// <summary>
    /// Initialization event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        registrationApproval.StopProcessing = string.IsNullOrEmpty(QueryHelper.GetString("hash", string.Empty));
    }


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            registrationApproval.StopProcessing = true;
        }
        else
        {
            // set address to underlying control
            registrationApproval.FromAddress = FromAddress;
            registrationApproval.NotifyAdministrator = NotifyAdministrator;
            registrationApproval.AdministratorEmail = AdministratorEmail;
            registrationApproval.SuccessfulApprovalText = SuccessfulApprovalText;
            registrationApproval.UnsuccessfulApprovalText = UnsuccessfulApprovalText;
            registrationApproval.UserDeletedText = UserDeletedText;
            registrationApproval.ConfirmationButtonCssClass = ConfirmationButtonCssClass;
            registrationApproval.ConfirmationButtonText = ConfirmationButtonText;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        Visible = registrationApproval.Visible;
    }
}