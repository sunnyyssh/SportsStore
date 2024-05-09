﻿namespace SportsStore.Models;

public sealed class EFStoreRepository : IStoreRepository
{
    private readonly StoreDbContext _context;
    
    public IQueryable<Product> Products => _context.Products;
    
    public void CreateProduct(Product product)
    {
        _context.Add(product);
        _context.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        _context.Remove(product);
        _context.SaveChanges();
    }

    public void SaveProduct(Product product)
    {
        _context.SaveChanges();
    }

    public EFStoreRepository(StoreDbContext context)
    {
        _context = context;
    }
}