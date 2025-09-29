using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Autores",
                columns: new[] { "AutorId", "Email", "Nome" },
                values: new object[,]
                {
                    { 1, "ana.silva@email.com", "Ana C. Silva" },
                    { 2, "bruno.alves@email.com", "Bruno Alves" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "AutorId", "Conteudo", "DataCriacao", "Titulo" },
                values: new object[,]
                {
                    { 1, 1, "C# é uma linguagem poderosa e versátil...", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Primeiros Passos com C#" },
                    { 2, 1, "A Programação Orientada a Objetos é fundamental...", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "POO em C# na Prática" },
                    { 3, 2, "ASP.NET Core permite criar aplicações web modernas...", new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Introdução ao ASP.NET Core" },
                    { 4, 2, "LINQ revolucionou a forma como consultamos dados...", new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Consultas com LINQ" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Autores",
                keyColumn: "AutorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Autores",
                keyColumn: "AutorId",
                keyValue: 2);
        }
    }
}
