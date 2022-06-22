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
    public int SpiceBlendId { get; set; }
    public string SpiceBlendName { get; set; }


    public virtual ICollection<HelloFreshSpiceBlendIngredient> JoinEntities { get; }
  }
}
