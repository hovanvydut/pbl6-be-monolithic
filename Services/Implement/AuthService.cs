using Microsoft.EntityFrameworkCore.Storage;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Microsoft.Extensions.Options;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using Monolithic.Helpers;
using Monolithic.Common;
using AutoMapper;
using System.Web;
using pbl6_password_hash;
using DotNetCore.CAP;

namespace Monolithic.Services.Implement;

public class AuthService : IAuthService
{
    private readonly IConfigSettingService _configSettingService;
    private readonly IUserAccountReposiory _userAccountRepo;
    private readonly IUserProfileReposiory _userProfileRepo;
    private readonly ISendMailHelper _sendMailHelper;
    private readonly ClientAppSettings _clientApp;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly DataContext _db;
    private readonly ICapPublisher _capBus;

    public AuthService(IConfigSettingService configSettingService,
                       IUserAccountReposiory userAccountRepo,
                       IUserProfileReposiory userProfileRepo,
                       ISendMailHelper sendMailHelper,
                       IConfiguration configuration,
                       ITokenService tokenService,
                       IMapper mapper,
                       DataContext db,
                       IOptions<ClientAppSettings> clientAppSettings,
                       ICapPublisher capBus)
    {
        _configSettingService = configSettingService;
        _userAccountRepo = userAccountRepo;
        _userProfileRepo = userProfileRepo;
        _sendMailHelper = sendMailHelper;
        _tokenService = tokenService;
        _mapper = mapper;
        _db = db;
        _capBus = capBus;
        _clientApp = clientAppSettings.Value;
    }

    public async Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
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
                newUserAccount.IsActived = true;
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
                await SendMailConfirm(newUserAccount, freeCredit);

                transaction.Commit();
                return _mapper.Map<UserRegisterResponseDTO>(newUserAccount);
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    private async Task SendMailConfirm(UserAccountEntity newUserAccount, double freeCredit)
    {
        var webClientPath = $"{_clientApp.EndUserAppUrl}/{_clientApp.ConfirmEmailPath}";
        var uriBuilder = new UriBuilder(webClientPath);
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
        await SendMailBackground(mailContent);
    }

    private async Task SendMailBackground(MailContent mailContent)
    {
        try
        {
            await _capBus.PublishAsync(WorkerConst.SEND_MAIL, mailContent);
        }
        catch { }
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

    public async Task<bool> ConfirmEmail(UserConfirmEmailDTO userConfirmEmailDTO)
    {
        int userId = userConfirmEmailDTO.UserId;
        string code = userConfirmEmailDTO.Code;
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

    public async Task ForgotPassword(string email)
    {
        var userDB = await _userAccountRepo.GetByEmail(email);
        if (userDB == null || !userDB.IsVerified)
            throw new BaseException(HttpCode.BAD_REQUEST, "Account is unverified or not registed");
        var newSecurityCode = CodeSecure.CreateRandomCode();
        userDB.SecurityCode = newSecurityCode;
        await _userAccountRepo.Update(userDB.Id, userDB);

        var webClientPath = $"{_clientApp.EndUserAppUrl}/{_clientApp.RecoverPasswordPath}";
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
        await SendMailBackground(mailContent);
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