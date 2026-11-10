using Software_Design_Exam.Domain.Entities;
using Software_Design_Exam.Domain.Interfaces;

namespace Software_Design_Exam.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CafeDbContext _db;

    public UserRepository(CafeDbContext db)
    {
        _db = db;
    }

    public IReadOnlyList<User> GetAll()
    {
        return _db.Users.ToList();
    }

    public User? GetById(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }
}