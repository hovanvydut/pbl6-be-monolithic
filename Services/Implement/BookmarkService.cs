using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class BookmarkService : IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly IMapper _mapper;

    public BookmarkService(IBookmarkRepository bookmarkRepository, IMapper mapper)
    {
        _bookmarkRepository = bookmarkRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<BookmarkDTO>> GetBookmarks(int guessId, BookmarkParams bookmarkParams)
    {
        PagedList<BookmarkEntity> bookmarkEntityList = await _bookmarkRepository.GetBookmarks(guessId,bookmarkParams);
        List<BookmarkDTO> bookmarkDTOList = bookmarkEntityList.Records.Select(b => _mapper.Map<BookmarkDTO>(b)).ToList();
        return new PagedList<BookmarkDTO>(bookmarkDTOList, bookmarkEntityList.TotalRecords);
    }

    public async Task<bool> CreateBookmark(int guessId, CreateBookmarkDTO createBookmarkDTO)
    {
        var bookmarkEntity = new BookmarkEntity()
        {
            GuestId = guessId,
            PostId = createBookmarkDTO.PostId
        };
        return await _bookmarkRepository.CreateBookmark(bookmarkEntity);
    }

    public async Task<bool> RemoveBookmark(int guessId, int bookmarkId)
    {
        return await _bookmarkRepository.RemoveBookmark(guessId, bookmarkId);
    }
}