<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_New_NewPage"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" Title="Content - New page"  Codebehind="NewPage.aspx.cs" %>

<%@ Register Src="TemplateSelection.ascx" TagName="TemplateSelection" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/EditMenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/PageTitle.ascx" TagName="PageTitle"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:CMSPanel ID="pnlContainer" runat="server" FixedPosition="true">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="PageHeader SimpleHeader" EnableViewState="false">
            <cms:PageTitle ID="titleElem" ShortID="pt" runat="server" HideTitle="true" />
        </asp:Panel>
        <cms:CMSPanel ID="pnlMenu" runat="server">
            <cms:editmenu ID="menuElem" ShortID="m" runat="server" ShowProperties="false" ShowSpellCheck="false"
                IsLiveSite="false" />
        </cms:CMSPanel>
    </cms:CMSPanel>
    <div class="new-page-dialog">
        <div class="PTSelection">
            <cms:MessagesPlaceHolder ID="plcMess" runat="server" IsLiveSite="false" ContainerCssClass="message-label"  />
            <asp:Panel ID="pnlContent" runat="server" CssClass="page-content-frame">
                <div class="page-name-form form-horizontal">
                    <div class="form-group">
                        <div class="editing-form-label-cell">
                            <cms:LocalizedLabel CssClass="control-label" ID="lblPageName" runat="server" ResourceString="NewPage.PageName" />
                        </div>
                        <div class="editing-form-value-cell">
                            <cms:CMSTextBox ID="txtPageName" runat="server" />
                            <asp:Label ID="lblError" runat="server" CssClass="form-control-error" EnableViewState="false" />
                        </div>
                    </div>
                </div>
                <cms:TemplateSelection ID="selTemplate" ShortID="s" runat="server" />
            </asp:Panel>
        </div>
    </div>
    <cms:CMSPanel ID="pnlFooterContent" runat="server">
        <div id="divFooter" class="dialog-footer control-group-inline">
        </div>
    </cms:CMSPanel>
</asp:Content>
