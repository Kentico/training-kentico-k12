<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Avatars_Dialogs_AvatarsGallery"
    MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master" Theme="Default"  Codebehind="AvatarsGallery.aspx.cs" %>

<%@ Register Src="~/CMSModules/Avatars/Controls/AvatarsGallery.ascx" TagName="Gallery"
    TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:Gallery ID="avatarsGallery" runat="server" Visible="true" DisplayButtons="true" />
</asp:Content>