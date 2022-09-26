using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PantryPlusRecipe.Models
{
  public class HelloFreshSpiceBlend
  {
    public HelloFreshSpiceBlend()
    {
      this.JoinEntities = new HashSet<HelloFreshSpiceBlendIngredient>();
    }
    public int HelloFreshSpiceBlendId { get; set; }
    public string HelloFreshSpiceBlendName { get; set; }


    public virtual ICollection<HelloFreshSpiceBlendIngredient> JoinEntities { get; }
  }
}
