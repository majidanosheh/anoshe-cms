using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.ContentEntry
{
    public class ContentEntryDto
    {
        public Guid Id { get; set; }
        public string ContentTypeApiSlug { get; set; }
        public Dictionary<string, object> ContentData { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}