using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IBookmarkService
{
    Task<PagedList<BookmarkDTO>> GetBookmarks(int guestId, BookmarkParams bookmarkParams);

    Task<bool> CreateBookmark(int guestId, CreateBookmarkDTO createBookmarkDTO);

    Task<bool> RemoveBookmark(int guestId, int postId);
}