namespace SportsStore.Models.ViewModel;

public sealed class ProductListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

    public PagingInfo PagingInfo { get; set; } = new();
    
    public string? CurrentCategory { get; set; }
}