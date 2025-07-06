using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Infrastructure.Database.Repositories;

namespace ToDoApp.Infrastructure.Database;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ToDoDbContext>(options =>
            options.UseSqlServer(@"server=(localdb)\MSSQLLocalDB;database=ToDo;trusted_connection=true;"));

        services.AddScoped<IToDoDbContext, ToDoDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IToDoRepository, ToDoRepository>();

        return services;
    }
}