using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ServerlessWiekonek.Users
{
  public static class GetAll
  {
    [FunctionName("Users_GetAll")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{candidateName:length(1,20)}/users")]HttpRequestMessage req,
      string candidateName,
      [Table("users", Connection = "AzureWebJobsStorage")]IQueryable<User> inTable,
      TraceWriter log)
    {
      var queries = req.GetQueryNameValuePairs();
      var paginate = queries.Any(q => q.Key == "_page");
      var page = 0;
      var limit = 0;

      if (paginate)
      {
        try
        {
          page = int.Parse(queries.First(q => q.Key == "_page").Value);
        }
        catch
        {
          req.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid _page value.");
        }

        var result = int.TryParse(queries.FirstOrDefault(q => q.Key == "_limit").Value, out limit);
        if (!result)
          limit = PageMetadata.DefaultPageSize;
      }


      var data =
        (from user in inTable
        where user.PartitionKey == candidateName
        select new UserApi(user))
        .ToArray();

      var total = data.Length;
      if (paginate)
      {
        data = data.Page(page, limit).ToArray();
      }


      var response = new PageResponse<UserApi>()
      {
        Data = data,
        Metadata = new PageMetadata()
        {
          Count = data.Length,
          Limit = limit,
          Page = page,
          Total = total
        }
      };

      return req.CreateResponse(HttpStatusCode.OK, response);
    }
  }
}
