using Microsoft.AspNetCore.Identity;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;


namespace PantryPlusRecipe.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string KrogerId { get; set; }
    public int? KrogerStoreId { get; set; }
    public string KrogerStoreName { get; set; }

    public class RequestTokenJson
    {
      public int Expires_In { get; set; }
      public string Access_Token { get; set; }
      public string Token_Type { get; set; }
      public string Refresh_Token { get; set; }
    }

    public class RequestIdJson
    {
      public string id { get; set; }
    }

    public static RequestTokenJson GetProfileToken(string code)
    {
      var token = KrogerAPIHelper.GetProfileToken(code, "authorization");
      var result = token.Result;
      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      RequestTokenJson requestJson = JsonConvert.DeserializeObject<RequestTokenJson>(jsonResponse.ToString());
      return requestJson;
    }

    public static Token CheckIfRefreshNeeded(Token currentToken)
    {

      if (currentToken.TokenValueExpiresAt < DateTime.Now)
      {
        Console.WriteLine("refresh needed");
        var token = KrogerAPIHelper.GetProfileToken(currentToken.RefreshToken, "refresh");
        var result = token.Result;
        JObject jsonResponse = JObject.Parse(result);
        Token newToken = new Token();
        newToken.RefreshToken = jsonResponse["refresh_token"].ToString();
        newToken.TokenValue = jsonResponse["access_token"].ToString();
        newToken.TokenValueExpiresAt = DateTime.Now.AddMinutes(30);
        Console.WriteLine("newToken.RefreshToken line 57: " + newToken.RefreshToken);
        return newToken;
      }
      return currentToken;
    }


    public static string GetProfileId(string token)
    {
      var id = KrogerAPIHelper.GetProfileId(token);
      var result = id.Result;
      JObject jsonResponse = JObject.Parse(result);
      var results = jsonResponse["data"]["id"];
      return results.ToString();
    }

    public static string GetStoreListings(string token, int zipcode, int miles, string store)
    {
      var id = KrogerAPIHelper.GetStoreListings(token, zipcode, miles, store);
      var result = id.Result;
      JObject jsonResponse = JObject.Parse(result);
      return jsonResponse.ToString();
    }
  }
}
