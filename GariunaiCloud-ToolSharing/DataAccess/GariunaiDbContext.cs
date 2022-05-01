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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        _seedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void _seedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, Email = "mail1.com", UserName = "Useris1", PhoneNumber = "+1233567812" },
            new User { UserId = 2, Email = "mail2.com", UserName = "Useris2", PhoneNumber = "+1233567812" });

        modelBuilder.Entity<Listing>().HasData(
            new
            {
                ListingId = 1L,
                DaysPrice = 123m,
                Deposit = 100m, 
                Title = "Golden Shovel",
                Description = "alskdjalkdjalskdjalksjdalskdjalksjdwiad", 
                OwnerUserId = 1L,
                City = "Klaipėda"
            },
            new
            {
                ListingId = 2L,
                DaysPrice = 12m, 
                Deposit = 10m, 
                Title = "Irankis", 
                Description = "AAAAAAAAAAAAAAAAAAAAAAAA",
                OwnerUserId = 2L,
                City = "Vilnius"
            },
            new
            {
                ListingId = 3L,
                DaysPrice = 1m, Deposit = 100m,
                Title = "Pjuklas",
                Description = "DAAAAAAAAAAAAAAAAAAAUG TEEEEEEEEEEEEEEEEEEKSTO",
                OwnerUserId = 1L,
                City = "Kaunas"
            },
            new
            {
                ListingId = 4L,
                DaysPrice = 0m, 
                Deposit = 1m,
                Title = "Dantu krapstukas",
                Description = "Labai pigu!!!!!!!!!!!!",
                OwnerUserId = 2L,
                City = "Balbieriškis"
            }
        );
    }
}