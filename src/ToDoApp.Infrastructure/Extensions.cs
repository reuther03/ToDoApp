using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Application;
using ToDoApp.Infrastructure.Auth;
using ToDoApp.Infrastructure.Database;
using ToDoApp.Infrastructure.Swagger;

namespace ToDoApp.Infrastructure;

public static class Extensions
{
    private const string CorsPolicy = "cors";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(cors =>
        {
            cors.AddPolicy(CorsPolicy, x =>
            {
                // x.WithOrigins("http://localhost:5000", "https://localhost:5000","http://localhost:50001", "https://localhost:5001")
                //     .WithMethods("GET", "POST", "PUT", "DELETE")
                //     .AllowAnyOrigin()
                //     .AllowAnyHeader()
                //     .AllowCredentials();
                x.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddControllers();
        services.AddDatabase(configuration);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocumentation();


        services.AddAuth(configuration);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(IApplicationAssembly).Assembly, typeof(IInfrastructureAssembly).Assembly);
        });

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseCors(CorsPolicy);
        app.UseSwaggerDocumentation();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}