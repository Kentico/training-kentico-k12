<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="Mapping.ascx.cs" Inherits="CMSModules_Membership_FormControls_Facebook_Mapping" %>
<%@ Register TagPrefix="cms" TagName="Mapping" Src="~/CMSModules/Membership/Controls/Facebook/Mapping.ascx" %>
<%@ Register TagPrefix="cms" TagName="Error" Src="~/CMSModules/Membership/Controls/Facebook/Error.ascx" %>
<div class="facebook-mapping-form-control">
    <cms:CMSUpdatePanel ID="MainUpdatePanel" runat="server">
        <ContentTemplate>
            <cms:LocalizedButton ID="EditMappingButton" runat="server" EnableViewState="false" ResourceString="general.edit" ButtonStyle="Default"></cms:LocalizedButton>
            <asp:HiddenField ID="MappingHiddenField" runat="server" EnableViewState="false" />
            <cms:Error ID="ErrorControl" runat="server" EnableViewState="false" />
            <asp:Panel ID="MappingPanel" runat="server" EnableViewState="false">
                <cms:Mapping ID="MappingControl" runat="server"></cms:Mapping>
            </asp:Panel>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</div>
