<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Friends_Controls_FriendsRejectedList"
     Codebehind="FriendsRejectedList.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<asp:Panel ID="pnlBody" runat="server" CssClass="Panel">
    <asp:PlaceHolder ID="plcLink" runat="server">
        <cms:LocalizedLinkButton ID="btnShowHide" runat="server" ResourceString="friends.showrejected"
            OnClick="btnShowHide_Click" EnableViewState="false" />
        <br />
        <br />
    </asp:PlaceHolder>
    <asp:Panel ID="pnlInner" runat="server">
        <asp:PlaceHolder ID="plcNoData" runat="server">
            <asp:Panel runat="server" ID="pnlLinkButtons">
                <cms:LocalizedLinkButton ID="btnApproveSelected" runat="server" ResourceString="friends.friendapproveall"
                    EnableViewState="false" CssClass="ContentLinkButton" />&nbsp;
                <cms:LocalizedLinkButton ID="btnRemoveSelected" OnClick="btnRemoveSelected_Click"
                    runat="server" ResourceString="friends.friendremoveall" EnableViewState="false"
                    CssClass="ContentLinkButton" />
                <br/>
            </asp:Panel>
            <asp:Label ID="lblInfo" runat="server" CssClass="InfoLabel" />
        </asp:PlaceHolder>
        <cms:UniGrid runat="server" ID="gridElem" GridName="~/CMSModules/Friends/Controls/FriendsRejectedList.xml" HideFilterButton="true" />
        <asp:HiddenField runat="server" ID="hdnRefresh" />
    </asp:Panel>
</asp:Panel>
