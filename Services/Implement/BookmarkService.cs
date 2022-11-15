using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using AutoMapper;
using Monolithic.Constants;

namespace Monolithic.Services.Implement;

public class BookmarkService : IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepo;
    private readonly IMediaRepository _mediaRepo;
    private readonly IStatisticService _statisticService;
    private readonly IMapper _mapper;

    public BookmarkService(IBookmarkRepository bookmarkRepo,
                           IMediaRepository mediaRepo,
                           IStatisticService statisticService,
                           IMapper mapper)
    {
        _bookmarkRepo = bookmarkRepo;
        _mediaRepo = mediaRepo;
        _statisticService = statisticService;
        _mapper = mapper;
    }

    public async Task<PagedList<BookmarkDTO>> GetBookmarks(int guessId, BookmarkParams bookmarkParams)
    {
        PagedList<BookmarkEntity> bookmarkEntityList = await _bookmarkRepo.GetBookmarks(guessId, bookmarkParams);
        List<BookmarkDTO> bookmarkDTOList = bookmarkEntityList.Records.Select(b => _mapper.Map<BookmarkDTO>(b)).ToList();
        foreach (var bookmarkDTO in bookmarkDTOList)
        {
            List<MediaEntity> mediaEntityList = await _mediaRepo.GetAllMediaOfPost(bookmarkDTO.Id);
            bookmarkDTO.Medias = mediaEntityList.Select(m => _mapper.Map<MediaDTO>(m)).ToList();
        }
        return new PagedList<BookmarkDTO>(bookmarkDTOList, bookmarkEntityList.TotalRecords);
    }

    public async Task<bool> CreateBookmark(int guessId, CreateBookmarkDTO createBookmarkDTO)
    {
        if (await _bookmarkRepo.IsExistsPostBookmarked(guessId, createBookmarkDTO.PostId))
            throw new BaseException(HttpCode.BAD_REQUEST, "This post have already bookmarked");

        var bookmarkEntity = new BookmarkEntity()
        {
            GuestId = guessId,
            PostId = createBookmarkDTO.PostId
        };
        await _statisticService.SaveBookmarkStatistic(createBookmarkDTO.PostId);
        return await _bookmarkRepo.CreateBookmark(bookmarkEntity);
    }

    public async Task<bool> RemoveBookmark(int guessId, int postId)
    {
        if (!await _bookmarkRepo.IsExistsPostBookmarked(guessId, postId))
            throw new BaseException(HttpCode.BAD_REQUEST, "This post is not bookmarked to remove");
        return await _bookmarkRepo.RemoveBookmark(guessId, postId);
    }
}