<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_WebParts_Development_WebPart_Design"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" EnableEventValidation="false"
     Codebehind="WebPart_Design.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/PageElements/HeaderActions.ascx" TagName="HeaderActions"
    TagPrefix="cms" %>

<%=DocType%>
<html xmlns="http://www.w3.org/1999/xhtml" <%=XmlNamespace%>>
<head id="Head1" runat="server" enableviewstate="false">
    <title id="Title1" runat="server">My site</title>
    <asp:Literal runat="server" ID="ltlTags" EnableViewState="false" />
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
            height: 100%;
            font-family: Arial;
            font-size: small;
        }
    </style>
</head>
<body class="WebPartDesign cms-bootstrap <%=BodyClass%>" <%=BodyParameters%>>
    <form id="form" runat="server">
        <asp:PlaceHolder runat="server" ID="plcManagers">
            <asp:ScriptManager ID="manScript" runat="server" ScriptMode="Release"
                EnableViewState="false" />
            <asp:Panel ID="pnlActions" runat="server" CssClass="cms-edit-menu" EnableViewState="false">
                <cms:HeaderActions ID="actionsElem" runat="server" ShortID="a" IsLiveSite="false" />
            </asp:Panel>
            <cms:CMSDocumentManager ID="docMan" ShortID="dm" runat="server" 
                IsLiveSite="false" />
            <cms:CMSPortalManager ID="manPortal" ShortID="m" runat="server" EnableViewState="false" />
        </asp:PlaceHolder>
        <div class="WebPartDefaultContentEnvelope">
            <div class="WebPartDefaultContent">
                <cms:CMSPagePlaceholder ID="plc" runat="server" Root="true" DisplayHeader="false">
                    <LayoutTemplate>
                        <cms:CMSWebPartZone runat="server" ZoneId="zone" DisplayHeader="false" AllowModifyWebPartCollection="false" />
                    </LayoutTemplate>
                </cms:CMSPagePlaceholder>
            </div>
        </div>
    </form>
</body>
</html>
