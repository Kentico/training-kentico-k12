using CMS.Helpers;

namespace Business.Services.Localization
{
    public class LocalizationService : BaseService, ILocalizationService
    {
        public string Localize(string resourceKey) =>
            ResHelper.GetString(resourceKey);

        public string LocalizeFormat(string resourceKey, params object[] args) =>
            ResHelper.GetStringFormat(resourceKey, args);
    }
}
