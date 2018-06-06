<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSAdminControls_UI_Selectors_DocumentGroupSelector"  Codebehind="DocumentGroupSelector.ascx.cs" %>
<div class="control-group-inline">
    <cms:CMSTextBox ID="txtGroups" runat="server" EnableViewState="false" ReadOnly="true" />
    <cms:LocalizedButton ID="btnChange" runat="server" ResourceString="general.change" ButtonStyle="Default" />
</div>
<asp:Literal runat="server" ID="ltlScript" EnableViewState="false" />
<asp:Button runat="server" ID="btnHidden" CssClass="HiddenButton" OnClick="btnHidden_Click" />
