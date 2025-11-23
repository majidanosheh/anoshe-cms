-- #################################################################
-- ### 1. ایمیل خود را اینجا وارد کنید ###
DECLARE @UserEmail NVARCHAR(256) = 'admin@system.com';
-- #################################################################


-- 2. تعریف متغیرها
DECLARE @RoleName NVARCHAR(256) = 'SuperAdmin';
DECLARE @RoleId UNIQUEIDENTIFIER;
DECLARE @UserId UNIQUEIDENTIFIER;

-- 3. پیدا کردن شناسه کاربر (اصلاح شده)
SELECT @UserId = Id FROM [Users] WHERE Email = @UserEmail;

IF @UserId IS NULL
BEGIN
    RAISERROR('!!! خطای بحرانی: کاربری با این ایمیل یافت نشد. اسکریپت متوقف شد. !!!', 16, 1);
    RETURN;
END

PRINT 'کاربر ' + @UserEmail + ' با موفقیت یافت شد.';

-- 4. ایجاد نقش "SuperAdmin" (اصلاح شده)
IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE Name = @RoleName)
BEGIN
    SET @RoleId = NEWID();
    INSERT INTO [Roles] (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (@RoleId, @RoleName, UPPER(@RoleName), NEWID());
    PRINT 'نقش SuperAdmin با موفقیت ایجاد شد.';
END
ELSE
BEGIN
    SELECT @RoleId = Id FROM [Roles] WHERE Name = @RoleName;
    PRINT 'نقش SuperAdmin از قبل وجود داشت.';
END

-- 5. تخصیص کاربر به نقش "SuperAdmin" (اصلاح شده)
IF NOT EXISTS (SELECT 1 FROM [UserRoles] WHERE UserId = @UserId AND RoleId = @RoleId)
BEGIN
    INSERT INTO [UserRoles] (UserId, RoleId)
    VALUES (@UserId, @RoleId);
    PRINT 'کاربر با موفقیت به نقش SuperAdmin اضافه شد.';
END
ELSE
BEGIN
    PRINT 'کاربر از قبل در نقش SuperAdmin قرار دارد.';
END

-- 6. افزودن تمام دسترسی‌های سیستمی به این نقش (اصلاح شده)
PRINT 'در حال افزودن تمام دسترسی‌های سیستمی به نقش SuperAdmin...';

;WITH AllPermissions (PermissionName) AS (
    -- (این لیست بر اساس فایل Permissions.cs ماست)
    SELECT 'Permissions.ViewUsers'
    UNION ALL SELECT 'Permissions.ManageUsers'
    UNION ALL SELECT 'Permissions.ViewRoles'
    UNION ALL SELECT 'Permissions.ManageRoles'
    UNION ALL SELECT 'Permissions.ManageSettings'
    UNION ALL SELECT 'Permissions.ViewForms'
    UNION ALL SELECT 'Permissions.ManageForms'
    UNION ALL SELECT 'Permissions.ViewSubmissions'
    UNION ALL SELECT 'Permissions.ManageContentTypes'
    UNION ALL SELECT 'Permissions.ManageContentEntries'
    UNION ALL SELECT 'Permissions.ManageMedia'
)
-- (اصلاح شده)
INSERT INTO [RoleClaims] (RoleId, ClaimType, ClaimValue)
SELECT @RoleId, 'Permission', p.PermissionName
FROM AllPermissions p
WHERE NOT EXISTS (
    SELECT 1
    FROM [RoleClaims]
    WHERE RoleId = @RoleId
    AND ClaimType = 'Permission'
    AND ClaimValue = p.PermissionName
);

PRINT '--- عملیات با موفقیت کامل انجام شد ---';
PRINT 'اکنون در پنل کاربری، Ctrl + F5 را بزنید تا دسترسی‌ها بازخوانی شوند.';