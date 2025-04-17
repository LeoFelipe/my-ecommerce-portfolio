using EcommercePortfolio.API.Carts.Models;
using EcommercePortfolio.Core.Messaging;
using EcommercePortfolio.Core.Notification;
using EcommercePortfolio.Domain.Carts;
using EcommercePortfolio.Domain.Carts.Entities;
using MediatR;

namespace EcommercePortfolio.API.Carts;

public class CartCommanderHandler(
    ICartRepository cartRepository,
    INotificationContext notification) : CommandHandler(notification), 
    IRequestHandler<AddCartCommand>,
    IRequestHandler<UpdateCartCommand>
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

        var cart = await _cartRepository.GetById(command.Id);

        if (cart == null)
        {
            AddValidationError("Cart not found");
            return;
        }

        if (cart.ClientId != command.ClientId)
        {
            AddValidationError("ClientId does not match with cart's ClientId");
            return;
        }

        cart.UpdateAllItems(command.Products.ToCartItems());

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

        var cart = await _cartRepository.GetById(command.Id);

        if (cart == null)
        {
            AddValidationError("Cart not found");
            return;
        }

        if (cart.ClientId != command.ClientId)
        {
            AddValidationError("ClientId does not match with cart's ClientId");
            return;
        }

        _cartRepository.Remove(cart);

        await PersistData(_cartRepository.UnitOfWork);
    }
}
