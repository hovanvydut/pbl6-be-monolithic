using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IStatisticRepository
{
    Task<List<StatisticEntity>> GetWithParams(StatisticParams statisticParams);

    Task<StatisticEntity> GetStatisticInNow(string key, int userAccountId);

    Task<bool> Create(StatisticEntity statisticEntity);

    Task<bool> Update(StatisticEntity statisticEntity);
}