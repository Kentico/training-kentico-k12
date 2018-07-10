<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Widgets_LiveDialogs_WidgetProperties"
     Codebehind="WidgetProperties.aspx.cs" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <style type="text/css">
        body
        {
            margin: 0px;
            height: 100%;
            width: 100%;
            overflow: hidden;
        }
    </style>

    <script type="text/javascript">
        //<![CDATA[
        function ChangeWidget(zoneId, widgetId, aliasPath) {
            CloseDialog();
            wopener.ConfigureWidget(zoneId, widgetId, aliasPath);
        }
        //]]>
    </script>

</head>
<frameset border="0" rows="40,*" runat="server" id="rowsFrameset">
    <frame name="widgetpropertiesheader" scrolling="no" noresize="noresize" frameborder="0"
        runat="server" id="frameHeader" />
    <frame name="widgetpropertiescontent" frameborder="0" noresize="noresize" scrolling="no"
        runat="server" id="frameContent" />
    <cms:NoFramesLiteral ID="ltlNoFrames" runat="server" />
</frameset>
</html>
