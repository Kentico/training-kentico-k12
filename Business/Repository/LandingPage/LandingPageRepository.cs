using System;
using System.Linq;

using CMS.DocumentEngine;

using Business.Dto.LandingPage;
using Business.Services.Query;

namespace Business.Repository.LandingPage
{
    public class LandingPageRepository : BaseRepository, ILandingPageRepository
    {

        public LandingPageRepository(IDocumentQueryService documentQueryService) : base(documentQueryService)
        {
        }

        public TLandingPageDto GetLandingPage<TKenticoLandingPage, TLandingPageDto>
            (string pageAlias, 
            Func<DocumentQuery<TKenticoLandingPage>, DocumentQuery<TKenticoLandingPage>> queryModifier = null, 
            Func<TKenticoLandingPage, TLandingPageDto, TLandingPageDto> selector = null)
            where TKenticoLandingPage : TreeNode, new()
            where TLandingPageDto : LandingPageDto, new()
        {
            var query = DocumentQueryService.GetDocument<TKenticoLandingPage>(pageAlias)
                .AddColumns("DocumentID", "DocumentName");

            if (queryModifier != null)
            {
                query = queryModifier(query);
            }

            Func<TKenticoLandingPage, TLandingPageDto> completeSelector = (landingPage) =>
            {
                var dto = new TLandingPageDto
                {
                    DocumentId = landingPage.DocumentID,
                    Title = landingPage.DocumentName
                };

                return selector != null ? selector(query, dto) : dto;
            };

            return query
                .Select(completeSelector)
                .FirstOrDefault();
        }
    }
}
