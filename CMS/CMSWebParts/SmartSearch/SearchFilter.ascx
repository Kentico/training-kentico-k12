<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_SmartSearch_SearchFilter"  Codebehind="~/CMSWebParts/SmartSearch/SearchFilter.ascx.cs" %>
<cms:LocalizedLabel runat="server" ID="lblError" Visible="false" CssClass="ErrorLabel"></cms:LocalizedLabel>

<cms:LocalizedLabel ID="lblFilter" runat="server" Display="false" EnableViewState="false" ResourceString="srch.filter" />
<cms:CMSDropDownList runat="server" ID="drpFilter" Visible="false"  CssClass="DropDownField" />
<cms:CMSRadioButtonList runat="server" ID="radlstFilter" Visible="false" CssClass="ContentRadioButtonList" />
<cms:CMSCheckBoxList runat="server" ID="chklstFilter" Visible="false" CssClass="ContentCheckBoxList" />
<cms:CMSTextBox runat="server" ID="txtFilter" Visible="false" CssClass="SearchFilterField" />
