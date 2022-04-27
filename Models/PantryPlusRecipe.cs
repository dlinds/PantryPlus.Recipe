using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PantryPlusRecipe.Models
{
  public class PantryPlusRecipeContext : IdentityDbContext<ApplicationUser>
  {
    public PantryPlusRecipeContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}
