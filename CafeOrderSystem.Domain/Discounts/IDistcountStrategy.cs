using Software_Design_Exam.Domain.Entities;

namespace Software_Design_Exam.Domain.Discounts;

public interface IDiscountStrategy
{
    decimal CalculateDiscount(Order order);
}