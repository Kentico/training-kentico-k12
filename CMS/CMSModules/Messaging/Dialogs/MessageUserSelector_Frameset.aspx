<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Dialogs_MessageUserSelector_Frameset"
    EnableEventValidation="false"  Codebehind="MessageUserSelector_Frameset.aspx.cs" %>

<!DOCTYPE html>
<html>
<head runat="server" enableviewstate="false">
    <title>Message user selector</title>
    <script type="text/javascript">
        //<![CDATA[
        function CloseAndRefresh(userId, mText, mId, mId2) {
            wopener.FillUserName(userId, mText, mId, mId2);
            CloseDialog();
        }

        function Refresh() {
            wopener.document.location.replace(wopener.document.location);
        }
        //]]>
    </script>
</head>
<frameset border="0" rows="<%=TabsFrameHeight%>, *, <%=FooterFrameHeight%>" id="rowsFrameset">
    <frame name="MessageUserSelectorHeader" src="MessageUserSelector_Header.aspx?showtab=<%=QueryHelper.GetText("showtab", "")%>&hidid=<%=QueryHelper.GetText("hidid", "")%>&mid=<%=QueryHelper.GetText("mid", "")%>&refresh=<%=QueryHelper.GetText("refresh", "")%>"
        frameborder="0" scrolling="no" noresize="noresize" />
    <frame name="MessageUserSelectorContent" src="MessageUserSelector_<%=QueryHelper.GetText("showtab", "")%>.aspx?hidid=<%=QueryHelper.GetText("hidid", "")%>&mid=<%=QueryHelper.GetText("mid", "")%>&refresh=<%=QueryHelper.GetText("refresh", "")%>"
        scrolling="auto" frameborder="0" />
    <frame name="MessageUserSelectorFooter" src="MessageUserSelector_Footer.aspx" />
    <cms:NoFramesLiteral ID="ltlNoFrames" runat="server" />
</frameset>
</html>
