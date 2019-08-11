using System.Collections.Generic;

namespace GraphQl.Server.Annotations
{
    public static class PageExtensions
    {
        public static Page<T> AsPage<T>(
            this IEnumerable<T> items,
            int pageIndex,
            int pageSize,
            int totalCount,
            int totalPages,
            int indexFrom = 0)
        {
            return new Page<T>(new PageInfo(pageIndex, pageSize, totalCount, totalPages, indexFrom), items);
        }
    }

    public class Page<T>
    {
        public PageInfo PageInfo { get; }

        public IEnumerable<T> Items { get; }

        public Page(PageInfo pageInfo, IEnumerable<T> items)
        {
            PageInfo = pageInfo;
            Items = items;
        }
    }

    public class PageInfo
    {
        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public int TotalPages { get; }

        public int IndexFrom { get; }

        public bool HasPreviousPage => PageIndex - IndexFrom > 0;

        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;


        public PageInfo(
            int pageIndex,
            int pageSize,
            int totalCount,
            int totalPages,
            int indexFrom)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
            IndexFrom = indexFrom;
        }
    }
}