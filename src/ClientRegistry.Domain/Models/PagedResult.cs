﻿namespace ClientRegistry.Domain.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }


}
