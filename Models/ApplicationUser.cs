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
    public string KrogerStoreId { get; set; }

    public class RequestTokenJson
    {
      public int Expires_In { get; set; }
      public string Access_Token { get; set; }
      public string Token_Type { get; set; }
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
    public static string GetProfileToken(string code)
    {
      var token = KrogerAPIHelper.GetProfileToken(code);
      var result = token.Result;
      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      RequestTokenJson requestJson = JsonConvert.DeserializeObject<RequestTokenJson>(jsonResponse.ToString());
      return requestJson.Access_Token;
    }

    public static string GetProfileId(string token)
    {
      var id = KrogerAPIHelper.GetProfileId(token);
      var result = id.Result;

      JObject jsonResponse = JObject.Parse(result);
      // Console.WriteLine("Token" + token);
      // Console.WriteLine("jsonResponse" + jsonResponse);
      var results = jsonResponse["data"]["id"];
      // Console.WriteLine(results);
      return results.ToString();
    }
  }
}
