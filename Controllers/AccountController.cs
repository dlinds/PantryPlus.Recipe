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
        // var email = _userManager.GetEmailAsync(user);
        // var userName = _userManager.GetUserNameAsync(user);
        ViewBag.FullName = _userManager.GetUserAsync(User).Result?.FirstName + " " + _userManager.GetUserAsync(User).Result?.LastName;
        ViewBag.Phone = await _userManager.GetPhoneNumberAsync(user);
        // ViewBag.Phone = phoneNumber;
        // ViewBag.FullName = fullName;
        ViewBag.KrogerStoreId = _userManager.GetUserAsync(User).Result?.KrogerStoreId;
        ViewBag.AuthPageTitle = "Account Details";
        ViewBag.PageTitle = "Account Details";
      }

      return View();
    }

    public async Task<IActionResult> LoginRegisterKrogerId(string id, string token)
    {
      var user = await _db?.Users?.SingleOrDefaultAsync(x => x.KrogerId == id);
      if (user == null) // if not registered yet, redirect to home and show finish registration screen
      {
        TempData["krogerId"] = id;
        return RedirectToAction("Index", "Home");
      }
      else //if they are registered, sign them in and go to home
      {
        string authenticationMethod = null;
        await _signInManager.SignInAsync(user, isPersistent: true, authenticationMethod);
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        Token newToken = new Token();
        newToken.TokenValue = token;
        // _db.Tokens.Add(newToken);
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

    public ActionResult ListKrogerLocations(int zipCode)
    {
      return View();
    }



    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      IdentityResult result = new IdentityResult { };
      // Console.WriteLine(model.KrogerId);
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
          // Console.WriteLine(error.Description);
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
