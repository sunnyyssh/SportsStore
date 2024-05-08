﻿using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;

namespace SportsStore.Tests;

public class CartPageTests
{
    [Fact]
    public void Can_Load_Cart() {
        // Arrange
        // - create a mock repository
        var p1 = new Product { ProductID = 1, Name = "P1" };
        var p2 = new Product { ProductID = 2, Name = "P2" };
        var mockRepo = new Mock<IStoreRepository>();
        mockRepo.Setup(m => m.Products).Returns(new [] { p1, p2 }.AsQueryable());

        // - create a cart 
        var testCart = new Cart();
        testCart.AddItem(p1, 2);
        testCart.AddItem(p2, 1);
        
        // - create a mock page context and session
        var mockSession = new Mock<ISession>();
        byte[] data = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(testCart));
        mockSession.Setup(c => 
            c.TryGetValue(It.IsAny<string>(), out data!));
        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c => 
            c.Session).Returns(mockSession.Object);
        
        // Action
        var cartModel = new CartModel(mockRepo.Object) {
            PageContext = new PageContext(new ActionContext {
                HttpContext = mockContext.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new PageActionDescriptor()
            })
        };
        cartModel.OnGet("myUrl");
        
        //Assert
        Assert.Equal(2, cartModel.Cart?.Lines.Count());
        Assert.Equal("myUrl", cartModel.ReturnUrl);
    }

    [Fact]
    public void Can_Update_Cart()
    {
        // Arrange
        // - create a mock repository
        var mockRepo =
            new Mock<IStoreRepository>();
        mockRepo.Setup(m => m.Products).Returns((new[]
        {
            new Product { ProductID = 1, Name = "P1" }
        }).AsQueryable());
        var testCart = new Cart();
        var mockSession = new Mock<ISession>();
        mockSession.Setup(s =>
                s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, val) =>
            {
                testCart = JsonSerializer.Deserialize<Cart>(
                    Encoding.UTF8.GetString(val));
            });
        var mockContext = new Mock<HttpContext>();
        mockContext.SetupGet(c =>
            c.Session).Returns(mockSession.Object);
        
        // Action
        var cartModel = new CartModel(mockRepo.Object)
        {
            PageContext = new PageContext(new ActionContext
            {
                HttpContext = mockContext.Object,
                RouteData = new RouteData(),

                ActionDescriptor = new PageActionDescriptor()
            })
        };
        cartModel.OnPost(1, "myUrl");
        
        //Assert
        Assert.Single(testCart.Lines);
        Assert.Equal("P1", testCart.Lines.First().Product.Name);
    }
}