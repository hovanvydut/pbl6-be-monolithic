using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IUserService
{
    Task<UserProfilePersonalDTO> GetUserProfilePersonal(int userId);

    Task<UserProfileAnonymousDTO> GetUserProfileAnonymous(int userId);

    Task<bool> UpdateUserProfile(int userId, UserProfileUpdateDTO userProfileUpdateDTO);
}