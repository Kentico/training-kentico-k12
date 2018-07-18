using Kentico.Services.Query;

namespace Kentico.Repository
{
    public abstract class BaseRepository
    {
        protected IDocumentQueryService DocumentQueryService { get; }

        protected BaseRepository(IDocumentQueryService documentQueryService)
        {
            DocumentQueryService = documentQueryService;
        }
    }
}
