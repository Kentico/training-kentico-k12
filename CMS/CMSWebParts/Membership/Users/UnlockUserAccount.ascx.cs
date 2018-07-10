using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;

public partial class CMSWebParts_Membership_Users_UnlockUserAccount : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Information text
    /// </summary>
    public string InformationText
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("InformationText"), "");
        }
        set
        {
            this.SetValue("InformationText", value);
        }
    }


    /// <summary>
    /// Successful unlock text
    /// </summary>
    public string SuccessfulUnlockText
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("SuccessfulUnlockText"), "");
        }
        set
        {
            this.SetValue("SuccessfulUnlockText", value);
        }
    }


    /// <summary>
    /// Unsuccessful unlock text
    /// </summary>
    public string UnsuccessfulUnlockText
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("UnsuccessfulUnlockText"), "");
        }
        set
        {
            this.SetValue("UnsuccessfulUnlockText", value);
        }
    }


    /// <summary>
    /// Redirection URL
    /// </summary>
    public string RedirectionUrl
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("RedirectionUrl"), "");
        }
        set
        {
            this.SetValue("RedirectionUrl", value);
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Content loaded event handler
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties
    /// </summary>
    protected void SetupControl()
    {
        if (this.StopProcessing)
        {
            // Do not process
            unlockAccount.StopProcessing = true;
        }
        else
        {
            if (!string.IsNullOrEmpty(QueryHelper.GetString("unlockaccounthash", string.Empty)))
            {
                unlockAccount.SuccessfulUnlockText = SuccessfulUnlockText;
                unlockAccount.UnsuccessfulUnlockText = UnsuccessfulUnlockText;
                unlockAccount.UnlockInfoText = InformationText;
                unlockAccount.RedirectionURL = RedirectionUrl;
            }
            else
            {
                unlockAccount.StopProcessing = true;
            }
        }
    }


    /// <summary>
    /// Reloads the control data
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }

    #endregion
}



