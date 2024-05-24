using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG_POE_PART_2_AGRI_ENEGRY.Data.Migrations
{
    /// <inheritdoc />
    public partial class FullUserInput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CellPhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellPhoneNumber",
                table: "AspNetUsers");
        }
    }
}
