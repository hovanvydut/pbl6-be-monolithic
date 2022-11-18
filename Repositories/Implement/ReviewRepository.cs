using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Monolithic.Extensions;
using Monolithic.Models.Common;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;
using Monolithic.Repositories.Interface;

namespace Monolithic.Repositories.Implement;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;
    public ReviewRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ReviewEntity> CreateReview(int userId, int postId, CreateReviewDTO dto)
    {
        ReviewEntity entity = _mapper.Map<ReviewEntity>(dto);
        entity.UserId = userId;
        entity.PostId = postId;
        await _db.Reviews.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<PagedList<ReviewEntity>> GetAllReviewOfPost(int postId, ReviewParams reqParams)
    {
        var entityList = _db.Reviews.Include(r => r.UserAccount.UserProfile)
                            .Where(r => r.PostId == postId)
                            .OrderByDescending(r => r.CreatedAt);

        return await entityList.ToPagedList(reqParams.PageNumber, reqParams.PageSize);
    }
    public async Task<double> GetAverageRatingOfPost(int postId)
    {
        var rating = await _db.Reviews
                .Where(r => r.PostId == postId)
                .Select(r => r.Rating)
                .DefaultIfEmpty()
                .AverageAsync();
        return rating;
    }
}