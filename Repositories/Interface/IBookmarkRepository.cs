using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Repositories.Interface;

public interface IBookmarkRepository
{
    Task<bool> IsExistsPostBookmarked(int guessId, int postId);

    Task<PagedList<BookmarkEntity>> GetBookmarks(int guessId, BookmarkParams bookmarkParams);

    Task<bool> CreateBookmark(BookmarkEntity bookmarkEntity);

    Task<bool> RemoveBookmark(int guessId, int postId);

    Task<List<int>> GetBookmarkedPostIds(int guessId);
}