using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Tests.Entities;

[TestFixture]
public class OrderTests
{
    private static Order CreateOrder() => new Order(new DateTime(2026, 11, 9), new User("Test", UserRole.Barista));
    private static Product CreateProduct(decimal price) => new Product("Test", price, ProductCategory.Pastry);

    [Test]
    public void NewOrder_IsActiveWithZeroTotal()
    {
        var order = CreateOrder();

        Assert.That(order.Status, Is.EqualTo(OrderStatus.Active));
        Assert.That(order.TotalPrice, Is.Zero);
    }

    [Test]
    public void TotalPrice_TwoItems_IsSumOfLineTotals()
    {
        
        var item1 = new OrderItem(2, CreateProduct(3.50m));
        var item2 = new OrderItem(1, CreateProduct(4.00m));

        var order = CreateOrder();
        order.AddItem(item1);
        order.AddItem(item2);
    }

    [Test]
    public void Complete_EmptyOrder_ThrowsInvalidOperationException()
    {
        var order = CreateOrder();
        
        Assert.Throws<InvalidOperationException>(() => order.Complete());
    }


    [Test]
    public void Complete_CancelledOerder_ThrowsInvalidOperationException()
    {
        var order = CreateOrder();
        var item = new OrderItem(1, CreateProduct(4.00m));
        order.AddItem(item);
        order.Cancel();
        Assert.Throws<InvalidOperationException>(()=> order.Complete());
    }

    [Test]
    public void AddItem_CompletedOrder_ThrowsInvalidOperationException()
    {
        var order = CreateOrder();
        var item = new OrderItem(1, CreateProduct(4.00m));
        var item1 = new OrderItem(1, CreateProduct(3.00m));
        order.AddItem(item);
        order.Complete();
        Assert.Throws<InvalidOperationException>(() => order.AddItem(item1));
    }




}