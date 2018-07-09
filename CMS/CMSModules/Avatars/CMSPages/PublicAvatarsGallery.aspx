<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Avatars_CMSPages_PublicAvatarsGallery"
    MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalDialogPage.master" Theme="Default"  Codebehind="PublicAvatarsGallery.aspx.cs" %>
<%@ Register Src="~/CMSModules/Avatars/Controls/AvatarsGallery.ascx" TagName="Gallery"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
   <cms:Gallery ID="avatarsGallery" runat="server" Visible="true" DisplayButtons="false" /> 
</asp:Content>
<asp:Content ID="cntFooter" runat="server" ContentPlaceHolderID="plcFooter">
    <div class="FloatRight">
        <cms:LocalizedButton runat="Server" ButtonStyle="Primary" ID="btnOk" OnClientClick = "addToHidden(); return false;" ResourceString="general.ok"
            EnableViewState="false" /><cms:LocalizedButton runat="server" ButtonStyle="Primary"
                ID="btnCancel" OnClientClick = "return CloseDialog();" ResourceString="general.cancel" EnableViewState="false" />
    </div>
</asp:Content>
