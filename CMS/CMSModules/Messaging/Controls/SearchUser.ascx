<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Messaging_Controls_SearchUser"  Codebehind="SearchUser.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<cms:MessagesPlaceHolder ID="plcMess" runat="server" />
<div class="ListPanel">
    <asp:Panel ID="pnlFilter" runat="server" EnableViewState="false">
        <div class="form-horizontal form-filter">
            <div class="form-group">
                <div class="filter-form-label-cell">
                    <cms:LocalizedLabel runat="server" ResourceString="selectusers.entersearch" AssociatedControlID="txtSearch" CssClass="control-label"></cms:LocalizedLabel>
                </div>
                <div class="filter-form-value-cell">
                    <cms:CMSTextBox ID="txtSearch" runat="server" EnableViewState="false" />
                </div>
            </div>
            <div class="form-group form-group-buttons">
                <div class="filter-form-buttons-cell">
                    <cms:LocalizedButton ID="btnSearch" runat="server" ResourceString="general.search" ButtonStyle="Primary" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <cms:UniGrid ID="gridUsers" runat="server" GridName="~/CMSModules/Messaging/Controls/SearchUser.xml"
        OrderBy="UserName" />
</div>
