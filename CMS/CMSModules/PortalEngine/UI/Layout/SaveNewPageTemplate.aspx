<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_PortalEngine_UI_Layout_SaveNewPageTemplate"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master"
    Title="Page template - Save as new" CodeBehind="SaveNewPageTemplate.aspx.cs" %>

<%@ Register Src="~/CMSFormControls/System/LocalizableTextBox.ascx" TagName="LocalizableTextBox"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/System/CodeName.ascx" TagName="CodeName" TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel runat="server" ID="pnlContent">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblTemplateDisplayName" runat="server" AssociatedControlID="txtTemplateDisplayName"
                        ResourceString="PortalEngine.SaveNewPageTemplate.DisplayName" ShowRequiredMark="true" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:LocalizableTextBox ID="txtTemplateDisplayName" MaxLength="200" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblTemplateCodeName" runat="server" AssociatedControlID="txtTemplateCodeName"
                        ResourceString="PortalEngine.SaveNewPageTemplate.CodeName" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CodeName ID="txtTemplateCodeName" MaxLength="100" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblTemplateCategory" runat="server" AssociatedControlID="categorySelector"
                        ResourceString="PortalEngine.SaveNewPageTemplate.Category" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:SelectPageTemplateCategory ID="categorySelector" runat="server" ShowTemplates="false" ShowAdHocCategory="false"
                        ShowRoot="false" EnableCategorySelection="true" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblTemplateDescription" runat="server" AssociatedControlID="txtTemplateDescription"
                        ResourceString="PortalEngine.SaveNewPageTemplate.Description" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSTextArea ID="txtTemplateDescription" runat="server" Rows="4" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" ID="lblKeep" runat="server" ResourceString="PortalEngine.SaveNewPageTemplate.Keep"
                        AssociatedControlID="chkKeep" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:CMSCheckBox ID="chkKeep" MaxLength="200" runat="server" Checked="true" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Literal ID="ltlScript" runat="server" />
</asp:Content>
<asp:Content ID="cntFooter" ContentPlaceHolderID="plcFooter" runat="server">
    <cms:LocalizedButton ID="btnOk" runat="server" ButtonStyle="Primary" ResourceString="general.saveandclose" EnableViewState="False"
        OnClick="btnOK_Click" />
</asp:Content>