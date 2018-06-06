<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="Mapping.ascx.cs" Inherits="CMSModules_Membership_Controls_Facebook_Mapping" %>
<%@ Import Namespace="CMS.ExternalAuthentication.Facebook" %>
<%@ Register TagPrefix="cms" TagName="FacebookError" Src="~/CMSModules/Membership/Controls/Facebook/Error.ascx" %>
<cms:FacebookError ID="FacebookError" runat="server" EnableViewState="false" />
<div id="ContainerControl" runat="server" visible="false" class="facebook-mapping-table">
    <table visible="false">
        <tbody>
            <tr>
                <td class="facebook-mapping-field-header"><%= HTMLHelper.HTMLEncode(GetString("fb.mapping.fieldheader"))%></td>
                <td class="facebook-mapping-attribute-header"><%= HTMLHelper.HTMLEncode(GetString("fb.mapping.attributeheader"))%></td>
            </tr>
            <asp:Repeater ID="UserMappingItemRepeater" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="facebook-mapping-field">
                            <%# HTMLHelper.HTMLEncode(GetUserFieldDisplayName(((EntityMappingItem)Container.DataItem).FieldName)) %>
                        </td>
                        <td class="facebook-mapping-attribute"><%# HTMLHelper.HTMLEncode(GetFacebookUserAttributeDisplayName(((EntityMappingItem)Container.DataItem).AttributeName)) %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="UserSettingsMappingItemRepeater" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="facebook-mapping-field">
                            <%# HTMLHelper.HTMLEncode(GetUserSettingsFieldDisplayName(((EntityMappingItem)Container.DataItem).FieldName)) %>
                            <span class="facebook-mapping-field-note">(<%= HTMLHelper.HTMLEncode(ResHelper.GetString("objecttype.cms_usersettings")) %>)</span>
                        </td>
                        <td class="facebook-mapping-attribute"><%# HTMLHelper.HTMLEncode(GetFacebookUserAttributeDisplayName(((EntityMappingItem)Container.DataItem).AttributeName)) %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>
<p id="MessageControl" runat="server" enableviewstate="false" visible="false"></p>
