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
    IRequestHandler<AddCartCommand>,
    IRequestHandler<UpdateCartCommand>,
    IRequestHandler<UpdateCartItemCommand>,
    IRequestHandler<RemoveCartCommand>,
    IRequestHandler<RemoveCartItemCommand>
{
    private readonly ICartRepository _cartRepository = cartRepository;

    public async Task Handle(AddCartCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            AddValidationError(command.ValidationResult);
            return;
        }

        await _cartRepository.Add((Cart)command);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            AddValidationError(command.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByIdAndClientId(command.Id, command.ClientId);

        if (cart == null)
        {
            AddValidationError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        cart.UpdateAllItems(command.Products.ToCartItems());

        _cartRepository.Update(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(UpdateCartItemCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            AddValidationError(command.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByIdAndClientId(command.Id, command.ClientId);

        if (cart == null)
        {
            AddValidationError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        if (!cart.HasItems(command.CartItem))
        {
            AddValidationError("Item does not exist in cart", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        cart.UpdateItem(command.CartItem);

        _cartRepository.Update(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(RemoveCartCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            AddValidationError(command.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByIdAndClientId(command.Id, command.ClientId);

        if (cart == null)
        {
            AddValidationError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        _cartRepository.Remove(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }

    public async Task Handle(RemoveCartItemCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid())
        {
            AddValidationError(command.ValidationResult);
            return;
        }

        var cart = await _cartRepository.GetByIdAndClientId(command.Id, command.ClientId);

        if (cart == null)
        {
            AddValidationError("Cart not found", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        if (!cart.HasItems(command.ProductId))
        {
            AddValidationError("Item does not exist in cart", EnumNotificationType.NOT_FOUND_ERROR);
            return;
        }

        cart.RemoveItem(command.ProductId);

        _cartRepository.Update(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }
}
