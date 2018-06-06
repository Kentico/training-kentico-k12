<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Messaging_CMSPages_MessageUserSelector_Search" Theme="Default"
    EnableEventValidation="false" MasterPageFile="~/CMSMasterPages/LiveSite/EmptyPage.master"  Codebehind="MessageUserSelector_Search.aspx.cs" %>

<%@ Register Src="~/CMSModules/Messaging/Controls/SearchUser.ascx" TagName="SearchUser"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="LiveSiteDialog">
        <div class="PageContent">
            <cms:SearchUser ID="searchElem" runat="server" IsLiveSite="true" />
        </div>
    </div>
</asp:Content>
