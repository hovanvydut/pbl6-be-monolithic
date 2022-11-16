using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;

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
        return await statistics.OrderBy(s => s.CreatedAt).ToListAsync();
    }

    public async Task<PostStatisticEntity> GetPostStatisticInNow(string key, int postId)
    {
        PostStatisticEntity postStatisticEntity = await _db.PostStatistics.FirstOrDefaultAsync(c =>
                                                        c.Key == key &&
                                                        c.PostId == postId &&
                                                        c.CreatedAt.Date == DateTime.Now.Date);
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

    public async Task<UserStatisticEntity> GetUserStatisticInNow(string key, int userId)
    {
        UserStatisticEntity userStatisticEntity = await _db.UserStatistics.FirstOrDefaultAsync(c =>
                                                        c.Key == key &&
                                                        c.UserId == userId &&
                                                        c.CreatedAt.Date == DateTime.Now.Date);
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