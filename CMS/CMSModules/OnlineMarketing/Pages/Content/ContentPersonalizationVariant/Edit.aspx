<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Content personalization variant properties" Inherits="CMSModules_OnlineMarketing_Pages_Content_ContentPersonalizationVariant_Edit" Theme="Default"  Codebehind="Edit.aspx.cs" %>
<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/ContentPersonalizationVariant/Edit.ascx"
    TagName="ContentPersonalizationVariantEdit" TagPrefix="cms" %>
    
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:ContentPersonalizationVariantEdit ID="editElem" runat="server" IsLiveSite="false" />
</asp:Content>