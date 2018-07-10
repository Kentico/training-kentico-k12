<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Membership_Registration_WindowsLiveID"
     Codebehind="~/CMSWebParts/Membership/Registration/WindowsLiveID.ascx.cs" %>
<asp:Label ID="lblError" runat="server" CssClass="ErrorLabel" Visible="false" EnableViewState="false" />
<asp:HyperLink ID="lnkSignIn" runat="server" Visible="false" EnableViewState="false"
    Style="cursor: pointer;" />
<cms:CMSButton ID="btnSignIn" runat="server" Visible="false" OnCommand="btnSignIn_Click"
    EnableViewState="false" ButtonStyle="Default" />
<cms:CMSButton ID="btnSignOut" runat="server" Visible="false" OnCommand="btnSignOut_Click"
    EnableViewState="false" ButtonStyle="Default" />
<asp:ImageButton ID="btnSignOutImage" runat="server" Visible="false" OnCommand="btnSignOut_Click"
    EnableViewState="false" />
<asp:LinkButton ID="btnSignOutLink" runat="server" Visible="false" OnCommand="btnSignOut_Click"
    EnableViewState="false" />
<asp:Button runat="server" ID="btnHidden" OnClick="btnHidden_Click" Style="display: none" Visible="False" />
