using Microsoft.Build.Framework;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ServerlessWiekonek.Users
{
  public class UserApi
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime Birthday { get; set; }

    public UserApi() { }

    public UserApi(User user)
    {
      Id = user.RowKey;
      FirstName = user.FirstName;
      LastName = user.LastName;
      Username = user.Username;
      Email = user.Email;
      Birthday = user.Birthday;
    }

    public User ToUser(string partitionKey)
    {
      return new User()
      {
        PartitionKey = partitionKey,
        RowKey = Id,
        ETag = "*", // wildcard
        FirstName = FirstName,
        LastName = LastName,
        Username = Username,
        Email = Email,
        Birthday = Birthday
      };
    }
  }

  public class User : TableEntity
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime Birthday { get; set; }
  }
}
