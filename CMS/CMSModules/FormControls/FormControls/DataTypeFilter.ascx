<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="DataTypeFilter.ascx.cs"
    Inherits="CMSModules_FormControls_FormControls_DataTypeFilter" %>
<%@ Register Src="~/CMSFormControls/System/UserControlTypeSelector.ascx" TagPrefix="cms"
    TagName="TypeSelector" %>
<asp:Panel CssClass="form-horizontal form-filter" runat="server" ID="pnlSearch">
    <div class="form-group">
        <div class="filter-form-label-cell">
            <cms:LocalizedLabel AssociatedControlID="drpTypeSelector" ID="lblTypeSelector" runat="server"
                EnableViewState="false" ResourceString="fieldeditor.formfieldtype" DisplayColon="true" CssClass="control-label" />
        </div>
        <div class="filter-form-value-cell">
            <cms:TypeSelector ID="drpTypeSelector" runat="server" CssClass="DropDownField" IncludeAllItem="true"
                AutoPostBack="true" />
        </div>
    </div>
</asp:Panel>
