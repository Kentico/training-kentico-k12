<%@ Page Language="C#" AutoEventWireup="true" Theme="default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Inherits="CMSModules_Friends_Administration_Users_User_Edit_Friends_Requested"
     Codebehind="User_Edit_Friends_Requested.aspx.cs" %>

<%@ Register Src="~/CMSModules/Friends/Controls/FriendsRequestedList.ascx" TagName="FriendsListRequested"
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
        <cms:FriendsListRequested ID="FriendsListRequested" runat="server" Visible="true"
            IsLiveSite="false" />
    </asp:PlaceHolder>
</asp:Content>
