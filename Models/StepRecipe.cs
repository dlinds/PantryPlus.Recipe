using System.Collections.Generic;
using System;

namespace PantryPlusRecipe.Models
{
  public class StepRecipe
  {
    public int StepRecipeId { get; set; }
    public int StepId { get; set; }
    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; }
    public virtual Step Step { get; set; }
  }
}
