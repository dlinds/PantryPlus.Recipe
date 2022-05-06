using System.Collections.Generic;
using System;

namespace PantryPlusRecipe.Models
{
  public class Recipe
  {
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

    public virtual ApplicationUser User { get; set; }
  }
}
