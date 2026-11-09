using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Data;

public static class CafeSeeder
{
    public static void Seed(CafeDbContext db)
    {
        if(db.Products.Any()) return; // only at the very first start
        db.Products.AddRange(
            new Product("Croissant", 3.50m, ProductCategory.Pastry),
            new Product("Cookie", 2.50m, ProductCategory.Pastry),
            new Product("Pain au chocolat", 4.50m, ProductCategory.Pastry),
            new Product("Baklava", 4.00m, ProductCategory.Pastry),
            new Product("Cappuccino", 4.00m, ProductCategory.HotBeverage),
            new Product("Espresso", 3.00m, ProductCategory.HotBeverage),
            new Product("Cafe Latte", 4.50m, ProductCategory.HotBeverage),
            new Product("Chai Latte", 3.50m, ProductCategory.HotBeverage),
            new Product("Iced Coffee", 5.00m, ProductCategory.ColdBeverage),
            new Product("Iced Match Latte", 6.00m, ProductCategory.ColdBeverage),
            new Product("Ice Tea", 4.00m, ProductCategory.ColdBeverage),
            new Product("Orangejuice", 4.50m, ProductCategory.ColdBeverage)
        );
        db.SaveChanges();


    }
}