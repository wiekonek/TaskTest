using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using ServerlessWiekonek.Users;

namespace ServerlessWiekonek
{
  public static class Landing
  {
    [FunctionName("Landing")]
    public static HttpResponseMessage Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{candidateName:length(1,20)}")]HttpRequestMessage req,
      string candidateName,
      [Table("users", Connection = "AzureWebJobsStorage")]IQueryable<User> inTable,
      TraceWriter log)
    {

       if (candidateName == "backup" || !(from user in inTable
         where user.PartitionKey == candidateName
         select user).ToArray().Any())
      {
        return new HttpResponseMessage(HttpStatusCode.NotFound);
      }

      var response = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new StringContent($@"
          <!doctype html>

          <html lang='en'>
            <head>
              <meta charset='utf-8'>
              <title> Zadanie testowe </title>
              <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css' integrity='sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm' crossorigin='anonymous'>
            </head>
            <body>
              <div class='container'>
                <div class='jumbotron mt-4'>
                  <h1 class='display-4'>Zadanie testowe</h1>
                  <p class='lead'>Kandydat: {candidateName}</p>
                  <hr class='my-4'>  
                  <h2>Opis zadania</h2>
                  <p class='text-justify'>Zadanie polega na wykonaniu prostego front-endu, który pozwala wyœwietlaæ i edytowaæ bazê u¿ytkowników systemu oraz dostêpnych filmów. Dostêp do bazy zapewniaj¹ 2 end-pointy (<code>/users</code> i <code>/movies</code>) REST / JSON, do których dokumentacja znajduje siê pod adresem:
                  <a href='https://documenter.getpostman.com/view/1299479/testtask-public/RVfqoDxV'>https://documenter.getpostman.com/view/1299479/testtask-public/RVfqoDxV</a>.</p>
                  <p class='text-justify'>Nale¿y wykonaæ stronê zawieraj¹c¹ menu z 2 pozycjami 'Users' i 'Movies'. Wybranie odpowiedniej pozycji z menu powoduje przejœcie do sekcji umo¿liwiaj¹cej wyœwietlanie i zarz¹dzanie odpowiednimi zasobami.</p>
                  <h6>Sekcja Users:</h6>
                  <ul>
                    <li>wyœwietla listê users z paginacj¹ - iloœæ elementów na stronê: 15</li>
                    <li>wyœwietla szczegó³owe informacje o elemencie user</li>
                    <li>umo¿liwia dodanie, edycjê oraz usuniêcie wybranego elementu user</li>
                  </ul>
                  <h6>Sekcja Movies:</h6>
                  <ul>
                    <li>wymaga przekazania klucza dostêpu do api (<code>example-token</code>)</li>
                    <li>wyœwietla filmy</li>
                    <li>wyœwietla szczegó³owe informacje o elemencie movie</li>
                    <li>umo¿liwia dodanie, edycjê oraz usuniêcie wybranego elementu movie</li>
                  </ul>
                  <p class='text-justify'>Oceniana bêdzie przede wszystkim poprawnoœæ wykonania ale równie¿ estetyka i ³atwoœæ obs³ugi strony.</p>
                  <p class='text-justify'>Zadanie nale¿y wykonaæ do niedzieli 11.02.</p>
                  <p class='text-justify'><em>Powodzenia!</em></p>
                </div>
              </div>
          </body>
        </html>
         ")
      };
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
      return response;
    }
  }
}
