using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcialFJCO.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAirQualityEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensoresCalidadAire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TipoGas = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensoresCalidadAire", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlertasAire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertasAire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertasAire_SensoresCalidadAire_SensorId",
                        column: x => x.SensorId,
                        principalTable: "SensoresCalidadAire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LecturasAire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    PM2_5 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PM10 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CO2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturasAire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LecturasAire_SensoresCalidadAire_SensorId",
                        column: x => x.SensorId,
                        principalTable: "SensoresCalidadAire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertasAire_SensorId",
                table: "AlertasAire",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturasAire_SensorId",
                table: "LecturasAire",
                column: "SensorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasAire");

            migrationBuilder.DropTable(
                name: "LecturasAire");

            migrationBuilder.DropTable(
                name: "SensoresCalidadAire");
        }
    }
}
