using System;

using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DeviceProfiles;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;


public partial class CMSModules_PortalEngine_UI_PageLayouts_PageLayout_CustomDeviceLayout : CMSModalPage
{
    #region "Variables"

    private int templateId;
    private int deviceProfileId;
    private bool showNewDeviceSelector = true;

    #endregion


    #region "Methods"

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Setup the page variables
        templateId = QueryHelper.GetInteger("templateid", 0);
        deviceProfileId = QueryHelper.GetInteger("deviceprofileid", 0);

        if (deviceProfileId > 0)
        {
            // Display device profile label
            showNewDeviceSelector = false;

            DeviceProfileInfo deviceLayout = DeviceProfileInfoProvider.GetDeviceProfileInfo(deviceProfileId);
            if (deviceLayout != null)
            {
                lblNewDevice.Text = GetString("devicelayout.createdevicelayout") + ": <strong>" + HTMLHelper.HTMLEncode(deviceLayout.ProfileDisplayName) + "</strong>";
            }
        }
        else
        {
            // Display device profile selector
            pnlNewDevice.Visible = true;
            lblNewDevice.Visible = false;
        }

        // Setup the page title
        PageTitle.TitleText = GetString("devicelayout.new");

        // Source device profile selector
        ucDeviceProfile.WhereCondition = "ProfileID IN (SELECT ProfileID FROM CMS_TemplateDeviceLayout WHERE PageTemplateID = " + templateId + ")";
        ucDeviceProfile.SpecialFields.Add(new SpecialField { Text = GetString("devicelayout.defaultdeviceprofile"), Value = "0" });

        // New device profile selector
        // Do not display profiles which have layout already defined
        ucNewDeviceProfile.WhereCondition = "(ProfileEnabled = 1) AND (ProfileID NOT IN (SELECT ProfileID FROM CMS_TemplateDeviceLayout WHERE (PageTemplateID = " + templateId + ")))";
        ucNewDeviceProfile.OrderBy = "ProfileOrder";
        ucNewDeviceProfile.SpecialFields.Add(new SpecialField { Text = GetString("devicelayout.selectdeviceprofile"), Value = "" });
        ucNewDeviceProfile.OnBeforeClientChanged = "EnableSelection(value.length > 0);";

        // Javascript
        rbtnDevice.Attributes.Add("onclick", "EnableSelection(true)");
        rbtnLayout.Attributes.Add("onclick", "EnableSelection(true)");
        rbtnEmptyLayout.Attributes.Add("onclick", "EnableSelection(true)");

        if (!RequestHelper.IsPostBack())
        {
            rbtnDevice.Checked = true;
        }

        // Register the page javascript
        string script = @"

function EnableSelection(enabled) {
    var pnlContent = $cmsj('#" + pnlContent.ClientID + @" :input');
    if (enabled) {
        pnlContent.removeAttr('disabled');
        
        var rbtnDevice = $cmsj('#" + rbtnDevice.ClientID + @"');
        var rbtnLayout = $cmsj('#" + rbtnLayout.ClientID + @"');
        var pnlLayout = $cmsj('#" + pnlLayout.ClientID + @" :input');
        var pnlDevice = $cmsj('#" + pnlDevice.ClientID + @" :input');
        if (rbtnDevice.is(':checked')) {
            pnlLayout.attr('disabled', 'disabled');
        }
        else if (rbtnLayout.is(':checked')) {
            pnlDevice.attr('disabled', 'disabled');
        }
        else {
            pnlLayout.attr('disabled', 'disabled');
            pnlDevice.attr('disabled', 'disabled');
        }
    }
    else {
        pnlContent.attr('disabled', 'disabled');
    }

    $cmsj('#" + pnlNewDevice.ClientID + @" :input').removeAttr('disabled');
}

$cmsj(document).ready(function () {
    EnableSelection(" + (showNewDeviceSelector ? "false" : "true") + @");
});";

        ScriptHelper.RegisterStartupScript(this, typeof(string), "layoutScript", script, true);
        ScriptHelper.RegisterJQuery(Page);

