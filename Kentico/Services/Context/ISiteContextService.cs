namespace Kentico.Services.Context
{
    public interface ISiteContextService
    {
        string SiteName { get; }
        string CurrentSiteCulture { get; }
        string PreviewCulture { get; }
        bool IsPreviewEnabled { get; }
    }
}
