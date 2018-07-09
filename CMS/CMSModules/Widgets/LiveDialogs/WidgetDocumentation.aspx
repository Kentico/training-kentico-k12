<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="WidgetDocumentation.aspx.cs"
    Inherits="CMSModules_Widgets_LiveDialogs_WidgetDocumentation" MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalDialogPage.master"
    Theme="Default" %>

<%@ Register Src="~/CMSModules/PortalEngine/Controls/WebParts/WebPartDocumentation.ascx"
    TagName="WebPartDocumentation" TagPrefix="cms" %>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="Server">
    <cms:WebPartDocumentation runat="server" ID="ucWebPartDocumentation" />
</asp:Content>
<asp:Content ID="cntFooter" ContentPlaceHolderID="plcFooter" runat="server">
    <div class="FloatRight" id="divFooter" runat="server">
        <cms:LocalizedButton ID="btnClose" runat="server" ButtonStyle="Primary" EnableViewState="False"
            OnClientClick="return CloseDialog();" ResourceString="WebPartDocumentDialog.Close" />
    </div>
</asp:Content>
