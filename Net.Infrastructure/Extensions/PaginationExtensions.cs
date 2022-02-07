using System;
using System.Collections.Generic;
using System.Linq;
using Net.Infrastructure.BaseTypes.Models;

namespace Net.Infrastructure.Extensions
{
    public static class PaginationExtensions
    {
        public static PageInfo<T> ToPagination<T>(this IEnumerable<T> source, int pageNumber = 1, int pageSize = 1)
        {
            var list = source.ToList();

            var pageInfo = new PageInfo<T>();

            pageInfo.PageNumber = pageNumber;
            pageInfo.PageSize = pageSize;
            pageInfo.TotalElements = list.Count;
            pageInfo.TotalPages = (int) Math.Ceiling(pageInfo.TotalElements / (double) pageSize);
            pageInfo.HasPreviousPage = pageNumber > 1;
            pageInfo.HasNextPage = pageNumber < pageInfo.TotalPages;
            pageInfo.Results = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return pageInfo;
        }

        public static PageInfo<T> ToPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
        {
            var pageInfo = new PageInfo<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalElements = totalCount
            };

            pageInfo.TotalPages = (int) Math.Ceiling(pageInfo.TotalElements / (double) pageSize);
            pageInfo.HasPreviousPage = pageNumber > 1;
            pageInfo.HasNextPage = pageNumber < pageInfo.TotalPages;
            pageInfo.Results = source.ToList();

            return pageInfo;
        }
    }
}