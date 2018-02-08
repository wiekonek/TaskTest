using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessWiekonek.Movies
{
  public static class Update
  {
    [FunctionName("Movies_Update")]
    public static async Task<HttpResponseMessage> Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "{candidateName:length(1,20)}/movies/{id:length(32,38)}")]HttpRequestMessage req,
      string candidateName,
      string id,
      [Table("movies", Connection = "AzureWebJobsStorage")]CloudTable outTable,
      TraceWriter log)
    {

      var errResponse = Computation.CheckCandidateName(candidateName);
      if (errResponse != null)
      {
        return errResponse;
      }
      errResponse = Computation.CheckAuthorization(req.Headers.Authorization);
      if (errResponse != null)
      {
        return errResponse;
      }

      var movie = await req.Content.ReadAsAsync<MovieApi>();
      movie.Id = id;


      if (string.IsNullOrEmpty(movie.Title))
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
          Content = new StringContent("A non-empty Title must be specified.")
        };
      }

      var movieToUpdate = movie.ToMovie(candidateName);
      movieToUpdate.ETag = "*";


      TableOperation updateOperation = TableOperation.Replace(movie.ToMovie(candidateName));
      try
      {
        TableResult result = outTable.Execute(updateOperation);
        return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
      }
      catch (StorageException e)
      {
        return new HttpResponseMessage((HttpStatusCode)e.RequestInformation.HttpStatusCode);
      }
    }
  }
}
