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
    private const string DATETIME_FORMAT = "dd/MM/yyyy";

    public StatisticService(IStatisticRepository statisticRepo,
                            IMapper mapper)
    {
        _statisticRepo = statisticRepo;
        _mapper = mapper;
    }

    #region PostStatistic
    public async Task<List<PostStatisticGroupDTO>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams)
    {
        var statistics = await _statisticRepo.GetPostStatisticWithParams(hostId, statisticParams);
        var statisticGroup = statistics.GroupBy(s => s.CreatedAt.Date);
        var listStatistic = new List<PostStatisticGroupDTO>();
        for (DateTime date = statisticParams.FromDate.Date; date <= statisticParams.ToDate.Date; date = date.AddDays(1))
        {
            var statistic = statisticGroup.FirstOrDefault(s => s.Key.Date == date.Date);
            if (statistic != null)
            {
                listStatistic.Add(new PostStatisticGroupDTO()
                {
                    StatisticDate = date.ToString(DATETIME_FORMAT),
                    Posts = statistic.Select(p => _mapper.Map<PostStatisticDTO>(p)),
                    StatisticValue = statistic.Sum(s => s.Value)
                });
            }
            else
            {
                listStatistic.Add(new PostStatisticGroupDTO()
                {
                    StatisticDate = date.ToString(DATETIME_FORMAT),
                    Posts = Enumerable.Empty<PostStatisticDTO>(),
                    StatisticValue = 0
                });
            }
        }
        return listStatistic;
    }

    public async Task<bool> SaveBookmarkStatistic(int postId)
    {
        return await HandlePostStatistic(StatisticType.BOOKMARK, 1, postId);
    }

    public async Task<bool> SaveViewPostDetailStatistic(int postId)
    {
        return await HandlePostStatistic(StatisticType.VIEW_POST_DETAIL, 1, postId);
    }

    public async Task<bool> SaveBookingStatistic(int postId)
    {
        return await HandlePostStatistic(StatisticType.BOOKING, 1, postId);
    }

    public async Task<bool> SaveGuestMetMotelStatistic(int postId)
    {
        return await HandlePostStatistic(StatisticType.GUEST_MET_MOTEL, 1, postId);
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
    public async Task<List<UserStatisticGroupDTO>> GetUserStatisticWithParams(UserStatisticParams statisticParams)
    {
        var statistics = await _statisticRepo.GetUserStatisticWithParams(statisticParams);
        var statisticGroup = statistics.GroupBy(s => s.CreatedAt.Date);
        var listStatistic = new List<UserStatisticGroupDTO>();
        for (DateTime date = statisticParams.FromDate; date <= statisticParams.ToDate; date = date.AddDays(1))
        {
            var statistic = statisticGroup.FirstOrDefault(s => s.Key.Date == date.Date);
            if (statistic != null)
            {
                listStatistic.Add(new UserStatisticGroupDTO()
                {
                    StatisticDate = date.ToString(DATETIME_FORMAT),
                    Users = statistic.Select(u => _mapper.Map<UserStatisticDTO>(u)),
                    StatisticValue = statistic.Sum(s => s.Value)
                });
            }
            else
            {
                listStatistic.Add(new UserStatisticGroupDTO()
                {
                    StatisticDate = date.ToString(DATETIME_FORMAT),
                    Users = Enumerable.Empty<UserStatisticDTO>(),
                    StatisticValue = 0
                });
            }
        }
        return listStatistic;
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