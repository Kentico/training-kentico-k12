namespace Business.Services.Localization
{
    /// <summary>
    /// Abstraction from Kentico's <see cref="CMS.Helpers.ResHelper"/>.
    /// </summary>
    public interface ILocalizationService : IService
    {
        /// <summary>
        /// Gets a resource string value.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <returns>Localized resource.</returns>
        string Localize(string resourceKey);

        /// <summary>
        /// Gets a localized resource and applies <see cref="string.Format(string, object[])"/> on the result.
        /// </summary>
        /// <param name="resourceKey">Resource key.</param>
        /// <param name="args">Values to put to the result string.</param>
        /// <returns></returns>
        string LocalizeFormat(string resourceKey, params object[] args);
    }
}
