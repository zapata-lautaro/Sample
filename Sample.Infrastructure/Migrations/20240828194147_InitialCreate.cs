using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "persons",
            columns: table => new
            {
                id = table.Column<string>(type: "text", nullable: false),
                name_first = table.Column<string>(type: "text", nullable: false),
                name_last = table.Column<string>(type: "text", nullable: false),
                language = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                bio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                version = table.Column<double>(type: "double precision", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_persons", x => x.id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "persons");
    }
}
