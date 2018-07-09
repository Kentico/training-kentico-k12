<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_LinkedIn_LinkedInLogon"  Codebehind="~/CMSWebParts/Membership/LinkedIn/LinkedInLogon.ascx.cs" %>
<cms:LocalizedLabel ID="lblError" runat="server" EnableViewState="false" CssClass="ErrorLabel webpart-error-label" Visible="False" />
<cms:CMSButton ID="btnSignIn" runat="server" OnCommand="btnSignIn_Click" EnableViewState="false" Visible="false" ButtonStyle="Default" />
<asp:ImageButton ID="btnSignInImage" runat="server" OnCommand="btnSignIn_Click" EnableViewState="false" Visible="false"  />
<asp:LinkButton ID="btnSignInLink" runat="server" OnCommand="btnSignIn_Click" EnableViewState="false" Visible="false" />
<cms:CMSButton ID="btnSignOut" runat="server" OnCommand="btnSignOut_Click" EnableViewState="false" Visible="false" ButtonStyle="Default" />
<asp:ImageButton ID="btnSignOutImage" runat="server" OnCommand="btnSignOut_Click" EnableViewState="false" Visible="false" />
<asp:LinkButton ID="btnSignOutLink" runat="server" OnCommand="btnSignOut_Click" EnableViewState="false" Visible="false" />
