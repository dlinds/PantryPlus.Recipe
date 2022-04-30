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

    public static string GetProductToken()
    {
      var token = KrogerAPIHelper.GetProductToken();
      var result = token.Result;
      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      RequestTokenJson requestJson = JsonConvert.DeserializeObject<RequestTokenJson>(jsonResponse.ToString());
      return requestJson.Access_Token;
    }
    public static RequestTokenJson GetProfileToken(string code)
    {
      var token = KrogerAPIHelper.GetProfileToken(code);
      var result = token.Result;
      // Console.WriteLine("AppUser 44: " + result);
      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      RequestTokenJson requestJson = JsonConvert.DeserializeObject<RequestTokenJson>(jsonResponse.ToString());
      // Console.WriteLine("AppUser 47: " + requestJson);
      return requestJson;
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
      // Console.WriteLine(jsonResponse);
      return jsonResponse.ToString();
    }
  }
}
