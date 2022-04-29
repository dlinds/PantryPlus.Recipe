// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// using PantryPlusRecipe.Models;


// namespace PantryPlusRecipe.Models
// {
//   public class PantryPlusRecipe : IdentityDbContext<ApplicationUser>
//   {
//     public DbSet<ApplicationUser> ApplicationUsers { get; set; }
//     public DbSet<Token> Tokens { get; set; }
//     public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
//     public PantryPlusRecipe(DbContextOptions options) : base(options) { }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//       optionsBuilder.UseLazyLoadingProxies();
//     }
//   }
// }
