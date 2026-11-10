using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;

namespace Software_Design_Exam.Domain.Services;

public class OrderService
{
    private readonly IProductRepository _products;
    private readonly IOrderRepository _orders;

    public OrderService(IProductRepository products, IOrderRepository orders)
    {
        _products = products;
        _orders = orders;
    }

    public Order StartOrder(User user, DateTime now)
    {
        return new Order(now, user);   
    }

    public void AddProduct(Order order, int productId, int quantity)
    {
        var product = _products.GetById(productId)
            ?? throw new InvalidOperationException("Product not found.");
        
        var item = new OrderItem(quantity, product);
        order.AddItem(item);
    }

    public void RemoveItem(Order order, OrderItem item)
    {
        order.RemoveItem(item);
    }

    public void CompleteOrder(Order order)
    {
        order.Complete();
        _orders.Add(order);
        _orders.SaveChanges();
    }

    public void CancelOrder(Order order)
    {
        order.Cancel();
    }
}