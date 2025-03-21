using ECommerceBook.Models;

using Microsoft.EntityFrameworkCore;

namespace ECommerceBook.DataAcess.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Scifi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }




                );

            modelBuilder.Entity<Product>().HasData(
                            new Product
                            {
                                Id=1,
                                Title = "C# Fundamentals",
                                Description = "Learn the basics of C# programming.",
                                ISBN = "978-1-234567-89-0",
                                Author = "John Doe",
                                ListPrice = 49.99,
                                Price = 45.99,
                                Price50 = 40.99,
                                Price100 = 35.99
                            },
                            new Product
                            {
                                Id=2,
                                Title = "ASP.NET Core Essentials",
                                Description = "A guide to ASP.NET Core development.",
                                ISBN = "978-1-234567-89-1",
                                Author = "Jane Smith",
                                ListPrice = 59.99,
                                Price = 55.99,
                                Price50 = 50.99,
                                Price100 = 45.99
                            },
                            new Product
                            {
                                Id = 3,
                                Title = "Entity Framework Core in Action",
                                Description = "Master EF Core for database interactions.",
                                ISBN = "978-1-234567-89-2",
                                Author = "Mike Brown",
                                ListPrice = 39.99,
                                Price = 35.99,
                                Price50 = 30.99,
                                Price100 = 25.99
                            },
                            new Product
                            {
                                Id = 4,
                                Title = "Blazor WebAssembly Unleashed",
                                Description = "Learn how to build web apps with Blazor.",
                                ISBN = "978-1-234567-89-3",
                                Author = "Sarah Connor",
                                ListPrice = 69.99,
                                Price = 65.99,
                                Price50 = 60.99,
                                Price100 = 55.99
                            },
                            new Product
                            {
                                Id = 5,
                                Title = "Microservices with .NET",
                                Description = "Building scalable microservices using .NET.",
                                ISBN = "978-1-234567-89-4",
                                Author = "Robert Martin",
                                ListPrice = 79.99,
                                Price = 75.99,
                                Price50 = 70.99,
                                Price100 = 65.99
                            }
                        );
        }
    }
}
