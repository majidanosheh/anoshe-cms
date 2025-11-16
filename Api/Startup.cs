using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using AnosheCms.Application.Interfaces;
using AnosheCms.Infrastructure.Services; // (جدید) اطمینان از ایمپورت شدن سرویس‌ها
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AnosheCms.Api.Authorization;
using AnosheCms.Domain.Constants;
using System.Reflection;
using System.Linq;

namespace AnosheCms.Api
{
    public class Startup
    {
        private readonly string _corsPolicyName = "AnosheCmsCorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();

            // (اصلاح شد) ثبت کامل تمام سرویس‌های پروژه
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<ISettingsService, SettingsService>();

            // (جدید) ثبت سرویس‌هایی که باعث کرش شده بودند
            services.AddScoped<IUserService, UserService>(); // (این سرویس نیز در بازنشانی کامل جا افتاده بود)
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IContentTypeService, ContentTypeService>();
            services.AddScoped<IContentEntryService, ContentEntryService>();
            // (سرویس‌های جدید فاز ۱.۴)
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFormService, FormService>();

            // (استفاده از پیاده‌سازی لاگ‌کننده به جای SMTP واقعی)
            services.AddScoped<IEmailService, LoggingEmailService>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })


            .AddJwtBearer(o => {
                var secretKey = Configuration["JwtSettings:Secret"]
                    ?? throw new InvalidOperationException("JWT Secret key is not configured.");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                var permissionType = typeof(Permissions);
                var allPermissions = permissionType.GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.IsLiteral && f.FieldType == typeof(string))
                    .Select(f => (string)f.GetValue(null))
                    .ToList();

                foreach (var permission in allPermissions)
                {
                    // (این کد به صورت خودکار دسترسی‌های جدید Forms را نیز ثبت می‌کند)
                    options.AddPolicy(permission, policy =>
                        policy.AddRequirements(new PermissionRequirement(permission)));
                }

                options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AnosheCms API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddCors(options => {
                options.AddPolicy(name: _corsPolicyName,
                    builder => {
                        builder.WithOrigins("http://localhost:5173", "http://localhost:8080")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnosheCms API v1"));
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(_corsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}