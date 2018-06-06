<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Friends_Controls_FriendsRequestedList"
     Codebehind="FriendsRequestedList.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<asp:Panel ID="pnlBody" runat="server" CssClass="Panel" EnableViewState="true">
    <asp:PlaceHolder ID="plcNoData" runat="server">
        <asp:Panel runat="server" ID="pnlLinkButtons">
            <cms:LocalizedLinkButton ID="btnRemoveSelected" OnClick="btnRemoveSelected_Click"
                runat="server" ResourceString="friends.friendremoveall" EnableViewState="false"
                CssClass="ContentLinkButton" />
            <br/>
        </asp:Panel>
        <asp:Label ID="lblInfo" runat="server" CssClass="InfoLabel" />
    </asp:PlaceHolder>
    <cms:UniGrid runat="server" ID="gridElem" GridName="~/CMSModules/Friends/Controls/FriendsRequestedList.xml" HideFilterButton="true" />
    <asp:HiddenField runat="server" ID="hdnRefresh" />
</asp:Panel>
