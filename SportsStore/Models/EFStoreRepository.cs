namespace SportsStore.Models;

public sealed class EFStoreRepository : IStoreRepository
{
    private readonly StoreDbContext _context;
    
    public IQueryable<Product> Products => _context.Products;

    public EFStoreRepository(StoreDbContext context)
    {
        _context = context;
    }
}