<%@ Page Language="C#" AutoEventWireup="true" Title="Content personalization variant list"
    Inherits="CMSModules_OnlineMarketing_Pages_Content_ContentPersonalizationVariant_List"
    Theme="Default"  Codebehind="List.aspx.cs" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/ContentPersonalizationVariant/List.ascx"
    TagName="ContentPersonalizationVariantList" TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>
<%@ Register Src="~/CMSModules/Content/Controls/editmenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<asp:Content ContentPlaceHolderID="plcBeforeContent" runat="server">
    <cms:editmenu ID="menuElem" runat="server" HandleWorkflow="false" ShowSave="false"
        HelpTopicName="personalization_variants" IsLiveSite="false" />
    <cms:CMSDocumentPanel ID="pnlDocInfo" runat="server" />
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel runat="server" ID="pnlWarning">
        <cms:DisabledModule runat="server" ID="ucDisabledModule" TestSettingKeys="CMSContentPersonalizationEnabled" />
    </asp:Panel>
    <asp:Panel ID="pnlContainer" runat="server">
        <cms:ContentPersonalizationVariantList ID="listElem" runat="server" IsLiveSite="false" />
    </asp:Panel>
</asp:Content>
