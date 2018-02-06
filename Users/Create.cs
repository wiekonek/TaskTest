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
  public static class Create
  {
    [FunctionName("Users_Create")]
    public static async Task<HttpResponseMessage> Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "{candidateName:length(1,20)}/users")]HttpRequestMessage req,
      string candidateName,
      [Table("users", Connection = "AzureWebJobsStorage")]ICollector<User> outTable,
      TraceWriter log)
    {
      var data = await req.Content.ReadAsAsync<UserApi>();

      var userApi = new UserApi()
      {
        Id = Guid.NewGuid().ToString(),
        FirstName = data.FirstName,
        LastName = data.LastName,
        Username = data.Username,
        Email = data.Email,
        Birthday = data.Birthday,
      };

      if (string.IsNullOrEmpty(userApi.FirstName))
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
          Content = new StringContent("A non-empty Firstname must be specified.")
        };
      }


      outTable.Add(userApi.ToUser(candidateName));
      return req.CreateResponse(HttpStatusCode.Created, userApi);
    }
  }
}
