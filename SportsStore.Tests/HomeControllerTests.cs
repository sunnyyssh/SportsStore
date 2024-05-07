using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModel;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void CanUseRepository()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new Product []
        {
            new() { ProductID = 1, Name = "P1"},
            new() { ProductID = 2, Name = "P2"},
        }.AsQueryable);

        var controller = new HomeController(mock.Object);

        var result = (controller.Index(null) as ViewResult)?.ViewData.Model
            as ProductListViewModel;
        var resultArray = result?.Products.ToArray();
        
        Assert.NotNull(resultArray);
        Assert.Equal(2, resultArray.Length);
        Assert.Equal("P1", resultArray[0].Name);
        Assert.Equal("P2", resultArray[1].Name);
    }

    [Fact]
    public void CanPaginate()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() {ProductID = 1, Name = "P1"},
            new() {ProductID = 2, Name = "P2"},
            new() {ProductID = 3, Name = "P3"},
            new() {ProductID = 4, Name = "P4"},
            new() {ProductID = 5, Name = "P5"}
        }.AsQueryable);

        var controller = new HomeController(mock.Object);
        controller.PageSize = 3;

        var result = (controller.Index(null, 2) as ViewResult)?.ViewData.Model
            as ProductListViewModel;
        var resultArr = result?.Products.ToArray();
        
        Assert.NotNull(resultArr);
        Assert.Equal(2, resultArr.Length);
        Assert.Equal("P4", resultArr[0].Name);
        Assert.Equal("P5", resultArr[1].Name);
    }

    [Fact]
    public void CanSendPaginationViewModel()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() { ProductID = 1, Name = "P1"},
            new() { ProductID = 2, Name = "P2"},
            new() { ProductID = 3, Name = "P3"},
            new() { ProductID = 4, Name = "P4"},
            new() { ProductID = 5, Name = "P5"},
        }.AsQueryable);

        var controller = new HomeController(mock.Object) { PageSize = 3 };

        var result = (controller.Index(null, 2) as ViewResult)?.Model as ProductListViewModel;
        var paging = result?.PagingInfo;
        Assert.NotNull(paging);
        Assert.Equal(2, paging.CurrentPage);
        Assert.Equal(3, paging.ItemsPerPage);
        Assert.Equal(5, paging.TotalItems);
        Assert.Equal(2, paging.TotalPages);
    }

    [Fact]
    public void CanFilterProducts()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() { ProductID = 1, Name = "P1", Category = "Cat1"},
            new() { ProductID = 2, Name = "P2", Category = "Cat2"},
            new() { ProductID = 3, Name = "P3", Category = "Cat1"},
            new() { ProductID = 4, Name = "P4", Category = "Cat2"},
            new() { ProductID = 5, Name = "P5", Category = "Cat3"},
        }.AsQueryable);

        var controller = new HomeController(mock.Object);
        controller.PageSize = 3;

        var result = ((controller.Index("Cat2", 1) as ViewResult)?.ViewData.Model
            as ProductListViewModel)?.Products.ToArray();
        
        Assert.NotNull(result);
        Assert.Equal("P2", result[0].Name);
        Assert.Equal("P4", result[1].Name);
        Assert.Equal("Cat2", result[0].Category);
        Assert.Equal("Cat2", result[1].Category);
    }
    
    [Fact]
    public void Generate_Category_Specific_Product_Count() {
        // Arrange
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((new Product[] {
            new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
            new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
            new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
            new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
            new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
        }).AsQueryable<Product>());
        HomeController target = new HomeController(mock.Object);
        target.PageSize = 3;

        static ProductListViewModel? GetModel(IActionResult result) 
            => (result as ViewResult)?.ViewData?.Model as ProductListViewModel;
        
        // Action
        int? res1 = GetModel(target.Index("Cat1"))?.PagingInfo.TotalItems;
        int? res2 = GetModel(target.Index("Cat2"))?.PagingInfo.TotalItems;
        int? res3 = GetModel(target.Index("Cat3"))?.PagingInfo.TotalItems;
        int? resAll = GetModel(target.Index(null))?.PagingInfo.TotalItems;
        // Assert
        Assert.Equal(2, res1);
        Assert.Equal(2, res2);
        Assert.Equal(1, res3);
        Assert.Equal(5, resAll);
    }
}