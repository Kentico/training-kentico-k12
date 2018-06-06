<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="GenerateHash.aspx.cs"
    Inherits="CMSModules_REST_FormControls_GenerateHash" Theme="Default" EnableEventValidation="true"
    MaintainScrollPositionOnPostback="true" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master"
    Title="REST Service - Generate authetication hash" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <cms:LocalizedLabel runat="server" ID="lblInfo" ResourceString="rest.generateauthhashinfo"
        EnableViewState="false" /><br />
    <br />
    <cms:CMSTextArea runat="server" ID="txtUrls" EnableViewState="false" Width="100%" Rows="9" />
</asp:Content>
<asp:Content ID="cntFooter" ContentPlaceHolderID="plcFooter" runat="server">
    <div class="FloatRight">
        <cms:CMSButton ID="btnAuthenticate" runat="server" ButtonStyle="Primary" EnableViewState="false" />
    </div>
</asp:Content>
