using System.Collections.Generic;

namespace GraphQl.Server.Annotations
{
    public static class PartialExtensions
    {
        public static Partial<T> AsPartial<T>(
            this IEnumerable<T> items,
            int startIndex,
            int partialSize,
            int totalCount)
        {
            return new Partial<T>(new PartialInfo(startIndex, partialSize, totalCount), items);
        }
    }

    public class Partial<T>
    {
        public PartialInfo PartialInfo { get; }

        public IEnumerable<T> Items { get; }

        public Partial(PartialInfo partialInfo, IEnumerable<T> items)
        {
            PartialInfo = partialInfo;
            Items = items;
        }
    }

    public class PartialInfo
    {
        public int StartIndex { get; }

        public int PartialSize { get; }

        public int TotalCount { get; }

        public bool HasMore => StartIndex + PartialSize < TotalCount;


        public PartialInfo(int startIndex, int partialSize, int totalCount)
        {
            StartIndex = startIndex;
            PartialSize = partialSize;
            TotalCount = totalCount;
        }
    }
}