using System;
using System.Collections.Generic;
using System.Linq; 

namespace Varghese_Demo.API.Pagination
{
    /// <summary>
    /// Pagination
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// CurrentPage
        /// </summary>
        public int CurrentPage { get; private set; }
        /// <summary>
        /// TotalPages
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// TotalCount
        /// </summary>
        public int TotalCount { get; private set; }
        /// <summary>
        /// HasPrevious
        /// </summary>
        public bool HasPrevious => (CurrentPage > 1);
        /// <summary>
        /// HasNext
        /// </summary>
        public bool HasNext => (CurrentPage < TotalPages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="count"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
