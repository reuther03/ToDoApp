﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ToDoApp.Infrastructure.Swagger;

// SwaggerExtensions to rozszerzenie IServiceCollection, które konfiguruje dokumentację Swagger
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddRouting(routing => routing.LowercaseUrls = true);
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ToDo API",
                Version = "v1"
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Description = "Raw JWT Bearer token",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new List<string>() }
            });

            swagger.EnableAnnotations();
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1"); });

        return app;
    }
}