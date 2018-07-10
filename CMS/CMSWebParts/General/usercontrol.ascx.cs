using System;
using System.Web.UI;

using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Base.Web.UI;

public partial class CMSWebParts_General_usercontrol : CMSAbstractWebPart
{
    private string mUserControlPath = "";

    /// <summary>
    /// Gets or sets the path of the user control.
    /// </summary>
    public string UserControlPath
    {
        get
        {
            return ValidationHelper.GetString(GetValue("UserControlPath"), mUserControlPath);
        }
        set
        {
            SetValue("UserControlPath", value);
            mUserControlPath = value;
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
        if (StopProcessing)
        {
            // Do not process
        }
        else
        {
        }
    }


    /// <summary>
    /// Loads the user control.
    /// </summary>
    protected void LoadUserControl()
    {
        if (!string.IsNullOrEmpty(UserControlPath))
        {
            try
            {
                Control ctrl = Page.LoadUserControl(UserControlPath);
                ctrl.ID = "userControlElem";
                Controls.Add(ctrl);
            }
            catch (Exception ex)
            {
                lblError.Text = "[" + ID + "] " + GetString("WebPartUserControl.ErrorLoad") + ": " + ex.Message;
                lblError.ToolTip = EventLogProvider.GetExceptionLogMessage(ex);
                lblError.Visible = true;
            }
        }
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // The control must load after OnInit to properly load its viewstate
       this.LoadUserControl();
    }
}