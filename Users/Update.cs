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
  public static class Update
  {
    [FunctionName("Users_Update")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Function, "put", Route = "{candidateName:length(1,20)}/users/{id:length(32,38)}")]UserApi user,
      string candidateName,
      string id,
      [Table("users", Connection = "AzureWebJobsStorage")]CloudTable outTable,
      TraceWriter log)
    {
      user.Id = id;


      if (string.IsNullOrEmpty(user.FirstName))
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
          Content = new StringContent("A non-empty Firstname must be specified.")
        };
      }

      var userToUpdate = user.ToUser(candidateName);
      userToUpdate.ETag = "*";


      TableOperation updateOperation = TableOperation.Replace(user.ToUser(candidateName));
      TableResult result = outTable.Execute(updateOperation);
      return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
    }
  }
}