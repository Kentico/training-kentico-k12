<%@ Page Language="C#" Theme="default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    AutoEventWireup="true" Inherits="CMSModules_Friends_Administration_Users_User_Edit_Friends_Rejected"
     Codebehind="User_Edit_Friends_Rejected.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsRejectedList.ascx" TagName="FriendsListRejected"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Label runat="server" ID="lblError" CssClass="ErrorLabel" Visible="false" EnableViewState="false" />
    <asp:PlaceHolder ID="plcTable" runat="server">
        <cms:FriendsListRejected ID="FriendsListRejected" runat="server" Visible="true" IsLiveSite="false" />
    </asp:PlaceHolder>
</asp:Content>
