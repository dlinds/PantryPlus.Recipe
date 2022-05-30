using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PantryPlusRecipe.Models
{
  public class PantryPlusRecipeContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<IngredientRecipe> IngredientRecipes { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<StepRecipe> StepRecipes { get; set; }
    public DbSet<Pantry> Pantry { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<HelloFreshToken> HelloFreshTokens { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartRecipe> CartRecipe { get; set; }

    // public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
    public PantryPlusRecipeContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}
