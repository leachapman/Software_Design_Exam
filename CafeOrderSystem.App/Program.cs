using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Software_Design_Exam.Data;
using Software_Design_Exam.Data.Repositories;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Services;
using SQLitePCL;

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

// OrderService
var orderService = new OrderService(productRepository, orderRepository);



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


Console.WriteLine("=======  Main Menu  ======");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 = New Order");
    Console.WriteLine("0 = Exit");
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