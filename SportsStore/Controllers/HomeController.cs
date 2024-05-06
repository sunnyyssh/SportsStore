using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index() => View();
}