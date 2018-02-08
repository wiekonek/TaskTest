using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessWiekonek.Users
{
  public static class Delete
  {
    [FunctionName("Users_Delete")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "{candidateName:length(1,20)}/users/{id:length(32,38)}")]HttpRequestMessage req,
      string candidateName,
      string id,
      [Table("users", Connection = "AzureWebJobsStorage")]CloudTable outTable,
      TraceWriter log)
    {
      var errResponse = Computation.CheckCandidateName(candidateName);
      if (errResponse != null)
      {
        return errResponse;
      }

      var updateOperation = TableOperation.Delete(new TableEntity(candidateName, id) { ETag = "*" });

      try
      {
        var result = outTable.Execute(updateOperation);
        return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);

      }
      catch (StorageException e)
      {
        return new HttpResponseMessage((HttpStatusCode)e.RequestInformation.HttpStatusCode);
      }
    }
  }
}
