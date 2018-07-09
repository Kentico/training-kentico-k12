<%@ Page Language="C#" AutoEventWireup="true" Theme="Default" Inherits="CMSModules_Widgets_LiveDialogs_WidgetProperties_Header"
    MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalSimplePage.master"  Codebehind="WidgetProperties_Header.aspx.cs" %>

<asp:Content ID="pnlContent" ContentPlaceHolderID="plcContent" runat="server">
    <asp:HiddenField ID="hdnSelected" runat="server" />
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
    <asp:Button ID="btnHidden" runat="server" EnableViewState="false" CssClass="HiddenButton"
        OnClick="btnHidden_Click" />
</asp:Content>
