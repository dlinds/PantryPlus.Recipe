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
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.IO;

namespace PantryPlusRecipe.Models
{

  public class HelloFreshAPIHelper
  {
    public static string GetBearerToken()
    {
      HttpClient client = new HttpClient();
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
      client.DefaultRequestHeaders.Accept.Clear();
      var response = client.GetStringAsync("https://www.hellofresh.com/recipes/blackened-chicken-penne-61b0d03ab3a03377ee6b1b04");
      string result = response.Result;
      string[] splitStr = result.Split(new string[] { "authFromServer: {\"tokenType\":\"Bearer\",\"accessToken\":\"" }, StringSplitOptions.None);
      string[] secondSplitStr = splitStr[1].Split(new string[] { "\",\"expiresIn\"" }, StringSplitOptions.None);
      return secondSplitStr[0];
    }

    public static async Task<string> GetHelloFreshRecipes(string ingredient, string bearerToken)
    {
      var client = new RestClient($"https://www.hellofresh.com/gw/api/recipes/search/suggestions?country=US&locale=en-US&take=10&q={ingredient}");
      client.Timeout = -1;
      var request = new RestRequest(Method.GET);
      request.AddHeader("referer", "https://www.hellofresh.com/recipes");
      request.AddHeader("Authorization", $"Bearer {bearerToken}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }

    public static async Task<string> GetHelloFreshRecipeById(string recipeId, string bearerToken)
    {
      var client = new RestClient($"https://www.hellofresh.com/gw/api/recipes/{recipeId}");
      client.Timeout = -1;
      var request = new RestRequest(Method.GET);
      request.AddHeader("referer", "https://www.hellofresh.com/recipes");
      request.AddHeader("Authorization", $"Bearer {bearerToken}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }
  }
}
