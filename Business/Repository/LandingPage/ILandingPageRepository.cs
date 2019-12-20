using System;

using CMS.DocumentEngine;

using Business.Dto.LandingPage;

namespace Business.Repository.LandingPage
{
    /// <summary>
    /// Represents a contract for a landing page.
    /// </summary>
    public interface ILandingPageRepository : IRepository
    {
        /// <summary>
        /// Retrieves a landing page DTO object.
        /// </summary>
        /// <typeparam name="TKenticoLandingPage">The generated Kentico landing page type.</typeparam>
        /// <typeparam name="TLandingPageDto">The landing page DTO class.</typeparam>
        /// <param name="pageAlias">Page alias.</param>
        /// <param name="queryModifier">Delegate that modifies the base query (e.g. adds columns).</param>
        /// <param name="selector">Delegate that maps additional properties from <typeparamref name="TKenticoLandingPage"/> onto 
        /// <typeparamref name="TLandingPageDto"/>, on top those present in <see cref="Business.Dto.LandingPage.LandingPageDto"/>.</param>
        /// <returns></returns>
        TLandingPageDto GetLandingPage<TKenticoLandingPage, TLandingPageDto>
            (string pageAlias, 
            Func<DocumentQuery<TKenticoLandingPage>, DocumentQuery<TKenticoLandingPage>> queryModifier = null, 
            Func<TKenticoLandingPage, TLandingPageDto, TLandingPageDto> selector = null)
            where TKenticoLandingPage : TreeNode, new()
            where TLandingPageDto : LandingPageDto, new();
    }
}
