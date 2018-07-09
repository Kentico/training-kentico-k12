<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="New automation process" Inherits="CMSModules_ContactManagement_Pages_Tools_Automation_Process_New"
    Theme="Default"  Codebehind="New.aspx.cs" %>

<%@ Register Src="~/CMSModules/Workflows/Controls/UI/Workflow/Edit.ascx" TagName="WorkflowEdit"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:WorkflowEdit ID="editElem" runat="server" IsLiveSite="false" />
</asp:Content>
