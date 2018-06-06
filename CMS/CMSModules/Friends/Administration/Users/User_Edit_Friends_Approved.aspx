<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Friends_Administration_Users_User_Edit_Friends_Approved"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Theme="default"  Codebehind="User_Edit_Friends_Approved.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsList.ascx" TagName="FriendsList"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <script type="text/javascript" language="javascript">
        //<![CDATA[
        function RefreshWOpener(w) {
            window.location = window.location.href;
        }
        //]]>
    </script>
    <asp:Label runat="server" ID="lblError" CssClass="ErrorLabel" Visible="false" EnableViewState="false" />
    <asp:PlaceHolder ID="plcTable" runat="server">
        <cms:FriendsList ID="FriendsList" runat="server" Visible="true" IsLiveSite="false" />
    </asp:PlaceHolder>
</asp:Content>
