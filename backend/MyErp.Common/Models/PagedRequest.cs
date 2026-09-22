namespace MyErp.Common.Models;

/// <summary>
/// Paging, sorting and search options accepted by list endpoints.
/// </summary>
public class PagedRequest
{
    public const int MaxPageSize = 200;

    private int _page = 1;
    private int _pageSize = 25;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value switch
        {
            < 1 => 1,
            > MaxPageSize => MaxPageSize,
            _ => value
        };
    }

    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public bool SortDescending { get; set; }

    public int Skip => (Page - 1) * PageSize;
}
