using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;
using Software_Design_Exam.Domain.Interfaces;

namespace Software_Design_Exam.Tests.Fakes;

public class FakeProductRepository : IProductRepository
{
    public Product? ProductToReturn {get;  set;}
    public int SaveChangesCount {get; private set;}
    public List<Product> Added { get; } = new();


    public void Add(Product product) => Added.Add(product);
    public Product? GetById(int id) => ProductToReturn;
    public void SaveChanges() => SaveChangesCount++;

    public IReadOnlyList<Product> GetMenu() => new List<Product>();
    public IReadOnlyList<Product> GetMenuByCategory(ProductCategory category) => new List<Product>();
    public IReadOnlyList<Product> GetAllProducts() => new List<Product>();
}