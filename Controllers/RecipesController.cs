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
  public class RecipesController : Controller
  {
    // private readonly ILogger<PantryController> _logger;
    private readonly PantryPlusRecipeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public RecipesController(UserManager<ApplicationUser> userManager, PantryPlusRecipeContext db)
    {
      _userManager = userManager;
      _db = db;
    }


    public class PostRecipeJson
    {
      public Dictionary<string, string[]> sections { get; set; }
      public List<string> ingredients { get; set; }
      public int cookTime { get; set; }
      public int prepTime { get; set; }
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
      var user = await _userManager.GetUserAsync(User);
      ViewBag.FastRecipes = await _db.Recipes.OrderBy(x => x.CategoryName).Where(x => (x.PrepMinutes + x.CookMinutes) < 30).ToListAsync();
      ViewBag.BudgetRecipes = await _db.Recipes.OrderBy(x => x.CategoryName).Where(x => x.Cost > 10).ToListAsync();
      ViewBag.Tasty = Recipe.GetTastyRecipes("chicken");
      return View();
    }
    public ActionResult ReturnPartial()
    {
      ViewBag.Tasty = Recipe.GetTastyRecipes("beef");
      return PartialView("HolidayPartialView", ViewBag.Tasty);
    }

    public IActionResult Create()
    {
      // ViewBag.Recipes = TastyAPIHelper.GetTastyRecipes("tomato");
      // Console.WriteLine(ViewBag.Recipes);
      // var listOfTags = TastyAPIHelper.GetTastyTags();

      // Recipe.GetTastyRecipes();
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(string jsonPost)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      dynamic posted = JObject.Parse(jsonPost);
      Recipe recipe = new Recipe();
      recipe.CookMinutes = posted.cookTime;
      recipe.PrepMinutes = posted.prepTime;
      recipe.CategoryName = posted.categoryName;
      recipe.Cost = posted.cost;
      recipe.Notes = posted.notes;
      recipe.Name = posted.recipeName;
      int numOfSections = 0;
      int numOfSteps = 0;
      recipe.User = currentUser;
      _db.Recipes.Add(recipe);
      _db.SaveChanges();
      foreach (var section in posted.sections)
      {
        int index = 0;
        foreach (var step in section[1])
        {
          string unique = Guid.NewGuid().ToString();
          string stepDetails = $"{unique}:-" + step;
          stepDetails = stepDetails.Replace($"{unique}:-{index + 1}. ", "");
          Step recipeStep = new Step();
          recipeStep.StepNumber = index + 1;
          recipeStep.Details = stepDetails;
          recipeStep.SectionNumber = numOfSections + 1;
          recipeStep.SectionName = section[0];
          recipeStep.User = currentUser;
          _db.Steps.Add(recipeStep);
          _db.SaveChanges();

          _db.StepRecipes.Add(new StepRecipe() { RecipeId = recipe.RecipeId, StepId = recipeStep.StepId });
          index++;
          _db.SaveChanges();
          numOfSteps++;
        }
        numOfSections++;
      }
      recipe.NumberOfSections = numOfSections;
      recipe.NumberOfSteps = numOfSteps;
      _db.SaveChanges();

      foreach (var postedIngred in posted.ingredients)
      {
        Ingredient ingredient = new Ingredient();
        ingredient.Count = postedIngred[0];
        if (postedIngred[1] != "")
        {
          ingredient.Measurement = postedIngred[1];
        }
        ingredient.Name = postedIngred[2];
        ingredient.User = currentUser;
        _db.Ingredients.Add(ingredient);
        _db.SaveChanges();
        _db.IngredientRecipes.Add(new IngredientRecipe() { RecipeId = recipe.RecipeId, IngredientId = ingredient.IngredientId });
        _db.SaveChanges();
      }


      return Json(new { Message = "message" });
    }
    [HttpGet("/recipe")]
    public async Task<ActionResult> IndividualRecipe(int id)
    {
      Recipe model = await _db.Recipes.Include(r => r.JoinEntitiesSteps).FirstOrDefaultAsync(r => r.RecipeId == id);

      ViewBag.KrogerStoreName = _userManager.GetUserAsync(User).Result?.KrogerStoreName;

      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);

      ViewBag.ItemCategories = await _db.Pantry.Where(x => x.User == currentUser).OrderBy(x => x.KrogerCategory).Select(x => x.KrogerCategory).Distinct().ToListAsync();

      ViewBag.PantryList = await _db.Pantry.Where(x => x.User == currentUser).OrderBy(x => x.KrogerCategory).Select(x => x.KrogerItemName.ToLower()).ToListAsync();
      return View(model);
    }


    public async Task<JsonResult> GetProductListings(string searchTerm, int page)
    {
      await RefreshToken();
      var user = await _userManager.GetUserAsync(User);
      var token = await _db.Tokens.FirstOrDefaultAsync(x => x.User == user);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      int? storeId = _userManager.GetUserAsync(User).Result?.KrogerStoreId;
      var result = Ingredient.GetKrogerProduct(token.TokenValue, searchTerm, storeId, page);
      return Json(result);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
