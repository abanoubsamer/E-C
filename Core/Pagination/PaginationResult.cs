using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Pagination
{
    public class PaginationResult<T> where T : class
    {
        public List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public object Meta { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPage;
        public List<string> Messages { get; set; } = new();
        public bool Succeeded { get; set; }
        
        
        public PaginationResult(List<T> data)
        {
            Data = data;         
        }

        internal PaginationResult(
            bool succeeded,
            List<T>data = default,
            List<string> msg = null,
            int count = 0,
            int page= 1 ,
            int pagesize= 10
            )
        {
        
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pagesize;
            TotalPage = (int)Math.Ceiling(count / (double)pagesize);
            TotalCount = count;

        }


        public static PaginationResult<T> Success(List<T> Data, int count, int page, int pageszie)
        {
            return new PaginationResult<T>(true, Data, null, count, page, pageszie);
        }


    }
}
