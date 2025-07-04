using Microsoft.Extensions.DependencyInjection;

namespace ToDoApp.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services;
    }
}