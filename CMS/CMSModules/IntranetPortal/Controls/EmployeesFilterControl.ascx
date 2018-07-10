<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_IntranetPortal_Controls_EmployeesFilterControl"  Codebehind="~/CMSModules/IntranetPortal/Controls/EmployeesFilterControl.ascx.cs" %>
<asp:Panel CssClass="Filter" DefaultButton="btnSelect" runat="server" ID="pnlUsersFilter">
    <div class="form-horizontal form-filter form-filter-employees">
        <div class="form-group">
            <div class="filter-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblValue" runat="server" EnableViewState="false" AssociatedControlID="txtValue"
                    ResourceString="employeesearch.searchexpression" />
            </div>
            <div class="filter-form-value-cell">
                <cms:CMSTextBox runat="server" ID="txtValue" EnableViewState="false"/>
                <cms:CMSButton runat="server" ID="btnSelect" OnClick="btnSelect_Click" EnableViewState="false" ButtonStyle="Default" />
            </div>
        </div>
        <div class="form-group">
            <div class="filter-form-label-cell">
                <cms:LocalizedLabel CssClass="control-label" ID="lblDepartment" runat="server" EnableViewState="false" AssociatedControlID="drpDepartment"
                    ResourceString="employeesearch.department" />
            </div>
            <div class="filter-form-value-cell">
                <cms:CMSDropDownList ID="drpDepartment" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="drpDepartment_SelectedIndexChanged" />
            </div>
        </div>
    </div>
</asp:Panel>
