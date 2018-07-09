<%@ Page Language="C#" Theme="Default" AutoEventWireup="true"
    Inherits="CMSFormControls_Selectors_InsertYouTubeVideo_Footer" EnableEventValidation="false"
    MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" Title="Insert youtube video - footer"  Codebehind="Footer.aspx.cs" %>

<%@ Register Src="~/CMSModules/Content/Controls/Dialogs/General/DialogFooter.ascx"
    TagName="Footer" TagPrefix="cms" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="PageFooterLine">
        <cms:Footer ID="footerElem" runat="server" IsYouTubeDialog="true" IsLiveSite="false" />
    </div>
</asp:Content>
