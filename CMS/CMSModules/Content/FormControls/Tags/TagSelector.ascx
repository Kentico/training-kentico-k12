<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_FormControls_Tags_TagSelector"
     Codebehind="TagSelector.ascx.cs" %>
<asp:Panel ID="pnlTagSelector" runat="server" DefaultButton="btnHidden" CssClass="tag-selector">
    <div class="control-group-inline">
        <cms:CMSTextBox ID="txtTags" runat="server" EnableViewState="true" CssClass="form-control" />
        <cms:CMSButton ID="btnSelect" runat="server" EnableViewState="false" ButtonStyle="Default" />
    </div>

    <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoComplete" TargetControlID="txtTags"
        ServiceMethod="TagsAutoComplete" ServicePath="~/CMSModules/Content/FormControls/Tags/TagSelectorService.asmx" MinimumPrefixLength="1"
        CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20" DelimiterCharacters=", "
        UseContextKey="true" CompletionListCssClass="autocomplete_completionListElement"
        CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
        OnClientItemSelected="itemSelected">
    </ajaxToolkit:AutoCompleteExtender>
    <cms:LocalizedLabel ID="lblInfoText" runat="server" ResourceString="tags.tagsselector.examples" CssClass="explanation-text"
        EnableViewState="false" />
    <asp:Button ID="btnHidden" runat="server" EnableViewState="false" CssClass="HiddenButton"
        OnClientClick="return false;" />
    <asp:HiddenField runat="server" ID="hdnDialogIdentifier" />
</asp:Panel>
