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

        public DocumentQuery<TDocument> GetDocument<TDocument>(int nodeId) where TDocument : TreeNode, new()
        {
            return GetDocuments<TDocument>()
                .TopN(1)
                .WhereEquals("NodeId", nodeId);

        }

        public DocumentQuery<TDocument> GetDocuments<TDocument>() where TDocument : TreeNode, new()
        {
            var query = DocumentHelper.GetDocuments<TDocument>();

            // Load latest version of documents as preview mode is enabled
            if (SiteContext.IsPreviewEnabled)
            {
                query = query
                    .AddColumns("NodeSiteID") // required for preview mode in Admin UI
                    .OnSite(SiteContext.SiteName) // there could be more sites with matching documents
                    .LatestVersion()
                    .Culture(SiteContext.PreviewCulture);
            } else {
                query = query
                    .OnSite(SiteContext.SiteName) // there could be more sites with matching documents
                    .Published()
                    .PublishedVersion()
                    .Culture(SiteContext.CurrentSiteCulture);
            }

            return query;
        }
    }
}
