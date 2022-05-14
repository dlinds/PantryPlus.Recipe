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

    public IActionResult Index()
    {
      Recipe favoriteRecipe = new Recipe();
      favoriteRecipe.ImgUrl = "https://www.budgetbytes.com/wp-content/uploads/2013/07/Creamy-Tomato-Spinach-Pasta-close.jpg";
      favoriteRecipe.Name = "Spicy Sausage and Arrabiata Marinara Pasta";
      favoriteRecipe.NumberOfSteps = 15;
      favoriteRecipe.PrepMinutes = 5;
      favoriteRecipe.CookMinutes = 45;
      ViewBag.Favorites = favoriteRecipe;

      Recipe budgetRecipe = new Recipe();
      budgetRecipe.ImgUrl = "https://www.wholesomeyum.com/wp-content/uploads/2021/12/wholesomeyum-Vegetable-Detox-Soup-Recipe-4.jpg";
      budgetRecipe.Name = "One Pan Chicken Noodle Soup";
      budgetRecipe.NumberOfSteps = 8;
      budgetRecipe.PrepMinutes = 15;
      budgetRecipe.CookMinutes = 25;
      ViewBag.Budget = budgetRecipe;

      Recipe fastRecipe = new Recipe();
      fastRecipe.ImgUrl = "https://www.thespruceeats.com/thmb/VNEd2NPOW3juPkCmuE4J98TT9Cg=/2000x2000/smart/filters:no_upscale()/classic-caesar-salad-recipe-996054-Hero_01-33c94cc8b8e841ee8f2a815816a0af95.jpg";
      fastRecipe.Name = "Chicken Caesar Salad";
      fastRecipe.NumberOfSteps = 10;
      fastRecipe.PrepMinutes = 5;
      fastRecipe.CookMinutes = 30;
      ViewBag.Fast = fastRecipe;

      return View();
    }

    public IActionResult Create()
    {
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
        // Console.WriteLine(section[0]);
        int index = 0;
        foreach (var step in section[1])
        {
          string stepDetails = "STARTSTEP:-" + step;
          stepDetails = stepDetails.Replace($"STARTSTEP:-{index + 1}. ", "");
          // Console.WriteLine(stepDetails);
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
    public async Task<ActionResult> Recipe(int id)
    {
      Recipe model = await _db.Recipes.Include(r => r.JoinEntitiesSteps).FirstOrDefaultAsync(r => r.RecipeId == id);
      return View(model);
    }


    public async Task<ActionResult> GetProductListings(string searchTerm)
    {
      var apiCallTask = ApplicationUser.GetProductToken();
      Console.WriteLine("apiCallTask " + apiCallTask);
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      int? storeId = _userManager.GetUserAsync(User).Result?.KrogerStoreId;

      // Token token = _db.Tokens.FirstOrDefault(t => t.User == currentUser);
      // Console.WriteLine("Current token: " + token.TokenValue);
      var result = Ingredient.GetKrogerProduct(apiCallTask, searchTerm, storeId);

      return Json(new { Message = result });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
