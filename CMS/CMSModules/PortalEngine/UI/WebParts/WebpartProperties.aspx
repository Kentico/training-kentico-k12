<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_WebParts_WebpartProperties"
     Codebehind="WebPartProperties.aspx.cs" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Web Part properties</title>
    <script type="text/javascript">
        //<![CDATA[

        var refreshPageOnClose = false;

        // Update the variant slider position (used for selecting the last (new) variant)
        function UpdateVariantPosition(itemCode, variantId) {
            if (wopener.UpdateVariantPosition) {
                wopener.UpdateVariantPosition(itemCode, variantId);
            }
        }

        //]]>
    </script>
</head>
<frameset border="0" id="rowsFrameset" runat="server">
    <frame name="webpartpropertiesheader" scrolling="no" noresize="noresize" frameborder="0"
         id="frameHeader" src="webpartproperties_header.aspx<%=Request.Url.Query%>" />
    <frame name="webpartpropertiescontent" frameborder="0" noresize="noresize" scrolling="no"
        runat="server" id="frameContent" />
    <cms:NoFramesLiteral ID="ltlNoFrames" runat="server" />
</frameset>
</html>
