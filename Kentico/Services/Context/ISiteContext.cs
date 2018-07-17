namespace Kentico.Services.Context
{
    public interface ISiteContext
    {
        string GetActiveSiteCulture();
        string GetPreviewCulture();
        bool IsPreviewEnabled();
    }
}
