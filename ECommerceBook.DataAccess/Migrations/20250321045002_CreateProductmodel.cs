using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateProductmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "John Doe", "Learn the basics of C# programming.", "978-1-234567-89-0", 49.990000000000002, 45.990000000000002, 35.990000000000002, 40.990000000000002, "C# Fundamentals" },
                    { 2, "Jane Smith", "A guide to ASP.NET Core development.", "978-1-234567-89-1", 59.990000000000002, 55.990000000000002, 45.990000000000002, 50.990000000000002, "ASP.NET Core Essentials" },
                    { 3, "Mike Brown", "Master EF Core for database interactions.", "978-1-234567-89-2", 39.990000000000002, 35.990000000000002, 25.989999999999998, 30.989999999999998, "Entity Framework Core in Action" },
                    { 4, "Sarah Connor", "Learn how to build web apps with Blazor.", "978-1-234567-89-3", 69.989999999999995, 65.989999999999995, 55.990000000000002, 60.990000000000002, "Blazor WebAssembly Unleashed" },
                    { 5, "Robert Martin", "Building scalable microservices using .NET.", "978-1-234567-89-4", 79.989999999999995, 75.989999999999995, 65.989999999999995, 70.989999999999995, "Microservices with .NET" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
