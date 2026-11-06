using System.Dynamic;

namespace Software_Design_Exam.Domain.Entities;

public class OrderItem
{
    public int Quantity {get; private set; }
    public decimal UnitPriceAtOrder {get; set; }
    public decimal IntermediatePrice => UnitPriceAtOrder * Quantity;
    // private readonly List<Ingredient> _ingredients = new();
    // public IReadOnlyList<Inredient> Ingredients => _ingredients; ---> later

    private OrderItem () { }
    public OrderItem (int quantity)
    {
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity cannot be less than 1");
        }

        Quantity = quantity;
    }

    
}