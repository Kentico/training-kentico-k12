using MedioClinic.Services.Query;

namespace MedioClinic.Repository
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
