using System;

using CMS.Helpers;
using CMS.Messaging;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Messaging_SendMessage : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates whether the anonymous user should be able to select recipient of the messages.
    /// </summary>
    public bool AllowAnonymousRecipientSelection
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowAnonymousRecipientSelection"), ucSendMessage.AllowAnonymousRecipientSelection);
        }
        set
        {
            SetValue("AllowAnonymousRecipientSelection", value);
            ucSendMessage.AllowAnonymousRecipientSelection = value;
        }
    }


    /// <summary>
    /// Gets or sets the default recipient of the message.
    /// </summary>
    public string DefaultRecipient
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DefaultRecipient"), ucSendMessage.DefaultRecipient);
        }
        set
        {
            SetValue("DefaultRecipient", value);
            ucSendMessage.DefaultRecipient = value;
        }
    }

    #endregion


    #region "Stop processing"

    /// <summary>
    /// Returns true if the control processing should be stopped.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return base.StopProcessing;
        }
        set
        {
            base.StopProcessing = value;
            ucSendMessage.StopProcessing = value;
        }
    }

    #endregion


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
            // Do nothing
            ucSendMessage.StopProcessing = true;
        }
        else
        {
            ucSendMessage.DefaultRecipient = DefaultRecipient;
            ucSendMessage.AllowAnonymousRecipientSelection = AllowAnonymousRecipientSelection;
            ucSendMessage.SendMessageMode = MessageActionEnum.New;
        }
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        SetupControl();
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        Visible = ucSendMessage.Visible;
    }
}