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

        var result = (controller.Index() as ViewResult)?.ViewData.Model
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

        var result = (controller.Index(2) as ViewResult)?.ViewData.Model
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

        var result = (controller.Index(2) as ViewResult)?.Model as ProductListViewModel;
        var paging = result?.PagingInfo;
        Assert.NotNull(paging);
        Assert.Equal(2, paging.CurrentPage);
        Assert.Equal(3, paging.ItemsPerPage);
        Assert.Equal(5, paging.TotalItems);
        Assert.Equal(2, paging.TotalPages);
    }
}