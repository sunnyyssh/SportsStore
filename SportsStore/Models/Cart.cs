namespace SportsStore.Models;

public sealed class Cart
{
    public List<CartLine> Lines { get; set; } = new();

    public void AddItem(Product product, int quantity)
    {
        var line = Lines
            .FirstOrDefault(p => p.Product.ProductID == product.ProductID);
        
        if (line is null)
        {
            Lines.Add(new CartLine
            {
                Product = product,
                Quantity = quantity,
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }

    public void RemoveLine(Product product)
        => Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);

    public decimal ComputeTotalValue()
        => Lines.Sum(e => e.Product.Price * e.Quantity);

    public void Clear() => Lines.Clear();
}

public sealed class CartLine
{
    public int CartLineId { get; set; }
    public required Product Product { get; init; }
    public required int Quantity { get; set; }
}