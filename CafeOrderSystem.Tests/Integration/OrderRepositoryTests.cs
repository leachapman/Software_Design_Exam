using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Software_Design_Exam.Data;
using Software_Design_Exam.Data.Repositories;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Tests.Integration;

[TestFixture]
public class OrderRepositoryIntegrationTests
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
    public void Add_ThenLoadInNewContext_ReturnsOrderWithItemsAndUser()
    {
        int savedId;

        using (var db = new CafeDbContext(_options))
        {
            var repo = new OrderRepository(db);
            var user = new User("Sabrina", UserRole.Barista);
            var product1 = new Product("Cookie", 3.00m, ProductCategory.Pastry);
            var product2 = new Product("Cappuccino", 4.50m, ProductCategory.HotBeverage);
            var item1 = new OrderItem(2, product1);
            var item2 = new OrderItem(1, product2);
            var order = new Order(new DateTime(2026, 11, 10, 9, 30, 0), user);

            order.AddItem(item1);
            order.AddItem(item2);
            
            repo.Add(order);
            repo.SaveChanges();
            savedId = order.Id;     
        }

        using (var db = new CafeDbContext(_options))
        {
            var repo = new OrderRepository(db);
            var loaded = repo.GetById(savedId);

            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded!.Items, Has.Count.EqualTo(2));
            Assert.That(loaded.TotalPrice, Is.EqualTo(10.50m));
            Assert.That(loaded.User.Name, Is.EqualTo("Sabrina"));

            var productNames = loaded.Items.Select(i => i.Product.Name).ToList();
            Assert.That(productNames, Does.Contain("Cookie"));
            Assert.That(productNames, Does.Contain("Cappuccino"));
        }
        
    }
}