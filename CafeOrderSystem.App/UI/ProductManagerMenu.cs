using System.Runtime.CompilerServices;
using Software_Design_Exam.Domain.Discounts;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;
using Software_Design_Exam.Domain.Services;

namespace Software_Design_Exam.App.UI;

public class ProductManagementMenu
{
    private readonly ProductService _productService;
    private readonly IProductRepository _productRepository;

    public ProductManagementMenu(ProductService productService, IProductRepository productRepository)
    {
        _productService = productService;
        _productRepository = productRepository;
    }

    public void Run (User user)
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

            var choice = ConsoleInput.ReadInt("Choice: ");

            if (choice == 0)  
            {
                break; 
            }

            switch (choice)
            {
                case 1: ShowAllProducts(); break;               
                case 2: CreateProduct(user); break;
                case 3: ChangePrice(user);break;
                case 4: Rename(user); break;
                case 5: RemoveFromMenu(user); break;
                case 6: AddBackToMenu(user); break;
                default:  Console.WriteLine("Invalid input."); break;                            
            }          
        }
    }

    public void ShowAllProducts()
    {
        Console.WriteLine();
                    Console.WriteLine("======  All products  ======");
                    Console.WriteLine();
                    foreach(var p in _productRepository.GetAllProducts())
                    {
                        Console.WriteLine($"{p.Id,3}  {p.Name,-20} {p.Category,-15} {p.Price,6:0.00} € {(p.IsAvailable ? "" : "(removed)")}" );
                    }
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
    }

    public void CreateProduct(User user)
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
                        _productService.CreateProduct(user, name!, price, category);
                        Console.WriteLine("Product created.");
                    });
    }

    public void ChangePrice(User user)
    {
        Console.WriteLine();
                    Console.WriteLine("======  Change Price  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");
                    var newPrice = ConsoleInput.ReadDecimal("New price (use ',' instead of '.'): ");

                    ConsoleHelper.RunSafely(() =>
                    {
                        _productService.ChangePrice(user, id, newPrice);
                        Console.WriteLine("Price changed.");
                    });
    }

    public void Rename(User user)
    {
        Console.WriteLine();
                    Console.WriteLine("======  Rename  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");
                    Console.Write("Name: ");
                    var newName = Console.ReadLine();

                    ConsoleHelper.RunSafely(() =>
                    {
                        _productService.Rename(user, id, newName!);
                        Console.WriteLine("Name changed.");
                    });
    }

    public void RemoveFromMenu(User user)
    {
        Console.WriteLine();
                    Console.WriteLine("======  Remove from Menu  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");

                    ConsoleHelper.RunSafely(() =>
                    {
                        _productService.RemoveFromMenu(user, id);
                        Console.WriteLine("Removed from Menu.");
                    });

    }

    public void AddBackToMenu(User user)
    {
        Console.WriteLine();
                    Console.WriteLine("======  Add back to Menu  ======");
                    Console.WriteLine();
                    var id = ConsoleInput.ReadInt("Id: ");

                    ConsoleHelper.RunSafely(() =>
                    {
                        _productService.AddToMenu(user, id);
                        Console.WriteLine("Added back to Menu.");
                    });
    }

}