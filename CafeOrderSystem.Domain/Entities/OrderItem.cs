namespace Software_Design_Exam.Domain.Entities;

public class OrderItem
{
    #region Properties
    public int Id { get; private set; }
    public int ProductId {get; private set;}
    public Product Product {get; private set;} = null!;
    public int Quantity {get; private set; }
    public decimal UnitPriceAtOrder {get; private set; }
    public decimal IntermediatePrice => UnitPriceAtOrder * Quantity;
    // private readonly List<Ingredient> _ingredients = new();
    // public IReadOnlyList<Inredient> Ingredients => _ingredients; ---> later
    #endregion

    #region Constructor

    private OrderItem () { }
    public OrderItem (int quantity, Product product)
    { 
        EnsureValidProduct(quantity, product);
        if (!product.IsAvailable)
            throw new ArgumentException("Product is not available.", nameof(product));

        Quantity = quantity;
        Product = product;
        UnitPriceAtOrder = Product.Price;
    }
    #endregion

    #region Methods 
    private void EnsureValidProduct(int quantity, Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity cannot be less than 1");
        }
    }

    public void ChangeQuantity(int quantity, Product product)
    {
        EnsureValidProduct(quantity, product);
        Quantity = quantity;
    }
    // changeIngredients()
    #endregion

    
}