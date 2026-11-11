using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Domain.Entities;

public class Product
{
    #region Properties
    public int Id {get; private set;}
    public string Name {get; private set;} = null!;
    public decimal Price {get; private set;}
    public ProductCategory Category {get; private set;}
    public bool IsAvailable { get; private set; } = true;
    #endregion

    #region Constructor
    public Product (string name, decimal price, ProductCategory category)
    {
        EnsureValidName(name);
        EnsureValidPrice(price);

        Name = name;
        Price = price;
        Category = category;
    }

    private Product () { }
    #endregion

    #region Methods
    private void EnsureValidPrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentException("The price cannot be negative");
        }
    }

    private void EnsureValidName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
    }

    public void ChangePrice(decimal price)
    {
        EnsureValidPrice(price);
        Price = price;
    }

    public void Rename(string name)
    {
        EnsureValidName(name);
        Name = name;
    }

    public void RemoveFromMenu()
    {
        if (!IsAvailable)
        {
            throw new InvalidOperationException("Product is already deleted.");
        }
        IsAvailable = false;
    }

    public void AddToMenu()
    {
        if (IsAvailable)
        {
            throw new InvalidOperationException("Product already exists.");
        }
        IsAvailable = true;


    }
    #endregion
}