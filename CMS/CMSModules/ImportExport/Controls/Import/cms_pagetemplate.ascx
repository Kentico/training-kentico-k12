<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="cms_pagetemplate.ascx.cs" Inherits="CMSModules_ImportExport_Controls_Import_cms_pagetemplate" %>
<asp:Panel runat="server" ID="pnlCheck" CssClass="wizard-section">
    <div class="checkbox-list-vertical">
        <cms:CMSCheckBox ID="chkObject" runat="server" />
        <cms:CMSCheckBox ID="chkVariants" runat="server" />
    </div>
</asp:Panel>