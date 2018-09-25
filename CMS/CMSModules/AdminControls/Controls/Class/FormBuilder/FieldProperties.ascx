<%@ Control Language="C#" AutoEventWireup="false" Inherits="CMSModules_AdminControls_Controls_Class_FormBuilder_FieldProperties"
     Codebehind="FieldProperties.ascx.cs" %>

<%@ Register Src="~/CMSFormControls/System/LocalizableTextBox.ascx" TagName="LocalizableTextBox"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/System/OptionsDesigner.ascx" TagName="OptionsDesigner"
    TagPrefix="cms" %>
<div class="PanelProperties">
    <div class="label-property Top">
        <cms:LocalizedLabel ID="lblLabel" runat="server" CssClass="control-label" ResourceString="general.label" DisplayColon="true" EnableViewState="false" />
    </div>
    <div class="field-property">
        <cms:MessagesPlaceHolder ID="mphLabel" runat="server" InfoTimeout="0" UseRelativePlaceHolder="false" Opacity="100" />
        <cms:LocalizableTextBox ID="txtLabel" runat="server" />
    </div>
    <asp:PlaceHolder ID="plcRequired" runat="server">
        <div class="label-property">
            <cms:CMSCheckBox ID="chkRequired" ResourceString="formbuilder.required" runat="server" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcDefaultValue" runat="server">
        <div class="label-property inline-block">
            <cms:LocalizedLabel ID="lblDefaultValue" runat="server" CssClass="control-label" EnableViewState="false" ResourceString="formbuilder.defaultvalue" DisplayColon="true" />
        </div>
        <asp:Panel ID="pnlDefValue" runat="server">
            <cms:MessagesPlaceHolder ID="mphDefaultValue" runat="server" InfoTimeout="0" UseRelativePlaceHolder="false" Opacity="100" />
            <cms:FormControl ID="defaultValue" runat="server" FormControlName="TextBoxControl" />
        </asp:Panel>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcOptions" runat="server">
        <div class="label-property">
            <cms:LocalizedLabel ID="lblOptions" runat="server" CssClass="control-label" EnableViewState="false" ResourceString="FormBuilder.options" DisplayColon="true" />
        </div>
        <div class="field-property">
            <cms:MessagesPlaceHolder ID="mphOptions" runat="server" InfoTimeout="0" UseRelativePlaceHolder="false" Opacity="100" />
            <cms:OptionsDesigner ID="optionsDesigner" runat="server" />
        </div>
    </asp:PlaceHolder>
    <div class="label-property">
        <cms:LocalizedLabel ID="lblExplanationText" CssClass="control-label" ResourceString="formbuilder.explanation" DisplayColon="true" runat="server" EnableViewState="false" />
    </div>
    <div class="field-property">
        <cms:MessagesPlaceHolder ID="mphExplanationText" runat="server" InfoTimeout="0" UseRelativePlaceHolder="false" Opacity="100" />
        <cms:LocalizableTextBox ID="txtExplanationText" runat="server" TextMode="MultiLine" />
    </div>
    <div class="label-property">
        <cms:LocalizedLabel ID="lblTooltip" CssClass="control-label" ResourceString="formbuilder.tooltip" DisplayColon="true" runat="server" EnableViewState="false" />
    </div>
    <div class="field-property">
        <cms:MessagesPlaceHolder ID="mphTooltip" InfoTimeout="0" UseRelativePlaceHolder="false" Opacity="100" runat="server" />
        <cms:LocalizableTextBox ID="txtTooltip" runat="server" TextMode="MultiLine" />
    </div>
</div>
