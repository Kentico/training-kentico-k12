<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_OnlineMarketing_Pages_Widgets_WidgetProperties_Variant"
    Theme="default" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
     Codebehind="WidgetProperties_Variant.aspx.cs" %>

<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/ContentPersonalizationVariant/Edit.ascx"
    TagName="ContentPersonalizationVariantEdit" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/MVTVariant/Edit.ascx"
    TagName="MvtVariantEdit" TagPrefix="cms" %>
<asp:Content ID="plcContent" runat="server" ContentPlaceHolderID="plcContent">
    <cms:ContentPersonalizationVariantEdit ID="cpEditElem" runat="server" IsLiveSite="false"
        Visible="false" />
    <cms:MvtVariantEdit ID="mvtEditElem" runat="server" IsLiveSite="false" Visible="false" />
    <asp:HiddenField runat="server" ID="hidRefresh" Value="0" />
</asp:Content>