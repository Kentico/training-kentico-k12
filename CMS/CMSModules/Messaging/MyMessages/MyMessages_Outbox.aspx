<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Inherits="CMSModules_Messaging_MyMessages_MyMessages_Outbox"
    Title="My messages - Inbox" ValidateRequest="false" Theme="Default"  Codebehind="MyMessages_Outbox.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/Outbox.ascx" TagName="Outbox" TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:Outbox ID="outboxElem" ShortID="o" runat="server" IsLiveSite="false" MarkReadMessage="true" />
</asp:Content>
