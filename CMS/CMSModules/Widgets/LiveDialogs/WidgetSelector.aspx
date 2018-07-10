<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Widgets_LiveDialogs_WidgetSelector"
    MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalDialogPage.master" Theme="Default" Title="Add widget"  Codebehind="WidgetSelector.aspx.cs" %>

<%@ Register Src="~/CMSModules/Widgets/Controls/WidgetSelector.ascx" TagName="WidgetSelector"
    TagPrefix="cms" %>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel runat="server" ID="pnlSelector">
        <cms:WidgetSelector runat="server" ID="selectElem" IsLiveSite="true" />
        <cms:LocalizedHidden ID="hdnMessage" runat="server" Value="{$widgets.NoWidgetSelected$}" EnableViewState="false" />
    </asp:Panel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="plcFooter">
    <cms:LocalizedButton runat="server" ID="btnOk" ResourceString="general.select" ButtonStyle="Primary" EnableViewState="false" OnClientClick="SelectCurrentWidget(); return false;" />
</asp:Content>