<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Workflow action properties" Inherits="CMSModules_Workflows_Pages_WorkflowAction_New" Theme="Default"  Codebehind="New.aspx.cs" %>
<%@ Register Src="~/CMSModules/Workflows/Controls/UI/WorkflowAction/Edit.ascx"
    TagName="WorkflowActionEdit" TagPrefix="cms" %>
    
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:WorkflowActionEdit ID="editElem" runat="server" IsLiveSite="false" />
</asp:Content>