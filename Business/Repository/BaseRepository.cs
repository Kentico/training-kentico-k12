using Business.Services.Query;

namespace Business.Repository
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
