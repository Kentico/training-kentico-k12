<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_Users_OnlineUsers"  Codebehind="~/CMSWebParts/Membership/Users/OnlineUsers.ascx.cs" %>

<cms:LocalizedLabel ID="lblError" runat="server" EnableViewState="false" CssClass="ErrorLabel webpart-error-label" Visible="false" />
<asp:Literal runat="server" ID="ltrAdditionaInfos" EnableViewState="false"  />
<asp:Repeater runat="server" ID="repUsers" EnableViewState="false" />
