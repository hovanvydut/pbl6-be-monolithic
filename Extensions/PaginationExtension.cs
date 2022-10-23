using Monolithic.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace Monolithic.Extensions;

public static class PaginationExtension
{
    public async static Task<PagedList<T>> ToPagedList<T>(
        this IQueryable<T> source, int pageNumber, int pageSize)
    {
        int count = source.Count();
        List<T> items = await source.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();
        return new PagedList<T>(items, count);
    }
}