<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSAdminControls_UI_Selectors_SelectDocumentGroup"  Codebehind="SelectDocumentGroup.ascx.cs" %>

<div class="form-horizontal">
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:LocalizedLabel CssClass="control-label" ID="lblGroups" runat="server" ResourceString="community.group.choosedocumentowner"
                DisplayColon="true" />
        </div>
        <div class="editing-form-value-cell">
            <asp:PlaceHolder runat="server" ID="plcGroupSelector" />
        </div>
    </div>
    <div class="form-group">
        <div class="editing-form-label-cell">
            <cms:LocalizedLabel CssClass="control-label" ID="lblInherit" runat="server" AssociatedControlID="chkInherit" ResourceString="community.group.inheritdocumentowner" />
        </div>
        <div class="editing-form-value-cell">
            <cms:CMSCheckBox ID="chkInherit" runat="server" Checked="false" />
        </div>
    </div>
</div>
<asp:Literal runat="server" ID="ltlScript" />