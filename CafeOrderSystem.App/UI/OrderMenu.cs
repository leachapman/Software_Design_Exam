using Software_Design_Exam.Domain.Discounts;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;
using Software_Design_Exam.Domain.Services;

namespace Software_Design_Exam.App.UI;

public class OrderMenu
{
    private readonly IProductRepository _products;
    private readonly OrderService _orderService;

    public OrderMenu (OrderService orderService, IProductRepository products)
    {
        _orderService = orderService;
        _products = products;
    }

    public void Run(User user)
    {
        var order = _orderService.StartOrder(user, DateTime.Now);

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("======  Menu  ======");
            Console.WriteLine();
            PrintMenu();
          
            var productId = ConsoleInput.ReadInt("Produkt-Id (0 = finished): ");
            if (productId == 0)
            {
                break;
            }

            var quantity = ConsoleInput.ReadInt("Quantity: ");

            ConsoleHelper.RunSafely(() => _orderService.AddProduct(order, productId, quantity));

            Console.WriteLine();
            Console.WriteLine($"======  Current order  ======");
            Console.WriteLine();
            PrintOrder(order);

        }

        string? choiceToFinish = null;
        while (choiceToFinish != "c" && choiceToFinish != "q")
        {
            Console.Write("Complete (c) or Cancel (q)? ");
            choiceToFinish = Console.ReadLine()?.Trim().ToLower();
        }

        ConsoleHelper.RunSafely(() =>
        {
            if (choiceToFinish == "c")
            {              
                IDiscountStrategy strategy =  ConsoleInput.ReadYesNo("Student? (y/n): ")
                    ? new StudentDiscount() 
                    : new NoDiscount();

                _orderService.CompleteOrder(order, strategy);

                Console.WriteLine();
                Console.WriteLine("===== Order saved. =====");
                Console.WriteLine();
                Console.WriteLine("The finished order:");
                PrintOrder(order);
                Console.WriteLine($"Discount: {order.DiscountAmount,6:0.00} €");
                Console.WriteLine($"Final Price: {order.FinalPrice, 6:0.00} €");
            }
            else
            {
                _orderService.CancelOrder(order);
                Console.WriteLine();
                Console.WriteLine("===== Order cancelled. =====");
            }
        });
    }

    public void PrintMenu()
    {
        foreach(var p in _products.GetMenu())
            {
                Console.WriteLine($"{p.Id,3}  {p.Name,-20} {p.Category,-15} {p.Price,6:0.00} €");
            }        
    }

    public void PrintOrder(Order order)
    {
        foreach (var item in order.Items)
            {
                Console.WriteLine($"{item.Quantity} x {item.Product.Name,-20} {item.IntermediatePrice,6:0.00} €");
            }
            Console.WriteLine($"Total: {order.TotalPrice:0.00} €");
    }
}