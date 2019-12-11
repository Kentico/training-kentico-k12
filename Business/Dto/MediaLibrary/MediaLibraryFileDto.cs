using System;

namespace Business.Dto.MediaLibrary
{
    public class MediaLibraryFileDto : IDto
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string DirectUrl { get; set; }
        public string PermanentUrl { get; set; }
        public string Extension { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
