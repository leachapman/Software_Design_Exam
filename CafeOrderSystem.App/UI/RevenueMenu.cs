using System.Runtime.CompilerServices;
using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;
using Software_Design_Exam.Domain.Services;
namespace Software_Design_Exam.App.UI;

public class RevenueMenu
{
    private readonly RevenueCalculator _revenue;
    private readonly IUserRepository _users;

    public RevenueMenu(RevenueCalculator revenue, IUserRepository users)
    {
        _revenue = revenue;
        _users = users;
    }

    public void Run(User user)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        Console.WriteLine($"===== Revenue {today} =====");

        ConsoleHelper.RunSafely(() =>
        {
            // daily revenue
           _revenue.GetDailyRevenue(user, today);

            //daily revenue per employee
            foreach(var employee in _users.GetAll())
            {
                var amount = _revenue.GetRevenueForEmployee(user, employee.Id, today);
                Console.WriteLine($"{employee.Name, -20}   {amount, 8:0.00} €");
            }
        });
    }
}