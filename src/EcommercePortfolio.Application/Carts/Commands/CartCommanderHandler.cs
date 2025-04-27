using EcommercePortfolio.Application.Carts.Dtos;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Carts.Entities;
using MediatR;

namespace EcommercePortfolio.Application.Carts.Commands;

public class CartCommanderHandler(
    ICartRepository cartRepository,
    INotificationContext notification) : CommandHandler(notification), 
    IRequestHandler<CreateCartCommand>,
    IRequestHandler<UpdateCartCommand>,
    IRequestHandler<UpdateCartItemCommand>,
    IRequestHandler<RemoveCartCommand>,
    IRequestHandler<RemoveCartItemCommand>
{
    private readonly ICartRepository _cartRepository = cartRepository;

    public async Task Handle(CreateCartCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByClientId(message.ClientId);

        if (cart != null)
        {
            AddError("Cart already exists", EnumNotificationType.VALIDATION_ERROR);
            return;
        }

        await _cartRepository.Add((Cart)message);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(UpdateCartCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByClientId(message.ClientId);

        if (cart == null)
        {
            AddError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        cart.UpdateAllItems(message.Products.MapToCartItems());

        _cartRepository.Update(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(UpdateCartItemCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByClientId(message.ClientId);

        if (cart == null)
        {
            AddError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        if (!cart.HasItems((CartItem)message.CartItem))
        {
            AddError("Item does not exist in cart", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        cart.UpdateItem((CartItem)message.CartItem);

        _cartRepository.Update(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(RemoveCartCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByClientId(message.ClientId);

        if (cart == null)
        {
            AddError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        _cartRepository.Remove(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(RemoveCartItemCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            AddError(message.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByClientId(message.ClientId);

        if (cart == null)
        {
            AddError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        if (!cart.HasItems(message.ProductId))
        {
            AddError("Item does not exist in cart", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        cart.RemoveItem(message.ProductId);

        _cartRepository.Update(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }
}
