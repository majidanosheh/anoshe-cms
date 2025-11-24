using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.ContentEntry
{
    public class ContentEntryCreateDto
    {
        public Dictionary<string, object> ContentData { get; set; }
        public string Status { get; set; }
    }
}