<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_Avatars_AvatarFilter"  Codebehind="AvatarFilter.ascx.cs" %>
<asp:Panel runat="server" ID="pnlFilter" DefaultButton="btnSearch">
  <div class="form-horizontal form-filter">
        <div class="form-group">
            <div class="filter-form-label-cell">
                <cms:LocalizedLabel ID="lblAvatarName" DisplayColon="true" runat="server" ResourceString="avat.avatarname" CssClass="control-label" EnableViewState="false" />
            </div>
            <div class="filter-form-condition-cell">
                <cms:CMSDropDownList ID="drpAvatarName" runat="server" />
            </div>
            <div class="filter-form-value-cell">
                <cms:CMSTextBox ID="txtAvatarName" runat="server"  MaxLength="200" />
            </div>
        </div>
        <div class="form-group">
            <div class="filter-form-label-cell">
                <cms:LocalizedLabel ID="lblAvatarType" DisplayColon="true" ResourceString="avat.avatartype" runat="server" CssClass="control-label" EnableViewState="false" />
             </div>
            <div class="filter-form-value-cell-wide">
                <cms:CMSDropDownList ID="drpAvatarType" runat="server" />
            </div>
        </div>
        <div class="form-group">
             <div class="filter-form-label-cell">
                <cms:LocalizedLabel ID="lblAvatarKind" ResourceString="avat.avatarkind" runat="server" DisplayColon="true" CssClass="control-label" EnableViewState="false" />
            </div>
            <div class="filter-form-value-cell-wide">
                <cms:CMSDropDownList ID="drpAvatarKind" runat="server" />
           </div>
        </div>
       <div class="form-group form-group-buttons">
            <div class="filter-form-buttons-cell-wide">
                <cms:LocalizedButton ID="btnReset" ButtonStyle="Default" runat="server" EnableViewState="false" />   
                <cms:LocalizedButton ID="btnSearch" ResourceString="general.filter" runat="server" ButtonStyle="Primary" EnableViewState="false" OnClick="btnSearch_Click" />
                
           </div>
       </div>
    </div>
</asp:Panel>
