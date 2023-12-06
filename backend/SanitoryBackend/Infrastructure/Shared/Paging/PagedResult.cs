using System;
using System.Linq.Expressions;
using Common.Helper;
using Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Paging
{
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }

        public PagedResult(
            List<T> items,
            int totalItems,
            int pageIndex,
            int pageSize
        )
        {
            this.CurrentPage = pageIndex;
            this.PageSize = pageSize;
            this.RowCount = totalItems;
            this.Results = items;
        }

        public static async Task<PagedResult<T>>
        ToPagedList(
            IQueryable<T> source,
            int pageNumber,
            int pageSize,
            string orderBy,
            int? direction,
            string orderBy2ndColumn,
            int? direction2ndColumn = -1
        )
        {
            var count = await source.CountAsync();
            if (direction > 0)
            {

                if (!String.IsNullOrEmpty(orderBy2ndColumn))
                {
                    if (direction2ndColumn > 0)
                    {
                        source = source.OrderBy(LinqUltilities.ToLambda<T>(orderBy)).ThenBy(LinqUltilities.ToLambda<T>(orderBy2ndColumn));
                    }
                    else
                    {
                        source = source.OrderBy(LinqUltilities.ToLambda<T>(orderBy)).ThenByDescending(LinqUltilities.ToLambda<T>(orderBy2ndColumn));
                    }
                }
                else
                {
                    // asc
                    source = source.OrderBy(LinqUltilities.ToLambda<T>(orderBy));
                }

            }
            else
            {
                if (!String.IsNullOrEmpty(orderBy2ndColumn))
                {
                    if (direction2ndColumn > 0)
                    {
                        source = source.OrderByDescending(LinqUltilities.ToLambda<T>(orderBy)).ThenBy(LinqUltilities.ToLambda<T>(orderBy2ndColumn));
                    }
                    else
                    {
                        source = source.OrderByDescending(LinqUltilities.ToLambda<T>(orderBy)).ThenByDescending(LinqUltilities.ToLambda<T>(orderBy2ndColumn));
                    }
                }
                else
                {
                    // desc
                    source =
                        source
                                .OrderByDescending(LinqUltilities.ToLambda<T>(orderBy));
                }
            }
            var items = new List<T>();
            if (pageNumber == 0 && pageSize == 0)
            {
                items =
                               await source
                                   .ToListAsync();
            }
            else
            {
                items =
                               await source
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();
            }
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
