using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Premiumstress.Core.Infrastructure;

namespace Premiumstress.Core
{
    /// <summary>
    ///     Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            Guard.ArgumentNotNull(source, "source");

            if (pageIndex == 0 && pageSize == int.MaxValue)
            {
                // avoid unnecessary SQL
                Init(source, pageIndex, pageSize, source.Count());
            }
            else
            {
                if (source.Provider is IDbAsyncQueryProvider)
                {
                    // the Lambda overloads for Skip() and Take() let EF use cached query plans, thus slightly increasing performance.
                    Init(source.Skip((pageIndex) * pageSize).Take(pageSize), pageIndex, pageSize, source.Count());
                }
                else
                {
                    Init(source.Skip((pageIndex) * pageSize).Take(pageSize), pageIndex, pageSize, source.Count());
                }
            }
        }

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            Guard.ArgumentNotNull(source, "source");

            Init(source.Skip(pageIndex*pageSize).Take(pageSize), pageIndex, pageSize, source.Count);
        }

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            Guard.ArgumentNotNull(source, "source");
            Init(source, pageIndex, pageSize, totalCount);
        }

        private void Init(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
           // Guard.PagingArgsValid(pageIndex, pageSize, "pageIndex", "pageSize");

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;

            AddRange(source);
        }

        #region IPageable Members

        // codehint: sm-add/edit

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber
        {
            get { return PageIndex + 1; }
            set { PageIndex = value - 1; }
        }

        public int TotalPages
        {
            get
            {
                var total = TotalCount/PageSize;

                if (TotalCount%PageSize > 0)
                    total++;

                return total;
            }
        }

        public bool HasPreviousPage
        {
            get { return PageIndex > 0; }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < (TotalPages - 1)); }
        }

        public int FirstItemIndex
        {
            get { return (PageIndex*PageSize) + 1; }
        }

        public int LastItemIndex
        {
            get { return Math.Min(TotalCount, ((PageIndex*PageSize) + PageSize)); }
        }

        public bool IsFirstPage
        {
            get { return (PageIndex <= 0); }
        }

        public bool IsLastPage
        {
            get { return (PageIndex >= (TotalPages - 1)); }
        }

        #endregion
    }
}