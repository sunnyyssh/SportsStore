using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModel;

namespace SportsStore.Controllers;

public sealed class HomeController(IStoreRepository repository) : Controller
{
    public int PageSize { get; set; } = 4;

    public IActionResult Index(int productPage = 1)
    {
        var products = repository.Products
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
        });
    }
}