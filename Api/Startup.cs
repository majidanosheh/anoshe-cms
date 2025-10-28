// File: AnosheCms.Api/Startup.cs

// --- شروع Using Directives ---
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
using System.Text;
// --- پایان Using Directives ---

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
            // 1. Connection String
            var connectionString = Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // 2. DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 3. Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // ... (تنظیمات Identity)
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // 4. HttpContextAccessor
            services.AddHttpContextAccessor();

            // 5. ICurrentUserService
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // 6. IAuthService
            services.AddScoped<IAuthService, AuthService>();

            // 7. IMediaService
            services.AddScoped<IMediaService, MediaService>();

            // ---*** کد جدید اضافه شده ***---
            // 8. IContentTypeService
            services.AddScoped<IContentTypeService, ContentTypeService>();
            // ---*** پایان کد جدید ***---

            // 9. JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                // ... (تنظیمات JWT)
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

            // 10. Controllers
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // 11. Swagger
            services.AddSwaggerGen(c =>
            {
                // ... (تنظیمات Swagger)
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AnosheCms API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    // ... (تنظیمات Swagger Auth)
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    // ... (تنظیمات Swagger Auth)
                });
            });

            // 12. CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: _corsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
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

            // app.UseHttpsRedirection(); 

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(_corsPolicyName);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}