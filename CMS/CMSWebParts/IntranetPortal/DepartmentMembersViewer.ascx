<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_IntranetPortal_DepartmentMembersViewer"  Codebehind="~/CMSWebParts/IntranetPortal/DepartmentMembersViewer.ascx.cs" %>
<%@ Register Src="~/CMSWebparts/Membership/Users/UsersFilter_files/UsersFilterControl.ascx"
    TagName="UsersFilterControl" TagPrefix="uc1" %>
<uc1:UsersFilterControl ID="filterUsers" runat="server" />
<cms:BasicRepeater ID="repUsers" runat="server" />
<cms:UsersDataSource ID="srcUsers" runat="server" />
<div class="Pager">
    <cms:UniPager ID="pagerElem" runat="server" />
</div>