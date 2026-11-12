using Microsoft.EntityFrameworkCore;
using Software_Design_Exam.Data;
using Software_Design_Exam.Data.Repositories;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Services;
using Software_Design_Exam.Domain.Enums;
using Software_Design_Exam.Domain.Discounts;
using Software_Design_Exam.App.UI;


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
var orderRepository = new OrderRepository(db);
var userRepository = new UserRepository(db);

// Services
var _orderService = new OrderService(productRepository, orderRepository);
var productService = new ProductService(productRepository);
var revenueCalculator = new RevenueCalculator(orderRepository);

var loginMenu = new LoginMenu(userRepository);
var orderMenu = new OrderMenu(_orderService, productRepository);
var productMenu = new ProductManagementMenu(productService, productRepository);
var revenueMenu = new RevenueMenu(revenueCalculator, userRepository);



// Login 
var user = loginMenu.Run();


// Main Menu

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=======  Main Menu  ======");
    Console.WriteLine();
    Console.WriteLine("1 = New Order");
    if(user.Role == UserRole.Manager)
        Console.WriteLine("2 = Manage products");
    if(user.Role == UserRole.Manager)
    {
        Console.WriteLine("3 = Revenue today");
    }
    Console.WriteLine("0 = Exit");
    Console.WriteLine();
    var choice = ConsoleInput.ReadInt("Choice: ");

    if (choice == 0)
    {
        break;
    }
    else if (choice == 1)
    {
        orderMenu.Run(user);
    }
    else if (choice == 2 && user.Role == UserRole.Manager)
    {
        productMenu.Run(user);
    }
    else if (choice == 3 && user.Role == UserRole.Manager)
    {
        revenueMenu.Run(user);
    }
    else
    {
        Console.WriteLine("Invalid Input");
    }
}