using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PantryPlusRecipe.Models
{
  public class Cart
  {
    public Cart()
    {
      this.JoinEntities = new HashSet<CartRecipe>();
    }
    public int CartId { get; set; }
    public string KrogerItemName { get; set; }
    public string KrogerItemSize { get; set; }
    public string KrogerUPC { get; set; }
    public string KrogerAisle { get; set; }
    public string KrogerImgLink { get; set; }
    public float KrogerCost { get; set; }
    public int ItemCount { get; set; }
    public int CountPlacedInCart { get; set; }

    public virtual ICollection<CartRecipe> JoinEntities { get; }
    public virtual ApplicationUser User { get; set; }

    public static string PutInKrogerCart(string token, string body)
    {
      return KrogerAPIHelper.PutProductsInKrogerCart(token, body).ToString();
    }


  }
}
