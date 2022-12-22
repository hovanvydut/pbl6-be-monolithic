using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Extensions;

namespace Monolithic.Repositories.Implement;

public class StatisticRepository : IStatisticRepository
{
    private readonly DataContext _db;
    public StatisticRepository(DataContext db)
    {
        _db = db;
    }

    #region PostStatistic
    public async Task<List<PostStatisticEntity>> GetPostStatisticWithParams(int hostId, PostStatisticParams statisticParams)
    {
        var statistics = _db.PostStatistics.Include(s => s.Post).Where(s =>
                                                s.Post.HostId == hostId &&
                                                s.Key == statisticParams.Key &&
                                                statisticParams.FromDate.Date <= s.CreatedAt.Date &&
                                                s.CreatedAt.Date <= statisticParams.ToDate.Date);
        if (!statisticParams.IncludeDeletedPost)
        {
            statistics = statistics.Where(s => s.Post.DeletedAt == null);
        }
        if (!String.IsNullOrEmpty(statisticParams.PostIds))
        {
            var postIds = statisticParams.PostIds.Split(",").Select(c => Convert.ToInt32(c));
            statistics = statistics.Where(s => postIds.Contains(s.PostId));
        }
        return await statistics.OrderByDescending(s => s.Value).ToListAsync();
    }

    public async Task<List<PostStatisticEntity>> GetTopPostStatistic(int hostId, PostTopStatisticParams statisticParams)
    {
        var statistic = _db.PostStatistics.Include(s => s.Post).Where(s =>
                                                s.Post.HostId == hostId &&
                                                s.Key == statisticParams.Key &&
                                                s.CreatedAt.Date == statisticParams.Date.Date);
        if (!statisticParams.IncludeDeletedPost)
        {
            statistic = statistic.Where(s => s.Post.DeletedAt == null);
        }
        statistic = statistic.OrderByDescending(s => s.Value);

        var statisticCount = statistic.Count();
        var topStatistic = await statistic.Take(statisticParams.Top).ToListAsync();

        /* Giả sử  statistic có 5 bản ghi, mình lấy ra top 4 cho topStatistic thì cái other chính là cái cuối cùng
        vì thế mình có thể trả ra statistic luôn, còn nếu lấy 3 trở xuống thì mới tính cái post Other, lớn hơn 
        hoặc bằng 5 thì cái topStatistic đã chứa toàn bộ rồi nên bỏ qua
        */
        var otherStatisticCount = statisticCount - topStatistic.Count;
        if (otherStatisticCount == 1)
        {
            return await statistic.ToListAsync();
        }
        if (otherStatisticCount > 1)
        {
            var bottomStatistic = statistic.Skip(topStatistic.Count).Sum(s => s.Value);
            topStatistic.Add(new PostStatisticEntity()
            {
                Value = bottomStatistic,
                PostId = 0,
                Post = new PostEntity()
                {
                    Title = "Khác",
                    Slug = "",
                    DeletedAt = null,
                }
            });
        }
        return topStatistic;
    }

    public async Task<PagedList<PostStatisticEntity>> GetPostStatisticInDate(int hostId, PostStatisticInDateParams statisticParams)
    {
        var statistic = _db.PostStatistics.Include(s => s.Post).Where(s =>
                                                s.Post.HostId == hostId &&
                                                s.Key == statisticParams.Key &&
                                                s.CreatedAt.Date == statisticParams.Date.Date);
        if (!statisticParams.IncludeDeletedPost)
        {
            statistic = statistic.Where(s => s.Post.DeletedAt == null);
        }
        if (!String.IsNullOrEmpty(statisticParams.SearchValue))
        {
            var searchValue = statisticParams.SearchValue.ToLower();
            statistic = statistic.Where(
                p => p.Post.Title.ToLower().Contains(searchValue) ||
                     p.Post.Slug.ToLower().Contains(searchValue)
            );
        }
        return await statistic.OrderByDescending(s => s.Value).ToPagedList(statisticParams.PageNumber, statisticParams.PageSize);
    }

