<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_FacebookConnect_FacebookConnectLogon"  Codebehind="~/CMSWebParts/Membership/FacebookConnect/FacebookConnectLogon.ascx.cs" %>

<asp:Label ID="lblError" runat="server" CssClass="ErrorLabel webpart-error-label" Visible="false" EnableViewState="false" />
<asp:PlaceHolder ID="plcFBButton" runat="server">
    <fb:login-button <%=ShowFacesAttr%> <%=WidthAttr%> <%=MaxRowsAttr%>  <%=ScopeAttr%> onlogin="Facebook_login();"><%=SignInText%></fb:login-button>
</asp:PlaceHolder>
<cms:CMSButton ID="btnSignOut" runat="server" Visible="false" EnableViewState="false" ButtonStyle="Default" />
<asp:HyperLink ID="lnkSignOutImageBtn" runat="server" Visible="false" EnableViewState="false">
    <asp:Image ID="imgSignOut" runat="server" Visible="false" EnableViewState="false" />
</asp:HyperLink>
<asp:HyperLink ID="lnkSignOutLink" runat="server" Visible="false" EnableViewState="false" />
