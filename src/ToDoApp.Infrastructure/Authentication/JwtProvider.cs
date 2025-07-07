using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Application.Database;
using ToDoApp.Domain.User;

namespace ToDoApp.Infrastructure.Authentication;

// JwtProvider to implementacja IJwtProvider, która generuje token JWT dla użytkownika
public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(User user)
    {
        // Tworzenie listy roszczeń (claims) dla tokenu JWT
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimConsts.UserId, user.Id.ToString()),
            new(ClaimConsts.Email, user.Email),
            new(ClaimConsts.Username, user.Username)
        ];

        // Ustawienie roszczeń dla tokenu JWT
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256Signature);

        // Tworzenie tokenu JWT z roszczeniami, datą ważności i podpisem
        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.Now.AddMinutes(60),
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}

// ClaimConsts to klasa zawierająca stałe nazwy roszczeń (claims) używanych w tokenie JWT
public static class ClaimConsts
{
    public const string UserId = "user_id";
    public const string Email = "user_email";
    public const string Username = "user_username";
}