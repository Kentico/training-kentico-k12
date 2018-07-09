<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Widgets_LiveDialogs_WidgetProperties_Properties" Theme="default"
    EnableEventValidation="false" ValidateRequest="false"  Codebehind="WidgetProperties_Properties.aspx.cs"
    MasterPageFile="~/CMSMasterPages/LiveSite/EmptyPage.master" %>

<%@ Register Src="~/CMSModules/Widgets/Controls/WidgetProperties.ascx" TagName="WidgetProperties"
    TagPrefix="cms" %>

<asp:Content ID="pnlContent" ContentPlaceHolderID="plcContent" runat="server">
    <asp:Panel runat="server" ID="pnlBody" CssClass="PageContent" >
        <cms:WidgetProperties ID="widgetProperties" runat="server" />
    </asp:Panel>
</asp:Content>
