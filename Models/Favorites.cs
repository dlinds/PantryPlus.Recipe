using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PantryPlusRecipe.Models
{
  public class Favorite
  {
    public int FavoriteId { get; set; }
    public int RecipeId { get; set; }

    public virtual ApplicationUser User { get; set; }
  }
}
