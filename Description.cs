using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ServerlessWiekonek
{
  public static class Description
  {
    [FunctionName("Description")]
    public static async Task<HttpResponseMessage> Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
      TraceWriter log)
    {
      dynamic data = await req.Content.ReadAsAsync<object>();

      return req.CreateResponse(HttpStatusCode.OK, "Hello! That's your task.");
    }
  }
}
