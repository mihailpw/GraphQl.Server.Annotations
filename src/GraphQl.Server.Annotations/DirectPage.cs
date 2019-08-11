using System.Collections.Generic;

namespace GraphQl.Server.Annotations
{
    public static class DirectPageExtensions
    {
        public static DirectPage<T> AsPage<T>(
            this IEnumerable<T> items,
            int pageIndex,
            int pageSize,
            int totalCount,
            int totalPages,
            int indexFrom = 0)
        {
            return new DirectPage<T>(items, pageIndex, pageSize, totalCount, totalPages, indexFrom);
        }
    }

    public class DirectPage<T>
    {
        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public int TotalPages { get; }

        public int IndexFrom { get; }

        public IEnumerable<T> Items { get; }

        public bool HasPreviousPage => PageIndex - IndexFrom > 0;

        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;


        public DirectPage(
            IEnumerable<T> items,
            int pageIndex,
            int pageSize,
            int totalCount,
            int totalPages,
            int indexFrom)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
            IndexFrom = indexFrom;
        }
    }
}