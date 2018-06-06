<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Friends_Controls_FriendsToApprovalList"
     Codebehind="FriendsToApprovalList.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<asp:Panel ID="pnlBody" runat="server" CssClass="Panel" EnableViewState="true">
    <asp:PlaceHolder ID="plcNoData" runat="server">
        <asp:Panel runat="server" ID="pnlLinkButtons">
            <cms:LocalizedLinkButton ID="btnApproveSelected" runat="server" ResourceString="friends.friendapproveall"
                EnableViewState="false" CssClass="ContentLinkButton" />&nbsp;
            <cms:LocalizedLinkButton ID="btnRejectSelected" runat="server" ResourceString="friends.friendrejectall"
                EnableViewState="false" CssClass="ContentLinkButton" />
            <br />
        </asp:Panel>
        <asp:Label ID="lblInfo" runat="server" CssClass="InfoLabel" />
    </asp:PlaceHolder>
    <cms:UniGrid runat="server" ID="gridElem" GridName="~/CMSModules/Friends/Controls/FriendsToApprovalList.xml" HideFilterButton="true" />
    <asp:HiddenField runat="server" ID="hdnRefresh" />
</asp:Panel>
