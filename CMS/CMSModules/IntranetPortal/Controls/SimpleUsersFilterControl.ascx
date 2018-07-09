<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_IntranetPortal_Controls_SimpleUsersFilterControl"  Codebehind="~/CMSModules/IntranetPortal/Controls/SimpleUsersFilterControl.ascx.cs" %>
<asp:Panel CssClass="Filter" DefaultButton="btnSelect" runat="server" ID="pnlUsersFilter">
    <span class="FilterSearch">
        <cms:LocalizedLabel ID="lblValue" runat="server" EnableViewState="false" AssociatedControlID="txtValue"
            Display="false" ResourceString="general.searchexpression" />
        <cms:CMSTextBox runat="server" ID="txtValue" EnableViewState="false" />
        <cms:CMSButton runat="server" ID="btnSelect" OnClick="btnSelect_Click" EnableViewState="false" ButtonStyle="Default" />
    </span>
</asp:Panel>
