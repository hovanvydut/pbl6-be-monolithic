using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class StatisticService : IStatisticService
{
    private const string DATETIME_FORMAT = "dd/MM/yyyy";
    private const int TOP_STATISTIC = 3;

    private readonly IStatisticRepository _statisticRepo;
    private readonly IMapper _mapper;
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
                    Posts = HandleTopPostStatistic(statistic),
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

    private IEnumerable<PostStatisticDTO> HandleTopPostStatistic(IGrouping<DateTime, PostStatisticEntity> statistic)
    {
        if (statistic.Count() > TOP_STATISTIC)
        {
            var topStatistic = statistic.Take(TOP_STATISTIC).ToList();
            var bottomStatistic = statistic.Skip(TOP_STATISTIC);
            topStatistic.Add(new PostStatisticEntity()
            {
                Value = bottomStatistic.Sum(s => s.Value),
                PostId = 0,
                Post = new PostEntity()
                {
                    Title = "Khác",
                    Slug = "",
                    DeletedAt = null,
                }
            });
            return topStatistic.Select(p => _mapper.Map<PostStatisticDTO>(p));
        }
        else
        {
            return statistic.Select(p => _mapper.Map<PostStatisticDTO>(p));
        }
    }

    public async Task<PagedList<PostStatisticDTO>> GetPostStatisticInDate(int hostId, PostStatisticInDateParams statisticParams)
    {
        PagedList<PostStatisticEntity> statisticEntityList = await _statisticRepo.GetPostStatisticInDate(hostId, statisticParams);
        List<PostStatisticDTO> statisticDTOList = statisticEntityList.Records.Select(b => _mapper.Map<PostStatisticDTO>(b)).ToList();
        return new PagedList<PostStatisticDTO>(statisticDTOList, statisticEntityList.TotalRecords);
    }

    public async Task<bool> SaveBookmarkStatistic(int postId)
    {
        return await HandleSavePostStatistic(StatisticType.BOOKMARK, 1, postId);
    }

    public async Task<bool> SaveViewPostDetailStatistic(int postId)
    {
        return await HandleSavePostStatistic(StatisticType.VIEW_POST_DETAIL, 1, postId);
    }

    public async Task<bool> SaveBookingStatistic(int postId)
    {
        return await HandleSavePostStatistic(StatisticType.BOOKING, 1, postId);
    }

    public async Task<bool> SaveGuestMetMotelStatistic(int postId)
    {
        return await HandleSavePostStatistic(StatisticType.GUEST_MET_MOTEL, 1, postId);
    }

    private async Task<bool> HandleSavePostStatistic(string key, double value, int postId)
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
                    Users = HandleTopUserStatistic(statistic),
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

    private IEnumerable<UserStatisticDTO> HandleTopUserStatistic(IGrouping<DateTime, UserStatisticEntity> statistic)
    {
        if (statistic.Count() > TOP_STATISTIC)
        {
            var topStatistic = statistic.Take(TOP_STATISTIC).ToList();
            var bottomStatistic = statistic.Skip(TOP_STATISTIC);
            topStatistic.Add(new UserStatisticEntity()
            {
                Value = bottomStatistic.Sum(s => s.Value),
                UserId = 0,
                UserAccount = new UserAccountEntity()
                {
                    Email = "Khác",
                }
            });
            return topStatistic.Select(p => _mapper.Map<UserStatisticDTO>(p));
        }
        else
        {
            return statistic.Select(p => _mapper.Map<UserStatisticDTO>(p));
        }
    }

    private async Task<bool> HandleSaveUserStatistic(string key, double value, int userId)
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

    public async Task<PagedList<UserStatisticDTO>> GetUserStatisticInDate(UserStatisticInDateParams statisticParams)
    {
        PagedList<UserStatisticEntity> statisticEntityList = await _statisticRepo.GetUserStatisticInDate(statisticParams);
        List<UserStatisticDTO> statisticDTOList = statisticEntityList.Records.Select(b => _mapper.Map<UserStatisticDTO>(b)).ToList();
        return new PagedList<UserStatisticDTO>(statisticDTOList, statisticEntityList.TotalRecords);
    }
    #endregion
}