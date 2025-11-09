// File: AnosheCms.Domain/Common/IAuditable.cs
using System;

namespace AnosheCms.Domain.Common
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
        Guid? LastModifiedBy { get; set; }
    }
}