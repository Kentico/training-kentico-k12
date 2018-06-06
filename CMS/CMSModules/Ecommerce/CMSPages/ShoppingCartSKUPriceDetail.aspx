<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Ecommerce_CMSPages_ShoppingCartSKUPriceDetail"
    Theme="Default" MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalDialogPage.master"
    Title="Shopping cart - Product price detail"  Codebehind="ShoppingCartSKUPriceDetail.aspx.cs" %>

<%@ Register Src="~/CMSModules/Ecommerce/Controls/ShoppingCart/ShoppingCartSKUPriceDetail.ascx"
    TagName="ShoppingCartSKUPriceDetail" TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="PageContent" style="width: 94%; padding: 20px 12px 15px 12px;">
        <cms:ShoppingCartSKUPriceDetail ID="ucSKUPriceDetail" runat="server" />
    </div>

    <script type="text/javascript">
        //<![CDATA[       
        function Close() {
            CloseDialog();
        }
        //]]>
    </script>

</asp:Content>
<asp:Content ID="cntFooter" runat="server" ContentPlaceHolderID="plcFooter">
    <div class="FloatRight">
        <cms:CMSButton ID="btnClose" runat="server" ButtonStyle="Primary" EnableViewState="false" />
    </div>
</asp:Content>
