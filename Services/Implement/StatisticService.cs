using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
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

    public async Task<List<StatisticDTO>> GetWithParams(StatisticParams statisticParams)
    {
        var statistics = await _statisticRepo.GetWithParams(statisticParams);
        return statistics.Select(s => _mapper.Map<StatisticDTO>(s)).ToList();
    }

    public async Task<bool> HandleStatistic(string key, double value, int userAccountId)
    {
        var statisticNow = await _statisticRepo.GetStatisticInNow(key, userAccountId);
        if (statisticNow == null)
        {
            var newStatistic = new StatisticEntity()
            {
                Key = key,
                Value = value,
                UserAccountId = userAccountId,
            };
            return await _statisticRepo.Create(newStatistic);
        }
        else
        {
            statisticNow.Value += value;
            return await _statisticRepo.Update(statisticNow);
        }
    }
}