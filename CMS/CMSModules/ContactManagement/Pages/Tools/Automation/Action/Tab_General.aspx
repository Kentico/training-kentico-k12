<%@ Page Language="C#" AutoEventWireup="True"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Marketing Automation Action Properties – General"
    Inherits="CMSModules_ContactManagement_Pages_Tools_Automation_Action_Tab_General" Theme="Default"  Codebehind="Tab_General.aspx.cs" %>

<%@ Register Src="~/CMSModules/Workflows/Controls/UI/WorkflowAction/Edit.ascx"
    TagName="WorkflowActionEdit" TagPrefix="cms" %>
    
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:WorkflowActionEdit ID="editElem" runat="server" IsLiveSite="false" HideAllowedObjects="true" />
</asp:Content>