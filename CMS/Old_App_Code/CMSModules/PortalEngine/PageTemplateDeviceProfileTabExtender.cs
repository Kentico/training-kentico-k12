using System;

using CMS;
using CMS.DeviceProfiles;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Modules;
using CMS.PortalEngine;
using CMS.UIControls;

[assembly: RegisterCustomClass("PageTemplateDeviceProfileTabExtender", typeof(PageTemplateDeviceProfileTabExtender))]

/// <summary>
/// Page template device edit tab control extender
/// </summary>
public class PageTemplateDeviceProfileTabExtender : ControlExtender<UITabs>
{
    #region "Variables"

    private String deviceName = String.Empty;

    #endregion


    #region "Methods"

    public override void OnInit()
    {
        Control.OnTabCreated += Control_OnTabCreated;
        
        var info = Control.UIContext.EditedObject as PageTemplateDeviceLayoutInfo;
        if (info != null)
        {
            DeviceProfileInfo dpi = DeviceProfileInfoProvider.GetDeviceProfileInfo(info.ProfileID);
            if (dpi != null)
            {
                deviceName = dpi.ProfileName;
            }
        }
    }


    /// <summary>
    /// Appends additional query parameters.
    /// </summary>
    protected void Control_OnTabCreated(object sender, TabCreatedEventArgs e)
    {
        if ((e.Tab != null) && !String.IsNullOrEmpty(deviceName))
        {
            e.Tab.RedirectUrl = URLHelper.AddParameterToUrl(e.Tab.RedirectUrl, "devicename", deviceName);
            e.Tab.RedirectUrl = ApplicationUrlHelper.AppendDialogHash(e.Tab.RedirectUrl);
        }
    }

    #endregion
}
