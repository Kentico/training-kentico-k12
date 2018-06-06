using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Messaging_ContactList : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the text which is displayed when no data found.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("ZeroRowsText"), lstContacts.ZeroRowsText), lstContacts.ZeroRowsText);
        }
        set
        {
            SetValue("ZeroRowsText", value);
            lstContacts.ZeroRowsText = value;
        }
    }


    /// <summary>
    /// Gets or sets the size of the page when paging is used.
    /// </summary>
    public string PageSize
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PageSize"), lstContacts.PageSize);
        }
        set
        {
            SetValue("PageSize", value);
            lstContacts.PageSize = value;
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
            lstContacts.StopProcessing = value;
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
            lstContacts.StopProcessing = true;
        }
        else
        {
            lstContacts.PageSize = PageSize;
            lstContacts.ZeroRowsText = ZeroRowsText;
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