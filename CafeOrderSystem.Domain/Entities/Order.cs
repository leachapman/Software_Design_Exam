using Software_Design_Exam.Domain.Enums;

namespace Software_Design_Exam.Domain.Entities;

public class Order
{
    //Properties
    public int Id {get; private set;}
    public decimal TotalPrice => _items.Sum(i => i.IntermediatePrice);
    public DateTime CreatedAt {get; private set;}
    public OrderStatus Status { get; private  set; } = OrderStatus.Active;
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items;

    // Constructor
    public Order (DateTime createdAt) 
    {
        
        CreatedAt = createdAt;
    }

    private Order () { }

    // Methods
    protected void EnsureActive()
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
        _items.Remove(item);

    }






}