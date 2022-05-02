using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GariunaiCloud_ToolSharing.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    ListingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaysPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OwnerUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.ListingId);
                    table.ForeignKey(
                        name: "FK_Listings_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PhoneNumber", "UserName" },
                values: new object[] { 1L, "mail1.com", "+1233567812", "Useris1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PhoneNumber", "UserName" },
                values: new object[] { 2L, "mail2.com", "+1233567812", "Useris2" });

            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "ListingId", "DaysPrice", "Deposit", "Description", "OwnerUserId", "Title" },
                values: new object[,]
                {
                    { 1L, 123m, 100m, "alskdjalkdjalskdjalksjdalskdjalksjdwiad", 1L, "Golden Shovel" },
                    { 2L, 12m, 10m, "AAAAAAAAAAAAAAAAAAAAAAAA", 2L, "Irankis" },
                    { 3L, 1m, 100m, "DAAAAAAAAAAAAAAAAAAAUG TEEEEEEEEEEEEEEEEEEKSTO", 1L, "Pjuklas" },
                    { 4L, 0m, 1m, "Labai pigu!!!!!!!!!!!!", 2L, "Dantu krapstukas" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_OwnerUserId",
                table: "Listings",
                column: "OwnerUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
