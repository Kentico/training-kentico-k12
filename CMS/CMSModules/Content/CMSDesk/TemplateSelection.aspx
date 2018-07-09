<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_TemplateSelection"
    Theme="Default"  Codebehind="TemplateSelection.aspx.cs" MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" %>

<%@ Register Src="New/TemplateSelection.ascx" TagName="TemplateSelection" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/EditMenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/PageTitle.ascx" TagName="PageTitle"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:CMSPanel ID="pnlContainer" runat="server">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="PageHeader SimpleHeader" EnableViewState="false">
            <cms:PageTitle ID="titleElem" ShortID="pt" runat="server" HideTitle="true" />
        </asp:Panel>
        <cms:CMSPanel ID="pnlMenu" runat="server">
            <cms:editmenu ID="menuElem" ShortID="m" runat="server" ShowProperties="false" ShowCreateAnother="false" IsLiveSite="false" />
        </cms:CMSPanel>
    </cms:CMSPanel>
    <div class="new-page-dialog">
        <div class="PTSelection white-wizard-body">
            <asp:Panel ID="pnlContent" runat="server" CssClass="page-content-frame">
                <cms:MessagesPlaceHolder runat="server" ID="plcMess" OffsetX="16" />
                <cms:TemplateSelection ID="selTemplate" runat="server" />
            </asp:Panel>
        </div>
    </div>
    <cms:CMSPanel ID="pnlFooterContent" runat="server">
        <div id="divFooter" class="dialog-footer control-group-inline">
        </div>
    </cms:CMSPanel>
</asp:Content>
