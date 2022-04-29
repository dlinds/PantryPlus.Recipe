using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PantryPlusRecipe.Models;

namespace PantryPlusRecipe.Controllers
{
  public class HomeController : Controller
  {
    public class RequestJson
    {
      public int Expires_In { get; set; }
      public string Access_Token { get; set; }
      public string Token_Type { get; set; }
    }
    private readonly PantryPlusRecipeContext _db;

    public HomeController(PantryPlusRecipeContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index(string getAuth, string code, string v)
    {
      if (getAuth == "product")
      {
        var apiCallTask = ApplicationUser.GetProductToken();
        // Console.WriteLine(apiCallTask);
      }
      else if (getAuth == "profile")
      {
        var token = ApplicationUser.GetProfileToken(code);
        var krogerId = ApplicationUser.GetProfileId(token);
        return RedirectToAction("LoginRegisterKrogerId", "Account", new { id = krogerId, token = token });
      }
      if (TempData["error"] != null)
      {
        // Console.WriteLine(TempData["error"]);
        ViewBag.Error = TempData["error"];
      }
      if (TempData["krogerId"] != null)
      {
        ViewBag.HomeView = "Register";
        ViewBag.KrogerId = TempData["krogerId"];
      }
      if (v == "register")
      {
        ViewBag.HomeView = "Register";
      }
      else if (v == "login")
      {
        ViewBag.HomeView = "Login";
      }
      return View();
    }
    [HttpGet("/GetAuthorizationCode")]
    public ActionResult GetAuthorizationCode()
    {
      return Redirect($"https://api.kroger.com/v1/connect/oauth2/authorize?scope=profile.compact&response_type=code&client_id={EnvironmentVariables.client_id}&redirect_uri=https://localhost:6003?getAuth=profile");
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
