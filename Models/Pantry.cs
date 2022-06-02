using System.Collections.Generic;
using System;
namespace PantryPlusRecipe.Models
{
  public class Pantry
  {
    public int PantryId { get; set; }
    public string KrogerCategory { get; set; }
    public string KrogerUPC { get; set; }
    public string KrogerItemName { get; set; }
    public string KrogerItemSize { get; set; }
    public string KrogerAisle { get; set; }
    public string KrogerImgLink { get; set; }
    public float KrogerCost { get; set; }
    public int ItemCount { get; set; }
    public string SearchTermWhenAdded { get; set; }

    public virtual ApplicationUser User { get; set; }
  }
}
