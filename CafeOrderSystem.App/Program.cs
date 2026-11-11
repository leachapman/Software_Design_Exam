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
var productsRepository = new ProductRepository(db);
var orderRepository = new OrderRepository(db);
var userRepository = new UserRepository(db);

// Services
var _orderService = new OrderService(productsRepository, orderRepository);
var productService = new ProductService(productsRepository);

var loginMenu = new LoginMenu(userRepository);
var orderMenu = new OrderMenu(_orderService, productsRepository);



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
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== Manage Products =====");
            Console.WriteLine();
            Console.WriteLine("Show all product (incl. removed) = 1");
            Console.WriteLine("Create product = 2");
            Console.WriteLine("Change Price = 3");
            Console.WriteLine("Rename = 4");
            Console.WriteLine("Remove from menu = 5");
            Console.WriteLine("Add back to menu = 6");
            Console.WriteLine("Back = 0");
            Console.WriteLine();

            var choiceManager = ConsoleInput.ReadInt("Choice: ");

            if (choiceManager == 0)  
            {
                break; 
            }

            switch (choiceManager)
            {

                case 1:
                {
                    Console.WriteLine();
                    Console.WriteLine("======  All products  ======");
                    Console.WriteLine();
                    foreach(var p in productsRepository.GetAllProducts())
                    {
                        Console.WriteLine($"{p.Id,3}  {p.Name,-20} {p.Category,-15} {p.Price,6:0.00} € {(p.IsAvailable ? "" : "(removed)")}" );
                    }
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                }
                
                case 2:
                {
                    Console.WriteLine();
                    Console.WriteLine("======  Create product  ======");
                    Console.WriteLine();
                    
                    Console.Write("Name: ");
                    var name = Console.ReadLine();

                    var price = ConsoleInput.ReadDecimal("Price (use ',' instead of '.'): ");

                    var category = ConsoleInput.ReadCategory();

                    ConsoleHelper.RunSafely(() => 
                    {
                        productService.CreateProduct(user, name!, price, category);
                        Console.WriteLine("Product created.");
                    });

                    break;
                }
                case 3:
                {
                    Console.WriteLine();
                    Console.WriteLine("======  Change Price  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");
                    var newPrice = ConsoleInput.ReadDecimal("New price (use ',' instead of '.'): ");

                    ConsoleHelper.RunSafely(() =>
                    {
                        productService.ChangePrice(user, id, newPrice);
                        Console.WriteLine("Price changed.");
                    });
                
                    break;
                }

                case 4:
                {
                    Console.WriteLine();
                    Console.WriteLine("======  Rename  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");
                    Console.Write("Name: ");
                    var newName = Console.ReadLine();

                    ConsoleHelper.RunSafely(() =>
                    {
                        productService.Rename(user, id, newName!);
                        Console.WriteLine("Name changed.");
                    });
                
                    break;
                }

                case 5:
                {
                    Console.WriteLine();
                    Console.WriteLine("======  Remove from Menu  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");

                    ConsoleHelper.RunSafely(() =>
                    {
                        productService.RemoveFromMenu(user, id);
                        Console.WriteLine("Removed from Menu.");
                    });

                    break;
                }

                case 6:
                {
                    Console.WriteLine();
                    Console.WriteLine("======  Add back to Menu  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");

                    ConsoleHelper.RunSafely(() =>
                    {
                        productService.AddToMenu(user, id);
                        Console.WriteLine("Added back to Menu.");
                    });

                    break;
                }
                default:
                    Console.WriteLine("Invalid input.");
                    break;         
            }          
        }
    }
    else
    {
        Console.WriteLine("Invalid Input");
    }
}