using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PantryPlusRecipe.Models
{
  public class HelloFreshToken
  {
    public int HelloFreshTokenId { get; set; }
    public string HelloFreshTokenValue {get;set;}
    public DateTime ExpirationDate {get;set;}
    public virtual ApplicationUser User { get; set; }

    public static HelloFreshToken GetToken ()
    {
      string bearerToken = HelloFreshAPIHelper.GetBearerToken();
      // Console.WriteLine(bearerToken);
      HelloFreshToken newToken = new HelloFreshToken();
      newToken.HelloFreshTokenValue = bearerToken;
      newToken.ExpirationDate = DateTime.Now.AddDays(20);
      return newToken;
    }
  }
}
