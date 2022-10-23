using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Repositories.Interface;

public interface IBookmarkRepository
{
    Task<PagedList<BookmarkEntity>> GetBookmarks(int guessId, BookmarkParams bookmarkParams);

    Task<bool> CreateBookmark(BookmarkEntity bookmarkEntity);

    Task<bool> RemoveBookmark(int guessId, int bookmarkId);
}