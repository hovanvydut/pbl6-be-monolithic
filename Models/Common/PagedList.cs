namespace Monolithic.Models.Common;

public class PagedList<T> : List<T>
{
    public IList<T> Records { get; set; }

    public int TotalRecords { get; set; }

    public PagedList() { }

    public PagedList(IList<T> items, int count)
    {
        Records = items;
        TotalRecords = count;
    }
}