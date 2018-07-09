using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Membership_Users_UsersFilter : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the filter button text.
    /// </summary>
    public string ButtonText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ButtonText"), "");
        }
        set
        {
            SetValue("ButtonText", value);
            filterUsers.ButtonText = value;
        }
    }


    /// <summary>
    /// Gets or sets the activity link text.
    /// </summary>
    public string SortActivityLinkText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SortActivityLinkText"), "");
        }
        set
        {
            SetValue("SortActivityLinkText", value);
            filterUsers.SortActivityLinkText = value;
        }
    }


    /// <summary>
    /// Gets or sets the user name link text.
    /// </summary>
    public string SortUserNameLinkText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SortUserNameLinkText"), "");
        }
        set
        {
            SetValue("SortUserNameLinkText", value);
            filterUsers.SortUserNameLinkText = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether cache should be disabled
    /// </summary>
    public bool DisableFilterCaching
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisableFilterCaching"), false);
        }
        set
        {
            SetValue("DisableFilterCaching", value);
            filterUsers.DisableFilterCaching = value;
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
    public void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            filterUsers.FilterName = ValidationHelper.GetString(GetValue("WebPartControlID"), ID);
            filterUsers.ButtonText = ButtonText;
            filterUsers.SortUserNameLinkText = SortUserNameLinkText;
            filterUsers.SortActivityLinkText = SortActivityLinkText;
            filterUsers.DisableFilterCaching = DisableFilterCaching;
        }
    }
}