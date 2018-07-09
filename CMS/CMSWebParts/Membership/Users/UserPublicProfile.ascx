<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_Users_UserPublicProfile"  Codebehind="~/CMSWebParts/Membership/Users/UserPublicProfile.ascx.cs" %>

<asp:Label ID="lblError" CssClass="ErrorLabel" runat="server" Visible="false" EnableViewState="false" />
<asp:PlaceHolder ID="plcContent" runat="server">
    <cms:DataForm ID="formElem" runat="server" IsLiveSite="true" DefaultFormLayout="SingleTable" />
    <asp:Label ID="lblNoProfile" runat="Server" CssClass="NoProfile" Visible="false"
        EnableViewState="false" />
</asp:PlaceHolder>
