using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;

namespace GariunaiCloud_ToolSharing.DataAccess;

#nullable disable
public class GariunaiDbContext : DbContext
{
    public GariunaiDbContext(DbContextOptions<GariunaiDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<DbImage> Images { get; set; }
    public DbSet<AccessLog> AccessLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<DbImage>()
            .HasIndex(u => u.Name)
            .IsUnique();
        
        _seedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void _seedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, Email = "mail1.com", Username = "Useris1", PhoneNumber = "+1233567812" },
            new User { UserId = 2, Email = "mail2.com", Username = "Useris2", PhoneNumber = "+1233567812" });

        modelBuilder.Entity<Listing>().HasData(
            new
            {
                ListingId = 1L,
                DaysPrice = 123m,
                Deposit = 100m, 
                Title = "Golden Shovel",
                Description = "alskdjalkdjalskdjalksjdalskdjalksjdwiad", 
                OwnerUserId = 1L,
                City = "Klaipėda",
                Hidden = false
            },
            new
            {
                ListingId = 2L,
                DaysPrice = 12m, 
                Deposit = 10m, 
                Title = "Irankis", 
                Description = "AAAAAAAAAAAAAAAAAAAAAAAA",
                OwnerUserId = 2L,
                City = "Vilnius",
                Hidden = false
            },
            new
            {
                ListingId = 3L,
                DaysPrice = 1m, Deposit = 100m,
                Title = "Pjuklas",
                Description = "DAAAAAAAAAAAAAAAAAAAUG TEEEEEEEEEEEEEEEEEEKSTO",
                OwnerUserId = 1L,
                City = "Kaunas",
                Hidden = false
            },
            new
            {
                ListingId = 4L,
                DaysPrice = 0m, 
                Deposit = 1m,
                Title = "Dantu krapstukas",
                Description = "Labai pigu!!!!!!!!!!!!",
                OwnerUserId = 2L,
                City = "Balbieriškis",
                Hidden = true
            }
        );
    }
}