<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="DocTypeSelection.ascx.cs"
    Inherits="CMSModules_Content_Controls_DocTypeSelection" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<div class="ContentNewClasses">
    <cms:LocalizedHeading runat="server" ID="headDocumentTypeSelection" EnableViewState="false" />
    <asp:Label ID="lblError" runat="server" CssClass="ContentLabel" ForeColor="Red" EnableViewState="false" />
</div>
<div class="ContentNewClasses UniGridClearPager content-block-50">
    <cms:UniGrid runat="server" ID="gridClasses" IsLiveSite="false" ZeroRowsText="" ShowActionsMenu="false">
        <GridColumns>
            <ug:Column Source="##ALL##" ExternalSourceName="classname" Caption="$general.codename$"
                Wrap="false" />
            <ug:Column Source="ClassDisplayName" Caption="$documenttype.name$" Wrap="false">
                <Filter Type="text" />
            </ug:Column>
        </GridColumns>
        <GridOptions DisplayFilter="true" />
    </cms:UniGrid>
</div>
<cms:CMSPanel ID="pnlSeparator" runat="server" CssClass="PageSeparator" />
<cms:UIPlaceHolder runat="server" ID="plcNewLinkNew" ElementName="New" ModuleName="CMS.Content">
    <cms:UIPlaceHolder runat="server" ID="plcNewLink" ElementName="New.LinkExistingDocument"
        ModuleName="CMS.Content">
        <asp:Panel runat="server" ID="pnlFooter" CssClass="ActivityPanel">
            <asp:HyperLink runat="server" ID="lnkNewLink" CssClass="content-new-link cms-icon-link" EnableViewState="false">
                <cms:CMSIcon runat="server" ID="iconNewLink" EnableViewState="false" CssClass="icon-chain" />
                <asp:Label ID="lblNewLink" runat="server" EnableViewState="false" />
            </asp:HyperLink>
        </asp:Panel>
    </cms:UIPlaceHolder>
</cms:UIPlaceHolder>
<cms:UIPlaceHolder runat="server" ID="plcNewABTestVariant" ElementName="New" ModuleName="CMS.Content">
    <cms:UIPlaceHolder runat="server" ID="plcNewVariantLink" ElementName="New.ABTestVariant"
        ModuleName="CMS.Content">
        <asp:Panel runat="server" ID="pnlABVariant" EnableViewState="false">
            <asp:HyperLink runat="server" ID="lnkNewVariant" CssClass="content-new-link cms-icon-link" EnableViewState="false">
                <cms:CMSIcon runat="server" ID="iconNewVariant" EnableViewState="false" CssClass="icon-two-squares-line" />
                <asp:Label ID="lblNewVariant" runat="server" EnableViewState="false" />
            </asp:HyperLink>
        </asp:Panel>
    </cms:UIPlaceHolder>
</cms:UIPlaceHolder>
