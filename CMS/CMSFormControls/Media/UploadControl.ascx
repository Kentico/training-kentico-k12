<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="UploadControl.ascx.cs"
    Inherits="CMSFormControls_Media_UploadControl" %>
<asp:Label CssClass="ErrorLabel" runat="server" ID="lblError" Visible="false" EnableViewState="false" />
<asp:PlaceHolder runat="server" ID="plcUpload">
    <cms:Uploader ID="uploader" runat="server" />
    <asp:Button ID="hdnPostback" CssClass="HiddenButton" runat="server" EnableViewState="false" />
</asp:PlaceHolder>
