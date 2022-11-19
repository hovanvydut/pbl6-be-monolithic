using Microsoft.EntityFrameworkCore.Storage;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class PriorityPostService : IPriorityPostService
{
    private readonly IPaymentHistoryService _paymentHistoryService;
    private readonly IConfigSettingService _configSettingService;
    private readonly IPriorityPostRepository _priorityPostRepo;
    private readonly IStatisticService _statisticService;
    private readonly IPostRepository _postRepo;
    private readonly IUserService _userService;
    private readonly DataContext _db;
    private readonly IMapper _mapper;
    public PriorityPostService(IPaymentHistoryService paymentHistoryService,
                               IConfigSettingService configSettingService,
                               IPriorityPostRepository priorityPostRepo,
                               IStatisticService statisticService,
                               IPostRepository postRepo,
                               IUserService userService,
                               DataContext db,
                               IMapper mapper)
    {
        _paymentHistoryService = paymentHistoryService;
        _configSettingService = configSettingService;
        _priorityPostRepo = priorityPostRepo;
        _statisticService = statisticService;
        _userService = userService;
        _postRepo = postRepo;
        _db = db;
        _mapper = mapper;
    }

    public async Task<bool> CreatePriorityPost(int hostId, PriorityPostCreateDTO priorityCreateDTO)
    {
        DateTime startTime = priorityCreateDTO.StartTime;
        DateTime endTime = startTime.AddDays(priorityCreateDTO.Days);
        if (startTime <= DateTime.Now)
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid start time to uptop post");

        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                // Check post valid for uptop
                var post = await _postRepo.GetPostById(priorityCreateDTO.PostId);
                if (post == null)
                    throw new BaseException(HttpCode.BAD_REQUEST, "This post is not exist or deleted");
                if (post.HostId != hostId)
                    throw new BaseException(HttpCode.BAD_REQUEST, "Cannot uptop other host's post");

                // Check post have already uptop in now
                var priorityPostDB = await _priorityPostRepo.GetByPostId(priorityCreateDTO.PostId);
                if (priorityPostDB != null)
                    throw new BaseException(HttpCode.BAD_REQUEST, "This post have already uptop");

                // check valid time for priority post
                if (!await isValidTime(post, startTime, endTime))
                    throw new BaseException(HttpCode.BAD_REQUEST, "This time range had enough priority post");

                var priorityPostEntity = _mapper.Map<PriorityPostEntity>(priorityCreateDTO);
                priorityPostEntity.StartTime = startTime;
                priorityPostEntity.EndTime = endTime;

                var priorityCreated = await _priorityPostRepo.Create(priorityPostEntity);

                // pay for uptop post
                var uptopPrice = await _configSettingService.GetValueByKey(ConfigSetting.UPTOP_PRICE);
                var uptopPaid = uptopPrice * priorityCreateDTO.Days;
                var payment = await _userService.UserMakePayment(hostId, uptopPaid);
                // Save payment history
                await _paymentHistoryService.PayForUptopPost(hostId, priorityCreateDTO.PostId,
                                                                uptopPaid, priorityCreateDTO.Days);
                // Save uptop statistic
                await _statisticService.SaveNumberOfUptopped(hostId);

                transaction.Commit();
                return payment;
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    private async Task<bool> isValidTime(PostEntity post, DateTime startTime, DateTime endTime)
    {
        var priorityPostParams = new PriorityPostParams()
        {
            AddressWardId = post.AddressWardId,
            StartTime = startTime,
            EndTime = endTime,
        };
        var priorityPostInWard = await _priorityPostRepo.GetListDuplicateTime(priorityPostParams);

        var maxPriorityPost = await _configSettingService.GetValueByKey(ConfigSetting.MAX_PRIORITY_POST);
        if (priorityPostInWard.Count >= maxPriorityPost)
            return false;

        return true;
    }

    public async Task<PriorityPostDTO> GetByPostId(int postId)
    {
        var priority = await _priorityPostRepo.GetByPostId(postId);
        return _mapper.Map<PriorityPostDTO>(priority);
    }

    public async Task<List<PriorityPostDTO>> GetPriorityDuplicateTime(PriorityPostParams priorityPostParams)
    {
        var priorities = await _priorityPostRepo.GetListDuplicateTime(priorityPostParams);
        return priorities.Select(p => _mapper.Map<PriorityPostDTO>(p)).ToList();
    }
}