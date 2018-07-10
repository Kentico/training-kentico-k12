<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Search_cmscompletesearchdialog"  Codebehind="~/CMSWebParts/Search/cmscompletesearchdialog.ascx.cs" %>

<div class="SearchDialog">
    <cms:CMSSearchDialog ID="srchDialog" runat="server" />
</div>
<div class="SearchResults">
    <cms:CMSSearchResults ID="srchResults" runat="server" />
</div>