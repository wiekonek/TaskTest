using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessWiekonek
{
  public static class PagingExtensions
  {


    //used by LINQ to SQL
    public static IQueryable<TSource> Page<TSource>(
      this IQueryable<TSource> source,
      int page,
      int pageSize = PageMetadata.DefaultPageSize)
    {
      return source.Skip((page - 1) * pageSize).Take(pageSize);
    }

    //used by LINQ
    public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source,
      int page,
      int pageSize = PageMetadata.DefaultPageSize)
    {
      return source.Skip((page - 1) * pageSize).Take(pageSize);
    }

  }
}
