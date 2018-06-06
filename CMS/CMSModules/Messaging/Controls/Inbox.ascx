<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Controls_Inbox"
     Codebehind="Inbox.ascx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Messaging/Controls/SendMessage.ascx" TagName="SendMessage"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Messaging/Controls/ViewMessage.ascx" TagName="ViewMessage"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/BreadCrumbs.ascx" TagName="Breadcrumbs" TagPrefix="cms" %>

<cms:CMSUpdatePanel ID="pnlInbox" runat="server">
    <ContentTemplate>
        <div class="MyMessagesPlaceholder">
            <cms:MessagesPlaceHolder ContainerCssClass="header-panel" ID="plcMess" runat="server" />
        </div>
        <asp:Panel ID="pnlBackToList" runat="Server" Visible="false">
            <cms:Breadcrumbs ID="ucBreadcrumbs" runat="server" HideBreadcrumbs="false" PropagateToMainNavigation="false" EnableViewState="false" />
        </asp:Panel>
        <asp:Panel ID="pnlNew" runat="server" CssClass="PageContent" Visible="false">
            <cms:LocalizedHeading runat="server" ID="headNewMessage" Level="4" ResourceString="messaging.newmessage" EnableViewState="false" />
            <cms:SendMessage ID="ucSendMessage" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlView" runat="server" Visible="false">
            <asp:Panel ID="pnlActions" CssClass="header-panel" runat="server" EnableViewState="false">
                <cms:LocalizedButton ID="btnReply" runat="Server" EnableViewState="false" ResourceString="Messaging.Reply" ButtonStyle="Primary" />
                <cms:LocalizedButton ID="btnForward" runat="Server" EnableViewState="false" ResourceString="Messaging.Forward" ButtonStyle="Primary" />
                <cms:LocalizedButton ID="btnDelete" runat="Server" EnableViewState="false" ResourceString="Messaging.Delete" ButtonStyle="Primary" />
            </asp:Panel>
            <div class="PageContent">
                <cms:LocalizedHeading runat="server" ID="headOriginalMessage" Level="4" ResourceString="Messaging.ViewMessage" EnableViewState="false" />
                <cms:ViewMessage ID="ucViewMessage" runat="server" StopProcessing="true" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlList" runat="server">
            <asp:Panel ID="pnlGeneralActions" runat="Server" CssClass="header-panel">
                <cms:LocalizedButton ID="btnNewMessage" runat="Server" ButtonStyle="Primary" EnableViewState="false" ResourceString="Messaging.NewMessage" />
            </asp:Panel>
            <div class="PageContent">
                <cms:UniGrid runat="server" ID="inboxGrid" ShortID="g" GridName="~/CMSModules/Messaging/Controls/Inbox.xml"
                    DelayedReload="true" OrderBy="MessageSent DESC" />
                <asp:Panel ID="pnlAction" runat="server" CssClass="form-horizontal mass-action">
                    <div class="form-group">
                        <div class="mass-action-value-cell">
                            <cms:CMSDropDownList ID="drpWhat" runat="server" />
                            <cms:CMSDropDownList ID="drpAction" runat="server" />
                            <cms:LocalizedButton ID="btnOk" runat="server" ResourceString="general.ok" ButtonStyle="Primary"
                                EnableViewState="false" />
                        </div>
                    </div>
                    <asp:Label ID="lblActionInfo" runat="server" EnableViewState="false" CssClass="InfoLabel" />
                </asp:Panel>
            </div>
        </asp:Panel>
        <asp:Button ID="btnHidden" runat="server" CssClass="HiddenButton" EnableViewState="false" />
        <asp:HiddenField ID="hdnValue" runat="server" EnableViewState="false" />
        <asp:LinkButton ID="lnkBackHidden" runat="server" EnableViewState="false" CausesValidation="false" />
    </ContentTemplate>
</cms:CMSUpdatePanel>