using System.Collections.Generic;
using System;
namespace PantryPlusRecipe.Models
{
  public class Pantry
  {
    // public Token()
    // {
    //   this.JoinEntities = new HashSet<ApplicationUserToken>();
    // }
    public int PantryId { get; set; }
    public string Name { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Category { get; set; }
    public string WeightCount { get; set; }


    // public virtual ICollection<ApplicationUserToken> JoinEntities { get; }
    public virtual ApplicationUser User { get; set; }
  }
}
