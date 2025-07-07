using ToDoApp.Api.OptionsSetup;
using ToDoApp.Application;
using ToDoApp.Domain;
using ToDoApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;

// Rejestracja usług w kontenerze DI
services
    .AddDomain()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// Konfiguracja opcji JWT
services.ConfigureOptions<JwtOptionsSetup>();
services.ConfigureOptions<JwtBearerOptionsSetup>();


var app = builder.Build();

app.UseInfrastructure();
app.Run();