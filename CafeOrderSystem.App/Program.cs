using Microsoft.EntityFrameworkCore;
using Software_Design_Exam.Data;
using Software_Design_Exam.Data.Repositories;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Services;
using Software_Design_Exam.Domain.Enums;


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
var orderService = new OrderService(productRepository, orderRepository);
var productService = new ProductService(productRepository);



// Login 
var users = userRepository.GetAll();
Console.WriteLine("=======  Login  ======");

foreach (var u in users)
{
    Console.WriteLine($"{u.Id}: {u.Name} ({u.Role})");
}
User? user = null;
while (user == null)
{
    var id = ReadInt("Your Id: ");
    user = userRepository.GetById(id);
    if (user == null)
    {
        Console.WriteLine("No employee with this Id found.");
    }
}
Console.WriteLine($"Welcome {user.Name}");




while (true)
{
    Console.WriteLine("=======  Main Menu  ======");
    Console.WriteLine();
    Console.WriteLine("1 = New Order");
    if(user.Role == UserRole.Manager)
        Console.WriteLine("2 = Manage products");
    Console.WriteLine("0 = Exit");
    Console.WriteLine();
    var choice = ReadInt("Choice: ");

    if (choice == 0)
    {
        break;
    }
    else if (choice == 1)
    {
        var order = orderService.StartOrder(user, DateTime.Now);

        while (true)
        {
            Console.WriteLine("======  Menu  ======");
            Console.WriteLine();
            foreach(var p in productRepository.GetMenu())
            {
                Console.WriteLine($"{p.Id,3}  {p.Name,-20} {p.Category,-15} {p.Price,6:0.00} €");
            }
            
            var productId = ReadInt("Produkt-Id (0 = finished): ");
            if (productId == 0)
            {
                break;
            }

            var quantity = ReadInt("Quantity: ");

            try
            {
                orderService.AddProduct(order, productId, quantity);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine($"======  Current order  ======");
            Console.WriteLine();
            foreach (var item in order.Items)
            {
                Console.WriteLine($"{item.Quantity} x {item.Product.Name,-20} {item.IntermediatePrice,6:0.00} €");
            }
            Console.WriteLine($"Total: {order.TotalPrice:0.00} €");

        }
        string? choiceToFinish = null;
        while (choiceToFinish != "c" && choiceToFinish != "q")
        {
            Console.Write("Complete (c) or Cancel (q)? ");
            choiceToFinish = Console.ReadLine()?.Trim().ToLower();
        }

        try
        {
            if (choiceToFinish == "c")
            {
                orderService.CompleteOrder(order);
                Console.WriteLine("===== Order saved. =====");
                Console.WriteLine();
                Console.WriteLine("The finished order:");
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"{item.Quantity} x {item.Product.Name,-20} {item.IntermediatePrice,6:0.00} €");
                }
                Console.WriteLine($"Total: {order.TotalPrice:0.00} €");

            }
            else
            {
                orderService.CancelOrder(order);
                Console.WriteLine("===== Order cancelled. =====");
            }
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
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

            var choiceManager = ReadInt("Choice: ");

            if (choiceManager == 0)  
            {
                break; 
            }

            switch (choiceManager)
            {

                case 1:
                {
                    Console.WriteLine("======  All products  ======");
                    Console.WriteLine();
                    foreach(var p in productRepository.GetAllProducts())
                    {
                        Console.WriteLine($"{p.Id,3}  {p.Name,-20} {p.Category,-15} {p.Price,6:0.00} € {(p.IsAvailable ? "" : "(removed)")}" );
                    }
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                }
                
                case 2:
                    {Console.WriteLine("======  Create product  ======");
                    Console.WriteLine();
                    
                    Console.Write("Name: ");
                    var name = Console.ReadLine();

                    var price = ReadDecimal("Price: ");

                    Console.WriteLine("Category:");
                    foreach (var c in Enum.GetValues<ProductCategory>())
                    {
                        Console.WriteLine($"  {(int)c} = {c}");
                    }

                    ProductCategory category;
                    while (true)
                    {
                        var number = ReadInt("Category number: ");
                        if (Enum.IsDefined(typeof(ProductCategory), number))
                        {
                            category = (ProductCategory)number;
                            break;
                        }
                        Console.WriteLine("Invalid category.");
                    }

                    try
                    {
                        productService.CreateProduct(user, name!, price, category);
                        Console.WriteLine("Product created.");
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    break;
                }
                case 3:
                {
                    Console.WriteLine("======  Change Price  ======");
                    Console.WriteLine();
                    var id = ReadInt("Id: ");
                    var newPrice = ReadDecimal("New price (use ',' instead of '.'): ");

                    try
                    {
                        productService.ChangePrice(user, id, newPrice);
                        Console.WriteLine("Price changed.");
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                }

                case 4:
                {
                    Console.WriteLine("======  Rename  ======");
                    Console.WriteLine();
                    var id = ReadInt("Id: ");
                    Console.Write("Name: ");
                    var newName = Console.ReadLine();

                    try
                    {
                        productService.Rename(user, id, newName!    );
                        Console.WriteLine("Name changed.");
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                }

                case 5:
                {
                    Console.WriteLine("======  Remove from Menu  ======");
                    Console.WriteLine();
                    var id = ReadInt("Id: ");

                    try
                    {
                        productService.RemoveFromMenu(user, id);
                        Console.WriteLine("Removed from Menu.");
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                }

                case 6:
                {
                    Console.WriteLine("======  Add back to Menu  ======");
                    Console.WriteLine();
                    var id = ReadInt("Id: ");
                    try
                    {
                        productService.AddToMenu(user, id);
                        Console.WriteLine("Added back to Menu.");
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
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


static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var value))
            return value;
        Console.WriteLine("Please enter a valid number.");
    }
}

static decimal ReadDecimal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (decimal.TryParse(Console.ReadLine(), out var value))
            return value;
        Console.WriteLine("Please enter a valid number (e.g. 3.50).");
    }
}