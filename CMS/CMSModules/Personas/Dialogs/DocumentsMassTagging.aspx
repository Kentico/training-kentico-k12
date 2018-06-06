<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="DocumentsMassTagging.aspx.cs"
    Inherits="CMSModules_Personas_Dialogs_DocumentsMassTagging" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master" Theme="Default" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/Controls/SelectionDialog.ascx" TagPrefix="cms"
    TagName="SelectionDialog" %>


<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:SelectionDialog runat="server" ID="selPersonas"></cms:SelectionDialog>
</asp:Content>
<asp:Content ID="cntFooter" ContentPlaceHolderID="plcFooter" runat="server">
    <cms:LocalizedButton runat="server" ID="btnPerformAction" ButtonStyle="Primary"/>
</asp:Content>
