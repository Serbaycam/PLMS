using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PLMS.Core.Entities;
using PLMS.Core.Entity;
using System.Reflection;
namespace PLMS.Repository.Contexts;

public class PLMSDbContext(DbContextOptions<PLMSDbContext> options) : IdentityDbContext<AuthIdentityUser, AuthIdentityRole, string>(options)
{
    DbSet<Color> Colors { get; set; }
    DbSet<Model> Models { get; set; }
    DbSet<ModelGroup> ModelGroups { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<OrderItemRecipe> OrderItemRecipes { get; set; }
    DbSet<Size> Sizes { get; set; }
    DbSet<SizeSet> SizeSets { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
