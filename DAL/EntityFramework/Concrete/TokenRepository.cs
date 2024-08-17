using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Concrete;

public class TokenRepository : GenericRepository<Token>, ITokenRepository
{
    private readonly DataContext _dataContext;

    public TokenRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> IsValidAsync(string accessToken, string refreshToken)
    {
        return await _dataContext.Tokens.AnyAsync(m => m.AccessToken == accessToken);
    }

    public async Task<Token> GetActiveTokenAsync(string accessToken)
    {
        return (await _dataContext.Tokens.FirstOrDefaultAsync(m => m.AccessToken == accessToken && !m.IsDeleted))!;
    }

    public async Task<Token> GetAsync(Expression<Func<Token, bool>> filter)
    {
        return (await _dataContext.Tokens.Include(m => m.User).FirstOrDefaultAsync(filter))!;
    }
}