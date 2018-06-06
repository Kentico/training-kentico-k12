using System;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Community_Friends_FriendsRejectedList : CMSAbstractWebPart
{
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
            lstRejected.StopProcessing = value;
        }
    }

    #endregion


    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    public override void ReloadData()
    {
        base.ReloadData();
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
            lstRejected.StopProcessing = true;
        }
        else
        {
            lstRejected.RedirectToAccessDeniedPage = false;
            lstRejected.UserID = MembershipContext.AuthenticatedUser.UserID;
            lstRejected.ShowLink = false;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        Visible = (AuthenticationHelper.IsAuthenticated() && lstRejected.HasData());
    }
}