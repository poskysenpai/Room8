using Microsoft.EntityFrameworkCore;
using Room8.Core.Dtos;

namespace Room8.Core.Utilities
{
    public static class Pagination
    {
        public static PaginatorDto<IEnumerable<TSource>> Paginate<TSource>(this IEnumerable<TSource> queryable,
            PaginationFilter? paginationFilter = null)
            where TSource : class
        {
            var count = queryable.Count();

            paginationFilter ??= new PaginationFilter();

            var pageResult = new PaginatorDto<IEnumerable<TSource>>
            {
                PageSize = paginationFilter.PageSize,
                CurrentPage = paginationFilter.PageNumber
            };

            pageResult.NumberOfPages = count % pageResult.PageSize != 0
                ? count / pageResult.PageSize + 1
                : count / pageResult.PageSize;

            pageResult.PageItems = queryable.Skip((pageResult.CurrentPage - 1) * pageResult.PageSize)
                .Take(pageResult.PageSize);

            return pageResult;
        }
    }
}
