using Software_Design_Exam.Domain.Entities;

namespace Software_Design_Exam.Domain.Interfaces;

public interface IUserRepository
{
    public IReadOnlyList<User> GetAll();
    public User? GetById(int id);
}