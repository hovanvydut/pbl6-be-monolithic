using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IBookmarkService
{
    Task<PagedList<BookmarkDTO>> GetBookmarks(int guessId, BookmarkParams bookmarkParams);

    Task<bool> CreateBookmark(int guessId, CreateBookmarkDTO createBookmarkDTO);

    Task<bool> RemoveBookmark(int guessId, int bookmarkId);
}