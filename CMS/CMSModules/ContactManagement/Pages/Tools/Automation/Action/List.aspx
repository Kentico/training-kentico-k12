<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Marketing Automation Action - List"
    Inherits="CMSModules_ContactManagement_Pages_Tools_Automation_Action_List" Theme="Default"  Codebehind="List.aspx.cs" %>

<%@ Register Src="~/CMSModules/Workflows/Controls/UI/WorkflowAction/List.ascx" TagName="WorkflowActionList" TagPrefix="cms" %>
    
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:WorkflowActionList ID="listElem" runat="server" IsLiveSite="false" />
</asp:Content>
