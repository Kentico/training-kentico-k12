<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_CMSPages_SendMessage"
    MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalSimplePage.master" Theme="default"  Codebehind="SendMessage.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/SendMessage.ascx" TagName="SendMessage"
    TagPrefix="cms" %>
<asp:Content ID="cntContent" runat="server" ContentPlaceHolderID="plcContent">
    <div class="MessagingBox MessagingDialog">
        <cms:SendMessage ID="ucSendMessage" runat="server" Visible="true" />
    </div>
</asp:Content>
