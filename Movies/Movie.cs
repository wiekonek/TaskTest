using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessWiekonek.Movies
{
  public class MovieApi
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Director { get; set; }
    public int Rating { get; set; }

    public MovieApi() { }

    public MovieApi(Movie movie)
    {
      Id = movie.RowKey;
      Title = movie.Title;
      Year = movie.Year;
      Director = movie.Director;
      Rating = movie.Rating;
    }

    public Movie ToMovie(string partitionKey)
    {
      return new Movie()
      {
        PartitionKey = partitionKey,
        RowKey = Id,
        ETag = "*", // wildcard
        Title = Title,
        Year = Year,
        Director = Director,
        Rating = Rating,
      };
    }
  }

  public class Movie : TableEntity
  {
    public string Title { get; set; }
    public int Year { get; set; }
    public string Director { get; set; }
    public int Rating { get; set; }
  }
}
