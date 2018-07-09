<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="StoreSettings_OnlineMarketing.aspx.cs"
    Inherits="CMSModules_Ecommerce_Pages_Tools_Configuration_StoreSettings_StoreSettings_OnlineMarketing"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register TagPrefix="cms" TagName="SettingsGroupViewer" Src="~/CMSModules/Settings/Controls/SettingsGroupViewer.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plcContent" runat="Server">
    <cms:SettingsGroupViewer ID="SettingsGroupViewer" runat="server" AllowGlobalInfoMessage="false" />
</asp:Content>
