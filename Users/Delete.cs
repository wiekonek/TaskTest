using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessWiekonek.Users
{
  public static class Delete
  {
    [FunctionName("Users_Delete")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "{candidateName:length(1,20)}/users/{id:length(32,38)}")]HttpRequestMessage req,
      string candidateName,
      string id,
      [Table("users", Connection = "AzureWebJobsStorage")]CloudTable outTable,
      TraceWriter log)
    {
      var updateOperation = TableOperation.Delete(new TableEntity(candidateName, id){ ETag = "*"} );
      var result = outTable.Execute(updateOperation);
      return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
    }
  }
}
