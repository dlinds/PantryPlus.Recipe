using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PantryPlusRecipe.Models
{
  public class PantryPlusRecipeContextFactory : IDesignTimeDbContextFactory<PantryPlusRecipeContext>
  {

    PantryPlusRecipeContext IDesignTimeDbContextFactory<PantryPlusRecipeContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var builder = new DbContextOptionsBuilder<PantryPlusRecipeContext>();

      builder.UseMySql(configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"]));

      return new PantryPlusRecipeContext(builder.Options);
    }
  }
}
