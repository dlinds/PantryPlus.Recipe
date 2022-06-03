using Microsoft.AspNetCore.Identity;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

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
    public string APIRecipeId { get; set; }

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
        recipeToAdd.APIRecipeId = recipe["id"].ToString();
        recipeToAdd.PrepMinutes = (recipe["prep_time_minutes"] != null) ? recipe["prep_time_minutes"] : 0;
        recipeToAdd.CookMinutes = (recipe["cook_time_minutes"] != null) ? recipe["cook_time_minutes"] : 0;
        recipeToAdd.NumberOfSteps = (recipe["instructions"] != null) ? recipe["instructions"].Count : 0;
        tastyRecipes.Add(recipeToAdd);
      }
      return tastyRecipes;
    }

    public static (Recipe, List<string>, List<Ingredient>) GetTastyById(string id)
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
      tastyRecipe.NumberOfSections = 1;
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
          ingredientToAdd.CountForAPIRecipe = (ingredient["measurements"][0]["quantity"] != null) ? ingredient["measurements"][0]["quantity"] : "";

          ingredientToAdd.Measurement = (ingredient["measurements"][0]["unit"]["abbreviation"] != null) ? ingredient["measurements"][0]["unit"]["abbreviation"] : "";

          ingredientList.Add(ingredientToAdd);
        }
      }

      return (tastyRecipe, instructionList, ingredientList);
    }


    //HELLO FRESH SECTION

    public static List<Recipe> GetHelloFreshRecipes(string searchTerm, string bearerToken)
    {
      var result = HelloFreshAPIHelper.GetHelloFreshRecipes(searchTerm, bearerToken);
      List<Recipe> helloFreshRecipes = new List<Recipe> { };
      dynamic posted = JObject.Parse(result.Result);
      // Console.WriteLine(posted);
      foreach (var recipe in posted["items"][0]["items"])
      {
        Recipe recipeToAdd = new Recipe();
        recipeToAdd.Name = recipe["title"];
        recipeToAdd.Notes = recipe["headline"];
        recipeToAdd.APIRecipeId = recipe["recipeId"];
        string cloudFrontURL = recipe["image"];
        string[] imgPath = cloudFrontURL.Split(new string[] { "/image/" }, StringSplitOptions.None);
        // recipeToAdd.ImgUrl = recipe["image"];
        recipeToAdd.ImgUrl = "https://img.hellofresh.com/c_fit,f_auto,h_400,q_25,w_400/hellofresh_s3/image/" + imgPath[1];
        helloFreshRecipes.Add(recipeToAdd);
      }
      return helloFreshRecipes;
    }

    public static (Recipe, List<string>, List<Ingredient>) GetHelloFreshById(string id, string bearerToken)
    // public static void GetHelloFreshById(string id, string bearerToken)
    {
      var result = HelloFreshAPIHelper.GetHelloFreshRecipeById(id, bearerToken);
      Recipe helloFreshRecipe = new Recipe();
      dynamic posted = JObject.Parse(result.Result);

      helloFreshRecipe.Name = posted["name"];
      helloFreshRecipe.Notes = posted["description"];
      helloFreshRecipe.APIRecipeId = posted["id"];
      //When total time = 25m and prep time = 10m, HF returns PT10M for Total Minutes, and PT25M for Prep.
      string cookMinutes = (posted["prepTime"] != null) ? (Regex.Replace(posted["prepTime"].ToString(), "[^.0-9]", "")) : "0";

      string prepMinutes = (posted["totalTime"] != null) ? (Regex.Replace(posted["totalTime"].ToString(), "[^.0-9]", "")) : "0";

      helloFreshRecipe.CookMinutes = int.Parse(cookMinutes) - int.Parse(prepMinutes);
      helloFreshRecipe.PrepMinutes = int.Parse(prepMinutes);


      helloFreshRecipe.ImgUrl = "https://img.hellofresh.com/c_fit,f_auto,h_600,q_80,w_800/hellofresh_s3" + posted["imagePath"];

      List<Ingredient> ingredientList = new List<Ingredient>();
      foreach (var ingredient in posted["ingredients"])
      {
        Ingredient newIngredient = new Ingredient();
        newIngredient.Name = ingredient["name"];
        string HFIngredientId = ingredient["id"];
        foreach (var measurement in posted["yields"][0]["ingredients"])
        {
          if (HFIngredientId == measurement["id"].ToString())
          {

            if (measurement["amount"].ToString() != null && measurement["amount"].ToString().Length > 0)
            {
              newIngredient.CountForAPIRecipe = measurement["amount"].ToString();
            }
            else
            {
              newIngredient.CountForAPIRecipe = "0";
            }

            newIngredient.Measurement = (measurement["unit"] == "unit") ? null : measurement["unit"];
          }
        }
        ingredientList.Add(newIngredient);
      }

      List<string> stepList = new List<string>();

      foreach (var instruction in posted["steps"])
      {
        stepList.Add(instruction["instructions"].ToString());
      }
      helloFreshRecipe.NumberOfSteps = stepList.Count;
      helloFreshRecipe.NumberOfSections = 1;
      return (helloFreshRecipe, stepList, ingredientList);

    }


  }
}
