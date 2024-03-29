﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PantryPlusRecipe.Models;
using System.IO;
using Microsoft.AspNetCore.Identity;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Data;

namespace PantryPlusRecipe.Controllers
{
  public class HomeController : Controller
  {
    public class RequestJson
    {
      public int Expires_In { get; set; }
      public string Access_Token { get; set; }
      public string Refresh_Token { get; set; }
      public string Token_Type { get; set; }
    }
    private readonly PantryPlusRecipeContext _db;

    public HomeController(PantryPlusRecipeContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index(string code, string getAuth, string v)
    {
      if (getAuth == "profile")
      {
        // System.IO.File.AppendAllText("LOGGING.txt", "Token: " + code.ToString());
        var token = ApplicationUser.GetProfileToken(code);
        var krogerId = ApplicationUser.GetProfileId(token.Access_Token);
        return RedirectToAction("LoginRegisterKrogerId", "Account", new { id = krogerId, token = token.Access_Token, refreshToken = token.Refresh_Token });
      }
      if (TempData["error"] != null)
      {
        ViewBag.Error = TempData["error"];
      }
      if (TempData["krogerId"] != null)
      {
        ViewBag.HomeView = "Register";
        ViewBag.KrogerId = TempData["krogerId"];
        ViewBag.Token = TempData["token"];
        ViewBag.RefreshToken = TempData["RefreshToken"];
        return View();
      }
      if (v == "register")
      {
        ViewBag.HomeView = "Register";
        return View();
      }
      else if (v == "login")
      {
        ViewBag.HomeView = "Login";
        return View();
      }

      return RedirectToAction("Index", "Recipes");


    }
    [HttpGet("/GetAuthorizationCode")]
    public ActionResult GetAuthorizationCode()
    {
      return Redirect($"https://api.kroger.com/v1/connect/oauth2/authorize?scope={EnvironmentVariables.scope}&redirect_uri={EnvironmentVariables.redirect_uri}&response_type=code&client_id={EnvironmentVariables.client_id}");
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
