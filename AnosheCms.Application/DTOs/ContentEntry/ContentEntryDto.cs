using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.ContentEntry
{
    public class ContentEntryDto
    {
        public Guid Id { get; set; }
        public Guid ContentTypeId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Dictionary<string, object> ContentData { get; set; }

        public ContentEntryDto() { }

        public ContentEntryDto(Guid id, Guid contentTypeId, string status, DateTime createdDate, string? createdBy, DateTime? lastModifiedDate, Dictionary<string, object> contentData)
        {
            Id = id;
            ContentTypeId = contentTypeId;
            Status = status;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            LastModifiedDate = lastModifiedDate;
            ContentData = contentData;
        }
    }
}