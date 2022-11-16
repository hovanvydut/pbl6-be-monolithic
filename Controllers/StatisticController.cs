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

    [HttpGet("/api/post-statistic")]
    [Authorize]
    public async Task<BaseResponse<List<PostStatisticGroupDTO>>> GetPostStatisticForHost([FromQuery] PostStatisticParams statisticParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var result = await _statisticService.GetPostStatisticWithParams(reqUser.Id, statisticParams);
        return new BaseResponse<List<PostStatisticGroupDTO>>(result);
    }
}