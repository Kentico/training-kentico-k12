using Business.Helpers;

namespace Business.Services.Localization
{
    public class LocalizationService : BaseService, ILocalizationService
    {
        public string Localize(string resourceKey) =>
            LocalizationHelper.Localize(resourceKey);

        public string LocalizeFormat(string resourceKey, params object[] args) =>
            LocalizationHelper.LocalizeFormat(resourceKey, args);
    }
}
