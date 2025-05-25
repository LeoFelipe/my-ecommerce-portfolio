using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.ApiGateways.MyFakePay.Enums;
using EcommercePortfolio.ApiGateways.MyFakePaymentApi;
using EcommercePortfolio.Orders.API.Application.Commands;
using EcommercePortfolio.Orders.Domain;
using EcommercePortfolio.Orders.Domain.ApiServices;
using EcommercePortfolio.Orders.Domain.Entities;
using EcommercePortfolio.Orders.UnitTests.Factories.Carts;
using EcommercePortfolio.Orders.UnitTests.Factories.Orders;
using EcommercePortfolio.Orders.UnitTests.Factories.Payments;
using FluentAssertions;
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

    [Fact]
    public async Task OrderCommand_Handle_ShouldPersistOrderSuccessfully()
    {
        // Arrange
        var quantity = 10;
        var price = 50;
        var command = OrderCommandFactory.BuildValidCreateOrderCommand();
        var cart = CartApiServiceFactory.BuildValidCart(quantity, price);
        var payment = PaymentApiServiceFactory.BuildValidPayment(command.ClientId, quantity * price);

        _cartApiServiceMock.Setup(x => x.GetCartByClientId(command.ClientId)).ReturnsAsync(cart);
        _paymentServiceMock.Setup(x => x.DoPayment(It.IsAny<PaymentApiResponse>())).ReturnsAsync(payment);
        _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
        _orderRepositoryMock.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _notificationContext.Any().Should().BeFalse();
        _orderRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
        _orderRepositoryMock.Verify(x => x.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task OrderCommand_Handle_ShouldAddValidationErrorsWhenCommandInvalid()
    {
        // Arrange
        var command = OrderCommandFactory.BuildCreateOrderCommand((EnumPaymentMethod)999, "", Guid.Empty);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _notificationContext.Any().Should().BeTrue();
        _notificationContext.Get().Should().Contain(n => n.Message == "Invalid cart id");
    }

    [Fact]
    public async Task OrderCommand_Handle_ShouldAddNotificationWhenCartNotFound()
    {
        // Arrange
        var command = OrderCommandFactory.BuildValidCreateOrderCommand();

        _cartApiServiceMock.Setup(x => x.GetCartByClientId(command.ClientId))
                           .ReturnsAsync((GetCartByClientIdResponse)null);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _notificationContext.Any().Should().BeTrue();
        _notificationContext.Get().Should().Contain(x => x.Message == "Cart not found");
    }

    [Fact]
    public async Task OrderCommand_Handle_ShouldAddNotificationWhenPaymentNotAuthorized()
    {
        // Arrange
        var command = OrderCommandFactory.BuildValidCreateOrderCommand();
        var cart = CartApiServiceFactory.BuildValidCart();
        var payment = PaymentApiServiceFactory.BuildPaymentNotAuthorized(command.ClientId);

        _cartApiServiceMock.Setup(x => x.GetCartByClientId(command.ClientId)).ReturnsAsync(cart);
        _paymentServiceMock.Setup(x => x.DoPayment(It.IsAny<PaymentApiResponse>())).ReturnsAsync(payment);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _notificationContext.Any().Should().BeTrue();
        _notificationContext.Get().Should().Contain(x => x.Message == "Payment not authorized");
    }
}
