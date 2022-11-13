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

    public async Task<List<StatisticEntity>> GetWithParams(StatisticParams statisticParams)
    {
        var statistics = _db.Statistics.Where(s =>
                                s.Key == statisticParams.Key &&
                                statisticParams.FromDate <= s.CreatedAt &&
                                s.CreatedAt <= statisticParams.ToDate);
        if (!String.IsNullOrEmpty(statisticParams.UserAccountIds))
        {
            var userAccountIds = statisticParams.UserAccountIds.Split(",").Select(c => Convert.ToInt32(c));
            statistics = statistics.Where(s => userAccountIds.Contains(s.UserAccountId));
        }
        return await statistics.OrderBy(s => s.CreatedAt).ToListAsync();
    }

    public async Task<StatisticEntity> GetStatisticInNow(string key, int userAccountId)
    {
        StatisticEntity statisticEntity = await _db.Statistics.FirstOrDefaultAsync(c =>
                                                        c.Key == key &&
                                                        c.UserAccountId == userAccountId &&
                                                        c.CreatedAt.Date == DateTime.Now.Date);
        if (statisticEntity == null) return null;
        _db.Entry(statisticEntity).State = EntityState.Detached;
        return statisticEntity;
    }

    public async Task<bool> Create(StatisticEntity statisticEntity)
    {
        await _db.Statistics.AddAsync(statisticEntity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(StatisticEntity statisticEntity)
    {
        _db.Statistics.Update(statisticEntity);
        return await _db.SaveChangesAsync() > 0;
    }
}