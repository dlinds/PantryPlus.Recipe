using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PantryPlusRecipe.Models;
using Microsoft.AspNetCore.Authorization;


namespace PantryPlusRecipe.Controllers
{
  [Authorize]
  public class RecipesController : Controller
  {
    private readonly ILogger<PantryController> _logger;

    public RecipesController(ILogger<PantryController> logger)
    {
      _logger = logger;
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
