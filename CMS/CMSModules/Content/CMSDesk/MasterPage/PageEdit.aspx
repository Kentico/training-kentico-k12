<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_MasterPage_PageEdit"
    Theme="Default" ValidateRequest="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Master page - page edit"  Codebehind="PageEdit.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="cms" Src="~/CMSModules/AdminControls/Controls/Preview/PreviewHierarchy.ascx"
    TagName="PreviewHierarchy" %>
<asp:Content ContentPlaceHolderID="plcContent" runat="server">
<script type="text/javascript" language="javascript">
    function refresh() {
        location.href = location.href;
    }
</script>
    <cms:PreviewHierarchy ID="ucHierarchy" runat="server" CookiesPreviewStateName="mp"
        ContentControlPath="~/CMSModules/Content/Controls/MasterPage.ascx" ShowPanelSeparator="false"
        StorePreviewScrollPosition="true" />
</asp:Content>
