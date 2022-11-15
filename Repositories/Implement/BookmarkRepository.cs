using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Extensions;

namespace Monolithic.Repositories.Implement;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly DataContext _db;

    public BookmarkRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<PagedList<BookmarkEntity>> GetBookmarks(int guestId, BookmarkParams bookmarkParams)
    {
        var bookmarks = _db.Bookmarks.Include(b => b.Post)
                            .ThenInclude(p => p.Category)
                            .Include(p => p.Post.AddressWard.AddressDistrict.AddressProvince)
                            .OrderByDescending(c => c.CreatedAt)
                            .Where(p => p.GuestId == guestId)
                            .Where(p => p.Post.DeletedAt == null);

        if (!String.IsNullOrEmpty(bookmarkParams.SearchValue))
        {
            var searchValue = bookmarkParams.SearchValue.ToLower();
            bookmarks = bookmarks.Where(
                p => p.Post.Title.ToLower().Contains(searchValue) ||
                     p.Post.Description.ToLower().Contains(searchValue) ||
                     p.Post.Address.ToLower().Contains(searchValue) ||
                     p.Post.AddressWard.Name.ToLower().Contains(searchValue) ||
                     p.Post.AddressWard.AddressDistrict.Name.ToLower().Contains(searchValue) ||
                     p.Post.AddressWard.AddressDistrict.AddressProvince.Name.ToLower().Contains(searchValue) ||
                     p.Post.Category.Name.ToLower().Contains(searchValue)
            );
        }
        return await bookmarks.ToPagedList(bookmarkParams.PageNumber, bookmarkParams.PageSize);
    }

    public async Task<bool> CreateBookmark(BookmarkEntity bookmarkEntity)
    {
        await _db.Bookmarks.AddAsync(bookmarkEntity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveBookmark(int guestId, int postId)
    {
        var bookmarkDB = await _db.Bookmarks
                        .FirstOrDefaultAsync(c => c.PostId == postId && c.GuestId == guestId);
        if (bookmarkDB == null) return false;
        _db.Entry(bookmarkDB).State = EntityState.Detached;
        _db.Bookmarks.Remove(bookmarkDB);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<List<int>> GetBookmarkedPostIds(int guestId)
    {
        return await _db.Bookmarks.Where(c => c.GuestId == guestId)
                            .Select(c => c.PostId).ToListAsync();
    }
    
    public async Task<bool> IsExistsPostBookmarked(int guestId, int postId)
    {
        return await _db.Bookmarks.AnyAsync(c => c.GuestId == guestId && c.PostId == postId);
    }
}