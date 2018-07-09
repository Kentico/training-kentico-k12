<%@ Control Language="C#" AutoEventWireup="True" Inherits="CMSModules_Workflows_Controls_UI_WorkflowAction_Edit"
     Codebehind="Edit.ascx.cs" %>
<%@ Register Src="~/CMSFormControls/Classes/AssemblyClassSelector.ascx" TagName="AssemblyClassSelector"
    TagPrefix="cms" %>

<cms:UIForm runat="server" ID="formElem" ObjectType="cms.workflowaction" RedirectUrlAfterCreate="" DefaultFieldLayout="TwoColumns" RefreshHeader="True">
    <SecurityCheck Resource="CMS.Workflow" Permission="modify" />
    <LayoutTemplate>
        <cms:FormCategory runat="server" ID="pnlGeneral" CategoryTitleResourceString="general.general">
            <cms:FormField runat="server" ID="aDisplayName" Field="ActionDisplayName" FormControl="LocalizableTextBox" ResourceString="general.displayname" DisplayColon="true" />
            <cms:FormField runat="server" ID="aName" Field="ActionName" FormControl="CodeName" ResourceString="general.codename" DisplayColon="true" />
            <cms:FormField runat="server" ID="aDescription" Field="ActionDescription" FormControl="TextAreaControl" ResourceString="general.description" DisplayColon="true" />
        </cms:FormCategory>
        <cms:FormCategory runat="server" ID="pnlImages" CategoryTitleResourceString="workflowaction.images">
            <cms:FormField runat="server" ID="aIconGUID" Field="ActionIconGUID" Layout="Inline">
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:FormLabel CssClass="control-label" ID="lblIcon" runat="server" EnableViewState="true" ResourceString="general.icon" DisplayColon="true" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:FormControl ID="ucIcon" runat="server" FormControlName="metafileorfonticonselector" />
                    </div>
                </div>
            </cms:FormField>
            <cms:FormField runat="server" ID="aThumbnailGUID" Field="ActionThumbnailGUID" Layout="Inline">
                <div class="form-group">
                    <div class="editing-form-label-cell">
                        <cms:FormLabel CssClass="control-label" ID="lblMetaFile" runat="server" EnableViewState="true" ResourceString="general.thumbnail" DisplayColon="true" />
                    </div>
                    <div class="editing-form-value-cell">
                        <cms:FormControl ID="ucThumbnail" runat="server" FormControlName="metafileorfonticonselector" />
                    </div>
                </div>
            </cms:FormField>
        </cms:FormCategory>
        <cms:FormCategory runat="server" ID="pnlAction" CategoryTitleResourceString="workflowaction.actionconfig">
            <cms:FormField runat="server" ID="aAssemblyName" Field="ActionAssemblyName" ResourceString="general.assemblyname"
                DisplayColon="true" UseFFI="false" ShowRequiredMark="True">
                <cms:AssemblyClassSelector ID="assemblyElem" runat="server" ClassNameColumnName="ActionClass" ShowClasses="true" />
            </cms:FormField>
            <cms:FormField runat="server" ID="aModule" Field="ActionResourceID" UseFFI="true" FormControl="ModuleSelector" />
            <cms:FormField runat="server" ID="aEnabled" Field="ActionEnabled" FormControl="CheckBoxControl" ResourceString="general.Enabled" DisplayColon="true" />
        </cms:FormCategory>
        <cms:FormSubmit runat="server" ID="aSubmit" />
    </LayoutTemplate>
</cms:UIForm>