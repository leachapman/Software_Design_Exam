using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Domain.Entities;

public class User
{
    #region Properties
    public int Id {get; private set;}
    public string Name {get; private set;} = null!;
    public UserRole Role {get; private set;}
    #endregion

    #region Constructor
    public User(string name, UserRole role)
    {
        EnsureValidName(name);
        Name = name;
        Role = role;
    }

    private User() { }
    #endregion

    #region Methods
    private void EnsureValidName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
    }
    #endregion
}