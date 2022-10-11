using Monolithic.Constants;

namespace Monolithic.Models.Common;

public abstract class ReqParam
{
    private int _pageNumber = 1;
    private int _pageSize = 10;
    private string _searchValue = "";

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(value, PaginationConfig.MinPageNumber);
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(value, PaginationConfig.MaxPageSize);
    }

    public string SearchValue
    {
        get => _searchValue;
        set => _searchValue = value;
    }

    public override string ToString()
    {
        return $"page number: {this.PageNumber}, "
                + $"page size: {this.PageSize}, "
                + $"search value: \"{this.SearchValue}\", ";
    }
}