using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Application.Database;
using ToDoApp.Common;
using ToDoApp.Infrastructure.Authentication;

namespace ToDoApp.Infrastructure.Auth;

// AuthExtensions to rozszerzenie IServiceCollection, które konfiguruje uwierzytelnianie JWT
public static class AuthExtensions
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // Rejestracja opcji JWT w kontenerze DI
        services.Configure<JwtOptions>(configuration.GetRequiredSection(JwtOptions.SectionName));
        var options = configuration.GetOptions<JwtOptions>(JwtOptions.SectionName);

        // dodanie konfiguracji uwierzytelniania JWT
        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // konfiguracja schematu JWT Bearer
            .AddJwtBearer(o =>
            {
                o.Audience = options.Audience;
                o.IncludeErrorDetails = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Issuer,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
                };
            });

        services.AddAuthorization();

        // Rejestracja usług związanych z uwierzytelnianiem
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IJwtProvider, JwtProvider>();
    }
}