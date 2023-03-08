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

    // POST
    [HttpGet("/api/post-statistic")]
    [Authorize(Roles = PostStatisticPermission.ViewInDateRange)]
    public async Task<BaseResponse<List<PostStatisticGroupDTO>>> GetPostStatisticForHost([FromQuery] PostStatisticParams statisticParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var result = await _statisticService.GetPostStatisticWithParams(reqUser.Id, statisticParams);
        return new BaseResponse<List<PostStatisticGroupDTO>>(result);
    }

    [HttpGet("/api/post-statistic/total-value")]
    [Authorize(Roles = PostStatisticPermission.ViewInDateRange)]
    public async Task<BaseResponse<double>> GetTotalPostStatisticValue([FromQuery] PostStatisticParams statisticParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var result = await _statisticService.GetTotalPostStatisticValue(reqUser.Id, statisticParams);
        return new BaseResponse<double>(result);
    }

    [HttpGet("/api/post-statistic/top")]
    [Authorize(Roles = PostStatisticPermission.ViewTopInDate)]
    public async Task<BaseResponse<List<PostStatisticDTO>>> GetTopPostStatistic([FromQuery] PostTopStatisticParams statisticParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var result = await _statisticService.GetTopPostStatistic(reqUser.Id, statisticParams);
        return new BaseResponse<List<PostStatisticDTO>>(result);
    }

    [HttpGet("/api/post-statistic/detail")]
    [Authorize(Roles = PostStatisticPermission.ViewDetailInDate)]
    public async Task<BaseResponse<PagedList<PostStatisticDTO>>> GetPostStatisticDetail([FromQuery] PostStatisticInDateParams statisticParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var result = await _statisticService.GetPostStatisticInDate(reqUser.Id, statisticParams);
        return new BaseResponse<PagedList<PostStatisticDTO>>(result);
    }

    // USER
    [HttpGet("/api/user-statistic")]
    [Authorize(Roles = UserStatisticPermission.ViewInDateRange)]
    public async Task<BaseResponse<List<UserStatisticGroupDTO>>> GetUserStatisticForAdmin([FromQuery] UserStatisticParams statisticParams)
    {
        var result = await _statisticService.GetUserStatisticWithParams(statisticParams);
        return new BaseResponse<List<UserStatisticGroupDTO>>(result);
    }

    [HttpGet("/api/user-statistic/total-value")]
    [Authorize(Roles = UserStatisticPermission.ViewInDateRange)]
    public async Task<BaseResponse<double>> GetTotalUserStatisticValue([FromQuery] UserStatisticParams statisticParams)
    {
        var result = await _statisticService.GetTotalUserStatisticValue(statisticParams);
        return new BaseResponse<double>(result);
    }

    [HttpGet("/api/user-statistic/top")]
    [Authorize(Roles = UserStatisticPermission.ViewTopInDate)]
    public async Task<BaseResponse<List<UserStatisticDTO>>> GetTopUserStatistic([FromQuery] UserTopStatisticParams statisticParams)
    {
        var result = await _statisticService.GetTopUserStatistic(statisticParams);
        return new BaseResponse<List<UserStatisticDTO>>(result);
    }

    [HttpGet("/api/user-statistic/detail")]
    [Authorize(Roles = UserStatisticPermission.ViewDetailInDate)]
    public async Task<BaseResponse<PagedList<UserStatisticDTO>>> GetUserStatisticDetail([FromQuery] UserStatisticInDateParams statisticParams)
    {
        var result = await _statisticService.GetUserStatisticInDate(statisticParams);
        return new BaseResponse<PagedList<UserStatisticDTO>>(result);
    }
}