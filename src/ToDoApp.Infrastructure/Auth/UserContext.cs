using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TripManager.Application.Abstractions;
using TripManager.Domain.Users;
using TripManager.Domain.Users.ValueObjects;
using TripManager.Infrastructure.Authentication;

namespace TripManager.Infrastructure.Auth;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    [MemberNotNullWhen(true, nameof(UserId), nameof(Email), nameof(Username))]
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public UserId? UserId => IsAuthenticated ? GetUserIdFromClaims(_httpContextAccessor.HttpContext?.User) : null;
    public Email? Email => IsAuthenticated ? GetEmailFromClaims(_httpContextAccessor.HttpContext?.User) : null;
    public Username? Username => IsAuthenticated ? GetUsernameFromClaims(_httpContextAccessor.HttpContext?.User) : null;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private static UserId? GetUserIdFromClaims(ClaimsPrincipal? claims)
    {
        if (claims is null)
            return null;

        var userId = claims.FindFirst(ClaimConsts.UserId)?.Value;
        return userId is null ? null : UserId.From(userId);
    }

    private static Email? GetEmailFromClaims(ClaimsPrincipal? claims)
    {
        if (claims is null)
            return null;

        var email = claims.FindFirst(ClaimConsts.Email)?.Value;
        return email is null ? null : new Email(email);
    }

    private static Username? GetUsernameFromClaims(ClaimsPrincipal? claims)
    {
        if (claims is null)
            return null;

        var username = claims.FindFirst(ClaimConsts.Username)?.Value;
        return username is null ? null : new Username(username);
    }
}