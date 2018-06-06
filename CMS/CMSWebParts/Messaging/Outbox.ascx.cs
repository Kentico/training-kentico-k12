using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Messaging_Outbox : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the text which is displayed when no data found.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("ZeroRowsText"), ucOutbox.ZeroRowsText), ucOutbox.ZeroRowsText);
        }
        set
        {
            SetValue("ZeroRowsText", value);
            ucOutbox.ZeroRowsText = value;
        }
    }


    /// <summary>
    /// Gets or sets the size of the page when paging is used.
    /// </summary>
    public int PageSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PageSize"), ucOutbox.PageSize);
        }
        set
        {
            SetValue("PageSize", value);
            ucOutbox.PageSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the original message should be shown.
    /// </summary>
    public bool ShowOriginalMessage
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowOriginalMessage"), ucOutbox.ShowOriginalMessage);
        }
        set
        {
            SetValue("ShowOriginalMessage", value);
            ucOutbox.ShowOriginalMessage = value;
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
            ucOutbox.StopProcessing = value;
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
            ucOutbox.StopProcessing = true;
        }
        else
        {
            ucOutbox.ZeroRowsText = ZeroRowsText;
            ucOutbox.PageSize = PageSize;
            ucOutbox.ShowOriginalMessage = ShowOriginalMessage;
        }
    }


    /// <summary>
    /// Reloads the data.
    /// </summary>
    public override void ReloadData()
    {
        SetupControl();
    }
}