using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IStatisticService
{
    #region PostStatistic
    Task<List<PostStatisticGroupDTO>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams);
    Task<List<PostStatisticDTO>> GetTopPostStatistic(int hostId, PostTopStatisticParams statisticParams);
    Task<PagedList<PostStatisticDTO>> GetPostStatisticInDate(int hostId, PostStatisticInDateParams statisticParams);
    Task<bool> SaveBookmarkStatistic(int postId);
    Task<bool> SaveViewPostDetailStatistic(int postId);
    Task<bool> SaveBookingStatistic(int postId);
    Task<bool> SaveGuestMetMotelStatistic(int postId);
    #endregion

    #region UserStatistic
    Task<List<UserStatisticGroupDTO>> GetUserStatisticWithParams(UserStatisticParams statisticParams);
    Task<List<UserStatisticDTO>> GetTopUserStatistic(UserTopStatisticParams statisticParams);
    Task<PagedList<UserStatisticDTO>> GetUserStatisticInDate(UserStatisticInDateParams statisticParams);
    Task<bool> SaveNumberOfPostCreated(int userId);
    Task<bool> SaveNumberOfUptopped(int userId);
    Task<bool> SaveRevenue(int userId, double amount);
    #endregion
}