    public async Task<PostStatisticEntity> GetPostStatisticInNow(string key, int postId)
    {
        var now = DateTime.Now.GetLocalTime().Date;
        PostStatisticEntity postStatisticEntity = await _db.PostStatistics.FirstOrDefaultAsync(c =>
                                                        c.Key == key &&
                                                        c.PostId == postId &&
                                                        c.CreatedAt.Date == now);
        if (postStatisticEntity == null) return null;
        _db.Entry(postStatisticEntity).State = EntityState.Detached;
        return postStatisticEntity;
    }

    public async Task<bool> CreatePostStatistic(PostStatisticEntity postStatisticEntity)
    {
        await _db.PostStatistics.AddAsync(postStatisticEntity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdatePostStatistic(PostStatisticEntity postStatisticEntity)
    {
        _db.PostStatistics.Update(postStatisticEntity);
        return await _db.SaveChangesAsync() > 0;
    }

    #endregion

    #region UserStatistic
    public async Task<List<UserStatisticEntity>> GetUserStatisticWithParams(UserStatisticParams statisticParams)
    {
        var statistics = _db.UserStatistics.Include(s => s.UserAccount).Where(s =>
                                s.Key == statisticParams.Key &&
                                statisticParams.FromDate.Date <= s.CreatedAt.Date &&
                                s.CreatedAt.Date <= statisticParams.ToDate.Date);
        if (!String.IsNullOrEmpty(statisticParams.UserIds))
        {
            var userIds = statisticParams.UserIds.Split(",").Select(c => Convert.ToInt32(c));
            statistics = statistics.Where(s => userIds.Contains(s.UserId));
        }
        return await statistics.OrderBy(s => s.CreatedAt).ToListAsync();
    }

    public async Task<List<UserStatisticEntity>> GetTopUserStatistic(UserTopStatisticParams statisticParams)
    {
        var statistic = _db.UserStatistics.Include(s => s.UserAccount).Where(s =>
                                                s.Key == statisticParams.Key &&
                                                s.CreatedAt.Date == statisticParams.Date.Date);
        statistic = statistic.OrderByDescending(s => s.Value);

        var statisticCount = statistic.Count();
        var topStatistic = await statistic.Take(statisticParams.Top).ToListAsync();

        var otherStatisticCount = statisticCount - topStatistic.Count;
        if (otherStatisticCount == 1)
        {
            return await statistic.ToListAsync();
        }
        if (otherStatisticCount > 1)
        {
            var bottomStatistic = statistic.Skip(topStatistic.Count).Sum(s => s.Value);
            topStatistic.Add(new UserStatisticEntity()
            {
                Value = bottomStatistic,
                UserId = 0,
                UserAccount = new UserAccountEntity()
                {
                    Email = "Khác",
                }
            });
        }
        return topStatistic;
    }

    public async Task<PagedList<UserStatisticEntity>> GetUserStatisticInDate(UserStatisticInDateParams statisticParams)
    {
        var statistic = _db.UserStatistics.Include(s => s.UserAccount).Where(s =>
                                                s.Key == statisticParams.Key &&
                                                s.CreatedAt.Date == statisticParams.Date.Date);
        if (!String.IsNullOrEmpty(statisticParams.SearchValue))
        {
            var searchValue = statisticParams.SearchValue.ToLower();
            statistic = statistic.Where(
                p => p.UserAccount.Email.ToLower().Contains(searchValue)
            );
        }
        return await statistic.OrderByDescending(s => s.Value).ToPagedList(statisticParams.PageNumber, statisticParams.PageSize);
    }

    public async Task<UserStatisticEntity> GetUserStatisticInNow(string key, int userId)
    {
        var now = DateTime.Now.GetLocalTime().Date;
        UserStatisticEntity userStatisticEntity = await _db.UserStatistics.FirstOrDefaultAsync(c =>
                                                        c.Key == key &&
                                                        c.UserId == userId &&
                                                        c.CreatedAt.Date == now);
        if (userStatisticEntity == null) return null;
        _db.Entry(userStatisticEntity).State = EntityState.Detached;
        return userStatisticEntity;
    }

    public async Task<bool> CreateUserStatistic(UserStatisticEntity userStatisticEntity)
    {
        await _db.UserStatistics.AddAsync(userStatisticEntity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateUserStatistic(UserStatisticEntity userStatisticEntity)
    {
        _db.UserStatistics.Update(userStatisticEntity);
        return await _db.SaveChangesAsync() > 0;
    }
    #endregion
}