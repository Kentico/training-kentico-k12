<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Messaging_Controls_MessageUserButtons"  Codebehind="MessageUserButtons.ascx.cs" %>
<asp:Panel ID="pnlButtons" runat="server" EnableViewState="false">
    <cms:LocalizedButton ButtonStyle="Default" ID="btnAddToContactList" runat="server" EnableViewState="false" OnClick="btnAddToContactList_Click" ResourceString="messaging.contactlist.add" />
    <cms:LocalizedButton ButtonStyle="Default" ID="btnAddToIgnoreList" runat="server" EnableViewState="false" OnClick="btnAddToIgnoreList_Click" ResourceString="messaging.ignorelist.add" />
</asp:Panel>
