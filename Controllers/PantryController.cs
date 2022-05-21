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
  public class PantryController : Controller
  {
    private readonly PantryPlusRecipeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public PantryController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PantryPlusRecipeContext db)
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

    public async Task<IActionResult> Index()
    {
      await RefreshToken();
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.KrogerStoreName = _userManager.GetUserAsync(User).Result?.KrogerStoreName;
      ViewBag.ItemCategories = await _db.Pantry.Where(x => x.User == currentUser).OrderBy(x => x.KrogerCategory).Select(x => x.KrogerCategory).Distinct().ToListAsync();
      ViewBag.PantryItems = await _db.Pantry.Where(x => x.User == currentUser).OrderBy(x => x.KrogerCategory).ToListAsync();
      return View();
    }

    [HttpPost]
    public async Task<JsonResult> SaveToPantry(string jsonPost)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      dynamic posted = JObject.Parse(jsonPost);
      string upc = posted.KrogerUPC;

      if (_db.Pantry.Any(x => x.KrogerUPC == upc && currentUser == x.User))
      {
        Pantry pantryItem = await _db?.Pantry?.SingleOrDefaultAsync(x => x.KrogerUPC == upc);
        pantryItem.ItemCount++;
        await _db.SaveChangesAsync();
      }
      else
      {
        Pantry item = new Pantry
        {
          KrogerUPC = posted.KrogerUPC,
          KrogerCategory = posted.KrogerCategory,
          KrogerAisle = posted.KrogerAisle,
          KrogerCost = posted.KrogerCost,
          KrogerItemName = posted.KrogerItemName,
          KrogerItemSize = posted.KrogerItemSize,
          KrogerImgLink = posted.KrogerImgLink,
          ItemCount = 1,
          User = currentUser
        };
        _db.Pantry.Add(item);
        await _db.SaveChangesAsync();
      }

      return Json("result");
    }

    [HttpPost]
    public async Task<JsonResult> AddSubtractTotal(int id, string method)
    {
      var user = await _userManager.GetUserAsync(User);
      Pantry item = _db.Pantry.Where(x => x.User == user).FirstOrDefault(x => x.PantryId == id);
      if (method == "add")
      {
        item.ItemCount++;
      }
      else if (method == "subtract")
      {
        if (item.ItemCount > 0)
        {
          item.ItemCount--;
          if (item.ItemCount == 0)
          {
            _db.Pantry.Remove(item);
            _db.SaveChanges();
          }

        }
      }
      await _db.SaveChangesAsync();
      return Json(new { Count = item.ItemCount, Price = item.KrogerCost });
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
