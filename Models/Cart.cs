using System.Collections.Generic;
using System;
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

    public virtual ICollection<CartRecipe> JoinEntities { get; }

    public virtual ApplicationUser User { get; set; }
  }
}
