<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Inherits="CMSModules_Messaging_MyMessages_MyMessages_ContactList"
    Title="My messages - Contact list" ValidateRequest="false" Theme="Default"  Codebehind="MyMessages_ContactList.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/ContactList.ascx" TagName="ContactList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:ContactList ID="contactListElem" ShortID="l" runat="server" IsLiveSite="false" ShowItemAsLink="false" />
</asp:Content>
