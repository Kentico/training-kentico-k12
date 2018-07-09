<%@ Page Language="C#" AutoEventWireup="true"  Codebehind="WebPartProperties_layout_menu.aspx.cs"
    Inherits="CMSModules_PortalEngine_UI_WebParts_WebPartProperties_layout_menu"
    MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalSimplePage.master" Theme="Default" %>

<%@ Register Src="~/CMSModules/PortalEngine/FormControls/WebPartLayouts/WebPartLayoutSelector.ascx"
    TagPrefix="cms" TagName="LayoutSelector" %>
<asp:Content ID="cntContent" runat="server" ContentPlaceHolderID="plcSiteSelector">
    <div id="pnlContent" runat="server">
        <div class="form-horizontal form-filter">
            <div class="form-group">
                <div class="filter-form-label-cell">
                    <asp:Label CssClass="control-label" ID="lblLayouts" runat="server" />
                </div>
                <div class="filter-form-value-cell-wide">
                    <cms:LayoutSelector runat="server" ID="selectLayout" OnChanged="drpLayouts_Changed"
                        IsLiveSite="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
