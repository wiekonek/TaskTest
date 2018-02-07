using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;


namespace ServerlessWiekonek.Movies
{
  public static class Create
  {
    [FunctionName("Movies_Create")]
    public static async Task<HttpResponseMessage> Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{candidateName:length(1,20)}/movies")]HttpRequestMessage req,
      string candidateName,
      [Table("movies", Connection = "AzureWebJobsStorage")]ICollector<Movie> outTable,
      TraceWriter log)
    {
      var data = await req.Content.ReadAsAsync<MovieApi>();

      var movieApi = new MovieApi()
      {
        Id = Guid.NewGuid().ToString(),
        Title = data.Title,
        Year = data.Year,
        Director = data.Director,
        Rating = data.Rating,
      };

      if (string.IsNullOrEmpty(movieApi.Title))
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
          Content = new StringContent("A non-empty Title must be specified.")
        };
      }


      outTable.Add(movieApi.ToMovie(candidateName));
      return req.CreateResponse(HttpStatusCode.Created, movieApi);
    }
  }
}
