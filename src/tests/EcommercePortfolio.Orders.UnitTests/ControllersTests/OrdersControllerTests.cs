using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.ApiGateways.MyFakePay.Enums;
using EcommercePortfolio.Orders.API.Application.Commands;
using EcommercePortfolio.Orders.API.Application.Dtos;
using EcommercePortfolio.Orders.API.Application.Queries;
using EcommercePortfolio.Orders.API.Controllers;
using EcommercePortfolio.Services.ObjectResponses;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Moq.AutoMock;
using System.Net;

namespace EcommercePortfolio.Orders.UnitTests.ControllersTests;

public class OrdersControllerTests
{
    private readonly Mock<IOrderQueries> _orderQueriesMock;
    private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        var mocker = new AutoMocker();
        _orderQueriesMock = mocker.GetMock<IOrderQueries>();
        _mediatorHandlerMock = mocker.GetMock<IMediatorHandler>();

        _controller = mocker.CreateInstance<OrdersController>();
    }

    [Fact]
    public async Task GetById_OrderExists_ShouldReturnOk()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new GetOrderResponse(
            orderId,
            Guid.NewGuid(),
            DateTime.Now,
            10.0m,
            [new OrderItemDto(1, "Product A", "Category A", 2, 10.0m)]);

        _orderQueriesMock.Setup(q => q.GetById(orderId)).ReturnsAsync(order);

        // Act
        var result = await _controller.GetById(orderId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseResult>(result.Value);
        Assert.True(response.Success);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(order, response.Response);
    }

    [Fact]
    public async Task GetById_OrderNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderQueriesMock.Setup(q => q.GetById(orderId)).ReturnsAsync((GetOrderResponse?)null);

        // Act
        var result = await _controller.GetById(orderId) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<ResponseResult>(result.Value);
        Assert.False(response.Success);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Cart not found", response.Response);
    }

    [Fact]
    public async Task GetAddressById_AddressExists_ShouldReturnOk()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var address = new GetAddressOrderResponse("12345-000", "Sergipe", "Aracaju", "Av. Teste", 100);

        _orderQueriesMock.Setup(q => q.GetAddressById(orderId)).ReturnsAsync(address);

        // Act
        var result = await _controller.GetAddressById(orderId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<ResponseResult>(result.Value);
        Assert.True(response.Success);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(address, response.Response);
    }

    [Fact]
    public async Task GetAddressById_AddressNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderQueriesMock.Setup(q => q.GetAddressById(orderId)).ReturnsAsync((GetAddressOrderResponse?)null);

        // Act
        var result = await _controller.GetAddressById(orderId) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<ResponseResult>(result.Value);
        Assert.False(response.Success);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Address not found", response.Response);
    }

    [Fact]
    public async Task GetByClientId_ShouldReturnOrders()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var orders = new List<GetOrderResponse>
        {
            new (
                clientId,
                Guid.NewGuid(),
                DateTime.Now,
                100.0m,
                [
                    new (1, "Product A", "Category A", 2, 10.0m)
                ])
        };

        _orderQueriesMock.Setup(q => q.GetByClientId(clientId)).ReturnsAsync(orders);

        // Act
        var result = await _controller.GetByClientId(clientId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var response = Assert.IsType<ResponseResult>(result.Value);
        Assert.True(response.Success);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(orders, response.Response);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnCreated()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new ObjectId().ToString(),
            Guid.NewGuid(),
            EnumPaymentMethod.PIX,
            new AddressDto("12345-000", "Sergipe", "Aracaju", "Av. Teste", 100));

        _mediatorHandlerMock.Setup(m => m.SendCommand(command)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateOrder(command);

        // Assert
        Assert.IsType<CreatedResult>(result);
    }
}
