<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_PageTemplates_ASPX_Template"
    Theme="Default" ValidateRequest="false"  Codebehind="Template.aspx.cs" %>

<%--REGISTER--%>
<%=DocType%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" enableviewstate="false">
    <asp:literal runat="server" id="ltlTags" enableviewstate="false" />
</head>
<body class="<%=BodyClass%>" <%=BodyParameters%>>
    <form id="form1" runat="server">
        <cms:CMSPortalManager ID="CMSPortalManager" runat="server" />
        <%--CONTENT--%>
    </form>
</body>
</html>
