using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Domain.Discounts;

public class StudentDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.10m;

    public decimal CalculateDiscount(Order order)
    {
        var highestHotBeveragePrice = order.Items
            .Where(i => i.Product.Category == ProductCategory.HotBeverage)
            .Select(i => i.UnitPriceAtOrder)
            .DefaultIfEmpty(0m)
            .Max();
        
        return highestHotBeveragePrice * Rate;
        
    }
}