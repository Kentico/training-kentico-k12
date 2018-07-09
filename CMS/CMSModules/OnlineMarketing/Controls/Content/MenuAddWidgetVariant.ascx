<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_OnlineMarketing_Controls_Content_MenuAddWidgetVariant"
     Codebehind="MenuAddWidgetVariant.ascx.cs" %>

<asp:Panel runat="server" ID="pnlWebPartMenu" CssClass="PortalContextMenu WebPartContextMenu">
    <cms:UIPlaceHolder ID="pnlUIAddMVTVariant" runat="server" ModuleName="CMS.Design"
        ElementName="WebPartProperties.Variant">
        <cms:ContextMenuItem runat="server" ID="iAddMVTVariant" />
        <cms:ContextMenuSeparator runat="server" ID="sep1" />
    </cms:UIPlaceHolder>
    <cms:UIPlaceHolder ID="pnlUIAddCPVariant" runat="server" ModuleName="CMS.Design"
        ElementName="WebPartProperties.Variant">
        <cms:ContextMenuItem runat="server" ID="iAddCPVariant" />
    </cms:UIPlaceHolder>
</asp:Panel>

<script type="text/javascript">
    //<![CDATA[
    function ContextAddWebPartMVTVariant(definition) {
        AddMVTVariant(definition.zoneId, definition.webPartId, definition.nodeAliasPath, definition.instanceGuid, definition.templateId, 'widget', '');
    }

    function ContextAddWebPartCPVariant(definition) {
        AddPersonalizationVariant(definition.zoneId, definition.webPartId, definition.nodeAliasPath, definition.instanceGuid, definition.templateId, 'widget', '');
    }
    //]]>
</script>

