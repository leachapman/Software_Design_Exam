using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Enums;
using Software_Design_Exam.Domain.Interfaces;

namespace Software_Design_Exam.Domain.Services;

public class RevenueCalculator
{
    private readonly IOrderRepository _orders;

    public RevenueCalculator(IOrderRepository orders)
    {
        _orders = orders;
    }

    private static void EnsureManager(User user)
    {
        if (user.Role != UserRole.Manager)
            throw new InvalidOperationException("Only managers can view revenue.");
    }

    public decimal GetDailyRevenue(User user, DateOnly date)
    {
        EnsureManager(user);
        var dailyRevenue = _orders.GetCompletedByDate(date).Sum(o => o.FinalPrice);
        return dailyRevenue;
    }

    public decimal GetRevenueForEmployee(User user, int userId, DateOnly date)
    {
        EnsureManager(user);
        return _orders.GetCompletedByDate(date)
        .Where(o => o.UserId == userId)
        .Sum(o => o.FinalPrice);
    }
}