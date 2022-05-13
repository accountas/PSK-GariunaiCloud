using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GariunaiCloud_ToolSharing.Migrations
{
    public partial class AddOptimisticLockingForListings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Listings",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Listings");
        }
    }
}
