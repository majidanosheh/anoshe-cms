USE [AnosheCmsDb];
SELECT 
    u.Email, 
    r.Name as RoleName, 
    rc.ClaimValue as Permission
FROM Users u
LEFT JOIN UserRoles ur ON u.Id = ur.UserId
LEFT JOIN Roles r ON ur.RoleId = r.Id
LEFT JOIN RoleClaims rc ON r.Id = rc.RoleId
WHERE u.Email = 'admin@system.com';