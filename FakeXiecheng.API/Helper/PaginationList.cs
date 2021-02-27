using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Helper
{
    public class PaginationList<T> : List<T>
    {
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public PaginationList(int totalCount, int currentPage, int pageSize, List<T> items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }
        public async static Task<PaginationList<T>> CreateAsync(int currentPage, int pageSize, IQueryable<T> result)
        {
            var totalCount = await result.CountAsync();
            var skip = (currentPage - 1) * pageSize;
            result = result.Skip(skip).Take(pageSize);
            
            var items = await result.ToListAsync();
            return new PaginationList<T>(totalCount, currentPage, pageSize, items);
        }
    }
}
