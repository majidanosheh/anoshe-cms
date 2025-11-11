// File: AnosheCms.Api/Authorization/PermissionRequirement.cs
using Microsoft.AspNetCore.Authorization;

namespace AnosheCms.Api.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}