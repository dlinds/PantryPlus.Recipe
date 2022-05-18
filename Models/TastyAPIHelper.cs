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
    public static async Task<string> GetStoreListings(string ingredient)
    {
      var client = new RestClient("https://tasty.p.rapidapi.com/recipes/auto-complete?prefix=chicken%20soup");
      var request = new RestRequest(Method.GET);
      request.AddHeader("X-RapidAPI-Host", "tasty.p.rapidapi.com");
      request.AddHeader("X-RapidAPI-Key", "SIGN-UP-FOR-KEY");
      IRestResponse response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }

  }
}
