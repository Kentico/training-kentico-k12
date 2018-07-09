<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSAdminControls_UI_Dialogs_ChangeGroup"
    Title="Untitled Page" ValidateRequest="false" Theme="default" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master"  Codebehind="ChangeGroup.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/Selectors/SelectDocumentGroup.ascx" TagName="SelectDocumentGroup"
    TagPrefix="cms" %>

<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="Server">
    <cms:SelectDocumentGroup ID="selectDocumentGroupElem" runat="server" />
</asp:Content>