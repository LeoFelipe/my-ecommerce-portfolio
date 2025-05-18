using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.ExternalServices.MyFakePay.Enums;
using EcommercePortfolio.ExternalServices.MyFakePayApi;
using EcommercePortfolio.Orders.API.Application.Commands;
using EcommercePortfolio.Orders.API.Application.Dtos;
using EcommercePortfolio.Orders.Domain;
using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Domain.Entities;
using Moq;

namespace EcommercePortfolio.Orders.UnitTests.CommandsTests;

public class OrderCommanderHandlerTests
{
    private readonly Mock<ICartApiService> _cartApiServiceMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly NotificationContext _notificationContext;
    private readonly OrderCommanderHandler _handler;

    public OrderCommanderHandlerTests()
    {
        _cartApiServiceMock = new Mock<ICartApiService>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _paymentServiceMock = new Mock<IPaymentService>();
        _notificationContext = new NotificationContext();

        _handler = new OrderCommanderHandler(
            _cartApiServiceMock.Object,
            _orderRepositoryMock.Object,
            _paymentServiceMock.Object,
            _notificationContext);
    }

    private static CreateOrderCommand CreateValidCommand() => new(
        "cart123",
        Guid.NewGuid(),
        EnumPaymentMethod.PIX,
        new AddressDto("49000-000", "Sergipe", "Aracaju", "Av. Teste", 100)
    );

    private static GetCartByClientIdResponse CreateValidCart(CreateOrderCommand command) => 
        new (
            command.CartId,
            command.ClientId,
            DateTime.UtcNow,
            20.0m,
            [
                new (1, "Produto A", "Cat", 2, 10.0m)
            ]
        );

    private static PaymentApiResponse CreateValidPaymentResponse(Guid orderId, Guid clientId)
    {
        var payment = new PaymentApiResponse(
            orderId,
            clientId,
            20.0m,
            EnumPaymentMethod.PIX);

        payment.AuthorizePayment();

        return payment;
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistOrder()
    {
        // Arrange
        var command = CreateValidCommand();
        var cart = CreateValidCart(command);

        _cartApiServiceMock.Setup(x => x.GetCartByClientId(command.ClientId)).ReturnsAsync(cart);
        _paymentServiceMock.Setup(x => x.DoPayment(It.IsAny<PaymentApiResponse>()))
                           .ReturnsAsync(CreateValidPaymentResponse(Guid.NewGuid(), command.ClientId));
        _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
        _orderRepositoryMock.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

        // Act
        await _handler.Handle(command, default);

        // Assert
        Assert.False(_notificationContext.Any());
        _orderRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
        _orderRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldAddValidationErrors()
    {
        // Arrange
        var command = new CreateOrderCommand("", Guid.Empty, (EnumPaymentMethod)999,
            new AddressDto("", "", "", "", 0));

        // Act
        await _handler.Handle(command, default);

        // Assert
        Assert.True(_notificationContext.Any());
        Assert.Contains(_notificationContext.Get(), n => n.Message == "Invalid cart id");
    }

    [Fact]
    public async Task Handle_CartNotFound_ShouldAddNotification()
    {
        var command = CreateValidCommand();
        _cartApiServiceMock.Setup(x => x.GetCartByClientId(command.ClientId))
                           .ReturnsAsync((GetCartByClientIdResponse?)null);

        await _handler.Handle(command, default);

        Assert.True(_notificationContext.Any());
        Assert.Contains(_notificationContext.Get(), x => x.Message == "Cart not found");
    }

    [Fact]
    public async Task Handle_PaymentNotAuthorized_ShouldAddNotification()
    {
        var command = CreateValidCommand();
        var cart = CreateValidCart(command);

        _cartApiServiceMock.Setup(x => x.GetCartByClientId(command.ClientId)).ReturnsAsync(cart);
        _paymentServiceMock.Setup(x => x.DoPayment(It.IsAny<PaymentApiResponse>()))
                           .ReturnsAsync(new PaymentApiResponse(Guid.NewGuid(), cart.ClientId, cart.TotalValue, command.PaymentMethod));

        await _handler.Handle(command, default);

        Assert.True(_notificationContext.Any());
        Assert.Contains(_notificationContext.Get(), x => x.Message == "Payment not authorized");
    }
}
