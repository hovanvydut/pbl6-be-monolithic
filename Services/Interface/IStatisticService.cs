using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IStatisticService
{
    Task<List<StatisticDTO>> GetWithParams(StatisticParams statisticParams);

    Task<bool> HandleStatistic(string key, double value, int userAccountId);
}