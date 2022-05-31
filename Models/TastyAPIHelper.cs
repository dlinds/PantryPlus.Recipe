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

  public class TastyAPIHelper
  {
    public static async Task<string> GetTastyRecipes(string ingredient)
    {
      var client = new RestClient($"https://tasty.p.rapidapi.com/recipes/list?from=0&size=30&q={ingredient}");
      var request = new RestRequest(Method.GET);
      request.AddHeader("X-RapidAPI-Host", "tasty.p.rapidapi.com");
      request.AddHeader("X-RapidAPI-Key", $"{EnvironmentVariables.tasty_api}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      // Console.WriteLine(response.Content);
      return response.Content;
    }

    public static async Task<string> GetTastyTags()
    {
      var client = new RestClient($"https://tasty.p.rapidapi.com/tags/list");
      var request = new RestRequest(Method.GET);
      request.AddHeader("X-RapidAPI-Host", "tasty.p.rapidapi.com");
      request.AddHeader("X-RapidAPI-Key", $"{EnvironmentVariables.tasty_api}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      // Console.WriteLine(response.Content);
      return response.Content;
    }

    public static async Task<string> GetTastyRecipeDetails(string id)
    {
      var client = new RestClient($"https://tasty.p.rapidapi.com/recipes/get-more-info?id={id}");
      var request = new RestRequest(Method.GET);
      request.AddHeader("X-RapidAPI-Host", "tasty.p.rapidapi.com");
      request.AddHeader("X-RapidAPI-Key", $"{EnvironmentVariables.tasty_api}");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      // Console.WriteLine(response.Content);
      return response.Content;
    }

  }
}
