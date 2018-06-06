<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_Advanced_EditableContent_Main"
    Theme="Default"  Codebehind="Main.aspx.cs" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>

<%@ Register Src="~/CMSModules/Content/Controls/editmenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<asp:Content ID="cntMenu" ContentPlaceHolderID="plcBeforeContent" runat="server">

    <cms:editmenu ID="menuElem" runat="server" ShowApplyWorkflow="False" ShowReject="true" ShowSubmitToApproval="true"
        ShowProperties="false" ShowSave="false" ShowCreateAnother="false" IsLiveSite="false" Visible="false" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="plcContent" runat="server">
    <asp:Panel ID="pnlEditableContent" runat="server" Visible="false">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblEditControl" runat="server" EnableViewState="false"
                        ResourceString="EditableContent.EditControl" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSDropDownList ID="drpEditControl" runat="server" AutoPostBack="true" CssClass="DropDownField" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblName" runat="server" EnableViewState="false"
                        ResourceString="General.CodeName" DisplayColon="true" ShowRequiredMark="true" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextBox ID="txtName" runat="server" />
                </div>
            </div>
            <div class="form-group" runat="server" ID="advancedEditables">
                <cms:ExtendedTextArea Visible="false" ID="txtAreaContent" runat="server" Height="330px" />
                <cms:CMSHtmlEditor Visible="false" ID="htmlContent" runat="server" Height="350px" />
                <cms:CMSEditableImage Visible="false" ID="imageContent" runat="server" IsLiveSite="false" />
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblContent" runat="server" EnableViewState="false"
                        ResourceString="header.content" DisplayColon="true" Visible="false" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextBox ID="txtContent" runat="server" Visible="false" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        //<![CDATA[
        var mainUrl = '';

        // Selects specified node
        function SelectNode(nodeName, nodeType) {
            document.location.replace(mainUrl + "&nodename=" + nodeName + "&nodetype=" + nodeType);
        }

        // Selects specified after saving
        function SelectNodeAfterImageSave(nodeName, nodeType) {
            document.location.replace(mainUrl + "&nodename=" + nodeName + "&nodetype=" + nodeType + "&imagesaved=true");
        }

        // Refreshes after saving
        function RefreshNode(nodeName, nodeType, nodeId) {
            parent.frames['tree'].RefreshNode(nodeName, nodeType, nodeId);
        }

        // Opens menu for creating new item
        function CreateNew(nodetype) {
            document.location.replace(mainUrl + "&createNew=true&nodetype=" + nodetype);
        }
        //]]>
    </script>
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
</asp:Content>
