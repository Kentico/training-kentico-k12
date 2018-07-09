<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="MVT variant list" Inherits="CMSModules_OnlineMarketing_Pages_Content_MVTVariant_List"
    Theme="Default"  Codebehind="List.aspx.cs" %>

<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/MVTVariant/List.ascx"
    TagName="MvtvariantList" TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel runat="server" ID="pnlDisabled">
        <cms:DisabledModule runat="server" ID="ucDisabledModule" TestSettingKeys="CMSAnalyticsEnabled;CMSMVTEnabled" />
    </asp:Panel>
    <cms:MvtvariantList ID="listElem" runat="server" IsLiveSite="false" />
</asp:Content>
