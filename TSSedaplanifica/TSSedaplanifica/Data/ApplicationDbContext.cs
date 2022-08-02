using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Admi");

            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .HasDatabaseName("IX_Category_Name")
                .IsUnique();

            builder.Entity<CategoryType>()
                .HasIndex(c => c.Name)
                .HasDatabaseName("IX_CategoryType_Name")
                .IsUnique();

            builder.Entity<CategoryTypeDer>()
                .HasIndex("CategoryId", "CategoryTypeId")
                .HasDatabaseName("IX_CategoryTypeDer_CategoryIdCategoryTypeId")
                .IsUnique();

            builder.Entity<City>()
                .HasIndex("StateId", "Name")
                .HasDatabaseName("IX_State_City_Name")
                .IsUnique();

            builder.Entity<Country>()
                .HasIndex(x => x.Name)
                .HasDatabaseName("IX_Country_Name")
                .IsUnique();

            builder.Entity<MeasureUnit>()
                .HasIndex(x => x.Name)
                .HasDatabaseName("IX_MeasureUnit_Name")
                .IsUnique();

            builder.Entity<Product>()
                .HasIndex(p => p.Name)
                .HasDatabaseName("IX_Product_Name")
                .IsUnique();

            builder.Entity<ProductCategory>()
                .HasIndex("CategoryId", "ProductId")
                .HasDatabaseName("IX_ProductCategory_CategoryIdProductId")
                .IsUnique();

            builder.Entity<School>()
                .HasIndex(s=> new {s.Name,s.DaneCode })
                .HasDatabaseName("IX_name_DaneCode")
                .IsUnique();

            builder.Entity<SolicitDetail>()
                .HasIndex("SolicitId", "ProductId")
                .HasDatabaseName("IX_SolicitDetail_Solicit_Product_Id")
                .IsUnique();

            builder.Entity<SolicitState>()
                .HasIndex(s => s.Name)
                .HasDatabaseName("IX_SolicitState_Name")
                .IsUnique();

            builder.Entity<State>()
                .HasIndex("CountryId", "Name")
                .HasDatabaseName("IX_Country_State_Name")
                .IsUnique();

            builder.Entity<Zone>()
                .HasIndex(z => z.Name)
                .HasDatabaseName("IX_Zone_Name")
                .IsUnique();

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<CategoryTypeDer> CategoryTypeDers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolImage> SchoolImages { get; set; }
        public DbSet<SchoolUser> SchoolUsers { get; set; }
        public DbSet<Solicit> Solicits { get; set; }
        public DbSet<SolicitDetail> SolicitDetails { get; set; }
        public DbSet<SolicitState> SolicitStates { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Zone> Zones { get; set; }
    }
}