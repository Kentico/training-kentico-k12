<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Inherits="CMSModules_Messaging_MyMessages_MyMessages_Inbox"
    Title="My messages - Inbox" ValidateRequest="false" Theme="Default"  Codebehind="MyMessages_Inbox.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/Inbox.ascx" TagName="Inbox" TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:Inbox ID="inboxElem" ShortID="i" runat="server" IsLiveSite="false" />
</asp:Content>
