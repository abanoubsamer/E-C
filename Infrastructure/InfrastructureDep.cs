using Domain.Models;
using Domain.OptionsConfiguration;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Microsoft.Extensions.Logging;
using Infrastructure.Data.AppDbContext;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class InfrastructureDep
    {
        public static IServiceCollection AddDBInjection(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<AppDbContext>(Options =>
              Options.UseLazyLoadingProxies().UseSqlServer(Configuration["ConnectionString:Defult"])
                .EnableSensitiveDataLogging()
                  .LogTo(Console.WriteLine, LogLevel.Information));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<AppDbContext>()
                   .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddDepInjection(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddNotificationjection(this IServiceCollection services)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "App_Data", "chat-3afa0-firebase-adminsdk-sal8j-9fdbbb568f.json");

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(path)
            });

            return services;
        }

        public static IServiceCollection AddHangfirejection(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHangfire(config =>
              config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSqlServerStorage(Configuration["ConnectionString:Defult"])); // استبدل بالاتصال الخاص بك

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddAuthServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.Configure<JwtOptions>(Configuration.GetSection("JWT"));

            Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))
                };
            })
            .AddGoogle(opt =>
            {
                IConfigurationSection googleConfigSection = Configuration.GetSection("Auth:Google");
                opt.ClientId = googleConfigSection["ClientID"];
                opt.ClientSecret = googleConfigSection["Clientsecret"];
                opt.CallbackPath = "/signin-google";
                opt.Scope.Add("email");
                opt.Scope.Add("profile");
                opt.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

                opt.SaveTokens = true;
            }); ;

            Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // hna ana bolh ay t3del 7sl fe ale permission yzhr fe s3tha m4 lazm arstr ale app
                options.ValidationInterval = TimeSpan.Zero;
            });
            return Services;
        }

        public static IServiceCollection AddMailSetting(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.Configure<EamilSettings>(Configuration.GetSection("EmailSettings"));

            return Services;
        }

        public static IServiceCollection AddSessionDep(this IServiceCollection Services)
        {
            Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None; // ✅ لازم مع CORS
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // ✅ ضروري عشان SameSite=None يشتغل

                // أو None أثناء التطوير
            });

            return Services;
        }
    }
}