// File: AnosheCms.Application/Interfaces/ICurrentUserService.cs
using System;

namespace AnosheCms.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string RemoteIpAddress { get; }
        string UserAgent { get; }
    }
}