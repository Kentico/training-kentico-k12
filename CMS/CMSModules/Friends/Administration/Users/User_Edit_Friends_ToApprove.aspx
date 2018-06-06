<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Theme="default" Inherits="CMSModules_Friends_Administration_Users_User_Edit_Friends_ToApprove"
     Codebehind="User_Edit_Friends_ToApprove.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsToApprovalList.ascx" TagName="FriendsListToApprove"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Label runat="server" ID="lblError" CssClass="ErrorLabel" Visible="false" EnableViewState="false" />
    <asp:PlaceHolder ID="plcTable" runat="server">
        <cms:FriendsListToApprove ID="FriendsListToApprove" runat="server" Visible="true"
            IsLiveSite="false" />
    </asp:PlaceHolder>
</asp:Content>
