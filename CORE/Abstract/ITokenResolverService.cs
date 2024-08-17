namespace CORE.Abstract;
public interface ITokenResolverService
{
    public string? GetTokenString();
    public bool IsValidToken();
    public Guid GetUserIdFromToken();
    public Guid? GetCompanyIdFromToken();
    public string GenerateRefreshToken();
    public string? GetRoleFromToken(string jwtToken);
    public string TrimToken(string? jwtToken);
}
