using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;
using Software_Design_Exam.Domain.Services;
using Software_Design_Exam.Tests.Fakes;

namespace Software_Design_Exam.Tests.Services;

[TestFixture]
public class ProductServiceTests
{
    private static User Manager() => new User("Mia", UserRole.Manager);
    private static User Barista() => new User("Sabrina", UserRole.Barista);
    private static Product Croissant() => new Product("Croissant", 3.50m, ProductCategory.Pastry);

    [Test]
    public void ChangePrice_AsBarista_ThrowsAndDoesNotSave()
    {
        var repo = new FakeProductRepository { ProductToReturn =  Croissant()};
        var service = new ProductService(repo);
        
        Assert.Throws<InvalidOperationException>(() => service.ChangePrice(Barista(), 1, 5.00m));
        Assert.That(repo.SaveChangesCount == 0);
    }

    [Test]
    public void ChangePrice_AsManager_ChangesPriceAndSaves()
    {
        var product = Croissant();
        var repo = new FakeProductRepository { ProductToReturn =  product};
        var service = new ProductService(repo);
        
        service.ChangePrice(Manager(), 1, 5.00m);
        Assert.That(product.Price, Is.EqualTo(5.00m));
        Assert.That(repo.SaveChangesCount, Is.EqualTo(1));
    }

    [Test]
    public void ChangePrice_UnknownProduct_Throws()
    {
        var repo = new FakeProductRepository { ProductToReturn =  null};
        var service = new ProductService(repo);

        Assert.Throws<InvalidOperationException>(() => service.ChangePrice(Manager(), 99, 4.00m));
    }

    [Test]
    public void CreateProduct_AsManager_AddsAndSaves()
    {
        var repo = new FakeProductRepository();
        var service = new ProductService(repo);
        var product = service.CreateProduct(Manager(), "Pie", 5.00m, ProductCategory.Pastry);

        
        Assert.That(repo.Added, Has.Count.EqualTo(1));
        Assert.That(repo.Added[0], Is.SameAs(product));
        Assert.That(repo.SaveChangesCount, Is.EqualTo(1));

    }

    [Test]
    public void CreateProduct_AsBarista_ThrowsInvalidOperationException()
    {
        var repo = new FakeProductRepository();
        var service = new ProductService(repo);

        Assert.Throws<InvalidOperationException>(() => service.CreateProduct(Barista(), "Pie", 5.00m, ProductCategory.Pastry));
        Assert.That(repo.SaveChangesCount == 0); 
    }
}