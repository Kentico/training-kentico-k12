<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_OnlineMarketing_Pages_WebParts_WebPartProperties_personalized"
    EnableEventValidation="false" Theme="Default" ValidateRequest="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
     Codebehind="WebPartProperties_personalized.aspx.cs" %>

<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/ContentPersonalizationVariant/Edit.ascx"
    TagName="ContentPersonalizationVariantEdit" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/MVTVariant/Edit.ascx"
    TagName="MvtVariantEdit" TagPrefix="cms" %>
<asp:Content ID="plcContent" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UIContextPanel ID="uiContextPanelCP" runat="server">
        <cms:ContentPersonalizationVariantEdit ID="cpEditElem" runat="server" IsLiveSite="false" Visible="false" />
    </cms:UIContextPanel>
    <cms:UIContextPanel ID="uiContextPanelMVT" runat="server">
        <cms:MvtVariantEdit ID="mvtEditElem" runat="server" IsLiveSite="false" Visible="false" />
    </cms:UIContextPanel>
</asp:Content>
