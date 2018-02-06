using System.Linq;
using System.Net;
using System.Net.Http;
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
      var query =
        from user in inTable
        where user.PartitionKey == candidateName
        select new UserApi(user);
      return req.CreateResponse(HttpStatusCode.OK, query);
    }
  }
}
