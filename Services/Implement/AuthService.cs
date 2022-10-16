using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using System.Security.Cryptography;
using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
using Monolithic.Helpers;
using System.Text;
using AutoMapper;
using System.Web;

namespace Monolithic.Services.Implement;

public class AuthService : IAuthService
{
    private readonly IUserAccountReposiory _userAccountRepository;
    private readonly IUserProfileReposiory _userProfileReposiory;
    private readonly ISendMailHelper _sendMailHelper;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUserAccountReposiory userAccountRepository,
                       IUserProfileReposiory userProfileReposiory,
                       ISendMailHelper sendMailHelper,
                       ITokenService tokenService,
                       IMapper mapper)
    {
        _userAccountRepository = userAccountRepository;
        _userProfileReposiory = userProfileReposiory;
        _sendMailHelper = sendMailHelper;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO, string scheme, string host)
    {
        // Return null if register email is exists
        if (await _userAccountRepository.GetByEmail(userRegisterDTO.Email) != null)
            return null;
        using var hmac = new HMACSHA512();
        var passwordByte = Encoding.UTF8.GetBytes(userRegisterDTO.Password);

        // Create user account
        var newUserAccount = _mapper.Map<UserAccountEntity>(userRegisterDTO);
        newUserAccount.PasswordSalt = hmac.Key;
        newUserAccount.PasswordHashed = hmac.ComputeHash(passwordByte);
        newUserAccount.IsVerified = false;
        await _userAccountRepository.Create(newUserAccount);

        // Create user profile with new user account Id
        var newUserProfile = _mapper.Map<UserProfileEntity>(userRegisterDTO);
        newUserProfile.CurrentCredit = 0;
        newUserProfile.UserAccountId = newUserAccount.Id;
        await _userProfileReposiory.Create(newUserProfile);

        // Send mail
        await SendMailConfirm(newUserAccount, scheme, host);

        return _mapper.Map<UserRegisterResponseDTO>(newUserAccount);
    }

    private async Task SendMailConfirm(UserAccountEntity newUserAccount, string scheme, string host)
    {
        var webServerPath = $"{scheme}://{host}/api/auth/confirm-email";
        var uriBuilder = new UriBuilder(webServerPath);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["userId"] = newUserAccount.Id.ToString();
        uriBuilder.Query = query.ToString();

        var mailContent = new MailContent()
        {
            ToEmail = newUserAccount.Email,
            Subject = "Confirm email to use Motel Finder",
            Body = $"Confirm the registration by clicking on the <a href='{uriBuilder}'>link</a>."
        };
        await _sendMailHelper.SendEmailAsync(mailContent);
    }

    public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO)
    {
        var currentUser = await _userAccountRepository.GetByEmail(userLoginDTO.Email);
        if (currentUser == null || !currentUser.IsVerified)
            return null;

        using var hmac = new HMACSHA512(currentUser.PasswordSalt);
        var passwordByte = Encoding.UTF8.GetBytes(userLoginDTO.Password);
        var computedHash = hmac.ComputeHash(passwordByte);
        if (!computedHash.SequenceEqual(currentUser.PasswordHashed))
            return null;

        return new UserLoginResponseDTO()
        {
            Id = currentUser.Id,
            Email = currentUser.Email,
            AccessToken = _tokenService.CreateToken(currentUser)
        };
    }

    public async Task<bool> ConfirmEmail(int userId)
    {
        var currentUser = await _userAccountRepository.GetById(userId);
        if (currentUser == null || currentUser.IsVerified)
            return false;

        currentUser.IsVerified = true;
        return await _userAccountRepository.Update(currentUser.Id, currentUser);
    }
}