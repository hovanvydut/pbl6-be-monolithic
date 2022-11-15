using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class StatisticController : BaseController
{
    private readonly IStatisticService _statisticService;

    public StatisticController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }

    // [HttpGet("personal")]
    // [Authorize]
    // public async Task<BaseResponse<List<StatisticDTO>>> GetStatisticDataPersonal([FromQuery] StatisticParams statisticParams)
    // {
    //     var reqUser = HttpContext.Items["reqUser"] as ReqUser;
    //     statisticParams.UserAccountIds = reqUser.Id.ToString();
    //     var result = await _statisticService.GetWithParams(statisticParams);
    //     return new BaseResponse<List<StatisticDTO>>(result);
    // }

    // [HttpGet("admin")]
    // [Authorize]
    // public async Task<BaseResponse<List<StatisticDTO>>> GetStatisticDataAdmin([FromQuery] StatisticParams statisticParams)
    // {
    //     var result = await _statisticService.GetWithParams(statisticParams);
    //     return new BaseResponse<List<StatisticDTO>>(result);
    // }
}