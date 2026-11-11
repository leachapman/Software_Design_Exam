using Software_Design_Exam.Domain.Entities;

namespace Software_Design_Exam.Domain.Discounts;

public class NoDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(Order order)
    {
        return 0m;
    }
}