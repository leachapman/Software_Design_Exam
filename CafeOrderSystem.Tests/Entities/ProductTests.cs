using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Tests.Entities;

[TestFixture]
public class ProductTests
{
    public void Constructor_NegativePrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Product("Crossaint", -1m, ProductCategory.Pastry));
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Constructor_EmptyName_ThrowsArgumentException(string? name)
    {
        Assert.Catch<ArgumentException>(() => new Product(name!, 3m, ProductCategory.ColdBeverage));
    }

    [Test]
    public void RemoveFromMenu_AvailableProduct_isNoLongerAvailable()
    {
        var product = new Product("Crossaint", 2m, ProductCategory.Pastry);
        product.RemoveFromMenu();
        Assert.That(product.IsAvailable == false);
    }

    [Test]
    public void RemoveFromMenu_AlreadyRemoved_ThrowsInvalidOperationException()
    {
        var product = new Product("Crossaint", 2m, ProductCategory.Pastry);
        product.RemoveFromMenu();
        Assert.Throws<InvalidOperationException>(() => product.RemoveFromMenu());
    }
} 