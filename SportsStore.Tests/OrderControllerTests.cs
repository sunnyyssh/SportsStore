﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

namespace SportsStore.Tests;

public class OrderControllerTests
{
    [Fact]
    public void Cannot_Checkout_Empty_Cart()
    {
        var mock = new Mock<IOrderRepository>();
        var cart = new Cart();
        var order = new Order();
        var target = new OrderController(mock.Object, cart);

        var result = target.Checkout(order) as ViewResult;
        
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        Assert.True(string.IsNullOrEmpty(result?.ViewName));
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    [Fact]
    public void Cannot_Checkout_Invalid_Shipping_Details()
    {
        var mock = new Mock<IOrderRepository>();
        var cart = new Cart();
        cart.AddItem(new Product(), 1);
        var target = new OrderController(mock.Object, cart);
        target.ModelState.AddModelError("error", "error");

        var result = target.Checkout(new Order()) as ViewResult;
        
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        Assert.True(string.IsNullOrEmpty(result?.ViewName));
        Assert.False(result?.ViewData.ModelState.IsValid);
    }

    [Fact]
    public void Can_Checkout_And_Submit_Order()
    {
        var mock = new Mock<IOrderRepository>();
        var cart = new Cart();
        cart.AddItem(new Product(), 1);
        var target = new OrderController(mock.Object, cart);

        var result = target.Checkout(new Order()) as RedirectToPageResult;
        
        mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
        Assert.Equal("/Completed", result?.PageName);
    }
}