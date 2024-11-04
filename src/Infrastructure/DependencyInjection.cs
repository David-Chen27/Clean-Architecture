using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Domain.Constants;
using Clean_Architecture.Infrastructure.Data;
using Clean_Architecture.Infrastructure.Data.Interceptors;
using Clean_Architecture.Infrastructure.Identity;
using Clean_Architecture.Infrastructure.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Clean_Architecture.Infrastructure.Bcrypt;
using Clean_Architecture.Infrastructure.Encryption;
using Clean_Architecture.Infrastructure.Mail;
using Serilog;

namespace Clean_Architecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);
            //.AddCookie(IdentityConstants.ApplicationScheme, options =>
            //{
            //    options.LoginPath = "/api/authentication/login";
            //    options.AccessDeniedPath = "/api/authentication/login";
            //});

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                // 設定密碼選項
                options.Password.RequireDigit = true; // 必須包含數字
                options.Password.RequiredLength = 12; // 最小長度為 12
                options.Password.RequireNonAlphanumeric = true; // 必須包含非字母數字字符
                options.Password.RequireUppercase = true; // 必須包含大寫字母
                options.Password.RequireLowercase = true; // 必須包含小寫字母
                options.Password.RequiredUniqueChars = 5; // 至少包含 5 個不重複的字符

                // 設定鎖定選項
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // 鎖定 15 分鐘
                options.Lockout.MaxFailedAccessAttempts = 5; // 5 次失敗後鎖定
                options.Lockout.AllowedForNewUsers = true;

                // 設定使用者選項
                options.User.RequireUniqueEmail = true;

                // 密碼過期
                // Note: 密碼過期邏輯需要自訂
            })
            .AddRoles<ApplicationRole>()
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints()
            .AddErrorDescriber<CustomIdentityErrorDescriber>();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<AccountService>();
        services.AddTransient<IApplicationAuthorizationService, AuthorizationService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddSingleton<IBcryptService, BcryptService>();
        services.AddSingleton<IMailService, MailService>();
        services.AddSingleton<IEmailSender<ApplicationUser>, EmailSenderService>();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
        
       // 自定義授權策略
        services.AddSingleton<IAuthorizationPolicyProvider, CustomizePolicyProvider>();
        services.AddScoped<IAuthorizationHandler, ResourcePermissionHandler>();

        // Setting Serilog
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.With<EventTypeEnricher>()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
        });

        return services;
    }
}
