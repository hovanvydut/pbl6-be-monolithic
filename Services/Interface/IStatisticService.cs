using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IStatisticService
{
    #region PostStatistic
    Task<List<PostStatisticGroupDTO>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams);
    // Task<List<PostStatisticDTO>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams);
    Task<bool> SaveBookmarkStatistic(int postId);
    Task<bool> SaveViewPostDetailStatistic(int postId);
    Task<bool> SaveBookingStatistic(int postId);
    Task<bool> SaveGuestMetMotelStatistic(int postId);
    #endregion

    #region UserStatistic
    Task<List<UserStatisticGroupDTO>> GetUserStatisticWithParams(UserStatisticParams statisticParams);
    #endregion
}