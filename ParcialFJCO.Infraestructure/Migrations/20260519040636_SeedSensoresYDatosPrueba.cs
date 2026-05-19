using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParcialFJCO.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSensoresYDatosPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SensoresCalidadAire",
                columns: new[] { "Id", "Estado", "TipoGas", "Ubicacion" },
                values: new object[,]
                {
                    { 1, "Activo", "PM2.5/PM10/CO2", "Planta 1 - Zona A" },
                    { 2, "Activo", "PM2.5/CO2", "Planta 1 - Zona B" }
                });

            migrationBuilder.InsertData(
                table: "AlertasAire",
                columns: new[] { "Id", "FechaHora", "Mensaje", "Nivel", "SensorId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 19, 3, 45, 0, 0, DateTimeKind.Utc), "La calidad del aire es poco saludable para grupos sensibles (niños, adultos mayores, personas con enfermedades respiratorias).", "Moderada", 1 },
                    { 2, new DateTime(2026, 5, 19, 3, 50, 0, 0, DateTimeKind.Utc), "Nivel de contaminación extremadamente alto. Riesgo severo para la salud.", "Extrema", 2 }
                });

            migrationBuilder.InsertData(
                table: "LecturasAire",
                columns: new[] { "Id", "CO2", "FechaHora", "PM10", "PM2_5", "SensorId" },
                values: new object[,]
                {
                    { 1, 600m, new DateTime(2026, 5, 19, 3, 40, 0, 0, DateTimeKind.Utc), 20m, 12m, 1 },
                    { 2, 900m, new DateTime(2026, 5, 19, 3, 45, 0, 0, DateTimeKind.Utc), 120m, 80m, 1 },
                    { 3, 6001m, new DateTime(2026, 5, 19, 3, 50, 0, 0, DateTimeKind.Utc), 60m, 40m, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AlertasAire",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AlertasAire",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LecturasAire",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LecturasAire",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LecturasAire",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SensoresCalidadAire",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SensoresCalidadAire",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
