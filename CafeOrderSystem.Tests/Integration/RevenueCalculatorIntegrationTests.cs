using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Software_Design_Exam.Data;
using Software_Design_Exam.Data.Repositories;
using Software_Design_Exam.Domain.Discounts;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;
using Software_Design_Exam.Domain.Services;

namespace Software_Design_Exam.Tests.Integration;

[TestFixture]
public class RevenueCalculatorIntegrationTests
{
    
    
    private SqliteConnection _connection = null!;
    private DbContextOptions<CafeDbContext> _options = null!; 

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open(); // connection must remain open, otherwise the database will be lost

        _options = new DbContextOptionsBuilder<CafeDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var db = new CafeDbContext(_options);
        db.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown() => _connection.Dispose();

    [Test]
    public void GetRevenueForEmployee_TwoEmployees_ReturnsOnlyOwnRevenue()
    {
        var day = new DateTime(2026, 11, 12, 10, 0, 0);
        int marieId, elenaId;

        using (var db = new CafeDbContext(_options))
        {
            var marie = new User("Marie", UserRole.Barista);
            var elena = new User("Elena", UserRole.Barista);
            var cappuccino = new Product("Cappuccino", 4.00m, ProductCategory.HotBeverage);

            // Order Marie = 8,00
            var orderMarie = new Order(day, marie);
            orderMarie.AddItem(new OrderItem(2, cappuccino));
            orderMarie.ApplyDiscount(new NoDiscount());
            orderMarie.Complete();

            // Order Elena = 4,00
            var orderElena = new Order(day, elena);
            orderElena.AddItem(new OrderItem(1, cappuccino));
            orderElena.ApplyDiscount(new NoDiscount());
            orderElena.Complete();

            var repo = new OrderRepository(db);
            repo.Add(orderMarie);
            repo.Add(orderElena);
            repo.SaveChanges();

            marieId = marie.Id;   
            elenaId = elena.Id;
        }

        using (var db = new CafeDbContext(_options))
        {
            var calculator = new RevenueCalculator(new OrderRepository(db));
            var manager = new User("Boss", UserRole.Manager);
            var date = DateOnly.FromDateTime(day);

            Assert.That(calculator.GetRevenueForEmployee(manager, marieId, date), Is.EqualTo(8.00m));
            Assert.That(calculator.GetRevenueForEmployee(manager, elenaId, date), Is.EqualTo(4.00m));
            Assert.That(calculator.GetDailyRevenue(manager, date), Is.EqualTo(12.00m));
        }
    }
}