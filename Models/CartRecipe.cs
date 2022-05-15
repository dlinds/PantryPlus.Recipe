using System.Collections.Generic;
using System;

namespace PantryPlusRecipe.Models
{
  public class CartRecipe
  {
    public int CartRecipeId { get; set; }
    public int RecipeId { get; set; }
    public int CartId { get; set; }

    public virtual Recipe Recipe { get; set; }
    public virtual Cart Cart { get; set; }
  }
}
