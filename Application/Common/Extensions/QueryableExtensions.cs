using System;
using System.Collections.Generic;
using System.Linq;

namespace QAForum.Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static (IList<T> items, int page, int pages) PaginateCollection<T>
            (this IQueryable<T> items, int pageSize, int page)
        {
            var itemsCount = items.Count();
            var pages = (int) Math.Ceiling((double) itemsCount / pageSize);
            if (itemsCount == 0)
            {
                return (items.ToList(),
                    page,
                    pages);
            }

            if (page > pages)
            {
                page = pages;
            }

            items = items.Skip((page - 1) * pageSize).Take(pageSize);
            return (items.ToList(),
                page,
                pages);
        }
    }
}