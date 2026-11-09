using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Software_Design_Exam.Data;
using Software_Design_Exam.Data.Repositories;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Tests.Integration;

[TestFixture]
public class ProductRepositoryIntegrationTests
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
    public void Add_ThenLoadInNewContext_ReturnsSavedProduct()
    {
        int savedId;

        // saving
        using (var db = new CafeDbContext(_options))
        {
            var repo = new ProductRepository(db);
            var product = new Product("Cappuccino", 5.00m, ProductCategory.HotBeverage);
            repo.Add(product);
            repo.SaveChanges();
            savedId = product.Id;
        }

        // reload
        using (var db = new CafeDbContext(_options))
        {
            var repo = new ProductRepository(db);
            var loaded = repo.GetById(savedId);

            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded!.Name, Is.EqualTo("Cappuccino"));
        }
    }

    [Test]
    public void AddandRemove_ThenLoadInNewContext_ReturnsValidProduct()
    {
        using (var db = new CafeDbContext(_options)){
            var repo = new ProductRepository(db);
            var product1 = new Product("Croissant", 3.00m, ProductCategory.Pastry);
            var product2 = new Product("Cookie", 3.50m, ProductCategory.Pastry);
            repo.Add(product1);
            repo.Add(product2);

            product2.RemoveFromMenu();
            repo.SaveChanges();
        }

        using (var db = new CafeDbContext(_options))
        {
            var repo = new ProductRepository(db);
            var loadedMenu = repo.GetMenu();
            var loadedAllProducts = repo.GetAllProducts();  
            var menuNames = loadedMenu.Select(p => p.Name).ToList();

            Assert.That(menuNames, Does.Contain("Croissant"));
            Assert.That(menuNames, Does.Not.Contain("Cookie"));   
            Assert.That(loadedMenu, Has.Count.EqualTo(1));
            Assert.That(loadedAllProducts, Has.Count.EqualTo(2));
        }
    }
    


}