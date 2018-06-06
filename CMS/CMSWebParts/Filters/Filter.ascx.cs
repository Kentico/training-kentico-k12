using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.DocumentEngine.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using CMS.IO;
using CMS.PortalEngine.Web.UI;
using CMS.Base.Web.UI;

public partial class CMSWebParts_Filters_Filter : CMSAbstractWebPart
{
    private string mFilterControlPath = null;
    private string mFilterName = null;
    private CMSAbstractBaseFilterControl mFilterControl = null;


    /// <summary>
    /// Gets or sets the path of the filter control.
    /// </summary>
    public string FilterControlPath
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterControlPath"), mFilterControlPath);
        }
        set
        {
            SetValue("FilterControlPath", value);
            mFilterControlPath = value;
        }
    }


    /// <summary>
    /// Gets or sets the name of the filter control.
    /// </summary>
    public string FilterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterName"), mFilterName);
        }
        set
        {
            SetValue("FilterName", value);
            mFilterName = value;
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
        // In design mode is pocessing of control stoped
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            LoadFilter();
        }
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        LoadFilter();
    }


    /// <summary>
    /// Load filter control according filterpath.
    /// </summary>
    private void LoadFilter()
    {
        if (mFilterControl == null)
        {
            if (FilterControlPath != null)
            {
                try
                {
                    if (File.Exists(Server.MapPath(FilterControlPath)))
                    {
                        mFilterControl = (Page.LoadUserControl(FilterControlPath)) as CMSAbstractBaseFilterControl;
                        if (mFilterControl != null)
                        {
                            mFilterControl.ID = "filterControl";
                            Controls.AddAt(0, mFilterControl);
                            mFilterControl.FilterName = FilterName;
                            if (Page != null)
                            {
                                mFilterControl.Page = Page;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogProvider.LogException("Filter control", "LOADFILTER", ex);
                }
            }
        }
    }
}