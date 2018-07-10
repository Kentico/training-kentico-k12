<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="MappingEditorItem.ascx.cs" Inherits="CMSModules_Membership_Controls_Facebook_MappingEditorItem" %>

<div class="form-group">
    <div class="editing-form-label-cell">
        <cms:LocalizedLabel DisplayColon="True" CssClass="control-label" ID="FieldLabel" runat="server" EnableViewState="false" />
    </div>
    <div class="editing-form-value-cell">
        <p id="MessageControl" runat="server" enableviewstate="false"></p>
        <cms:CMSDropDownList ID="AttributeDropDownList" runat="server" EnableViewState="false" CssClass="LongDropDownList" />
    </div>
</div>