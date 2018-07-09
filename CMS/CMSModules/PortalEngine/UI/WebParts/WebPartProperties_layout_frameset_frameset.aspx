<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="WebPartProperties_layout_frameset_frameset.aspx.cs"
    Inherits="CMSModules_PortalEngine_UI_WebParts_WebPartProperties_layout_frameset_frameset" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server" enableviewstate="false">
    <title>Untitled Page</title>
    <style type="text/css">
        body
        {
            margin: 0px;
            height: 100%;
            width: 100%;
            overflow: hidden;
        }
    </style>
</head>
<frameset border="0" rows="35,*" runat="server" id="rowsFrameset">
    <frame name="webpartlayoutheader" frameborder="0" noresize="noresize" scrolling="no"
         id="frameHeader" src="WebPartProperties_layout_frameset_header.aspx?<%=QueryHelper.EncodedQueryString%>" />
    <frame name="webpartlayoutcontent" frameborder="0" noresize="noresize" 
         id="frameContent"  />
    <cms:NoFramesLiteral ID="ltlNoFrames" runat="server" />
</frameset>
</html>
