using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Domain.Interfaces;

public interface IProductRepository
{
    IReadOnlyList<Product> GetMenu(); //all products that are still available
    IReadOnlyList<Product> GetAllProducts();    //all products, including removed ones (for manager)
    IReadOnlyList<Product> GetMenuByCategory(ProductCategory category);
    Product? GetById(int id); //one specific, or null when not found
    void Add(Product product); // add a product (for manager)
    void SaveChanges();
}