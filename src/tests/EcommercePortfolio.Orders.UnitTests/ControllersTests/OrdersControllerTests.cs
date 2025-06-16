using EcommercePortfolio.Core.Mediator;
using EcommercePortfolio.ApiGateways.MyFakePay.Enums;
using EcommercePortfolio.Orders.API.Application.Commands;
using EcommercePortfolio.Orders.API.Application.Dtos;
using EcommercePortfolio.Orders.API.Application.Queries;
using EcommercePortfolio.Orders.API.Controllers;
using EcommercePortfolio.Services.ObjectResponses;
using FluentAssertions;
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
    public async Task OrdersController_GetById_ShouldReturnOk_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.CreateVersion7();
        var order = new GetOrderResponse(
            orderId,
            Guid.CreateVersion7(),
            DateTime.Now,
            10.0m,
            [new OrderItemDto(1, "Product A", "Category A", 2, 10.0m)]);

        _orderQueriesMock.Setup(q => q.GetById(orderId)).ReturnsAsync(order);

        // Act
        var result = await _controller.GetById(orderId) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var response = result.Value.Should().BeOfType<ResponseResult>().Subject;
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Response.Should().Be(order);
    }

    [Fact]
    public async Task OrdersController_GetById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = Guid.CreateVersion7();
        _orderQueriesMock.Setup(q => q.GetById(orderId)).ReturnsAsync((GetOrderResponse)null);

        // Act
        var result = await _controller.GetById(orderId) as NotFoundObjectResult;

        // Assert
        result.Should().NotBeNull();
        var response = result.Value.Should().BeOfType<ResponseResult>().Subject;
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Response.Should().Be("Cart not found");
    }

    [Fact]
    public async Task OrdersController_GetAddressById_ShouldReturnOk_WhenAddressExistsForOrder()
    {
        // Arrange
        var orderId = Guid.CreateVersion7();
        var address = new GetAddressOrderResponse("12345-000", "Sergipe", "Aracaju", "Av. Teste", 100);

        _orderQueriesMock.Setup(q => q.GetAddressById(orderId)).ReturnsAsync(address);

        // Act
        var result = await _controller.GetAddressById(orderId) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var response = result.Value.Should().BeOfType<ResponseResult>().Subject;
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Response.Should().Be(address);
    }

    [Fact]
    public async Task OrdersController_GetAddressById_ShouldReturnNotFound_WhenAddressDoesNotExistForOrder()
    {
        // Arrange
        var orderId = Guid.CreateVersion7();
        _orderQueriesMock.Setup(q => q.GetAddressById(orderId)).ReturnsAsync((GetAddressOrderResponse)null);

        // Act
        var result = await _controller.GetAddressById(orderId) as NotFoundObjectResult;

        // Assert
        result.Should().NotBeNull();
        var response = result.Value.Should().BeOfType<ResponseResult>().Subject;
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Response.Should().Be("Address not found");
    }

    [Fact]
    public async Task OrdersController_GetByClientId_ShouldReturnOrdersSuccessfully_WhenClientHasOrders()
    {
        // Arrange
        var clientId = Guid.CreateVersion7();
        var orders = new List<GetOrderResponse>
        {
            new (
                clientId,
                Guid.CreateVersion7(),
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
        result.Should().NotBeNull();
        var response = result.Value.Should().BeOfType<ResponseResult>().Subject;
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Response.Should().Be(orders);
    }

    [Fact]
    public async Task OrdersController_CreateOrder_ShouldReturnCreatedSuccessfully_WhenValidOrderDataProvided()
    {
        // Arrange
        var command = new CreateOrderCommand(
            new ObjectId().ToString(),
            Guid.CreateVersion7(),
            EnumPaymentMethod.PIX,
            new AddressDto("12345-000", "Sergipe", "Aracaju", "Av. Teste", 100));

        _mediatorHandlerMock.Setup(m => m.SendCommand(command)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateOrder(command);

        // Assert
        result.Should().BeOfType<CreatedResult>();
    }
}
