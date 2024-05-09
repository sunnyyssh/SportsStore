using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public sealed class OrderController : Controller
{
    private readonly IOrderRepository _repository;

    private readonly Cart _cart;
    
    public IActionResult Checkout() => View(new Order());

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (_cart.Lines.Count == 0)
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }

        if (!ModelState.IsValid)
        {
            return View();
        }

        order.Lines = _cart.Lines.ToArray();
        _repository.SaveOrder(order);
        _cart.Clear();
        return RedirectToPage("/Completed", new { orderId = order.OrderId });
    }

    public OrderController(IOrderRepository repository, Cart cart)
    {
        _repository = repository;
        _cart = cart;
    }
}