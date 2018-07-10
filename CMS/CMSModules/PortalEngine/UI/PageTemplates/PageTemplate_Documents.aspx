<%@ Page Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true"
    Inherits="CMSModules_PortalEngine_UI_PageTemplates_PageTemplate_Documents" Title="Page Template Edit - Pages"
    Theme="Default"  Codebehind="PageTemplate_Documents.aspx.cs" %>

<%@ Register Src="~/CMSModules/AdminControls/Controls/Documents/Documents.ascx" TagName="Documents"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Filters/DocumentFilter.ascx" TagName="DocumentFilter"
    TagPrefix="cms" %>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="server">
    <asp:PlaceHolder ID="plcFilter" runat="server">
        <cms:DocumentFilter ID="filterDocuments" runat="server" />
        <br />
        <br />
    </asp:PlaceHolder>
    <cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <cms:Documents ID="docElem" runat="server" ListingType="PageTemplateDocuments" IsLiveSite="false" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Content>
