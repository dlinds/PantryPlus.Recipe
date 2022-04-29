using System.Collections.Generic;

namespace PantryPlusRecipe.Models
{
  public class Token
  {
    public Token()
    {
      this.JoinEntities = new HashSet<ApplicationUserToken>();
    }
    public int TokenId { get; set; }
    public int ExpiresIn { get; set; }
    public string TokenValue { get; set; }
    public string RefreshToken { get; set; }


    public virtual ICollection<ApplicationUserToken> JoinEntities { get; }
    public virtual ApplicationUser User { get; set; }
  }
}
