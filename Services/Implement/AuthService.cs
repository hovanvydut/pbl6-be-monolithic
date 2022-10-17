using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using System.Security.Cryptography;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
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
        var newUserAccount = _mapper.Map<UserAccountEntity>(userRegisterDTO);
        var newUserProfile = _mapper.Map<UserProfileEntity>(userRegisterDTO);
        // Throw exception if register unique info is exists
        if (await _userAccountRepository.GetByEmail(newUserAccount.Email) != null)
            throw new BaseException(HttpCode.BAD_REQUEST, "This email is existed");
        if (await _userProfileReposiory.IsInvalidNewProfile(newUserProfile))
            throw new BaseException(HttpCode.BAD_REQUEST, "Phone number or identity number is existed");

        // Create user account
        using var hmac = new HMACSHA512();
        var passwordByte = Encoding.UTF8.GetBytes(userRegisterDTO.Password);
        newUserAccount.PasswordSalt = hmac.Key;
        newUserAccount.PasswordHashed = hmac.ComputeHash(passwordByte);
        newUserAccount.IsVerified = false;
        await _userAccountRepository.Create(newUserAccount);

        // Create user profile with new user account Id
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
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is not registed or email confirmed");

        using var hmac = new HMACSHA512(currentUser.PasswordSalt);
        var passwordByte = Encoding.UTF8.GetBytes(userLoginDTO.Password);
        var computedHash = hmac.ComputeHash(passwordByte);
        if (!computedHash.SequenceEqual(currentUser.PasswordHashed))
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid password");

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
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is already verified or not registed");

        currentUser.IsVerified = true;
        return await _userAccountRepository.Update(currentUser.Id, currentUser);
    }
}