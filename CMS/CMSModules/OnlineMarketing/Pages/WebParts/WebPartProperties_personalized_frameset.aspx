<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="WebPartProperties_personalized_frameset.aspx.cs" Inherits="CMSModules_OnlineMarketing_Pages_WebParts_WebPartProperties_personalized_frameset" %>


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
<frameset border="0" rows="*,64" runat="server" id="rowsFrameset">
    <frame name="webpartpropertiescontent" frameborder="0" noresize="noresize" scrolling="no"
        runat="server" id="frameContent" />
    <frame name="webpartpropertiesbuttons" frameborder="0" noresize="noresize" scrolling="no"
        runat="server" id="frameButtons" />
    <cms:NoFramesLiteral ID="ltlNoFrames" runat="server" />
</frameset>
</html>