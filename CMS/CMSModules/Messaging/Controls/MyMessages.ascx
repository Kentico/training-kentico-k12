<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Controls_MyMessages"
     Codebehind="MyMessages.ascx.cs" %>
<%@ Register Src="~/CMSModules/Messaging/Controls/Inbox.ascx" TagName="Inbox" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Messaging/Controls/Outbox.ascx" TagName="Outbox" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Messaging/Controls/ContactList.ascx" TagName="ContactList"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Messaging/Controls/IgnoreList.ascx" TagName="IgnoreList"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/PageTitle.ascx" TagName="title"
    TagPrefix="cms" %>
<asp:Panel runat="server" ID="pnlBody" CssClass="MyMessages">
    <asp:Panel runat="server" ID="pnlHeader" CssClass="TabsHeader">
        <cms:UITabs ID="tabMenu" ShortID="t" runat="server" />
    </asp:Panel>
    <div class="TabsContent">
        <cms:Inbox ID="ucInbox" ShortID="i" runat="server" />
        <cms:Outbox ID="ucOutbox" ShortID="o" runat="server" />
        <cms:ContactList ID="ucContactList" ShortID="cl" runat="server" />
        <cms:IgnoreList ID="ucIgnoreList" ShortID="il" runat="server" />
    </div>
</asp:Panel>
<asp:Literal runat="server" ID="litMessage" Visible="false" EnableViewState="false" />
