<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Friends_Controls_FriendsList"
     Codebehind="FriendsList.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<asp:Panel ID="pnlBody" runat="server" CssClass="Panel" EnableViewState="true">
    <asp:PlaceHolder ID="plcNoData" runat="server">
        <asp:Panel runat="server" ID="pnlLinkButtons">
            <cms:LocalizedLinkButton ID="btnRejectSelected" runat="server" ResourceString="friends.friendrejectall"
                EnableViewState="false" CssClass="ContentLinkButton" />&nbsp;
            <cms:LocalizedLinkButton ID="btnRemoveSelected" OnClick="btnRemoveSelected_Click"
                runat="server" ResourceString="friends.friendremoveall" EnableViewState="false"
                CssClass="ContentLinkButton" />
            <br/>
        </asp:Panel>
        <asp:Label ID="lblInfo" runat="server" CssClass="InfoLabel" />
    </asp:PlaceHolder>
    <cms:UniGrid runat="server" ID="gridElem" GridName="~/CMSModules/Friends/Controls/FriendsList.xml" HideFilterButton="true" />
    <asp:HiddenField runat="server" ID="hdnRefresh" />
</asp:Panel>
