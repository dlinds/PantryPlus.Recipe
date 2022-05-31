using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PantryPlusRecipe.Models
{
  public class Ingredient
  {
    public Ingredient()
    {
      this.JoinEntities = new HashSet<IngredientRecipe>();
    }
    public int IngredientId { get; set; }
    public string Name { get; set; }
    public string Measurement { get; set; }
    public float Count { get; set; }
    public string CountForAPIRecipe { get; set; }

    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<IngredientRecipe> JoinEntities { get; }


    public static string GetKrogerProduct(string token, string searchTerm, int? storeId, int page)
    {
      var id = KrogerAPIHelper.GetProductListings(token, searchTerm, storeId, page);
      var result = id.Result;
      JObject jsonResponse = JObject.Parse(result);
      return jsonResponse.ToString();
    }


  }
}
