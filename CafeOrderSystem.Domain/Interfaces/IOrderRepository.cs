using Software_Design_Exam.Domain.Entities;

namespace Software_Design_Exam.Domain.Interfaces;

public interface IOrderRepository
{
    void Add(Order order);
    void SaveChanges();
    Order? GetById(int id);
    IReadOnlyList<Order> GetCompletedByDate(DateOnly date);
}