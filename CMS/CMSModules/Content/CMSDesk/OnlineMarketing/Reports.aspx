<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="Reports.aspx.cs" Inherits="CMSModules_Content_CMSDesk_OnlineMarketing_Reports"
    Theme="Default" Title="Page's reports" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    EnableEventValidation="false" %>

<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>
<%@ Register Src="~/CMSModules/WebAnalytics/Controls/SelectGraphTypeAndPeriod.ascx"
    TagName="GraphType" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/WebAnalytics/Controls/GraphPreLoader.ascx" TagName="GraphPreLoader"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="header-panel">
        <asp:Panel runat="server" ID="pnlDisabled">
            <cms:DisabledModule runat="server" ID="ucDisabledModule" TestSettingKeys="CMSAnalyticsEnabled" />
        </asp:Panel>
        <cms:GraphPreLoader runat="server" ID="ucGraphPreLoader" />
        <cms:GraphType runat="server" ID="ucGraphType" />
    </div>
    <asp:Panel ID="reportTypePnl" class="header-panel radio-list-vertical" runat="server">
        <cms:CMSRadioButton runat="server" ID="rbContent" ResourceString="development.content"
            AutoPostBack="true" CssClass="PageReportRadioButton" GroupName="Radio" Checked="true" />
        <cms:CMSRadioButton runat="server" ID="rbTraffic" ResourceString="analytics_codename.trafficsources"
            AutoPostBack="true" CssClass="PageReportRadioButton" GroupName="Radio" />
    </asp:Panel>
    <div class="ReportBody">
        <asp:Panel runat="server" ID="pnlContent">
        </asp:Panel>
    </div>
</asp:Content>
