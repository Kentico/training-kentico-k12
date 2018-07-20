using CMS.DocumentEngine;
using Kentico.Services.Context;

namespace Kentico.Services.Query
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
                    .LatestVersion(true)
                    .Published(false)
                    .Culture(SiteContext.PreviewCulture);
            } else
            {
                query = query
                    .PublishedVersion(true)
                    .Culture(SiteContext.CurrentSiteCulture);
            }

            return query;
        }
    }
}
