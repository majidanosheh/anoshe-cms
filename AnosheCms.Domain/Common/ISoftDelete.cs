// File: AnosheCms.Domain/Common/ISoftDelete.cs
using System;

namespace AnosheCms.Domain.Common
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedDate { get; set; }
        Guid? DeletedBy { get; set; }
    }
}