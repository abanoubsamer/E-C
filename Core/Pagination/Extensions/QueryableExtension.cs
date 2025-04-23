using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Pagination.Extensions
{
    public static class QueryableExtension
    {

        public static async Task<PaginationResult<T>> ToPaginationListAsync<T> (
            this  IQueryable<T> source,
            int pageNumber,
            int pageSize
            ) where T : class
        {
            if (source == null)
            {
                throw new Exception("Empty");

            }

          

            //init
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;


            //get count source
            var count = await source.AsNoTracking().CountAsync();

            // if data empty
               if (count == 0) return PaginationResult<T>.Success(new List<T>(), count, pageSize, pageSize);

            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

             return PaginationResult<T>.Success(items, count, pageNumber, pageSize);

        }

    }
}
