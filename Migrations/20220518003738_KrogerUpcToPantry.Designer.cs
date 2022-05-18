﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PantryPlusRecipe.Models;

namespace PantryPlusRecipe.Migrations
{
    [DbContext(typeof(PantryPlusRecipeContext))]
    [Migration("20220518003738_KrogerUpcToPantry")]
    partial class KrogerUpcToPantry
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("KrogerId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("KrogerStoreId")
                        .HasColumnType("int");

                    b.Property<string>("KrogerStoreName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CountPlacedInCart")
                        .HasColumnType("int");

                    b.Property<int>("ItemCount")
                        .HasColumnType("int");

                    b.Property<string>("KrogerAisle")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<float>("KrogerCost")
                        .HasColumnType("float");

                    b.Property<string>("KrogerImgLink")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("KrogerItemName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("KrogerItemSize")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("KrogerUPC")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("CartId");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.CartRecipe", b =>
                {
                    b.Property<int>("CartRecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("CartRecipeId");

                    b.HasIndex("CartId");

                    b.HasIndex("RecipeId");

                    b.ToTable("CartRecipe");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float>("Count")
                        .HasColumnType("float");

                    b.Property<string>("Measurement")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("IngredientId");

                    b.HasIndex("UserId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.IngredientRecipe", b =>
                {
                    b.Property<int>("IngredientRecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("IngredientId")
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("IngredientRecipeId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("IngredientRecipes");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Pantry", b =>
                {
                    b.Property<int>("PantryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("KrogerUPC")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("WeightCount")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("PantryId");

                    b.HasIndex("UserId");

                    b.ToTable("Pantry");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("CookMinutes")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<string>("ImgUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("NumberOfSections")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfSteps")
                        .HasColumnType("int");

                    b.Property<int>("PrepMinutes")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("RecipeId");

                    b.HasIndex("UserId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Step", b =>
                {
                    b.Property<int>("StepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SectionName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SectionNumber")
                        .HasColumnType("int");

                    b.Property<int>("StepNumber")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("StepId");

                    b.HasIndex("UserId");

                    b.ToTable("Steps");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.StepRecipe", b =>
                {
                    b.Property<int>("StepRecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("StepId")
                        .HasColumnType("int");

                    b.HasKey("StepRecipeId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("StepId");

                    b.ToTable("StepRecipes");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Token", b =>
                {
                    b.Property<int>("TokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("RefreshTokenExpiresAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TokenAuthType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TokenValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("TokenValueExpiresAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("TokenId");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Cart", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.CartRecipe", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.Cart", "Cart")
                        .WithMany("JoinEntities")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryPlusRecipe.Models.Recipe", "Recipe")
                        .WithMany("JoinEntitiesCart")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Ingredient", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.IngredientRecipe", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.Ingredient", "Ingredient")
                        .WithMany("JoinEntities")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryPlusRecipe.Models.Recipe", "Recipe")
                        .WithMany("JoinEntitiesIngredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Pantry", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Recipe", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Step", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.StepRecipe", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.Recipe", "Recipe")
                        .WithMany("JoinEntitiesSteps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryPlusRecipe.Models.Step", "Step")
                        .WithMany("JoinEntities")
                        .HasForeignKey("StepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("Step");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Token", b =>
                {
                    b.HasOne("PantryPlusRecipe.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Cart", b =>
                {
                    b.Navigation("JoinEntities");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Ingredient", b =>
                {
                    b.Navigation("JoinEntities");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Recipe", b =>
                {
                    b.Navigation("JoinEntitiesCart");

                    b.Navigation("JoinEntitiesIngredients");

                    b.Navigation("JoinEntitiesSteps");
                });

            modelBuilder.Entity("PantryPlusRecipe.Models.Step", b =>
                {
                    b.Navigation("JoinEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
