using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PantryPlusRecipe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;



namespace PantryPlusRecipe.Controllers
{
  [Authorize]
  public class CartController : Controller
  {
    private readonly PantryPlusRecipeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(UserManager<ApplicationUser> userManager, PantryPlusRecipeContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public ActionResult Index()
    {
      return View();
    }


    [HttpPost]
    public async Task<JsonResult> PutToCart(string jsonPost)
    {
      dynamic posted = JObject.Parse(jsonPost);
      var body = @"{" + "\n" +
      @"    ""items"": [" + "\n";
      int count = 0;
      foreach (var item in posted.data.items)
      {
        body += "        {\n       \"quantity\": " + item.quantity + ",\n       \"upc\": \"" + item.upc + "\"\n      }";
        if (count != posted.data.items.Count - 1)
        {
          body += ",\n";
        }
        else
        {
          body += "\n";
        }
        count++;
      }
      body += @"    ]" + "\n" +
      @"}";
      var user = await _userManager.GetUserAsync(User);
      var currentToken = await _db.Tokens.FirstOrDefaultAsync(x => x.User == user);
      // Console.WriteLine(currentToken.TokenValue);
      var response = Cart.PutInCart(currentToken.TokenValue, body);
      return Json(response);
    }
  }
}
