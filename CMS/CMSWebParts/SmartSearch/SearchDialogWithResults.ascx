<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSWebParts_SmartSearch_SearchDialogWithResults"  Codebehind="~/CMSWebParts/SmartSearch/SearchDialogWithResults.ascx.cs" %>
<%@ Register Src="~/CMSModules/SmartSearch/Controls/SearchResults.ascx" TagName="SearchResults"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/SmartSearch/Controls/SearchDialog.ascx" TagName="SearchDialog"
    TagPrefix="cms" %>
<div class="SearchDialog">
    <cms:SearchDialog ID="srchDialog" runat="server" />
</div>
<div class="SearchResults">
    <cms:SearchResults ID="srchResults" runat="server" />
</div>
