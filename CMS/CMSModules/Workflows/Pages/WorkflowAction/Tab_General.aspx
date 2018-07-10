<%@ Page Language="C#" AutoEventWireup="True"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Workflow action properties – General"
    Inherits="CMSModules_Workflows_Pages_WorkflowAction_Tab_General" Theme="Default"  Codebehind="Tab_General.aspx.cs" %>            
<%@ Register Src="~/CMSModules/Workflows/Controls/UI/WorkflowAction/Edit.ascx"
    TagName="WorkflowActionEdit" TagPrefix="cms" %>
    
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:WorkflowActionEdit ID="editElem" runat="server" IsLiveSite="false" />
</asp:Content>