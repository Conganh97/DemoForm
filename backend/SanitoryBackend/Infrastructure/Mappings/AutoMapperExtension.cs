using System.Reflection;
using AutoMapper;
using Infrastructure.Shared.Paging;

namespace Infrastructure.Mappings
{
    public static class AutoMapperExtension
    {
        public static IMappingExpression<TSource, TDestination>
            IgnoreAllNonExisting<TSource, TDestination>(
                this IMappingExpression<TSource, TDestination> expression
            )
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties =
                typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }

                if (property.Name == "CreatedDate")
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }

            return expression;
        }

        public static Task<PagedResult<TDestination>>
            PaginatedListAsync<TDestination>(
                this IQueryable<TDestination> queryable,
                int pageIndex,
                int pageSize,
                string orderBy2ndColumn,
                int? direction2ndColumn = -1,
                string orderBy = "LastModifiedDate",
                int? direction = -1
            )
            where TDestination : class
        {
            return PagedResult<TDestination>
                .ToPagedList(queryable,
                    pageIndex,
                    pageSize,
                    string.IsNullOrEmpty(orderBy) ? "LastModifiedDate" : orderBy,
                    direction, orderBy2ndColumn, direction2ndColumn);
        }
    }
}