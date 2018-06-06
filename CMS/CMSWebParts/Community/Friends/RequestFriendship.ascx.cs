using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Community_Friends_RequestFriendship : CMSAbstractWebPart
{
    /// <summary>
    /// Gets or sets link text.
    /// </summary>
    public string LinkText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LinkText"), string.Empty);
        }
        set
        {
            SetValue("LinkText", value);
        }
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
        if (StopProcessing || MembershipContext.AuthenticatedUser.IsPublic())
        {
            requestFriendshipElem.StopProcessing = true;
            Visible = false;
        }
        else
        {
            requestFriendshipElem.LinkText = LinkText;
            requestFriendshipElem.UserID = MembershipContext.AuthenticatedUser.UserID;
        }
    }
}