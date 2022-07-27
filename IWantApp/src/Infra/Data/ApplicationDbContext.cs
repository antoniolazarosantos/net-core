using IWantApp.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //FluentAPI
        builder.Entity<Product>().Property(p => p.Description).HasMaxLength(255).IsRequired(false);
        builder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();
        builder.Entity<Category>().ToTable("Categories");
        builder.Entity<Category>().Property(c => c.Name).IsRequired();
        //builder.Entity<Category>().Property(p => p.Name).HasMaxLength(20).IsRequired();
        //builder.Entity<Tag>()
        //    .Property(p => p.Nome)
        //    .HasMaxLength(20)
        //    .IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
            .HaveMaxLength(100);

    }
}
