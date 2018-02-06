using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessWiekonek.Movies
{
  public class Movie : TableEntity
  {
    public string Title { get; set; }
    public int Year { get; set; }
    public string Director { get; set; }
    public int Rating { get; set; }
  }
}
