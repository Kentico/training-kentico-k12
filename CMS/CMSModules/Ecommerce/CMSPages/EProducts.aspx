<%@ Page Title="Download" Language="C#" MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalDialogPage.master"
    AutoEventWireup="true"  Codebehind="EProducts.aspx.cs" Inherits="CMSModules_Ecommerce_CMSPages_EProducts"
    Theme="Default" %>

<%@ Register TagPrefix="cms" TagName="UniGrid" Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<%-- Dialog content --%>
<asp:Content ID="Content6" ContentPlaceHolderID="plcContent" runat="server">
    <%-- Downloads --%>
    <asp:Panel ID="pnlDownloads" runat="server">
        <cms:UniGrid ID="downloadsGridElem" runat="server" IsLiveSite="true">
            <GridColumns>
                <ug:Column Source="##ALL##" ExternalSourceName="file" Caption="$com.orderdownloadsdialog.file$"
                    AllowSorting="true" CssClass="NoWrap" />
                <ug:Column Source="OrderItemValidTo" ExternalSourceName="expiration" Caption="$com.orderdownloadsdialog.expiration$"
                    AllowSorting="true" CssClass="NoWrap" />
            </GridColumns>
            <PagerConfig DefaultPageSize="10" />
        </cms:UniGrid>
    </asp:Panel>
</asp:Content>
<%-- Dialog footer --%>
<asp:Content ContentPlaceHolderID="plcFooter" runat="server">
    <div class="FloatRight">
        <%-- Close --%>
        <cms:CMSButton ID="btnClose" runat="server" ButtonStyle="Primary" EnableViewState="false"
            OnClientClick="return CloseDialog();" />
    </div>
</asp:Content>
