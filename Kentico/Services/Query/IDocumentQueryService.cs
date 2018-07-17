using CMS.DocumentEngine;

namespace Kentico.Services.Query
{
    public interface IDocumentQueryService
    {
        DocumentQuery<TDocument> GetDocuments<TDocument>() where TDocument : TreeNode, new();
    }
}
