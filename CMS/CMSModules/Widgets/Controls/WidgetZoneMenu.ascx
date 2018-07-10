<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Widgets_Controls_WidgetZoneMenu"  Codebehind="WidgetZoneMenu.ascx.cs" %>
<asp:Panel runat="server" ID="pnlZoneMenu" CssClass="PortalContextMenu ZoneContextMenu">
    <cms:UIPlaceHolder ID="pnlUIProperties" runat="server" ModuleName="CMS.Design" ElementName="Design.WebPartZoneProperties">
        <asp:Panel runat="server" ID="pnlConfigureZone" CssClass="Item">
            <asp:Panel runat="server" ID="pnlConfigureZonePadding" CssClass="ItemPadding">
                <asp:Label runat="server" ID="lblConfigureZone" CssClass="Name" EnableViewState="false"
                    Text="ConfigureZone" />
            </asp:Panel>
        </asp:Panel>
    </cms:UIPlaceHolder>
    <cms:UIPlaceHolder ID="pnlUIWebWebPart" runat="server" ModuleName="CMS.Design" ElementName="Design.AddWebParts">
        <asp:Panel runat="server" ID="pnlSep1" CssClass="Separator"></asp:Panel>
        <asp:Panel runat="server" ID="pnlNewWebPart" CssClass="Item">
            <asp:Panel runat="server" ID="pnlNewWebPartPadding" CssClass="ItemPadding">
                <asp:Label runat="server" ID="lblNewWebPart" CssClass="Name" EnableViewState="false"
                    Text="NewWebPart" />
            </asp:Panel>
        </asp:Panel>
    </cms:UIPlaceHolder>
    <asp:Panel runat="server" ID="pnlCopyAllItem" CssClass="Item">
        <asp:Panel runat="server" ID="pnlCopyAllItemPadding" CssClass="ItemPadding">
            <asp:Label runat="server" ID="lblCopy" CssClass="Name" EnableViewState="false"
                Text="CopyWebPart" />
        </asp:Panel>
    </asp:Panel>
    <cms:UIPlaceHolder ID="pnlUIPaste" runat="server" ModuleName="CMS.Design" ElementName="Design.AddWebParts">
        <asp:Panel runat="server" ID="pnlPaste" CssClass="Item">
            <asp:Panel runat="server" ID="pnlPastePadding" CssClass="ItemPadding">
                <asp:Label runat="server" ID="lblPaste" CssClass="Name" EnableViewState="false"
                    Text="PasteWebPart" />
            </asp:Panel>
        </asp:Panel>
    </cms:UIPlaceHolder>
    <cms:UIPlaceHolder ID="pnlUIDelete" runat="server" ModuleName="CMS.Design" ElementName="Design.RemoveWebParts">
        <asp:Panel runat="server" ID="Panel1" CssClass="Separator"></asp:Panel>
        <asp:Panel runat="server" ID="pnlDelete" CssClass="Item">
            <asp:Panel runat="server" ID="pnlDeletePadding" CssClass="ItemPadding">
                <asp:Label runat="server" ID="lblDelete" CssClass="Name" EnableViewState="false"
                    Text="Delete" />
            </asp:Panel>
        </asp:Panel>
    </cms:UIPlaceHolder>
</asp:Panel>

<script type="text/javascript">
    //<![CDATA[
    var lm_widgetZoneMenu = 'widgetZoneMenu';

    function ContextGetWidgetZoneDefinition() {
        return GetContextMenuParameter(lm_widgetZoneMenu);
    }

    function ContextConfigureWidgetZone() {
        ConfigureWebPartZone(ContextGetWidgetZoneDefinition());
    }

    function ContextNewWidget() {
        NewWidget(ContextGetWidgetZoneDefinition());
    }

    function ContextRemoveAllWidgets() {
        RemoveAllWidgets(ContextGetWidgetZoneDefinition());
    }

    function ContextCopyAllWidgets() {
        var zone = ContextGetWidgetZoneDefinition();
        zone.webPartId = '';
        CopyWebPart(ContextGetWidgetZoneDefinition(), lm_widgetZoneMenu);
    }

    function ContextPasteWidgetZone() {
        PasteWebPart(ContextGetWidgetZoneDefinition());
    }

    PM_EnsurePasteHandler(lm_widgetZoneMenu, '<%= pnlPaste.ClientID %>');
    //]]>
</script>

