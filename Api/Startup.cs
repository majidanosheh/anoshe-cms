// File: Api/Startup.cs
// (نسخه نهایی بازسازی‌شده برای فاز ۴)
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
using AnosheCms.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity; // (ضروری برای فاز ۴)

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

            // ---*** (بخش Identity بازسازی شد - فاز ۴) ***---
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // (بر اساس 177.txt - می‌توانید اینها را سخت‌گیرانه‌تر کنید)
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            // (استفاده از DbContext و مدل‌های Guid جدید ما)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            // ---*** (پایان بخش Identity) ***---

            services.AddHttpContextAccessor();

            // ---*** (ثبت سرویس‌های پروژه - فاز ۴) ***---
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // (سرویس‌های جدید فاز ۴)
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            // (سرویس‌های ماژول‌های قبلی)
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IContentTypeService, ContentTypeService>();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IContentEntryService, ContentEntryService>(); // (بر اساس بازسازی ContentItem)

            services.AddScoped<IUserService, UserService>();
            // ---*** (پایان بخش سرویس‌ها) ***---

            // ---*** (پیکربندی JWT - فاز ۴) ***---
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
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
                    ClockSkew = TimeSpan.Zero // (بدون تاخیر زمانی)
                };
            });
            // ---*** (پایان بخش JWT) ***---

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // (Swagger/OpenAPI بدون تغییر، اما ضروری)
            services.AddSwaggerGen(c =>
            {
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
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });

            // (CORS بدون تغییر، اما ضروری)
            services.AddCors(options =>
            {
                options.AddPolicy(name: _corsPolicyName,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173") // آدرس فرانت‌اند
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

            app.UseStaticFiles(); // (برای wwwroot/uploads)
            app.UseRouting();
            app.UseCors(_corsPolicyName);

            // (مهم: Authentication باید قبل از Authorization باشد)
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}