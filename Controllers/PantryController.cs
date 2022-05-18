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

    public IActionResult Index()
    {
      ViewBag.KrogerStoreName = _userManager.GetUserAsync(User).Result?.KrogerStoreName;
      return View();
    }

    [HttpPost]
    public async Task<JsonResult> SaveToPantry(string jsonPost)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      dynamic posted = JObject.Parse(jsonPost);
      string upc = posted.KrogerUPC;

      if (_db.Pantry.Any(x => x.KrogerUPC == upc))
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
