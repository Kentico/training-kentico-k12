<%@ Page Language="C#" AutoEventWireup="True"
    MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" Title="Automation process – Steps"
    Inherits="CMSModules_ContactManagement_Pages_Tools_Automation_Process_Tab_Steps" Theme="Default"  Codebehind="Tab_Steps.aspx.cs" %>      

<%@ Register Src="~/CMSModules/Workflows/Controls/WorkflowDesigner.ascx" TagName="WorkflowDesigner"
    TagPrefix="cms" %>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="server">
    <cms:WorkflowDesigner ID="designerElem" ReadOnly="false" runat="server" />
</asp:Content>
