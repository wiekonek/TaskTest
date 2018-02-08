using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessWiekonek
{
  public static class Computation
  {
    public static HttpResponseMessage CheckCandidateName(string candidateName)
    {
      if (candidateName == "backup")
      {
        return new HttpResponseMessage(HttpStatusCode.Forbidden);
      }
      return null;
    }

    public static HttpResponseMessage CheckAuthorization(AuthenticationHeaderValue auth)
    {
      if (auth != null && auth.Scheme == "Bearer" && auth.Parameter == "example-token")
      {
        return null;
      }
      return new HttpResponseMessage(HttpStatusCode.Unauthorized);
    }

  }
}
