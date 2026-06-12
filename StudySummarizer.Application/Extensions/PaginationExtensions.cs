using StudySummarizer.Application.DTOs;

namespace StudySummarizer.Application.Extensions;

public static class PaginationExtensions
{
    public static PaginatedResponse<T> ToPaginatedResponse<T>(
        this IEnumerable<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        return new PaginatedResponse<T>
        {
            Items = items.ToList(),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public static PaginatedResponse<T> GetPaginated<T>(
        this IEnumerable<T> items,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var totalCount = items.Count();
        var paginatedItems = items
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return paginatedItems.ToPaginatedResponse(pageNumber, pageSize, totalCount);
    }
}
