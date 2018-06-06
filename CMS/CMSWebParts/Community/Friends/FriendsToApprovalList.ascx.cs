using System;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;

public partial class CMSWebParts_Community_Friends_FriendsToApprovalList : CMSAbstractWebPart
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
            lstApproval.StopProcessing = value;
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
            lstApproval.StopProcessing = true;
        }
        else
        {
            lstApproval.RedirectToAccessDeniedPage = false;
            lstApproval.UserID = MembershipContext.AuthenticatedUser.UserID;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        Visible = (AuthenticationHelper.IsAuthenticated() && lstApproval.HasData());
    }
}