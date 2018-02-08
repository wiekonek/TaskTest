using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessWiekonek.Movies;
using ServerlessWiekonek.Users;

namespace ServerlessWiekonek.Admin
{
  public class CandidateApi
  {
    public string Name;
  }

  public static class CreateCandidate
  {
    [FunctionName("CreateCandidate")]
    [StorageAccount("AzureWebJobsStorage")]
    public static async Task<HttpResponseMessage> Run(
      [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "candidates")]HttpRequestMessage req,
      [Table("users", Connection = "AzureWebJobsStorage")]CloudTable usersTable,
      [Table("movies", Connection = "AzureWebJobsStorage")]CloudTable moviesTable,
      TraceWriter log)
    {
      var data = await req.Content.ReadAsAsync<CandidateApi>();
      if(data == null || string.IsNullOrEmpty(data.Name))
      {
        return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
      }
      var candidateName = data.Name;

      var errResponse = Computation.CheckCandidateName(candidateName);
      if (errResponse != null)
      {
        return await Task.FromResult(errResponse);
      }

      try
      {
        Task.WaitAll(
          CreateEntitiesAsync(candidateName, moviesTable),
          CreateEntitiesAsync(candidateName, usersTable)
        );
      }
      catch
      {
        return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.Conflict));
      }

      return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.Created));
    }

    private static async Task<IList<TableResult>> CreateEntitiesAsync(string candidateName, CloudTable table)
    {
      var filterBackup = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "backup");
      var query = new TableQuery()
      {
        FilterString = filterBackup
      };
      var backupMovies = table.ExecuteQuery(query);
      var operation = new TableBatchOperation();
      foreach (var movie in backupMovies)
      {
        movie.PartitionKey = candidateName;
        operation.InsertOrReplace(movie);
      }
      return await table.ExecuteBatchAsync(operation);
    }
  }

  public static class Options
  {
    [FunctionName("Candidate_Options")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "candidates")]HttpRequestMessage req,
      TraceWriter log)
    {
      var resp = req.CreateResponse(HttpStatusCode.OK);
      resp.Headers.Add("Access-Control-Allow-Methods", new[] { "POST", "OPTIONS" });
      resp.Headers.Add("Access-Control-Allow-Origin", "*");
      return resp;
    }
  }
}
