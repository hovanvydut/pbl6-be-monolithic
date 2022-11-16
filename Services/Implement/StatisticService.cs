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
        var statistics = await _statisticRepo.GetPostStatisticWithParams(hostId, statisticParams);
        var statisticGroup = statistics.GroupBy(s => s.CreatedAt.Date);
        var listStatistic = new List<PostStatisticDTO>();
        for (DateTime date = statisticParams.FromDate; date <= statisticParams.ToDate; date = date.AddDays(1))
        {
            var statistic = statisticGroup.FirstOrDefault(s => s.Key.Date == date.Date);
            if (statistic != null)
            {
                listStatistic.Add(new PostStatisticDTO()
                {
                    StatisticDate = date.ToString("dd/MM/yyyy"),
                    Posts = statistic.Select(p => new PostStatisticDetail()
                    {
                        Id = p.PostId,
                        Title = p.Post.Title,
                        Slug = p.Post.Slug,
                        StatisticValue = p.Value
                    }),
                    StatisticValue = statistic.Sum(s => s.Value)
                });
            }
            else
            {
                listStatistic.Add(new PostStatisticDTO()
                {
                    StatisticDate = date.ToString("dd/MM/yyyy"),
                    Posts = Enumerable.Empty<PostStatisticDetail>(),
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
    public async Task<List<UserStatisticDTO>> GetUserStatisticWithParams(UserStatisticParams statisticParams)
    {
        var statistics = await _statisticRepo.GetUserStatisticWithParams(statisticParams);
        var statisticGroup = statistics.GroupBy(s => s.CreatedAt.Date);
        var listStatistic = new List<UserStatisticDTO>();
        for (DateTime date = statisticParams.FromDate; date <= statisticParams.ToDate; date = date.AddDays(1))
        {
            var statistic = statisticGroup.FirstOrDefault(s => s.Key.Date == date.Date);
            if (statistic != null)
            {
                listStatistic.Add(new UserStatisticDTO()
                {
                    StatisticDate = date.ToString("dd/MM/yyyy"),
                    Users = statistic.Select(u => new UserStatisticDetail()
                    {
                        Id = u.UserId,
                        Email = u.UserAccount.Email,
                        StatisticValue = u.Value
                    }),
                    StatisticValue = statistic.Sum(s => s.Value)
                });
            }
            else
            {
                listStatistic.Add(new UserStatisticDTO()
                {
                    StatisticDate = date.ToString("dd/MM/yyyy"),
                    Users = Enumerable.Empty<UserStatisticDetail>(),
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