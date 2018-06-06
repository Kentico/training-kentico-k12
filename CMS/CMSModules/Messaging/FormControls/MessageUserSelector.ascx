<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Messaging_FormControls_MessageUserSelector"  Codebehind="MessageUserSelector.ascx.cs" %>
<cms:CMSTextBox ID="txtUser" runat="server" EnableViewState="false" CssClass="ToField" />
    <cms:CMSButton ID="btnSelect" runat="server" ButtonStyle="Default" EnableViewState="false" />
<asp:HiddenField ID="hiddenField" runat="server" />