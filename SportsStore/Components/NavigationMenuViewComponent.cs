using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public sealed class NavigationMenuViewComponent : ViewComponent
{
    private readonly IStoreRepository _repository;

    public IViewComponentResult Invoke()
    {
        // говнокод ON
        ViewBag.SelectedCategory = RouteData?.Values["category"] ?? "no-category-selected"; 
        // говнокод OFF
        
        Console.WriteLine(ViewBag.SelectedCategory.GetType());
        
        return View(_repository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x));
    }

    public NavigationMenuViewComponent(IStoreRepository repository)
    {
        _repository = repository;
    }
}