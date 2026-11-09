using Microsoft.EntityFrameworkCore;
using Software_Design_Exam.Data;
using Software_Design_Exam.Data.Repositories;
using Software_Design_Exam.Domain.Entities;
Console.OutputEncoding = System.Text.Encoding.UTF8;

// path for database file, next to the program, works on every device
var dbPath = Path.Combine(AppContext.BaseDirectory, "cafe.db");

// create DbContext with SQLite
var options = new DbContextOptionsBuilder<CafeDbContext>()
    .UseSqlite($"Data Source={dbPath}")
    .Options;
using var db = new CafeDbContext(options);

// create and populate a database
db.Database.EnsureCreated();
CafeSeeder.Seed(db);

// create repository
var productRepository = new ProductRepository(db);

// display menu
var menu = productRepository.GetMenu();
Console.WriteLine("=======  Menu  ======");
foreach(Product p in menu)
{
    Console.WriteLine($"{p.Name,-20} {p.Category,-15} {p.Price,6:0.00} €");
}