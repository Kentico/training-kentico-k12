<%@ Page Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Theme="Default"
    AutoEventWireup="true"  Codebehind="List.aspx.cs" Inherits="CMSModules_Workflows_Pages_WorkflowStep_SourcePoint_List"
    Title="Workflows - Source Point List" %>

<%@ Register Src="~/CMSModules/Workflows/Controls/UI/WorkflowStep/SourcePoint/List.ascx"
    TagName="SourcePointList" TagPrefix="cms" %>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="server">
    <cms:SourcePointList ID="listElem" runat="server" />
</asp:Content>
