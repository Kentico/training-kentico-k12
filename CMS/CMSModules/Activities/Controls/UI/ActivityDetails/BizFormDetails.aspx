<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master"
    Inherits="CMSModules_Activities_Controls_UI_ActivityDetails_BizFormDetails" Theme="Default"
     Codebehind="BizFormDetails.aspx.cs" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:BizForm ID="bizRecord" runat="server" IsLiveSite="false" DefaultFormLayout="Divs" />
</asp:Content>