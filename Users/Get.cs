using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ServerlessWiekonek.Users
{
  public static class Get
  {
    [FunctionName("Users_Get")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{candidateName:length(1,20)}/users/{id:length(32,38)}")]HttpRequestMessage req,
      string candidateName,
      string id,
      [Table("users", Connection = "AzureWebJobsStorage")]IQueryable<User> inTable,
      TraceWriter log)
    {
      try
      {
        var query =
          (from user in inTable
           where user.PartitionKey == candidateName
           where user.RowKey == id
           select new UserApi(user))
          .First();
        return req.CreateResponse(HttpStatusCode.OK, query);
      }
      catch
      {
        return req.CreateErrorResponse(HttpStatusCode.NotFound, "No such user");
      }
      // TODO: Add error handling!
    }
  }
}
