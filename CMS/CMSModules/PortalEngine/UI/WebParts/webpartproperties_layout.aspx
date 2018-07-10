<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_WebParts_webpartproperties_layout"
    Theme="Default" EnableEventValidation="false"  Codebehind="webpartproperties_layout.aspx.cs"
    MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" %>

<%@ Register TagPrefix="cms" Src="~/CMSModules/AdminControls/Controls/Preview/PreviewHierarchy.ascx"
    TagName="PreviewHierarchy" %>
<asp:Content runat="server" ID="cntContent" ContentPlaceHolderID="plcContent">
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
        var refreshPageOnClose = false;

        // Event risen when the Ok button is clicked
        function OnOkHandler() {
            actionPerformed('saveandclose');
        }

        // Event risen when the Apply button is clicked
        function OnApplyHandler() {
            actionPerformed('save');
        }
        //]]>
    </script>
    <cms:PreviewHierarchy ID="ucHierarchy" runat="server" CookiesPreviewStateName="wpl"
        ContentControlPath="~/CMSModules/PortalEngine/Controls/Layout/General.ascx" ShowPanelSeparator="false" />
</asp:Content>