        Save += SaveAction;
    }


    /// <summary>
    /// Save button is clicked.
    /// </summary>
    protected void SaveAction(object sender, EventArgs e)
    {
        // Check permissions
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerUIElement("CMS.Design", "Design.EditLayout"))
        {
            RedirectToUIElementAccessDenied("CMS.Design", "Design.EditLayout");
        }

        // New device profile
        int newDeviceProfileId = deviceProfileId;
        if (newDeviceProfileId == 0)
        {
            newDeviceProfileId = ValidationHelper.GetInteger(ucNewDeviceProfile.Value, 0);
        }

        if (newDeviceProfileId == 0)
        {
            // Show error - select device profile first
            ShowError(GetString("devicelayout.selectdeviceprofile.error"));
            rbtnDevice.Checked = true;
            return;
        }

        PageTemplateInfo pti = PageTemplateInfoProvider.GetPageTemplateInfo(templateId);
        if ((pti != null) && (newDeviceProfileId > 0))
        {
            string layoutCode = null;
            string layoutCSS = null;
            int layoutId = 0;
            LayoutTypeEnum layoutType = LayoutTypeEnum.Ascx;

            if (rbtnDevice.Checked)
            {
                // Copy from device
                int selectedDeviceProfileId = ValidationHelper.GetInteger(ucDeviceProfile.Value, 0);
                if (selectedDeviceProfileId > 0)
                {
                    // Existing device profile
                    PageTemplateDeviceLayoutInfo selectedDeviceLayout = PageTemplateDeviceLayoutInfoProvider.GetTemplateDeviceLayoutInfo(templateId, selectedDeviceProfileId);
                    if (selectedDeviceLayout != null)
                    {
                        layoutId = selectedDeviceLayout.LayoutID;
                        layoutCode = selectedDeviceLayout.LayoutCode;
                        layoutCSS = selectedDeviceLayout.LayoutCSS;
                        layoutType = selectedDeviceLayout.LayoutType;
                    }
                }
                else
                {
                    // Default device
                    layoutType = pti.PageTemplateLayoutType;

                    if (pti.LayoutID > 0)
                    {
                        layoutId = pti.LayoutID;
                    }
                    else
                    {
                        layoutCode = pti.PageTemplateLayout;
                        layoutCSS = pti.PageTemplateCSS;
                    }
                }
            }
            else if (rbtnLayout.Checked)
            {
                // Use existing layout
                int selectedLayoutId = ValidationHelper.GetInteger(ucLayout.Value, 0);
                LayoutInfo selectedLayout = LayoutInfoProvider.GetLayoutInfo(selectedLayoutId);
                if (selectedLayout != null)
                {
                    layoutType = selectedLayout.LayoutType;

                    if (chkCopy.Checked)
                    {
                        // Copy layout code
                        layoutCode = selectedLayout.LayoutCode;
                        layoutCSS = selectedLayout.LayoutCSS;
                    }
                    else
                    {
                        // Copy layout id
                        layoutId = selectedLayoutId;
                    }
                }
            }
            else if (rbtnEmptyLayout.Checked)
            {
                layoutCode = "<cms:CMSWebPartZone ZoneID=\"zoneA\" runat=\"server\" />";
            }

            PageTemplateDeviceLayoutInfo deviceLayout = PageTemplateDeviceLayoutInfoProvider.GetTemplateDeviceLayoutInfo(templateId, newDeviceProfileId);

            if (deviceLayout == null)
            {
                // Create a new device profile layout object
                deviceLayout = new PageTemplateDeviceLayoutInfo();
                deviceLayout.PageTemplateID = templateId;
                deviceLayout.ProfileID = newDeviceProfileId;
            }

            // Modify the device profile layout object with updated values
            deviceLayout.LayoutID = layoutId;
            deviceLayout.LayoutType = layoutType;
            deviceLayout.LayoutCode = layoutCode;
            deviceLayout.LayoutCSS = layoutCSS;

            // Save the device profile layout object
            PageTemplateDeviceLayoutInfoProvider.SetTemplateDeviceLayoutInfo(deviceLayout);
            UIContext.EditedObject = deviceLayout;
            CMSObjectManager.CheckOutNewObject(Page);

            // Register refresh page scripts
            ScriptHelper.RegisterStartupScript(this, typeof(string), "deviceLayoutSaved", "if (wopener) { wopener.location.replace(wopener.location); } CloseDialog();", true);
        }
    }

    #endregion;

}