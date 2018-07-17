using CMS.DocumentEngine;
using Kentico.Services.Context;

namespace Kentico.Services.Query
{
    public class DocumentQueryService : IDocumentQueryService
    {
        private ISiteContext SiteContext { get; }

        public DocumentQueryService(ISiteContext siteContext)
        {
            SiteContext = siteContext;
        }

        public DocumentQuery<TDocument> GetDocuments<TDocument>() where TDocument : TreeNode, new()
        {
            var query = DocumentHelper.GetDocuments<TDocument>();

            // Load latest version of documents as preview mode is enabled
            if (SiteContext.IsPreviewEnabled())
            {
                query = query.LatestVersion(true).Published(false).Culture(SiteContext.GetPreviewCulture());
            } else
            {
                query = query.Published(true).Culture(SiteContext.GetActiveSiteCulture());
            }

            return query;
        }
    }
}
