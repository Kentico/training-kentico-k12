using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;

public partial class CMSWebParts_Messaging_MyMessages : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the WebPart CSS class value.
    /// </summary>
    public override string CssClass
    {
        get
        {
            return base.CssClass;
        }
        set
        {
            base.CssClass = value;
            ucMyMessages.CssClass = value;
        }
    }


    /// <summary>
    /// Gets or sets the query string parameter name.
    /// </summary>
    public string ParameterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ParameterName"), ucMyMessages.ParameterName);
        }
        set
        {
            SetValue("ParameterName", value);
            ucMyMessages.ParameterName = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'inbox' is displayed.
    /// </summary>
    public bool DisplayInbox
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayInbox"), ucMyMessages.DisplayInbox);
        }
        set
        {
            SetValue("DisplayInbox", value);
            ucMyMessages.DisplayInbox = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'outbox' is displayed.
    /// </summary>
    public bool DisplayOutbox
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayOutbox"), ucMyMessages.DisplayOutbox);
        }
        set
        {
            SetValue("DisplayOutbox", value);
            ucMyMessages.DisplayOutbox = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'contact list' is displayed.
    /// </summary>
    public bool DisplayContactList
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayContactList"), ucMyMessages.DisplayContactList);
        }
        set
        {
            SetValue("DisplayContactList", value);
            ucMyMessages.DisplayContactList = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether 'ignore list' is displayed.
    /// </summary>
    public bool DisplayIgnoreList
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayIgnoreList"), ucMyMessages.DisplayIgnoreList);
        }
        set
        {
            SetValue("DisplayIgnoreList", value);
            ucMyMessages.DisplayIgnoreList = value;
        }
    }


    /// <summary>
    /// Gets or sets the message which should be displayed for public users.
    /// </summary>
    public string NotAuthenticatedMessage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NotAuthenticatedMessage"), string.Empty);
        }
        set
        {
            SetValue("NotAuthenticatedMessage", value);
            ucMyMessages.NotAuthenticatedMessage = value;
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
            ucMyMessages.StopProcessing = value;
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
    /// Setup control.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
            ucMyMessages.StopProcessing = true;
        }
        else
        {
            ucMyMessages.ParameterName = ParameterName;
            ucMyMessages.CssClass = CssClass;
            ucMyMessages.DisplayInbox = DisplayInbox;
            ucMyMessages.DisplayOutbox = DisplayOutbox;
            ucMyMessages.DisplayIgnoreList = DisplayIgnoreList;
            ucMyMessages.DisplayContactList = DisplayContactList;
            ucMyMessages.NotAuthenticatedMessage = NotAuthenticatedMessage;
            ucMyMessages.IsLiveSite = (ViewMode != ViewModeEnum.DashboardWidgets);
            AdditionalCssClass = "MyMessagesWebPart";
        }
    }


    /// <summary>
    /// Reload data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }
}