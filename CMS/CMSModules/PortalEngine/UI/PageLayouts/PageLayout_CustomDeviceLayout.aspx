<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_PageLayouts_PageLayout_CustomDeviceLayout"
    Theme="Default" ValidateRequest="false" EnableEventValidation="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="New device layout"  Codebehind="PageLayout_CustomDeviceLayout.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelectorControl"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:CMSPanel ID="pnlContent" runat="server" CssClass="form-horizontal">
        <div class="form-group">
            <cms:CMSPanel ID="pnlNewDevice" Visible="false" runat="server">
                <cms:LocalizedHeading runat="server" Level="4" ResourceString="devicelayout.createdevicelayout" />
                <asp:Panel ID="pnlNewDeviceSelector" runat="server" CssClass="DeviceLayoutPadding selector-subitem">
                    <cms:UniSelectorControl ID="ucNewDeviceProfile" ShortID="ndps" ObjectType="CMS.DeviceProfile"
                        runat="server" ReturnColumnName="ProfileID" SelectionMode="SingleDropDownList"
                        AllowEmpty="false" IsLiveSite="false" />
                </asp:Panel>
            </cms:CMSPanel>
            <cms:LocalizedLabel ID="lblNewDevice" runat="server" CssClass="DeviceLayoutLabel" />
        </div>
        <cms:CMSPanel ID="pnlDeviceLayout" runat="server" CssClass="DeviceLayoutSection">
            <cms:LocalizedHeading ID="LocalizedHeading1" runat="server" Level="4" ResourceString="devicelayout.selectlayoutsource" />
            <div class="radio-list-vertical">
                <cms:CMSRadioButton ID="rbtnDevice" runat="server" ResourceString="devicelayout.copyfromdevice"
                    GroupName="rbtnTypes" />
                <asp:Panel ID="pnlDevice" runat="server" CssClass="selector-subitem">
                    <cms:UniSelectorControl ID="ucDeviceProfile" ShortID="dps" ObjectType="CMS.DeviceProfile"
                        runat="server" ReturnColumnName="ProfileID" SelectionMode="SingleDropDownList"
                        AllowEmpty="false" IsLiveSite="false" />
                </asp:Panel>
                <cms:CMSRadioButton ID="rbtnLayout" runat="server" ResourceString="devicelayout.useexistinglayout"
                    GroupName="rbtnTypes" />
                <asp:Panel ID="pnlLayout" runat="server" CssClass="selector-subitem">
                    <cms:UniSelectorControl ID="ucLayout" ShortID="ls" ObjectType="CMS.Layout" runat="server"
                        AllowEmpty="false" ReturnColumnName="LayoutID" SelectionMode="SingleDropDownList"
                        IsLiveSite="false" />
                    <div class="control-group-inline">
                        <cms:CMSCheckBox ID="chkCopy" runat="server" Checked="true" ResourceString="devicelayout.copyascustom"
                            ToolTipResourceString="devicelayout.copyasnew.tooltip" />
                    </div>
                </asp:Panel>
                <cms:CMSRadioButton ID="rbtnEmptyLayout" runat="server" ResourceString="devicelayout.useemptylayout"
                    GroupName="rbtnTypes" />
            </div>
        </cms:CMSPanel>
    </cms:CMSPanel>
</asp:Content>