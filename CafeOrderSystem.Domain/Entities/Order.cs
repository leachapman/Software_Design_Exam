using Software_Design_Exam.Domain.Discounts;
using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Domain.Entities;

public class Order
{
    #region Properties
    public int Id {get; private set;}
    public decimal TotalPrice => _items.Sum(i => i.IntermediatePrice);
    public DateTime CreatedAt {get; private set;}
    public OrderStatus Status { get; private  set; } = OrderStatus.Active;
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items;
    public User User {get; private set;} = null!;
    public int UserId {get; private set;}
    public decimal DiscountAmount {get; private set;}
    public decimal FinalPrice => (TotalPrice - DiscountAmount);
    #endregion

    #region Constructor
    public Order (DateTime createdAt, User user) 
    {
        ArgumentNullException.ThrowIfNull(user);
        CreatedAt = createdAt;
        User = user;
    }

    private Order () { }
    #endregion

    #region Methods
    private void EnsureActive()
    {
        if (Status != OrderStatus.Active)
        {
            throw new InvalidOperationException("You cannot continue as the order is not active");
        }
    }
    public void Cancel()
    {
        EnsureActive();        
        Status = OrderStatus.Cancelled;        
    }

    public void Complete()
    {
        EnsureActive();
        if(_items.Count == 0)
        {
            throw new InvalidOperationException("You cannot complete an empty order!");
        }
        Status = OrderStatus.Completed;   
    }

    public void AddItem(OrderItem item)
    {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(item);       
        _items.Add(item);
    }

    public void RemoveItem(OrderItem item)
    {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(item);

        if (!_items.Contains(item)){
            throw new InvalidOperationException("The Item does not exist.");
        }
        _items.Remove(item);

    }

    public void ApplyDiscount(IDiscountStrategy strategy)
    {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(strategy);
        DiscountAmount = strategy.CalculateDiscount(this);
    }
    #endregion
}