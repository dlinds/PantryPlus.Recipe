using Microsoft.AspNetCore.Identity;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;

namespace PantryPlusRecipe.Models
{
  public class Recipe
  {
    public Recipe()
    {
      this.JoinEntitiesSteps = new HashSet<StepRecipe>();
      this.JoinEntitiesIngredients = new HashSet<IngredientRecipe>();
      this.JoinEntitiesCart = new HashSet<CartRecipe>();
    }
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public string ImgUrl { get; set; }
    public int PrepMinutes { get; set; }
    public int CookMinutes { get; set; }
    public string CategoryName { get; set; }
    public int Cost { get; set; }
    public int NumberOfSections { get; set; }
    public int NumberOfSteps { get; set; }
    public string Notes { get; set; }
    // public int? TastyId { get; set; }

    public virtual ICollection<StepRecipe> JoinEntitiesSteps { get; }
    public virtual ICollection<IngredientRecipe> JoinEntitiesIngredients { get; }
    public virtual ICollection<CartRecipe> JoinEntitiesCart { get; }
    public virtual ApplicationUser User { get; set; }

    public static List<Recipe> GetTastyRecipes(string searchTerm)
    {
      var result = TastyAPIHelper.GetTastyRecipes(searchTerm);
      List<Recipe> tastyRecipes = new List<Recipe> { };
      dynamic posted = JObject.Parse(result.Result);
      foreach (var recipe in posted["results"])
      {
        Recipe recipeToAdd = new Recipe();
        recipeToAdd.Name = recipe["name"];
        recipeToAdd.Notes = recipe["description"];
        recipeToAdd.ImgUrl = recipe["thumbnail_url"];
        recipeToAdd.RecipeId = (int)recipe["id"];
        recipeToAdd.PrepMinutes = (recipe["prep_time_minutes"] != null) ? recipe["prep_time_minutes"] : 0;
        recipeToAdd.CookMinutes = (recipe["cook_time_minutes"] != null) ? recipe["cook_time_minutes"] : 0;
        recipeToAdd.NumberOfSteps = (recipe["instructions"] != null) ? recipe["instructions"].Count : 0;
        tastyRecipes.Add(recipeToAdd);
      }
      return tastyRecipes;
    }

    public static (Recipe, List<string>, List<Ingredient>) GetTastyById(int id)
    {
      var result = TastyAPIHelper.GetTastyRecipeDetails(id);
      Recipe tastyRecipe = new Recipe();
      dynamic posted = JObject.Parse(result.Result);
      tastyRecipe.Name = posted["name"];
      tastyRecipe.Notes = posted["description"];
      tastyRecipe.ImgUrl = posted["thumbnail_url"];
      tastyRecipe.PrepMinutes = (posted["prep_time_minutes"] != null) ? posted["prep_time_minutes"] : 0;
      tastyRecipe.CookMinutes = (posted["cook_time_minutes"] != null) ? posted["cook_time_minutes"] : 0;
      tastyRecipe.NumberOfSteps = (posted["instructions"] != null) ? posted["instructions"].Count : 0;

      List<string> instructionList = new List<string>();
      if (posted["instructions"] != null)
      {
        foreach (var instruction in posted["instructions"])
        {

          instructionList.Add(instruction["display_text"].ToString());
        }
      }

      List<Ingredient> ingredientList = new List<Ingredient>();
      if (posted["sections"] != null)
      {
        foreach (var ingredient in posted["sections"][0]["components"])
        {
          Ingredient ingredientToAdd = new Ingredient();
          ingredientToAdd.Name = ingredient["ingredient"]["name"].ToString();
          // ingredientToAdd.Count = ingredient["measurements"][0]["quantity"];
          ingredientToAdd.CountForTasty = (ingredient["measurements"][0]["quantity"] != null) ? ingredient["measurements"][0]["quantity"] : "";

          ingredientToAdd.Measurement = (ingredient["measurements"][0]["unit"]["abbreviation"] != null) ? ingredient["measurements"][0]["unit"]["abbreviation"] : "";

          ingredientList.Add(ingredientToAdd);
        }
      }

      return (tastyRecipe, instructionList, ingredientList);
    }
  }
}
