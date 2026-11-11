using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Domain.Services;

public class ProductService
{
    private readonly IProductRepository _products;

    public ProductService(IProductRepository products)
    {
        _products = products;
    }

    private static void EnsureManager(User user)
    {
        if (user.Role != UserRole.Manager)
        {
            throw new InvalidOperationException("Only managers can manage products");
        }
    }

    private Product GetProductOrThrow (int id) => _products.GetById(id) ?? throw new InvalidOperationException("Product not found.");
    

    public Product CreateProduct (User user, string name, decimal price, ProductCategory category)
    {
        EnsureManager(user);
        var product = new Product(name, price, category);
        _products.Add(product);
        _products.SaveChanges();
        return product;        
    }

    public void ChangePrice (User user, int id, decimal newPrice)
    {
        EnsureManager(user);
        var product = GetProductOrThrow(id);
        product.ChangePrice(newPrice);
        _products.SaveChanges();
    }

    public void Rename (User user, int id, string newName)
    {
        EnsureManager(user);
        var product = GetProductOrThrow(id);
        product.Rename(newName);
        _products.SaveChanges();
    }

    public void RemoveFromMenu (User user, int id)
    {
        EnsureManager(user);
        var product = GetProductOrThrow(id);
        product.RemoveFromMenu();
        _products.SaveChanges();       
    }

    public void AddToMenu (User user, int id)
    {
        EnsureManager(user);
        var product = GetProductOrThrow(id);
        product.AddToMenu();
        _products.SaveChanges(); 
    }
}