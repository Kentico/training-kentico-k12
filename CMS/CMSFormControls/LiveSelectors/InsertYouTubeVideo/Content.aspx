<%@ Page Title="" Language="C#" Theme="Default" MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalSimplePage.master"
    AutoEventWireup="true" EnableEventValidation="false" Inherits="CMSFormControls_LiveSelectors_InsertYouTubeVideo_Content"
     Codebehind="Content.aspx.cs" %>

<%@ Register Src="~/CMSModules/Content/Controls/Dialogs/YouTube/YouTubeProperties.ascx"
    TagName="YouTubeProperties" TagPrefix="cms" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plcContent" runat="Server">
    <div class="PageContent">
        <cms:YouTubeProperties ID="youTubeProp" runat="server" />
    </div>
</asp:Content>
