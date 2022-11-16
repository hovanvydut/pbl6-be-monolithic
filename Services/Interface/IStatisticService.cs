using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IStatisticService
{
    #region PostStatistic
    Task<List<PostStatisticGroupDTO>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams);
    Task<PagedList<PostStatisticDTO>> GetPostStatisticInDate(int hostId, PostStatisticInDateParams statisticParams);
    Task<bool> SaveBookmarkStatistic(int postId);
    Task<bool> SaveViewPostDetailStatistic(int postId);
    Task<bool> SaveBookingStatistic(int postId);
    Task<bool> SaveGuestMetMotelStatistic(int postId);
    #endregion

    #region UserStatistic
    Task<List<UserStatisticGroupDTO>> GetUserStatisticWithParams(UserStatisticParams statisticParams);
    Task<PagedList<UserStatisticDTO>> GetUserStatisticInDate(UserStatisticInDateParams statisticParams);
    #endregion
}