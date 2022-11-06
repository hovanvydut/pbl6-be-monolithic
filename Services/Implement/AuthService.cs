using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using Monolithic.Helpers;
using AutoMapper;
using System.Web;

namespace Monolithic.Services.Implement;

public class AuthService : IAuthService
{
    private readonly IConfigSettingService _configSettingService;
    private readonly IUserAccountReposiory _userAccountRepo;
    private readonly IUserProfileReposiory _userProfileRepo;
    private readonly ISendMailHelper _sendMailHelper;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IConfigSettingService configSettingService,
                       IUserAccountReposiory userAccountRepo,
                       IUserProfileReposiory userProfileRepo,
                       ISendMailHelper sendMailHelper,
                       IConfiguration configuration,
                       ITokenService tokenService,
                       IMapper mapper)
    {
        _configSettingService = configSettingService;
        _userAccountRepo = userAccountRepo;
        _userProfileRepo = userProfileRepo;
        _sendMailHelper = sendMailHelper;
        _configuration = configuration;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO, string scheme, string host)
    {
        var newUserAccount = _mapper.Map<UserAccountEntity>(userRegisterDTO);
        var newUserProfile = _mapper.Map<UserProfileEntity>(userRegisterDTO);
        // Throw exception if register unique info is exists
        if (await _userAccountRepo.GetByEmail(newUserAccount.Email) != null)
            throw new BaseException(HttpCode.BAD_REQUEST, "This email is existed");
        if (await _userProfileRepo.IsInvalidNewProfile(newUserProfile))
            throw new BaseException(HttpCode.BAD_REQUEST, "Phone number or identity number is existed");

        // Create user account
        var passwordHash = PasswordSecure.GetPasswordHash(userRegisterDTO.Password);
        newUserAccount.PasswordSalt = passwordHash.PasswordSalt;
        newUserAccount.PasswordHashed = passwordHash.PasswordHashed;
        newUserAccount.IsVerified = false;
        newUserAccount.SecurityCode = CodeSecure.CreateRandomCode();
        await _userAccountRepo.Create(newUserAccount);

        // Create user profile with new user account Id
        var postPrice = await _configSettingService.GetValueByKey(ConfigSetting.POST_PRICE);
        var freePost = await _configSettingService.GetValueByKey(ConfigSetting.FREE_POST);
        var freeCredit = postPrice * freePost;
        newUserProfile.CurrentCredit = freeCredit;
        newUserProfile.UserAccountId = newUserAccount.Id;
        await _userProfileRepo.Create(newUserProfile);

        // Send mail
        await SendMailConfirm(newUserAccount, freeCredit, scheme, host);

        return _mapper.Map<UserRegisterResponseDTO>(newUserAccount);
    }

    private async Task SendMailConfirm(UserAccountEntity newUserAccount, double freeCredit,
                                        string scheme, string host)
    {
        var webServerPath = $"{scheme}://{host}/api/auth/confirm-email";
        var uriBuilder = new UriBuilder(webServerPath);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["userId"] = newUserAccount.Id.ToString();
        query["code"] = newUserAccount.SecurityCode;
        uriBuilder.Query = query.ToString();

        var mailContent = new MailContent()
        {
            ToEmail = newUserAccount.Email,
            Subject = "Confirm email to use Motel Finder",
            Body = $"Confirm the registration by clicking on the <a href='{uriBuilder}'>link</a> and receive {freeCredit} credit"
        };
        await _sendMailHelper.SendEmailAsync(mailContent);
    }

    public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO)
    {
        var currentUser = await _userAccountRepo.GetByEmail(userLoginDTO.Email);
        if (currentUser == null)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account has not registed yet");
        if (!currentUser.IsVerified)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account has not email confirmed yet");
        if (!currentUser.IsActived)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is blocked");

        var passwordHash = new PasswordHash(currentUser.PasswordHashed, currentUser.PasswordSalt);
        if (!PasswordSecure.IsValidPasswod(userLoginDTO.Password, passwordHash))
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid password");

        var accessToken = await _tokenService.CreateToken(currentUser);
        return new UserLoginResponseDTO()
        {
            Id = currentUser.Id,
            Email = currentUser.Email,
            DisplayName = currentUser.UserProfile.DisplayName,
            AccessToken = accessToken
        };
    }

    public async Task<bool> ConfirmEmail(int userId, string code)
    {
        var currentUser = await _userAccountRepo.GetById(userId);
        if (currentUser == null || currentUser.IsVerified)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is already verified or not registed");
        if (currentUser.SecurityCode != code)
            throw new BaseException(HttpCode.BAD_REQUEST, "Security code is invalid");
        currentUser.IsVerified = true;
        currentUser.SecurityCode = "";
        return await _userAccountRepo.Update(currentUser.Id, currentUser);
    }

    public async Task<bool> ChangePassword(int userId, UserChangePasswordDTO userChangePasswordDTO)
    {
        var userDB = await _userAccountRepo.GetById(userId);
        if (userDB == null || !userDB.IsVerified)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is unverified or not registed");

        var passwordHashDB = new PasswordHash(userDB.PasswordHashed, userDB.PasswordSalt);
        if (!PasswordSecure.IsValidPasswod(userChangePasswordDTO.OldPassword, passwordHashDB))
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid old password");

        var newPasswordHash = PasswordSecure.GetPasswordHash(userChangePasswordDTO.NewPassword);
        userDB.PasswordHashed = newPasswordHash.PasswordHashed;
        userDB.PasswordSalt = newPasswordHash.PasswordSalt;
        return await _userAccountRepo.Update(userId, userDB);
    }

    public async Task ForgotPassword(string email, string scheme, string host)
    {
        var userDB = await _userAccountRepo.GetByEmail(email);
        if (userDB == null || !userDB.IsVerified)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is unverified or not registed");
        var newSecurityCode = CodeSecure.CreateRandomCode();
        userDB.SecurityCode = newSecurityCode;
        await _userAccountRepo.Update(userDB.Id, userDB);

        var webClientUrl = _configuration["ClientApp:Url"];
        var webClientPath = $"{webClientUrl}/auth/recover-password";
        var uriBuilder = new UriBuilder(webClientPath);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["userId"] = userDB.Id.ToString();
        query["code"] = newSecurityCode;
        uriBuilder.Query = query.ToString();

        var mailContent = new MailContent()
        {
            ToEmail = email,
            Subject = "Motel Finder password recovery",
            Body = $"Clicking on the <a href='{uriBuilder}'>link</a> to recover your password"
        };
        await _sendMailHelper.SendEmailAsync(mailContent);
    }

    public async Task RecoverPassword(UserRecoverPasswordDTO userRecoverPasswordDTO)
    {
        var userDB = await _userAccountRepo.GetById(userRecoverPasswordDTO.UserId);
        if (userDB == null || !userDB.IsVerified)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is unverified or not registed");
        if (userDB.SecurityCode != userRecoverPasswordDTO.Code)
            throw new BaseException(HttpCode.BAD_REQUEST, "Security code is invalid");

        var newPasswordHash = PasswordSecure.GetPasswordHash(userRecoverPasswordDTO.NewPassword);
        userDB.PasswordHashed = newPasswordHash.PasswordHashed;
        userDB.PasswordSalt = newPasswordHash.PasswordSalt;
        userDB.SecurityCode = "";
        await _userAccountRepo.Update(userRecoverPasswordDTO.UserId, userDB);
    }
}