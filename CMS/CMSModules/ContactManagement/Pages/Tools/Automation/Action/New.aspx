<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Marketing Automatin Action - New" Inherits="CMSModules_ContactManagement_Pages_Tools_Automation_Action_New"
    Theme="Default"  Codebehind="New.aspx.cs" %>

<%@ Register Src="~/CMSModules/Workflows/Controls/UI/WorkflowAction/Edit.ascx" TagName="WorkflowActionEdit"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:WorkflowActionEdit ID="editElem" runat="server" IsLiveSite="false" HideAllowedObjects="true" />
</asp:Content>
