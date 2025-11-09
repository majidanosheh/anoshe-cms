// File: AnosheCms.Domain/Entities/MediaFile.cs
using AnosheCms.Domain.Common;
using System;

namespace AnosheCms.Domain.Entities
{
    // (اگر کلاس پایه متفاوتی دارید، آن را جایگزین BaseAuditableEntity کنید)
    // این کلاس باید Id, CreatedDate, CreatedBy و... را فراهم کند
    public class MediaFile : AuditableBaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
    }
}