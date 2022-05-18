using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PantryPlusRecipe.Models;
using System.Threading.Tasks;
using PantryPlusRecipe.ViewModels;
using System;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq;

namespace PantryPlusRecipe.Controllers
{
  public class AccountController : Controller
  {
    private readonly PantryPlusRecipeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PantryPlusRecipeContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    private async Task<bool> RefreshMyToken()
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

    public async Task<IActionResult> Index(string m = "")
    {
      if (!User.Identity.IsAuthenticated)
      {
        if (m == "LoginFail")
        {
          ViewBag.Message = "There was an issue when signing in with either your email or password. Please try again.";
        }
        ViewBag.AuthPageTitle = "Login";
        ViewBag.PageTitle = "Login";
      }
      else
      {
        var user = await _userManager.GetUserAsync(User);
        ViewBag.FullName = _userManager.GetUserAsync(User).Result?.FirstName + " " + _userManager.GetUserAsync(User).Result?.LastName;
        ViewBag.Phone = await _userManager.GetPhoneNumberAsync(user);
        ViewBag.KrogerStoreName = _userManager.GetUserAsync(User).Result?.KrogerStoreName;
        ViewBag.AuthPageTitle = "Account Details";
        ViewBag.PageTitle = "Account Details";
      }

      return View();
    }

    public async Task<bool> HandleTokenRequest(ApplicationUser user, string token, string refreshToken)
    {
      var currentToken = await _db?.Tokens?.SingleOrDefaultAsync(x => x.User == user);
      return true;

    }

    public async Task<IActionResult> LoginRegisterKrogerId(string id, string token, string refreshToken)
    {
      var user = await _db?.Users?.SingleOrDefaultAsync(x => x.KrogerId == id);
      if (user == null) // if not registered yet, redirect to home and show finish registration screen
      {
        TempData["krogerId"] = id;
        return RedirectToAction("Index", "Home");
      }
      else //if they are registered, sign them in and go to home, and add token to DB
      {
        string authenticationMethod = null;
        await _signInManager.SignInAsync(user, isPersistent: true, authenticationMethod);
        Token newToken = new Token();
        newToken.TokenValue = token;
        newToken.User = user;
        newToken.RefreshToken = refreshToken;
        newToken.TokenAuthType = "profile:compact";
        newToken.TokenValueExpiresAt = DateTime.Now.AddMinutes(30);
        newToken.RefreshTokenExpiresAt = DateTime.Now.AddDays(180);
        _db.Tokens.Add(newToken);
        _db.SaveChanges();
        return RedirectToAction("Index", "Home");
      }
    }

    [HttpPost]
    public async Task<IActionResult> AddPhoneNumber(string phoneNumber)
    {
      var user = await _userManager.GetUserAsync(User);
      user.PhoneNumber = String.Format("{0:(###) ###-####}", Int64.Parse(Regex.Replace(phoneNumber, "[^0-9]", "")));
      IdentityResult result = await _userManager.UpdateAsync(user);
      return RedirectToAction("Index");
    }


    public ActionResult Register()
    {
      if (!User.Identity.IsAuthenticated)
      {
        ViewBag.AuthPageTitle = "Login";
      }
      else
      {
        ViewBag.AuthPageTitle = "Account Details";
      }
      if (TempData["krogerId"] != null)
      {
        ViewBag.KrogerId = TempData["krogerId"];
      }
      ViewBag.PageTitle = "Register";
      return View();
    }

    public async Task<JsonResult> ListKrogerLocations(int zipCode, int miles, string store)
    {
      await RefreshMyToken();
      if (miles > 30 || miles < 1)
      {
        miles = 30;
      }
      if (store == null)
      {
        store = "FRED";
      }
      var user = await _userManager.GetUserAsync(User);
      var currentToken = await _db.Tokens.FirstOrDefaultAsync(x => x.User == user);
      // currentToken = await _db.Tokens.FirstOrDefaultAsync(x => x.TokenAuthType == "profile:compact");
      var result = ApplicationUser.GetStoreListings(currentToken.TokenValue, zipCode, miles, store);
      return Json(result);
    }

    [HttpPost]
    public async Task<ActionResult> SetStore(int krogerStoreId, string krogerStoreName)
    {
      var user = await _userManager.GetUserAsync(User);
      user.KrogerStoreId = krogerStoreId;
      user.KrogerStoreName = krogerStoreName;
      IdentityResult result = await _userManager.UpdateAsync(user);
      return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      IdentityResult result = new IdentityResult { };
      if (model.KrogerId != null)
      {
        var user = new ApplicationUser { UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, KrogerId = model.KrogerId };
        result = await _userManager.CreateAsync(user);
      }
      else
      {
        var user = new ApplicationUser { UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email };
        result = await _userManager.CreateAsync(user, model.Password);
      }
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "Home", new { v = "login" });
      }
      else
      {
        foreach (var error in result.Errors)
        {
          TempData["error"] = error.Description;
        }
        return RedirectToAction("Index", "Home");
      }
    }

    public ActionResult Login()
    {
      return RedirectToAction("Index", "Home", new { v = "login" });
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return RedirectToAction("Index", new { m = "LoginFail" });
      }
    }

    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}
