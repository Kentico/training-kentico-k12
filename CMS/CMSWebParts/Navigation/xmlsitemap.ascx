<%@ Control Language="C#" AutoEventWireup="true"  Codebehind="~/CMSWebParts/Navigation/xmlsitemap.ascx.cs"
    Inherits="CMSWebParts_Navigation_xmlsitemap" %>
<cms:googlesitemap runat="server" id="ucSiteMap"> 

    <HeaderTemplate><?xml version="1.0" encoding="UTF-8" ?>
<urlset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd" xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"></HeaderTemplate>
    
    <IndexHeaderTemplate><?xml version="1.0" encoding="UTF-8"?>
<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"></IndexHeaderTemplate>
    
    <ItemTemplate><url>
            <%# TransformationHelper.HelperObject.GetSitemapItem(Container.DataItem, "loc") %>
            <%# TransformationHelper.HelperObject.GetSitemapItem(Container.DataItem, "lastmod") %>
            <%# TransformationHelper.HelperObject.GetSitemapItem(Container.DataItem, "changefreq") %>
            <%# TransformationHelper.HelperObject.GetSitemapItem(Container.DataItem, "priority") %>
        </url></ItemTemplate>
    
    <IndexItemTemplate><sitemap>
            <%# TransformationHelper.HelperObject.GetSitemapItem(Container.DataItem, "loc") %>
            <%# TransformationHelper.HelperObject.GetSitemapItem(Container.DataItem, "lastmod") %></sitemap></IndexItemTemplate>

    <IndexFooterTemplate></sitemapindex></IndexFooterTemplate>

    <FooterTemplate></urlset></FooterTemplate>

</cms:googlesitemap>
<asp:Label runat="server" ID="lbSiteMap" Visible="false" EnableViewState="false" CssClass="InfoLabel"></asp:Label>

