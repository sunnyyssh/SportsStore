using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModel;

namespace SportsStore.Controllers;

public sealed class HomeController(IStoreRepository repository) : Controller
{
    public int PageSize { get; set; } = 4;

    public IActionResult Index(string? category, int productPage = 1)
    {
        var products = repository.Products
            .Where(p => category == null || category == p.Category)
            .OrderBy(p => p.ProductID)
            .Skip((productPage - 1) * PageSize)
            .Take(PageSize);
            
        var paging = new PagingInfo
        {
            TotalItems = repository.Products.Count(),
            ItemsPerPage = PageSize,
            CurrentPage = productPage
        };
        
        return View(new ProductListViewModel
        {
            PagingInfo = paging,
            Products = products,
            CurrentCategory = category
        });
    }
}