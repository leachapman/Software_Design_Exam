using System.Xml.Serialization;
using Software_Design_Exam.Domain.Discounts;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;
using Software_Design_Exam.Domain.Services;
using Software_Design_Exam.Tests.Fakes;

namespace Software_Design_Exam.Tests.Services;

[TestFixture]
public class RevenueCalculatorTests{

    [Test]    
    public void GetDailyRevenue_TwoOrders_SumsFinalPrices()
    {
        var order1 = new Order(new DateTime(2026, 11, 12, 10, 0, 0), new User("Marie", UserRole.Barista));
        var order2 = new Order(new DateTime(2026, 11, 12, 10, 0, 0), new User("Marie", UserRole.Barista));

        order1.AddItem(new OrderItem(2, new Product("Cappuccino", 4.00m, ProductCategory.HotBeverage)));
        order1.ApplyDiscount(new StudentDiscount());
        order1.Complete();

        order2.AddItem(new OrderItem(2, new Product("Cookie", 5.00m, ProductCategory.HotBeverage)));
        order2.ApplyDiscount(new NoDiscount());
        order2.Complete();

        var repo = new FakeOrderRepository();
        repo.CompletedOrders.Add(order1);
        repo.CompletedOrders.Add(order2);
        var calculator = new RevenueCalculator(repo);

        var revenue = calculator.GetDailyRevenue(new User("Boss", UserRole.Manager), new DateOnly(2026, 11, 12));

        Assert.That(revenue, Is.EqualTo(17.60m));
    }

    public void GetDailyRevenue_NoOrders_ReturnsZero()
    {
        var repo = new FakeOrderRepository();
        var calculator = new RevenueCalculator(repo);
        var revenue = calculator.GetDailyRevenue(new User("Boss", UserRole.Manager), new DateOnly(2026, 11, 12));

        Assert.That(revenue, Is.Zero);
    }

    public void GetDailyRevenue_AsBarista_Throws()
    {
        var order = new Order(new DateTime(2026, 11, 12, 10, 0, 0), new User("Marie", UserRole.Barista));

        order.AddItem(new OrderItem(2, new Product("Cappuccino", 4.00m, ProductCategory.HotBeverage)));
        order.ApplyDiscount(new StudentDiscount());
        order.Complete();

        var repo = new FakeOrderRepository();
        repo.CompletedOrders.Add(order);
        var calculator = new RevenueCalculator(repo);

        var user = new User("Barista", UserRole.Barista);

        Assert.Throws<InvalidOperationException>(() => calculator.GetDailyRevenue(user, new DateOnly(2026, 11, 12)));
    }
}