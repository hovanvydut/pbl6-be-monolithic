using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IStatisticRepository
{
    #region PostStatistic
    Task<List<PostStatisticEntity>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams);

    Task<PostStatisticEntity> GetPostStatisticInNow(string key, int postId);

    Task<bool> CreatePostStatistic(PostStatisticEntity postStatisticEntity);

    Task<bool> UpdatePostStatistic(PostStatisticEntity postStatisticEntity);
    #endregion

    #region PostStatistic
    Task<List<UserStatisticEntity>> GetUserStatisticWithParams(UserStatisticParams statisticParams);

    Task<UserStatisticEntity> GetUserStatisticInNow(string key, int userId);

    Task<bool> CreateUserStatistic(UserStatisticEntity userStatisticEntity);

    Task<bool> UpdateUserStatistic(UserStatisticEntity userStatisticEntity);
    #endregion
}