using System.Threading.Tasks;
using RestSharp;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PantryPlusRecipe.Models;

namespace PantryPlusRecipe.Models
{

  public class KrogerAPIHelper
  {

    public static async Task<string> GetProductToken()
    {
      var client = new RestClient("https://api.kroger.com/v1/connect/oauth2/token");
      var request = new RestRequest(Method.POST);
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={EnvironmentVariables.client_id}&client_secret={EnvironmentVariables.client_secret}&scope=product.compact", ParameterType.RequestBody);
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }

    public static async Task<string> GetProfileToken(string code)
    {
      var client = new RestClient("https://api.kroger.com/v1/connect/oauth2/token");
      var request = new RestRequest(Method.POST);
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      // Console.WriteLine(code);
      request.AddParameter("application/x-www-form-urlencoded", $"grant_type=authorization_code&code=" + code + "&redirect_uri=https://localhost:6003?getAuth=profile&client_id={EnvironmentVariables.client_id}&client_secret={EnvironmentVariables.client_secret}&scope=profile.compact", ParameterType.RequestBody);
      IRestResponse response = await client.ExecuteTaskAsync(request);
      // Console.WriteLine("line 38 KAPI" + response.Content);
      return response.Content;
    }

    public static async Task<string> GetProfileId(string token)
    {
      var client = new RestClient("https://api.kroger.com/v1/identity/profile");
      client.Timeout = -1;
      var request = new RestRequest(Method.GET);
      request.AddHeader("Authorization", $"Bearer {token}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }


    //     public static async Task<string> GetProfileId(string token)
    // {
    //   var client = new RestClient("https://api.kroger.com/v1/identity/profile");
    //   client.Timeout = -1;
    //   var request = new RestRequest(Method.GET);
    //   request.AddHeader("content-type", "application/x-www-form-urlencoded");
    //   request.AddHeader("Authorization", $"Bearer {token}");
    //   IRestResponse response = await client.ExecuteTaskAsync(request);
    //   Console.WriteLine("Profile ID::::::::::::::::::: " + response.Content);
    //   return response.Content;
    // }

    // public static async Task<string> GetMilk(string token)
    // {
    //   var client = new RestClient("https://api.kroger.com/v1/products?filter.term=milk&filter.limit=5");
    //   client.Timeout = -1;
    //   var request = new RestRequest(Method.GET);
    //   request.AddHeader("Authorization", $"Bearer {token}");
    //   IRestResponse response = await client.ExecuteTaskAsync(request);
    //   Console.WriteLine(response.Content);
    //   return response.Content;
    // }
  }
}
