<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Community_Friends_FriendshipManagement"  Codebehind="~/CMSWebParts/Community/Friends/FriendshipManagement.ascx.cs" %>
<div class="FriendsManagement">
    <asp:PlaceHolder ID="plcConfirm" runat="server" Visible="false">
        <asp:Label ID="lblConfirm" runat="server" EnableViewState="false" CssClass="InfoLabel" /><br />
        <cms:CMSButton ID="btnApprove" runat="server" ButtonStyle="Default" />&nbsp;<cms:CMSButton
            ID="btnReject" runat="server" ButtonStyle="Default" />
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcMessage" runat="server" Visible="false">
        <asp:Label ID="lblInfo" runat="server" CssClass="FriendsManagementLabel" EnableViewState="false" /><br />
        <br />
        <asp:HyperLink ID="lnkMyFriends" runat="server" CssClass="MyFriendsLink" EnableViewState="false" />
    </asp:PlaceHolder>
</div>
