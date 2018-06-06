<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_OpenID_OpenIDLogon"  Codebehind="~/CMSWebParts/Membership/OpenID/OpenIDLogon.ascx.cs" %>
<asp:Panel ID="pnlLogon" runat="server" class="OpenIDContainer">
    <cms:CMSTextBox ID="txtInput" runat="server" />
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
    <asp:Literal ID="ltlProvidersVariables" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnValue" runat="server" />
    <cms:CMSButton ID="btnSignIn" runat="server" Visible="false" OnCommand="btnSignIn_Click"
        EnableViewState="false" ButtonStyle="Default" />
    <asp:ImageButton ID="btnSignInImage" runat="server" Visible="false" OnCommand="btnSignIn_Click"
        EnableViewState="false" CssClass="OpenIDSignIn" />
    <asp:LinkButton ID="btnSignInLink" runat="server" Visible="false" OnCommand="btnSignIn_Click"
        EnableViewState="false" />
    <cms:CMSButton ID="btnSignOut" runat="server" Visible="false" OnCommand="btnSignOut_Click"
        EnableViewState="false" ButtonStyle="Default" />
    <asp:ImageButton ID="btnSignOutImage" runat="server" Visible="false" OnCommand="btnSignOut_Click"
        EnableViewState="false" CssClass="OpenIDSignOut" />
    <asp:LinkButton ID="btnSignOutLink" runat="server" Visible="false" OnCommand="btnSignOut_Click"
        EnableViewState="false" />
    <cms:LocalizedLabel runat="server" ID="lblError" CssClass="ErrorLabel" Visible="false"
        EnableViewState="false" />
</asp:Panel>
