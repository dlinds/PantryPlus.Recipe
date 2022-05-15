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
    private readonly SignInManager<ApplicationUser> _signInManager;

    public CartController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PantryPlusRecipeContext db)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _db = db;
    }
    public ActionResult Index()
    {
      return View();
    }
    [HttpGet("/GetCartAuthorizationCode")]
    public ActionResult GetCartAuthorizationCode()
    {
      return Redirect($"https://api.kroger.com/v1/connect/oauth2/authorize?scope=cart.basic:write&response_type=code&client_id={EnvironmentVariables.client_id}&redirect_uri=https://localhost:6003?getAuth=cart");
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
      // currentToken = await _db.Tokens.FirstOrDefaultAsync(x => x.TokenAuthType == "cart.basic:write");
      var response = Cart.PutInCart(currentToken.TokenValue, body);
      return Json(response);
    }

    // public async Task<IActionResult> CartAuthTokenSuccess(string id, string token, string refreshToken)
    // {
    //   var user = await _db?.Users?.SingleOrDefaultAsync(x => x.KrogerId == id);

    //   Token newToken = new Token();
    //   newToken.TokenValue = token;
    //   newToken.User = user;
    //   newToken.RefreshToken = refreshToken;
    //   newToken.TokenAuthType = "cart.basic:write";
    //   newToken.TokenValueExpiresAt = DateTime.Now.AddMinutes(30);
    //   newToken.RefreshTokenExpiresAt = DateTime.Now.AddDays(180);
    //   _db.Tokens.Add(newToken);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");

    // }


  }
}
