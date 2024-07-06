namespace Room8.Core.Dtos;

public class PaginatorDto<T>
{
    public T? PageItems { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int NumberOfPages { get; set; }
}

public class PaginationFilter
{
    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize is > 10 or < 1 ? 10 : pageSize;
    }

    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
