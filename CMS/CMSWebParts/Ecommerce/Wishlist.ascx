<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Ecommerce_Wishlist"  Codebehind="~/CMSWebParts/Ecommerce/Wishlist.ascx.cs" %>

       
<table class="WishlistTable" style="width: 100%" cellpadding="0" cellspacing="0">  
    <tr>        
        <td colspan="2">
            <table class="CartStepTable" style="width: 100%" cellpadding="3" border="0">          
                <tr>
                    <td class="CartStepHeader">
                        <asp:Label ID="lblTitle" runat="server" EnableViewState="false" />
                    </td>
                </tr>                                 
                <tr class="CartStepBody">
                    <td style="padding-top:0px;">
                        <asp:Panel ID="pnlWishlist" runat="server" CssClass="CartStepPanel">
                            <asp:Panel ID="pnlWishlistInner" runat="server" CssClass="CartStepInnerPanel" >
                                <asp:Label ID="lblInfo" runat="server" CssClass="WishlistInfo" EnableViewState="false" Visible="false" />
                                <cms:queryrepeater id="repeater" runat="server" /> 
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="btnContinue">
            <cms:CMSButton ID="btnContinue" runat="server" OnClick="btnContinue_Click" ButtonStyle="Default" />
        </td>
        <td></td>
    </tr>
</table>
<asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
