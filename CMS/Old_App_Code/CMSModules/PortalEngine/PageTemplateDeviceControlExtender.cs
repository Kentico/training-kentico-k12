using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using CMS;
using CMS.Base;
using CMS.Core;
using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;

[assembly: RegisterCustomClass("PageTemplateDeviceControlExtender", typeof(PageTemplateDeviceControlExtender))]

/// <summary>
/// Page template devices extender
/// </summary>
public class PageTemplateDeviceControlExtender : ControlExtender<UniGrid>
{
    int templateId;

    public override void OnInit()
    {
        templateId = Control.UIContext.ParentObjectID;

        Control.OnAction += deviceLayoutsGrid_OnAction;
        Control.OnExternalDataBound += Control_OnExternalDataBound;
        Control.ShowObjectMenu = false;
    }


    object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        DataRowView drv;
        GridViewRow gvr;
        
        switch (sourceName.ToLowerCSafe())
        {
            case "edit":
                if (sender is CMSAccessibleButton)
                {
                    CMSGridActionButton btnEdit = (CMSGridActionButton)sender;
                    gvr = (GridViewRow)parameter;
                    drv = (DataRowView)gvr.DataItem;

                    int id = drv["ProfileID"].ToInteger(0);
                    PageTemplateDeviceLayoutInfo layout = PageTemplateDeviceLayoutInfoProvider.GetTemplateDeviceLayoutInfo(templateId, id);

                    // Replace object ID
                    if (layout != null)
                    {
                        Regex reg = RegexHelper.GetRegex("&objectid=\\d+");
                        btnEdit.OnClientClick = reg.Replace(btnEdit.OnClientClick, "&objectid=" + layout.TemplateDeviceLayoutID);
                    }
                }
                break;
        }

        return parameter;
    }


    /// <summary>
    /// Devices the layouts grid_ on action.
    /// </summary>
    protected void deviceLayoutsGrid_OnAction(string actionName, object actionArgument)
    {
        // Check permissions
        bool isAuthorized = (MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(ModuleName.DESIGN, "Design"));

        switch (actionName)
        {
            case "delete":
                int deviceProfileId = ValidationHelper.GetInteger(actionArgument, 0);
                if (deviceProfileId > 0)
                {
                    // Check permissions
                    if (isAuthorized)
                    {
                        // Delete layout
                        PageTemplateDeviceLayoutInfo deviceLayout = PageTemplateDeviceLayoutInfoProvider.GetTemplateDeviceLayoutInfo(templateId, deviceProfileId);
                        PageTemplateDeviceLayoutInfoProvider.DeleteTemplateDeviceLayoutInfo(deviceLayout);
                    }
                }

                break;
        }
    }
}
