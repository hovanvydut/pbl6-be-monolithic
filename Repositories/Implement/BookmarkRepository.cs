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

    public async Task<PagedList<BookmarkEntity>> GetBookmarks(int guessId, BookmarkParams bookmarkParams)
    {
        var bookmarks = _db.Bookmarks.Include(b => b.Post)
                            .ThenInclude(p => p.Category)
                            .Include(p => p.Post.AddressWard.AddressDistrict.AddressProvince)
                            .OrderByDescending(c => c.CreatedAt)
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

    public async Task<bool> RemoveBookmark(int guessId, int bookmarkId)
    {
        var bookmarkDB = await _db.Bookmarks
                        .FirstOrDefaultAsync(c => c.Id == bookmarkId && c.GuestId == guessId);
        if (bookmarkDB == null) return false;
        _db.Entry(bookmarkDB).State = EntityState.Detached;
        _db.Bookmarks.Remove(bookmarkDB);
        return await _db.SaveChangesAsync() > 0;
    }
}