using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Concrete;

public class AuthService(IMapper mapper,
                         IUserRepository userRepository,
                         ITokenRepository tokenRepository,
                         ITokenResolverService tokenResolverService) : IAuthService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly ITokenResolverService _tokenResolverService = tokenResolverService;

    public async Task<string?> GetUserSaltAsync(string email)
    {
        return await _userRepository.GetUserSaltAsync(email);
    }

    public async Task<IDataResult<UserResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var data = await _userRepository.GetAsync(m => m.Email == dto.Email &&
                                                       m.Password == dto.Password);

        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserResponseDto>> LoginByTokenAsync()
    {
        var userId = _tokenResolverService.GetUserIdFromToken();

        var data = await _userRepository.GetAsync(m => m.Id == userId);
        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public async Task<IResult> LogoutAsync(string accessToken)
    {
        var token = await _tokenRepository.GetActiveTokenAsync(accessToken);
        await _tokenRepository.SoftDeleteAsync(token);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> LogoutRemovedUserAsync(Guid userId)
    {
        var tokens = (await _tokenRepository.GetListAsync(m => m.UserId == userId)).ToList();
        tokens.ForEach(m => m.IsDeleted = true);
        await _tokenRepository.UpdateRangeAsync(tokens);

        return new SuccessResult(EMessages.Success.Translate());
    }
}