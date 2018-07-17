namespace Kentico.Services.Context
{
    public interface ISiteContextService
    {
        string CurrentSiteCulture { get; }
        string PreviewCulture { get; }
        bool IsPreviewEnabled { get; }
    }
}
