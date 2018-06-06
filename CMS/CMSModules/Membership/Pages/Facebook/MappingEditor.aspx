<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="MappingEditor.aspx.cs" Inherits="CMSModules_Membership_Pages_Facebook_MappingEditor" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master" EnableEventValidation="false" Theme="Default" %>
<%@ Register TagPrefix="cms" TagName="Error" Src="~/CMSModules/Membership/Controls/Facebook/Error.ascx" %>
<%@ Register TagPrefix="cms" TagName="Mapping" Src="~/CMSModules/Membership/Controls/Facebook/Mapping.ascx" %>
<%@ Register TagPrefix="cms" TagName="MappingEditorItem" Src="~/CMSModules/Membership/Controls/Facebook/MappingEditorItem.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="plcContent" runat="Server" EnableViewState="false">
    <asp:HiddenField ID="MappingHiddenField" runat="server" EnableViewState="false" />
    <asp:Panel ID="MappingPanel" runat="server" EnableViewState="false" CssClass="Hidden">
        <cms:Mapping ID="MappingControl" runat="server" EnableViewState="false" />
        <cms:AlertLabel runat="server" AlertType="Warning" ResourceString="fb.mapping.warning" CssClass="facebook-mapping-table-warning"></cms:AlertLabel>
    </asp:Panel>
    <asp:Panel ID="MainPanel" runat="server" EnableViewState="false">
        <cms:Error ID="ErrorControl" runat="server" EnableViewState="false" MessagesEnabled="true" />
        <div class="form-horizontal">
                <asp:Repeater ID="UserMappingItemRepeater" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <cms:MappingEditorItem ID="MappingEditorItemControl" runat="server" EnableViewState="false" />
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="UserSettingsMappingItemRepeater" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <cms:MappingEditorItem ID="MappingEditorItemControl" runat="server" EnableViewState="false" />
                    </ItemTemplate>
                </asp:Repeater>
        </div>
    </asp:Panel>
    <script type="text/javascript">

        $cmsj(document).ready(function () {
            var mappingField = document.getElementById('<%= MappingHiddenField.ClientID %>');
            var sourceMappingField = wopener.document.getElementById('<%= SourceMappingHiddenFieldClientId %>');
            if (mappingField != null && sourceMappingField != null && mappingField.value != null && mappingField.value != '') {
                $cmsj(sourceMappingField).val(mappingField.value);
                var panelElement = document.getElementById('<%= MappingPanel.ClientID %>');
                var sourcePanelElement = wopener.document.getElementById('<%= SourceMappingPanelClientId %>');
                if (panelElement != null && sourcePanelElement != null) {
                    var content = $cmsj(panelElement).html();
                    $cmsj(sourcePanelElement).html(content);
                }
                CloseDialog();
            }
            else {
                var attributeLabel = '<%= HTMLHelper.HTMLEncode(GetString("fb.attributetype.attribute"))%>';
                var restrictedAttributeLabel = '<%= HTMLHelper.HTMLEncode(GetString("fb.attributetype.restrictedattribute"))%>';
                $cmsj("select.AttributeDropDownList").each(function (index, select) {
                    $cmsj(select).find('option[value^="Attribute"]').wrapAll("<optgroup label='" + attributeLabel + "'>");
                    $cmsj(select).find('option[value^="RestrictedAttribute"]').wrapAll("<optgroup label='" + restrictedAttributeLabel + "'>");

                    var html = $cmsj(select).html();
                    $cmsj(select).html(html);
                });
            }
        });

    </script>
</asp:Content>