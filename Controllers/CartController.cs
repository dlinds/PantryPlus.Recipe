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

    private async Task<bool> RefreshToken()
    {
      var user = await _userManager.GetUserAsync(User);
      Token currentToken = await _db?.Tokens?.SingleOrDefaultAsync(x => x.User == user);
      Token newToken = ApplicationUser.CheckIfRefreshNeeded(currentToken);
      currentToken.RefreshToken = newToken.RefreshToken;
      currentToken.TokenValue = newToken.TokenValue;
      currentToken.TokenValueExpiresAt = newToken.TokenValueExpiresAt;
      _db.Entry(currentToken).State = EntityState.Modified;
      await _db.SaveChangesAsync();
      return false;
    }

    public async Task<ActionResult> Index()
    {
      var user = await _userManager.GetUserAsync(User);
      ViewBag.CartItems = await _db.Carts.Where(x => x.User == user).Include(x => x.JoinEntities).OrderBy(x => x.KrogerAisle.Length).ToListAsync();
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
      ViewBag.KrogerStoreName = _userManager.GetUserAsync(User).Result?.KrogerStoreName;
      ViewBag.ListOfRecipes = recipeList;
      return View();
    }

    [HttpPost]
    public async Task<JsonResult> SaveToCart(string jsonPost)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      dynamic posted = JObject.Parse(jsonPost);
      string UPC = posted.KrogerUPC;
      Cart itemInDatabase = await _db.Carts.Where(x => x.User == currentUser && x.KrogerUPC == UPC).FirstOrDefaultAsync();

      if (itemInDatabase != null)
      {
        itemInDatabase.ItemCount++;
        _db.Entry(itemInDatabase).State = EntityState.Modified;
        await _db.SaveChangesAsync();
      }
      else
      {
        Cart item = new Cart
        {
          KrogerUPC = posted.KrogerUPC,
          KrogerAisle = posted.KrogerAisle,
          KrogerCost = posted.KrogerCost,
          KrogerItemName = posted.KrogerItemName,
          KrogerItemSize = posted.KrogerItemSize,
          KrogerImgLink = posted.KrogerImgLink,
          KrogerCategory = posted.KrogerCategory,
          ItemCount = 1,
          User = currentUser
        };
        _db.Carts.Add(item);
        await _db.SaveChangesAsync();
        if (posted.RecipeId != null)
        {
          _db.CartRecipe.Add(new CartRecipe() { RecipeId = posted.RecipeId, CartId = item.CartId });
          await _db.SaveChangesAsync();
        }
      }
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
      await RefreshToken();
      var user = await _userManager.GetUserAsync(User);
      var cartItems = await _db.Carts.Where(x => x.User == user && x.ItemCount > 0).Include(x => x.JoinEntities).ToListAsync();
      var body = @"{" + "\n" +
      @"    ""items"": [" + "\n";
      int count = 0; //only used for comma in json
      foreach (var item in cartItems)
      {
        // foreach (var join in item.JoinEntities)
        // {
        //   if (join.Recipe.RecipeId == id)
        //   {
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

        // }
        count++;
        // }
        item.CountPlacedInCart += item.ItemCount;
        item.ItemCount = 0;
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
      }
      body += @"    ]" + "\n" +
      @"}";

      var currentToken = await _db.Tokens.FirstOrDefaultAsync(x => x.User == user);
      var response = Cart.PutInKrogerCart(currentToken.TokenValue, body);

      return Json(response);
    }

    [HttpPost]
    public async Task<JsonResult> RemoveIngredientsFromRecipe(int recipeId)
    {
      var user = await _userManager.GetUserAsync(User);
      var cartItems = await _db.Carts.Where(x => x.User == user).Include(x => x.JoinEntities).ToListAsync();
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      foreach (var item in cartItems)
      {
        if (item.JoinEntities.Count == 0)
        {
          Pantry alreadyInPantry = await _db.Pantry.Where(x => x.User == user && item.KrogerUPC == x.KrogerUPC).FirstOrDefaultAsync();
          if (alreadyInPantry != null)
          {
            alreadyInPantry.ItemCount += item.CountPlacedInCart;
            _db.Entry(alreadyInPantry).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            _db.Carts.Remove(item);
            await _db.SaveChangesAsync();
          }
          else
          {
            Pantry pantryItem = new Pantry
            {
              KrogerItemName = item.KrogerItemName,
              KrogerImgLink = item.KrogerImgLink,
              KrogerItemSize = item.KrogerItemSize,
              KrogerUPC = item.KrogerUPC,
              User = currentUser,
              ItemCount = item.CountPlacedInCart,
              KrogerAisle = item.KrogerAisle,
              KrogerCategory = item.KrogerCategory,
              KrogerCost = item.KrogerCost
            };
            _db.Pantry.Add(pantryItem);
            await _db.SaveChangesAsync();
            _db.Carts.Remove(item);
            await _db.SaveChangesAsync();
          }
        }
        else
        {
          foreach (var join in item.JoinEntities)
          {
            if (join.Recipe.RecipeId == recipeId)
            {
              Pantry alreadyInPantry = await _db.Pantry.Where(x => x.User == user && item.KrogerUPC == x.KrogerUPC).FirstOrDefaultAsync();
              if (alreadyInPantry != null)
              {
                alreadyInPantry.ItemCount += item.CountPlacedInCart;
                _db.Entry(alreadyInPantry).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                _db.Carts.Remove(item);
                await _db.SaveChangesAsync();
              }
              else
              {
                Pantry pantryItem = new Pantry
                {
                  KrogerItemName = item.KrogerItemName,
                  KrogerImgLink = item.KrogerImgLink,
                  KrogerItemSize = item.KrogerItemSize,
                  KrogerUPC = item.KrogerUPC,
                  User = currentUser,
                  ItemCount = item.CountPlacedInCart,
                  KrogerAisle = item.KrogerAisle,
                  KrogerCategory = item.KrogerCategory,
                  KrogerCost = item.KrogerCost
                };
                _db.Pantry.Add(pantryItem);
                await _db.SaveChangesAsync();
                _db.Carts.Remove(item);
                await _db.SaveChangesAsync();
              }
            }
          }
        }
      }
      return Json("worked");
    }

  }
}
