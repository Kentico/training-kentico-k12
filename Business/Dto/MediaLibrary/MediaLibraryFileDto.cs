namespace Business.Dto.MediaLibrary
{
    public class MediaLibraryFileDto : IDto
    {
        public string Title { get; set; }
        public string DirectUrl { get; set; }
        public string PermanentUrl { get; set; }
        public string Extension { get; set; }
    }
}
