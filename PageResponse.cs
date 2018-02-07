using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessWiekonek
{
  public class PageResponse<T>
  {
    public T[] Data;
    public PageMetadata Metadata;
  }

  public class PageMetadata
  {
    public const int DefaultPageSize = 10;

    public int Page;
    public int Limit = DefaultPageSize;
    public int Count;
    public int Total;

  }
}
