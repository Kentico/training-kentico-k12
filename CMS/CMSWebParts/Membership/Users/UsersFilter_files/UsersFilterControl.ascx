<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_Membership_Users_UsersFilter_files_UsersFilterControl"  Codebehind="~/CMSWebParts/Membership/Users/UsersFilter_files/UsersFilterControl.ascx.cs" %>
<asp:Panel CssClass="Filter" DefaultButton="btnSelect" runat="server" ID="pnlUsersFilter">
    <span class="FilterSort">
        <asp:Label runat="server" ID="lblSortBy" EnableViewState="false" />
        <asp:LinkButton runat="server" ID="lnkSortByUserName" OnClick="lnkSortByUserName_Click"
            EnableViewState="false" />
        <asp:LinkButton runat="server" ID="lnkSortByActivity" OnClick="lnkSortByActivity_Click"
            EnableViewState="false" />
    </span><span class="FilterSearch">
        <cms:LocalizedLabel ID="lblValue" runat="server" EnableViewState="false" AssociatedControlID="txtValue"
            Display="false" ResourceString="general.searchexpression" />
        <cms:CMSTextBox runat="server" ID="txtValue" EnableViewState="false" />
        <cms:CMSButton runat="server" ID="btnSelect" OnClick="btnSelect_Click" EnableViewState="false" ButtonStyle="Default" />
    </span>
</asp:Panel>
