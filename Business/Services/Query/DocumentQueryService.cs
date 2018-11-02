using System;
using CMS.DocumentEngine;
using Business.Services.Context;

namespace Business.Services.Query
{
    public class DocumentQueryService : IDocumentQueryService
    {
        private ISiteContextService SiteContext { get; }

        public DocumentQueryService(ISiteContextService siteContext)
        {
            SiteContext = siteContext;
        }

        public DocumentQuery<TDocument> GetDocument<TDocument>(Guid nodeGuid) where TDocument : TreeNode, new()
        {
            return GetDocuments<TDocument>()
                .TopN(1)
                .WhereEquals("NodeGUID", nodeGuid);

        }

        public DocumentQuery<TDocument> GetDocuments<TDocument>() where TDocument : TreeNode, new()
        {
            var query = DocumentHelper.GetDocuments<TDocument>();

            // Loads the latest version of documents as preview mode is enabled
            if (SiteContext.IsPreviewEnabled)
            {
                query = query
                    .AddColumns("NodeSiteID") // Required for preview mode in Admin UI
                    .OnSite(SiteContext.SiteName) // There could be more sites with matching documents
                    .LatestVersion()
                    .Culture(SiteContext.PreviewCulture);
            } else {
                query = query
                    .OnSite(SiteContext.SiteName) // There could be more sites with matching documents
                    .Published()
                    .PublishedVersion()
                    .Culture(SiteContext.CurrentSiteCulture);
            }

            return query;
        }
    }
}
