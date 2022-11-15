using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IStatisticService
{
    #region PostStatistic
    Task<List<PostStatisticDTO>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams);
    Task<bool> SaveBookmarkStatistic(int userAccountId);
    #endregion

    #region UserStatistic
    Task<List<UserStatisticDTO>> GetUserStatisticWithParams(UserStatisticParams statisticParams);
    #endregion
}