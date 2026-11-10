using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;
using Software_Design_Exam.Domain.Interfaces;

namespace Software_Design_Exam.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CafeDbContext _db;

    public ProductRepository(CafeDbContext db)
    {
        _db = db;
    }

    #region Methods
    public void Add(Product product)
    {
        _db.Products.Add(product);
    }

    public IReadOnlyList<Product> GetAllProducts()
    {
        return _db.Products.OrderBy(p => p.Name).ToList();
    }

    public Product? GetById(int id)
    {
        return _db.Products.Find(id);
    }

    public IReadOnlyList<Product> GetMenu()
    {
        return _db.Products.Where(p => p.IsAvailable).OrderBy(p => p.Name).ToList();
    }

    public IReadOnlyList<Product> GetMenuByCategory(ProductCategory category)
    {
        return _db.Products.Where(p => p.IsAvailable && p.Category == category).OrderBy(p => p.Name).ToList();
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }

    #endregion
}