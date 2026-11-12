using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;

namespace Software_Design_Exam.Tests.Fakes;

public class FakeOrderRepository : IOrderRepository
{
    public List<Order> CompletedOrders { get; } = new();

    public IReadOnlyList<Order> GetCompletedByDate(DateOnly date) => CompletedOrders;

    public void Add(Order order) { }
    public void SaveChanges() { }
    public Order? GetById(int id) => null;
}