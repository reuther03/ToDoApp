namespace ToDoApp.Infrastructure.Authentication;

// JwtOptions to klasa konfiguracyjna dla opcji JWT
public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
}