using System.Collections.Generic;
using System;

namespace PantryPlusRecipe.Models
{
  public class HelloFreshSpiceBlendIngredient
  {
    public int HelloFreshSpiceBlendIngredientId { get; set; }
    public int IngredientId { get; set; }
    public int HelloFreshSpiceBlendId { get; set; }

    public virtual HelloFreshSpiceBlend HelloFreshSpiceBlend { get; set; }
    public virtual Ingredient Ingredient { get; set; }
  }
}
