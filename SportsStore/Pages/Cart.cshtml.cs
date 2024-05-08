using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages;

public class CartModel : PageModel
{
    private readonly IStoreRepository _repository;

    public Cart? Cart { get; set; }

    public string ReturnUrl { get; set; } = "/";

    public void OnGet(string? returnUrl)
    {
        ReturnUrl = returnUrl ?? "/";
        Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
    }

    public IActionResult OnPost(long productId, string returnUrl)
    {
        var product = _repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
        
        if (product is not null)
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.AddItem(product, 1);
            HttpContext.Session.SetJson("cart", Cart);
        }

        return RedirectToPage(new { returnUrl });
    }
    
    public CartModel(IStoreRepository repository)
    {
        _repository = repository;
    }
}