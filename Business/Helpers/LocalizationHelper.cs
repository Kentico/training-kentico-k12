using CMS.Helpers;

namespace Business.Helpers
{
    public static class LocalizationHelper
    {
        public static string Localize(string resourceKey) =>
            ResHelper.GetString(resourceKey);

        public static string LocalizeFormat(string resourceKey, params object[] args) =>
            ResHelper.GetStringFormat(resourceKey, args);
    }
}
