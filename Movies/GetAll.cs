using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ServerlessWiekonek.Movies
{
  public static class GetAll
  {
    [FunctionName("Movies_GetAll")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{candidateName:length(1,20)}/movies")]HttpRequestMessage req,
      string candidateName,
      [Table("movies", Connection = "AzureWebJobsStorage")]IQueryable<Movie> inTable,
      TraceWriter log)
    {
      var query =
        from movie in inTable
        where movie.PartitionKey == candidateName
        select new MovieApi(movie);

      return req.CreateResponse(HttpStatusCode.OK, query);
    }
  }
}
