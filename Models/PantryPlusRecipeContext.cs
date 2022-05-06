using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PantryPlusRecipe.Models
{
  public class PantryPlusRecipeContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    // public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
    public PantryPlusRecipeContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}
