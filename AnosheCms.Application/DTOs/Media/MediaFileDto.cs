using System;

namespace AnosheCms.Application.DTOs.Media
{
    public class MediaFileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}