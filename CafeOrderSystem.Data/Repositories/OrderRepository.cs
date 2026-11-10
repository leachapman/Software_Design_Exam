using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;
using Software_Design_Exam.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Software_Design_Exam.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly CafeDbContext _db;

    public OrderRepository(CafeDbContext db)
    {
        _db = db;
    }

    #region Methods
    public void Add(Order order)
    {
        _db.Orders.Add(order);
    }

    public Order? GetById(int id)
    { 
        return _db.Orders
        .Include(o => o.Items)
            .ThenInclude(i => i.Product)
        .Include(o => o.User)
        .FirstOrDefault(o => o.Id == id);
    }

    public IReadOnlyList<Order> GetCompletedByDate(DateOnly date)
    {
        var start = date.ToDateTime(TimeOnly.MinValue);
        var end = start.AddDays(1);

        return _db.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Include(o => o.User)
            .Where(o => o.Status == OrderStatus.Completed
                    && o.CreatedAt >= start
                    && o.CreatedAt < end)
            .ToList();
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }
    #endregion
}