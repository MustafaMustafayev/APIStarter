using CORE.Abstract;
using CORE.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace CORE.Concrete;

public class TokenResolverService : ITokenResolverService
{
    private readonly ConfigSettings _config;
    private readonly IHttpContextAccessor _context;
    private readonly IEncryptionService _encryptionService;
    public TokenResolverService(ConfigSettings config,
                       IHttpContextAccessor context,
                       IEncryptionService encryptionService)
    {
        _config = config;
        _context = context;
        _encryptionService = encryptionService;
    }

    public string GetTokenString()
    {
        return _context.HttpContext?.Request.Headers[_config.AuthSettings.HeaderName].ToString()!;
    }

    public Guid GetUserIdFromToken()
    {
        var token = GetJwtSecurityToken();
        var userId = _encryptionService.Decrypt(token.Claims.First(c => c.Type == _config.AuthSettings.TokenUserIdKey).Value);
        return Guid.Parse(userId);
    }

    public Guid? GetCompanyIdFromToken()
    {
        var token = GetJwtSecurityToken();
        if (token is null)
        {
            return null;
        }

        var companyIdClaim = token.Claims.First(c => c.Type == _config.AuthSettings.TokenCompanyIdKey);

        if (companyIdClaim is null || string.IsNullOrEmpty(companyIdClaim.Value))
        {
            return null;
        }

        return Guid.Parse(companyIdClaim.Value);
    }

    public bool IsValidToken()
    {
        var tokenString = GetTokenString();

        if (string.IsNullOrEmpty(tokenString) || tokenString.Length < 7)
        {
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_config.AuthSettings.SecretKey);
        try
        {
            tokenHandler.ValidateToken(tokenString[7..], new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public string TrimToken(string? jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken) || jwtToken.Length < 7)
        {
            throw new Exception();
        }

        return jwtToken[7..];
    }

    public string? GetRoleFromToken(string? tokenString)
    {
        var token = GetJwtSecurityToken();
        if (token == null)
        {
            return null;
        }

        var roleIdClaim = token.Claims.First(c => c.Type == _config.AuthSettings.Role);

        if (roleIdClaim is null || string.IsNullOrEmpty(roleIdClaim.Value))
        {
            return null;
        }

        return roleIdClaim.Value;
    }

    private JwtSecurityToken GetJwtSecurityToken()
    {
        var tokenString = GetTokenString();
        return new JwtSecurityToken(tokenString[7..]);
    }
}