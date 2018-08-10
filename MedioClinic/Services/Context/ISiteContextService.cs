namespace MedioClinic.Services.Context
{
    public interface ISiteContextService
    {
        /// <summary>
        /// Holds the current codename of sitse
        /// </summary>
        string SiteName { get; }

        /// <summary>
        /// Holds active culture codename
        /// </summary>
        string CurrentSiteCulture { get; }

        /// <summary>
        /// Indicates what preview culture should be used for preview mode
        /// </summary>
        string PreviewCulture { get; }

        /// <summary>
        /// Indicates if preview mode is enabled
        /// </summary>
        bool IsPreviewEnabled { get; }
    }
}
