using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG_POE_PART_2_AGRI_ENEGRY.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeIDsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMPLOYEE_IDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPLOYEE_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMPLOYEE = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_IDS", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMPLOYEE_IDS");
        }
    }
}
