using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ServerlessWiekonek.Movies
{
  public static class Get
  {
    [FunctionName("Movies_Get")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{candidateName:length(1,20)}/movies/{id:length(32,38)}")]HttpRequestMessage req,
      string candidateName,
      string id,
      [Table("movies", Connection = "AzureWebJobsStorage")]IQueryable<Movie> inTable,
      TraceWriter log)
    {
      try
      {
        var query =
          (from movie in inTable
           where movie.PartitionKey == candidateName
           where movie.RowKey == id
           select new MovieApi(movie))
          .First();
        return req.CreateResponse(HttpStatusCode.OK, query);
      }
      catch
      {
        return req.CreateErrorResponse(HttpStatusCode.NotFound, "No such movie");
      }
      // TODO: Add error handling!
    }
  }
}
