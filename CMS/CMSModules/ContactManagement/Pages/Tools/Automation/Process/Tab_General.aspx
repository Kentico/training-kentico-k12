<%@ Page Language="C#" AutoEventWireup="True"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Automation process – General"
    Inherits="CMSModules_ContactManagement_Pages_Tools_Automation_Process_Tab_General" Theme="Default"  Codebehind="Tab_General.aspx.cs" %>            
<%@ Register Src="~/CMSModules/Workflows/Controls/UI/Workflow/Edit.ascx"
    TagName="WorkflowEdit" TagPrefix="cms" %>
    
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:WorkflowEdit ID="editElem" runat="server" IsLiveSite="false" />
</asp:Content>