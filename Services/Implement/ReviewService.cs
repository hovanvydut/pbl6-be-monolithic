using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepo;
    private readonly IBookingService _bookingService;
    private readonly DataContext _db;
    private readonly IMapper _mapper;
    private readonly IMediaRepository _mediaRepo;
    private readonly INotificationService _notyService;
    private readonly IAIService _aiService;

    public ReviewService(IReviewRepository reviewRepo,
                        IBookingService bookingService, DataContext db, IMapper mapper,
                        IMediaRepository mediaRepo, INotificationService notyService,
                        IAIService aIService)
    {
        _reviewRepo = reviewRepo;
        _bookingService = bookingService;
        _db = db;
        _mapper = mapper;
        _mediaRepo = mediaRepo;
        _notyService = notyService;
        _aiService = aIService;
    }

    public async Task<bool> CheckCanReview(int userId, int postId)
    {
        return await _bookingService.CheckMetBooking(userId, postId); ;
    }

    public async Task CreateReview(int userId, int postId, CreateReviewDTO dto)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                bool hasMet = await _bookingService.CheckMetBooking(userId, postId);
                if (!hasMet)
                {
                    throw new BaseException(HttpCode.BAD_REQUEST, "You must be visited this inn before reviewing");
                }

                if (dto.Rating < 1 || dto.Rating > 4)
                {
                    throw new BaseException(HttpCode.BAD_REQUEST, "Rating is invalid");
                }

                ReviewEntity savedEntity = await _reviewRepo.CreateReview(userId, postId, dto);

                // save media
                List<MediaEntity> mediaEntityList = dto.Medias
                            .Select(m => _mapper.Map<MediaEntity>(m)).ToList();
                foreach (var media in mediaEntityList)
                {
                    media.EntityType = EntityType.REVIEW;
                    media.EntityId = savedEntity.Id;
                }
                await _mediaRepo.Create(mediaEntityList);

                transaction.Commit();

                // Notification
                await _notyService.CreateReviewOnPostNoty(new ReviewNotificationDTO()
                {
                    OriginUserId = userId,
                    PostId = postId,
                    ReviewId = savedEntity.Id,
                    ReviewContent = savedEntity.Content,
                    ReviewRating = savedEntity.Rating,
                });

                // AI analyse review's sentiment
                await _aiService.analyseReview(savedEntity.Id);
                Console.WriteLine("Created review Id = " + savedEntity.Id);
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task<PagedList<ReviewDTO>> GetAllReviewOfPost(int postId, ReviewParams reqParams)
    {
        PagedList<ReviewEntity> reviewEntityList = await _reviewRepo.GetAllReviewOfPost(postId, reqParams);
        List<ReviewDTO> reviewDTOList = reviewEntityList.Records.Select(r => _mapper.Map<ReviewDTO>(r)).ToList();

        foreach (var e in reviewEntityList.Records)
        {
            Console.WriteLine(e.UserAccount.UserProfile.DisplayName);
        }
        // attach media on reviewDTOList;
        foreach (var reviewDTO in reviewDTOList)
        {
            List<MediaEntity> mediaEntityList = await _mediaRepo.GetAllMediaOfReview(reviewDTO.Id);
            reviewDTO.Medias = mediaEntityList.Select(m => _mapper.Map<MediaDTO>(m)).ToList();
        }

        return new PagedList<ReviewDTO>(reviewDTOList, reviewEntityList.TotalRecords);
    }

    public async Task<double> GetAverageRatingOfPost(int postId)
    {
        return await _reviewRepo.GetAverageRatingOfPost(postId);
    }
}