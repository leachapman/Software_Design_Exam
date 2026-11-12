using Software_Design_Exam.Domain.Discounts;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Tests.Discounts;

[TestFixture]
public class StudentDiscountTests
{
    private static Order OrderWith(params (string name, decimal price, ProductCategory cat, int qty)[] lines)
    {
        var order = new Order(new DateTime(2026, 11, 12), new User("Test", UserRole.Barista));
        foreach (var l in lines)
            order.AddItem(new OrderItem(l.qty, new Product(l.name, l.price, l.cat)));
        return order;
    }

    [Test]
    public void CalculateDiscount_TwoCoffees_Returns10PercentOfMostExpensive()
    {
        var order = OrderWith(
        ("Cappuccino", 4.00m, ProductCategory.HotBeverage, 1),
        ("Latte",      4.50m, ProductCategory.HotBeverage, 1));

        var discount = new StudentDiscount().CalculateDiscount(order);

        Assert.That(discount, Is.EqualTo(0.45m));
    }

    [Test]
    public void CalculateDiscount_NoCoffee_ReturnsZero()
    {
        var order = OrderWith(
        ("Croissant", 4.00m, ProductCategory.Pastry, 1));

        var discount = new StudentDiscount().CalculateDiscount(order);

        Assert.That(discount == 0);
    }

    [Test]
    public void CalculateDiscount_CoffeeQuantityTwo_DiscountsOnlyOne()
    {
        var order = OrderWith(
        ("Cappuccino", 4.00m, ProductCategory.HotBeverage, 2));

        var discount = new StudentDiscount().CalculateDiscount(order);
        Assert.That(discount, Is.EqualTo(0.40m));
    }

    [Test]
    public void NoDiscount_AnyOrder_ReturnsZero()
    {
        var order = OrderWith(
        ("Cappuccino", 4.00m, ProductCategory.HotBeverage, 1),
        ("Latte",      4.50m, ProductCategory.HotBeverage, 1),
        ("Croissant", 4.00m, ProductCategory.Pastry, 1));

        var discount = new NoDiscount().CalculateDiscount(order);

        Assert.That(discount == 0);
    }

}