using System.Collections.Generic;
using System;
namespace PantryPlusRecipe.Models
{
  public class Token
  {
    // public Token()
    // {
    //   this.JoinEntities = new HashSet<ApplicationUserToken>();
    // }
    public int TokenId { get; set; }
    public DateTime TokenValueExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public string TokenValue { get; set; }
    public string RefreshToken { get; set; }
    public string TokenAuthType { get; set; }

    // public virtual ICollection<ApplicationUserToken> JoinEntities { get; }
    public virtual ApplicationUser User { get; set; }
  }
}
