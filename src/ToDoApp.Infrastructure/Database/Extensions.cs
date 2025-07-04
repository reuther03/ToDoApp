using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Application.Database;

namespace ToDoApp.Infrastructure.Database;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ToDoDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(@"server=(localdb)\MSSQLLocalDB;database=trip;trusted_connection=true;")));
        return services;

        services.AddScoped<IToDoDbContext, ToDoDbContext>();
    }
}