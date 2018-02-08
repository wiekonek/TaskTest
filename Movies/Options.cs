using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ServerlessWiekonek.Movies
{
  public static class Options
  {
    [FunctionName("Movies_Options")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "{candidateName:length(1,20)}/movies")]HttpRequestMessage req,
      TraceWriter log)
    {
      var resp = req.CreateResponse(HttpStatusCode.OK);
      resp.Headers.Add("Access-Control-Allow-Methods", new[] { "GET", "POST", "OPTIONS" });
      resp.Headers.Add("Access-Control-Allow-Origin", "*");
      return resp;
    }
  }

  public static class Options_Ext
  {
    [FunctionName("Movies_Options_Ext")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "{candidateName:length(1,20)}/movies/{id:length(32,38)}")]HttpRequestMessage req,
      TraceWriter log)
    {
      var resp = req.CreateResponse(HttpStatusCode.OK); 
      resp.Headers.Add("Access-Control-Allow-Methods", new[] { "GET", "PUT", "DELETE", "OPTIONS" });
      resp.Headers.Add("Access-Control-Allow-Origin", "*");
      return resp;
    }
  }
}
