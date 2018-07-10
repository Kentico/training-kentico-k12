<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSPages_GoogleSiteMap"
    ContentType="text/xml"  Codebehind="GoogleSiteMap.aspx.cs" %><?xml version="1.0" encoding="UTF-8" ?>
<urlset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"
    xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
<cms:GoogleSitemap runat="server" ID="googleSitemap" TransformationName="CMS.Root.GoogleSiteMap" CacheMinutes="0" OrderBy="NodeLevel, NodeOrder, NodeName" />
</urlset>
