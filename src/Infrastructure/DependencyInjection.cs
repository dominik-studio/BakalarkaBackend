using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PromobayBackend.Application.Common.Interfaces;
using PromobayBackend.Domain.Constants;
using PromobayBackend.Infrastructure.Data;
using PromobayBackend.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PromobayBackend.Application.Common.Interfaces.Data;
using PromobayBackend.Domain.Entities;
using PromobayBackend.Infrastructure.Data.Repositories;
using PromobayBackend.Infrastructure.Identity;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PromobayBackendDb");
        Guard.Against.Null(connectionString, message: "Connection string 'PromobayBackendDb' not found.");
        
        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
            
            options.ConfigureWarnings(warnings =>
            {
                warnings.Log(RelationalEventId.PendingModelChangesWarning);
            });

        });

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();
        
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        
        
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();

        builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
        builder.Services.AddAuthorization(options =>
            {
                // options.AddPolicy(Policies.CanPurge, builder =>
                // {
                //     builder
                //         .RequireRealmRoles("admin");
                // });
            })
            .AddKeycloakAuthorization(builder.Configuration);
    }
}
