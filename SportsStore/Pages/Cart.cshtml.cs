using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages;

public class CartModel : PageModel
{
    private readonly IStoreRepository _repository;

    public Cart Cart { get; set; }

    public string ReturnUrl { get; set; } = "/";

    public void OnGet(string? returnUrl)
    {
        ReturnUrl = returnUrl ?? "/";
    }

    public IActionResult OnPost(long productId, string returnUrl)
    {
        var product = _repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
        
        if (product is not null)
        {
            Cart.AddItem(product, 1);
        }

        return RedirectToPage(new { returnUrl });
    }
    
    public IActionResult OnPostRemove(long productId, string returnUrl) 
    {
        Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductID == productId).Product);
        return RedirectToPage(new { returnUrl });
    }
    
    public CartModel(IStoreRepository repository, Cart cart)
    {
        Cart = cart;
        _repository = repository;
    }
}