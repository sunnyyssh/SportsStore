using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public sealed class CartSummaryViewComponent : ViewComponent
{
    private readonly Cart _cart;

    public IViewComponentResult Invoke()
    {
        return View(_cart);
    }

    public CartSummaryViewComponent(Cart cart)
    {
        _cart = cart;
    }
}