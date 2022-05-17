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

    public async Task<ActionResult> Index()
    {
      var user = await _userManager.GetUserAsync(User);
      ViewBag.CartItems = await _db.Carts.Where(x => x.User == user).Include(x => x.JoinEntities).ToListAsync();
      List<string> recipeNameList = new List<string> { };
      List<Recipe> recipeList = new List<Recipe> { };
      foreach (var item in ViewBag.CartItems)
      {
        foreach (var join in item.JoinEntities)
        {
          if (!recipeNameList.Contains(join.Recipe.Name))
          {
            recipeList.Add(join.Recipe);
            recipeNameList.Add(join.Recipe.Name);
          }
        }
      }
      ViewBag.ListOfRecipes = recipeList;
      return View();
    }

    [HttpPost]
    public async Task<JsonResult> SaveToCart(string jsonPost)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      dynamic posted = JObject.Parse(jsonPost);
      Cart item = new Cart
      {
        KrogerUPC = posted.KrogerUPC,
        KrogerAisle = posted.KrogerAisle,
        KrogerCost = posted.KrogerCost,
        KrogerItemName = posted.KrogerItemName,
        KrogerItemSize = posted.KrogerItemSize,
        KrogerImgLink = posted.KrogerImgLink,
        ItemCount = 1,
        User = currentUser
      };
      _db.Carts.Add(item);
      await _db.SaveChangesAsync();
      _db.CartRecipe.Add(new CartRecipe() { RecipeId = posted.RecipeId, CartId = item.CartId });
      await _db.SaveChangesAsync();
      return Json("result");
    }

    [HttpPost]
    public async Task<JsonResult> AddSubtractTotal(int id, string method)
    {
      var user = await _userManager.GetUserAsync(User);
      Cart item = _db.Carts.Where(x => x.User == user).FirstOrDefault(x => x.CartId == id);
      if (method == "add")
      {
        item.ItemCount++;
      }
      else if (method == "subtract")
      {
        if (item.ItemCount > 0)
        {
          item.ItemCount--;
        }
      }
      await _db.SaveChangesAsync();
      return Json(new { Count = item.ItemCount, Price = item.KrogerCost });
    }

    [HttpPost]
    public async Task<JsonResult> PutToCart(int id)
    {
      var user = await _userManager.GetUserAsync(User);
      var cartItems = await _db.Carts.Where(x => x.User == user && x.ItemCount > 0).Include(x => x.JoinEntities).ToListAsync();
      Console.WriteLine("cartItems.Count" + cartItems.Count);
      var body = @"{" + "\n" +
      @"    ""items"": [" + "\n";
      int count = 0; //only used for comma in json
      foreach (var item in cartItems)
      {
        foreach (var join in item.JoinEntities)
        {
          if (join.Recipe.RecipeId == id)
          {
            body += "        {\n       \"quantity\": " + item.ItemCount + ",\n       \"upc\": \"" + item.KrogerUPC + "\"\n      }";
            if (count < cartItems.Count - 1)
            {
              body += ",\n";
            }
            else
            {
              body += "\n";
            }
            // _db.Carts.Remove(item);
            // await _db.SaveChangesAsync();

          }
          count++;
        }
        item.CountPlacedInCart += item.ItemCount;
        item.ItemCount = 0;
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
      }
      body += @"    ]" + "\n" +
      @"}";

      Console.WriteLine(body);
      var currentToken = await _db.Tokens.FirstOrDefaultAsync(x => x.User == user);
      var response = Cart.PutInKrogerCart(currentToken.TokenValue, body);
      return Json(response);
    }

  }
}
