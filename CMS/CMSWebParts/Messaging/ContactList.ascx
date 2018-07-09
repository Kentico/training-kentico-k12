<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Messaging_ContactList"  Codebehind="~/CMSWebParts/Messaging/ContactList.ascx.cs" %>
<%@ Register Src="~/CMSModules/Messaging/Controls/ContactList.ascx" TagName="ContactList"
    TagPrefix="cms" %>
<cms:ContactList ID="lstContacts" runat="server" />
