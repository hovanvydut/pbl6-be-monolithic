using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class StatisticService : IStatisticService
{
    private readonly IStatisticRepository _statisticRepo;
    private readonly IMapper _mapper;

    public StatisticService(IStatisticRepository statisticRepo,
                            IMapper mapper)
    {
        _statisticRepo = statisticRepo;
        _mapper = mapper;
    }

    #region PostStatistic
    public async Task<List<PostStatisticDTO>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams)
    {
        var statistics = await _statisticRepo.GetPostStatisticWithParams(hostId,statisticParams);
        return statistics.Select(s => _mapper.Map<PostStatisticDTO>(s)).ToList();
    }

    public async Task<bool> SaveBookmarkStatistic(int postId)
    {
        return await HandlePostStatistic(StatisticType.BOOKMARK, 1, postId);
    }

    private async Task<bool> HandlePostStatistic(string key, double value, int postId)
    {
        var statisticNow = await _statisticRepo.GetPostStatisticInNow(key, postId);
        if (statisticNow == null)
        {
            var newStatistic = new PostStatisticEntity()
            {
                Key = key,
                Value = value,
                PostId = postId,
            };
            return await _statisticRepo.CreatePostStatistic(newStatistic);
        }
        else
        {
            statisticNow.Value += value;
            return await _statisticRepo.UpdatePostStatistic(statisticNow);
        }
    }
    #endregion

    #region UserStatistic
    public async Task<List<UserStatisticDTO>> GetUserStatisticWithParams(UserStatisticParams statisticParams)
    {
        var statistics = await _statisticRepo.GetUserStatisticWithParams(statisticParams);
        return statistics.Select(s => _mapper.Map<UserStatisticDTO>(s)).ToList();
    }

    private async Task<bool> HandleUserStatistic(string key, double value, int userId)
    {
        var statisticNow = await _statisticRepo.GetUserStatisticInNow(key, userId);
        if (statisticNow == null)
        {
            var newStatistic = new UserStatisticEntity()
            {
                Key = key,
                Value = value,
                UserId = userId,
            };
            return await _statisticRepo.CreateUserStatistic(newStatistic);
        }
        else
        {
            statisticNow.Value += value;
            return await _statisticRepo.UpdateUserStatistic(statisticNow);
        }
    }
    #endregion
}