using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;

namespace Software_Design_Exam.App.UI;

public class LoginMenu
{
    private readonly IUserRepository _users;

    public LoginMenu(IUserRepository users)
    {
        _users = users;
    }

    public User Run()
    {
        Console.WriteLine("=======  Login  ======");

        foreach (var u in _users.GetAll())
        {
            Console.WriteLine($"{u.Id}: {u.Name} ({u.Role})");
        }
        while (true)
        {
            var id = ConsoleInput.ReadInt("Your Id: ");
            var user = _users.GetById(id);
            if (user != null)
            {
                Console.WriteLine($"Welcome {user.Name}");
                return user;
            }
            Console.WriteLine("No employee with this Id found.");
        }     
    }
}