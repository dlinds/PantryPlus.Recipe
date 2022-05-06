using System.Collections.Generic;
using System;

namespace PantryPlusRecipe.Models
{
  public class RecipeStep
  {
    public int RecipeStepId { get; set; }
    public int RecipeId { get; set; }
    public int StepNumber { get; set; }
    public string Step { get; set; }
    public int SectionNumber { get; set; }
    public string SectionName { get; set; }

    public virtual ApplicationUser User { get; set; }
  }
}
