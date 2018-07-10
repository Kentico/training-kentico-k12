<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="FieldAppearance.ascx.cs"
    Inherits="CMSModules_AdminControls_Controls_Class_FieldEditor_FieldAppearance" %>

<%@ Register Src="~/CMSModules/FormControls/FormControls/FormControlSelector.ascx"
    TagPrefix="cms" TagName="FormControlSelector" %>
<cms:LocalizedHeading runat="server" Level="4" ResourceString="templatedesigner.section.fieldappearance"></cms:LocalizedHeading>
<asp:Panel ID="pnlAppearance" runat="server" Enabled="true" CssClass="FieldPanel">
    <div class="form-horizontal">
        <asp:PlaceHolder ID="plcVisibility" runat="server" Visible="false">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblVisibility" runat="server" EnableViewState="false" ResourceString="TemplateDesigner.Visibility" AssociatedControlID="ctrlVisibility" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:EditingFormControl ID="ctrlVisibility" runat="server" AllowMacroEditing="False" FormControlName="DropDownVisibilityControl" IsLiveSite="false" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblVisibilityControl" runat="server" EnableViewState="false"
                        ResourceString="TemplateDesigner.VisibilityControl" AssociatedControlID="drpVisibilityControl" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSDropDownList ID="drpVisibilityControl" runat="server" DataValueField="UserControlCodeName"
                        DataTextField="UserControlDisplayName" CssClass="DropDownField" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblChangeVisibility" runat="server" EnableViewState="false"
                        ResourceString="TemplateDesigner.ChangeVisibility" AssociatedControlID="chkChangeVisibility" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox ID="chkChangeVisibility" runat="server" CssClass="CheckBoxMovedLeft" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblFieldCaption" runat="server" EnableViewState="false" ResourceString="TemplateDesigner.FieldCaption" />
            </div>
            <div class="editing-form-value-cell">
                <cms:EditingFormControl ID="txtFieldCaption" runat="server" AllowMacroEditing="true" FormControlName="LocalizableTextBox" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblFieldDescription" runat="server" EnableViewState="false"
                    ResourceString="TemplateDesigner.FieldDescription" />
            </div>
            <div class="editing-form-value-cell">
                <cms:EditingFormControl ID="txtDescription" runat="server" AllowMacroEditing="true" FormControlName="LocalizableTextArea" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblExplanationText" runat="server" EnableViewState="false" ResourceString="TemplateDesigner.ExplanationText" />
            </div>
            <div class="editing-form-value-cell">
                <cms:EditingFormControl ID="txtExplanationText" runat="server" AllowMacroEditing="true" FormControlName="LocalizableTextBox" IsLiveSite="false" />
            </div>
        </div>
        <div class="form-group">
            <div class="editing-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblField" runat="server" EnableViewState="false" ResourceString="objecttype.cms_formusercontrol"
                    DisplayColon="true" />
            </div>
            <div class="editing-form-value-cell">
                <cms:FormControlSelector ID="drpControl" runat="server" AutoPostBack="True" ForcedValueSet="true" />
            </div>
        </div>
        <asp:PlaceHolder ID="plcPublicField" runat="server">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblPublicField" runat="server" EnableViewState="false" ResourceString="TemplateDesigner.PublicField" AssociatedControlID="chkPublicField" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox ID="chkPublicField" runat="server" CssClass="CheckBoxMovedLeft" />
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcInheritance" runat="server" Visible="true">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblControlInheritable" runat="server" DisplayColon="true"
                        EnableViewState="false" ResourceString="fieldeditor.allowinheritance" AssociatedControlID="chkControlInheritable" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox ID="chkControlInheritable" runat="server" CssClass="CheckBoxMovedLeft" />
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Panel>