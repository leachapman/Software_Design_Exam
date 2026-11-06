using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Tests.Entities;

[TestFixture]
public class OrderItemTests
{
    [Test]
    public void Constructor_AvailableProduct_CopiesCurrentPrice()
    {
        // Arrange
        var product = new Product("Croissant", 3.50m, ProductCategory.Pastry);

        // Act
        var item = new OrderItem(2, product);

        // Assert
        Assert.That(item.UnitPriceAtOrder, Is.EqualTo(3.50m));
        Assert.That(item.IntermediatePrice, Is.EqualTo(7.00m));
    }

    [Test]
    public void Constructor_UnavailableProduct_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product("Croissant", 3.50m, ProductCategory.Pastry);
        product.RemoveFromMenu();

        // Act + Assert
        Assert.Throws<ArgumentException>(() => new OrderItem(1, product));
    }
}