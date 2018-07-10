<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Community_Members_GroupMembersViewer"  Codebehind="~/CMSWebParts/Community/Members/GroupMembersViewer.ascx.cs" %>
<%@ Register Src="~/CMSWebparts/Membership/Users/UsersFilter_files/UsersFilterControl.ascx"
    TagName="MembersFilterControl" TagPrefix="cms" %>
<%@ Register TagPrefix="cms" Namespace="CMS.Community.Web.UI" Assembly="CMS.Community.Web.UI" %>
<cms:MembersFilterControl ID="filterMembers" runat="server" />
<cms:BasicRepeater ID="repMembers" runat="server" />
<cms:MembersDataSource ID="srcMembers" runat="server" />
<div class="Pager">
    <cms:UniPager ID="pagerElem" runat="server" />
</div>
