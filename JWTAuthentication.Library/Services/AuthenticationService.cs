using JWTAuthentication.Library.Helpers;
using JWTAuthentication.Library.Interfaces;
using JWTAuthentication.Library.Model.DB;
using JWTAuthentication.Library.Model.Dto;
using JWTAuthentication.Library.Model.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using static JWTAuthentication.Library.Model.Utilities.Utility;

namespace JWTAuthentication.Library.Services
{
    internal class AuthenticationService(JwtAuthDbContext db, IOptions<JwtSettings> options, ILogger<AuthenticationService> logger) : IAuthenticationService
    {
        private readonly TokenService tokenService = new(options);
    public JwtToken GenerateToken(AWSUserInfo userInfo)
    {
        try
        {
            var user = db.Users.Include(p => p.Tokens).FirstOrDefault(p => p.IdSub == userInfo.Sub) ?? InsertNewUser(userInfo);
            var token = tokenService.GenerateToken(user);
            var refreshToken = tokenService.GenerateRefreshToken();
            var result = new JwtToken()
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                UserName = user.NamUser
            };
            UpdateRefreshToken(user, result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw new InvalidOperationException();
        }
    }

    private User InsertNewUser(AWSUserInfo userInfo)
    {
        var user = new User()
        {
            IdUser = 0,
            IdSub = userInfo.Sub,
            NamUser = userInfo.Email,
            IsActive = true,
            DtmCreate = DateTime.UtcNow,
            DtmUpdate = DateTime.UtcNow
        };
        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }
    public JwtToken GenerateRefreshToken(RefreshTokenRequest request)
    {

        var user = GetUserFromExpiredToken(request);
        if (user == null || !ValidRefreshToken(request, user))
        {
            throw new InvalidDataException(ErrorHandlers[Utility.ErrorHandler.InvalidData.ToString()]);
        }

        return GenerateToken(new AWSUserInfo() { Sub = user.IdSub });
    }

    private void UpdateRefreshToken(User user, JwtToken result)
    {
        var refreshExpired = DateTime.UtcNow
            .AddDays(Convert.ToInt32(options?.Value.RefreshTokenExpiredDay));
        if (user.Tokens != null && user.Tokens.Count > 0)
        {
            var token = user.Tokens.First();
            token.RefreshToken = result.RefreshToken;
            token.DtmRefreshTokenExpired = refreshExpired;

        }
        else
        {
            var token = new UserToken
            {
                IdUser = user.IdUser,
                RefreshToken = result.RefreshToken,
                DtmRefreshTokenExpired = refreshExpired

            };
            db.UserTokens.Add(token);
        }
        db.SaveChanges();

    }
    private User? GetUserFromExpiredToken(RefreshTokenRequest request)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userInfo = JsonSerializer
          .Deserialize<JwtUserInfo>(principal.Claims.Any() ?
          principal.Claims.First().Value : string.Empty);
        return db.Users
              .Include(p => p.Tokens)
              .FirstOrDefault(p => userInfo != null && p.IdUser == userInfo.IdUser);
    }
    private static bool ValidRefreshToken(RefreshTokenRequest request, User user)
    {
        if (user == null
          || !string.Equals(user.Tokens?
          .FirstOrDefault(p => p.DtmRefreshTokenExpired >= DateTime.UtcNow)?
          .RefreshToken, request.RefreshToken))
        {
            return false;
        }
        else { return true; }
    }

}
}
