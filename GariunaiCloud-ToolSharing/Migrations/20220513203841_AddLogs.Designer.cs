// <auto-generated />
using System;
using GariunaiCloud_ToolSharing.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GariunaiCloud_ToolSharing.Migrations
{
    [DbContext(typeof(GariunaiDbContext))]
    [Migration("20220513203841_AddLogs")]
    partial class AddLogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.AccessLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Method")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AccessLogs");
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.DbImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.Listing", b =>
                {
                    b.Property<long>("ListingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ListingId"), 1L, 1);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DaysPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Hidden")
                        .HasColumnType("bit");

                    b.Property<long?>("ImageId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OwnerUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ListingId");

                    b.HasIndex("ImageId");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("Listings");

                    b.HasData(
                        new
                        {
                            ListingId = 1L,
                            City = "Klaipėda",
                            DaysPrice = 123m,
                            Deposit = 100m,
                            Description = "alskdjalkdjalskdjalksjdalskdjalksjdwiad",
                            Hidden = false,
                            OwnerUserId = 1L,
                            Title = "Golden Shovel"
                        },
                        new
                        {
                            ListingId = 2L,
                            City = "Vilnius",
                            DaysPrice = 12m,
                            Deposit = 10m,
                            Description = "AAAAAAAAAAAAAAAAAAAAAAAA",
                            Hidden = false,
                            OwnerUserId = 2L,
                            Title = "Irankis"
                        },
                        new
                        {
                            ListingId = 3L,
                            City = "Kaunas",
                            DaysPrice = 1m,
                            Deposit = 100m,
                            Description = "DAAAAAAAAAAAAAAAAAAAUG TEEEEEEEEEEEEEEEEEEKSTO",
                            Hidden = false,
                            OwnerUserId = 1L,
                            Title = "Pjuklas"
                        },
                        new
                        {
                            ListingId = 4L,
                            City = "Balbieriškis",
                            DaysPrice = 0m,
                            Deposit = 1m,
                            Description = "Labai pigu!!!!!!!!!!!!",
                            Hidden = true,
                            OwnerUserId = 2L,
                            Title = "Dantu krapstukas"
                        });
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.Order", b =>
                {
                    b.Property<long>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("OrderId"), 1L, 1);

                    b.Property<DateTime?>("CompletionDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ConfirmationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("Date");

                    b.Property<long?>("ListingId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("PlacementDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("Date");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("OrderId");

                    b.HasIndex("ListingId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1L,
                            Email = "mail1.com",
                            PhoneNumber = "+1233567812",
                            Role = 0,
                            UserName = "Useris1"
                        },
                        new
                        {
                            UserId = 2L,
                            Email = "mail2.com",
                            PhoneNumber = "+1233567812",
                            Role = 0,
                            UserName = "Useris2"
                        });
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.Listing", b =>
                {
                    b.HasOne("GariunaiCloud_ToolSharing.Models.DbImage", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("GariunaiCloud_ToolSharing.Models.User", "Owner")
                        .WithMany("Listings")
                        .HasForeignKey("OwnerUserId");

                    b.Navigation("Image");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.Order", b =>
                {
                    b.HasOne("GariunaiCloud_ToolSharing.Models.Listing", "Listing")
                        .WithMany("Orders")
                        .HasForeignKey("ListingId");

                    b.HasOne("GariunaiCloud_ToolSharing.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.Navigation("Listing");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.Listing", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("GariunaiCloud_ToolSharing.Models.User", b =>
                {
                    b.Navigation("Listings");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
