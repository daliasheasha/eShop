using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// A single line item within an <see cref="Order"/>, capturing the product,
/// its unit price, any per-line discount, and the quantity ordered.
/// </summary>
public class OrderItem
    : Entity
{
    /// <summary>The display name of the ordered product.</summary>
    [Required]
    public string ProductName { get; private set; }

    /// <summary>Relative URL of the product image.</summary>
    public string PictureUrl { get; private set;}

    /// <summary>The price of a single unit of the product.</summary>
    public decimal UnitPrice { get; private set;}

    /// <summary>The absolute discount applied to this line.</summary>
    public decimal Discount { get; private set; }

    /// <summary>The number of units ordered for this line.</summary>
    public int Units { get; private set; }

    /// <summary>The catalog identifier of the ordered product.</summary>
    public int ProductId { get; private set; }

    protected OrderItem() { }

    public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        if (units <= 0)
        {
            throw new OrderingDomainException("Invalid number of units for an order item");
        }

        if ((unitPrice * units) < discount)
        {
            throw new OrderingDomainException("Order item total cannot be lower than the applied discount");
        }

        ProductId = productId;

        ProductName = productName;
        UnitPrice = unitPrice;
        Discount = discount;
        Units = units;
        PictureUrl = pictureUrl;
    }
    
    /// <summary>Increases or sets the discount applied to this line.</summary>
    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
        {
            throw new OrderingDomainException("Discount is not valid");
        }

        Discount = discount;
    }

    /// <summary>Adds the supplied quantity to the units already on this line.</summary>
    public void AddUnits(int units)
    {
        if (units < 0)
        {
            throw new OrderingDomainException("Invalid units");
        }

        Units += units;
    }
}
