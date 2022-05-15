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

    // public static async Task<string> GetProductToken()
    // {
    //   var client = new RestClient("https://api.kroger.com/v1/connect/oauth2/token");
    //   var request = new RestRequest(Method.POST);
    //   request.AddHeader("content-type", "application/x-www-form-urlencoded");
    //   request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={EnvironmentVariables.client_id}&client_secret={EnvironmentVariables.client_secret}&scope=product.compact", ParameterType.RequestBody);
    //   IRestResponse response = await client.ExecuteTaskAsync(request);
    //   return response.Content;
    // }

    public static async Task<string> GetProfileToken(string code)
    {
      var client = new RestClient("https://api.kroger.com/v1/connect/oauth2/token");
      var request = new RestRequest(Method.POST);
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded", $"grant_type=authorization_code&code=" + code + $"&redirect_uri=https://localhost:6003?getAuth=profile&client_id={EnvironmentVariables.client_id}&client_secret={EnvironmentVariables.client_secret}&scope=profile.compact", ParameterType.RequestBody);
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }
    // public static async Task<string> GetCartToken(string code)
    // {
    //   //https://api.kroger.com/v1/connect/oauth2/authorize?scope={{SCOPES}}&response_type=code&client_id={{CLIENT_ID}}&redirect_uri={{REDIRECT_URI}}

    //   var client = new RestClient("https://api.kroger.com/v1/connect/oauth2/token");
    //   var request = new RestRequest(Method.POST);
    //   request.AddHeader("content-type", "application/x-www-form-urlencoded");
    //   request.AddParameter("application/x-www-form-urlencoded", $"grant_type=authorization_code&code=" + code + $"&redirect_uri=https://localhost:6003?getAuth=profile&client_id={EnvironmentVariables.client_id}&client_secret={EnvironmentVariables.client_secret}&scope=cart.basic:write", ParameterType.RequestBody);
    //   foreach (var req in request.Parameters)
    //   {
    //     Console.WriteLine(req);
    //   }
    //   IRestResponse response = await client.ExecuteTaskAsync(request);
    //   return response.Content;
    // }
    public static async Task<string> GetProfileId(string token)
    {
      var client = new RestClient("https://api.kroger.com/v1/identity/profile");
      client.Timeout = -1;
      var request = new RestRequest(Method.GET);
      request.AddHeader("Authorization", $"Bearer {token}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }

    public static async Task<string> GetStoreListings(string token, int zipcode, int miles, string store)
    {
      var client = new RestClient($"https://api.kroger.com/v1/locations?filter.zipCode.near={zipcode}&filter.limit=30&filter.radiusInMiles={miles}&filter.chain={store}");
      client.Timeout = -1;
      var request = new RestRequest(Method.GET);
      request.AddHeader("Authorization", $"Bearer {token}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      // Console.WriteLine(response.Content);
      return response.Content;
    }


    public static async Task<string> GetProductListings(string token, string searchTerm, int? storeId, int page)
    {
      page = ((page - 1) * 6) + 1;
      string searchPageParam = $"&filter.start={page}";
      if (page == 1)
      {                       // blank out the search when it's first page. If only on
        searchPageParam = ""; // product is returned (Puff Pastry), then no results
      }                       // are returned if search results start at page 1
      var client = new RestClient($"https://api.kroger.com/v1/products?filter.term={searchTerm}&filter.locationId={storeId}&filter.fulfillment=inStore&filter.limit=6{searchPageParam}");
      client.Timeout = -1;
      var request = new RestRequest(Method.GET);
      request.AddHeader("Authorization", $"Bearer {token}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      // Console.WriteLine(response.Content);
      return response.Content;
    }

    public static async Task<IRestResponse> PutProductsInCart(string token, string body)
    {
      var client = new RestClient("https://api.kroger.com/v1/cart/add");
      client.Timeout = -1;
      var request = new RestRequest(Method.PUT);
      request.AddHeader("Content-Type", "application/json");
      request.AddHeader("Authorization", $"Bearer {token}");
      request.AddParameter("application/json", body, ParameterType.RequestBody);
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response;
    }



  }
}
