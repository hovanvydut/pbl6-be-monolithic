using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IUserService
{
    Task<PagedList<UserDTO>> GetAllUsers(UserParams userParams);

    Task<UserProfilePersonalDTO> GetUserProfilePersonal(int userId);

    Task<UserProfileAnonymousDTO> GetUserProfileAnonymous(int userId);

    Task<bool> UpdateUserProfile(int userId, UserProfileUpdateDTO userProfileUpdateDTO);

    Task<bool> UpdateUserAccount(int userId, UserAccountUpdateDTO userAccountUpdateDTO);

    Task<bool> UserMakePayment(int userId, double amount);
}