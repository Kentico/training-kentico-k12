<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Messaging_CMSPages_PublicMessageUserSelector" Title="Untitled Page"
    ValidateRequest="false" MasterPageFile="~/CMSMasterPages/LiveSite/Dialogs/ModalDialogPage.master"
    Theme="default"  Codebehind="PublicMessageUserSelector.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/SearchUser.ascx" TagName="SearchUser"
    TagPrefix="cms" %>
<asp:Content ID="cntContent" runat="server" ContentPlaceHolderID="plcContent">

    <script type="text/javascript">
        //<![CDATA[
        function CloseAndRefresh(userId, mText, mId, mId2) {
            wopener.FillUserName(userId, mText, mId, mId2);
            CloseDialog();
        }

        //]]>
    </script>

    <div class="LiveSiteDialog">
        <cms:SearchUser ID="searchElem" runat="server" IsLiveSite="true" />
    </div>
</asp:Content>
