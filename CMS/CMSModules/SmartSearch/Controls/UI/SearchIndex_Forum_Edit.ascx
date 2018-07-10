<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_SmartSearch_Controls_UI_SearchIndex_Forum_Edit"  Codebehind="SearchIndex_Forum_Edit.ascx.cs" %>

<%@ Register Src="~/CMSFormControls/Sites/SiteSelector.ascx" TagName="SelectSite" TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>

<cms:CMSUpdatePanel runat="server" ID="pnlForumEdit" UpdateMode="Always">
    <ContentTemplate>
        <cms:DisabledModule runat="server" ID="ucDisabledModule" TestSettingKeys="CMSSearchIndexingEnabled" InfoText="{$srch.searchdisabledinfo$}" />
        <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
        <div class="form-horizontal">
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblSite" EnableViewState="false" ResourceString="srch.index.site"
                        DisplayColon="true" AssociatedControlID="selSite" />
                </div>
                <div class="editing-form-value-cell">
                    <cms:SelectSite IsLiveSite="false" ID="selSite" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-label-cell">
                    <cms:LocalizedLabel CssClass="control-label" runat="server" ID="lblForum" EnableViewState="false" ResourceString="srch.index.forum"
                        DisplayColon="true" AssociatedControlID="plcForumSelector" />
                </div>
                <div class="editing-form-value-cell">
                    <asp:PlaceHolder runat="server" ID="plcForumSelector" />
                    <asp:PlaceHolder runat="server" ID="plcForumsInfo">
                        <div class="explanation-text">
                            <cms:LocalizedLabel runat="server" ID="lblClassNamesInfo" ResourceString="srch.index.forumsinfo"
                                CssClass="explanation-text" />
                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
            <div class="form-group">
                <div class="editing-form-value-cell editing-form-value-cell-offset">
                    <cms:FormSubmitButton runat="server" ID="btnOk" OnClick="btnOK_Click" EnableViewState="false" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</cms:CMSUpdatePanel>