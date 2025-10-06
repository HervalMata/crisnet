using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class PaymentService(IConfiguration config, ICartService cartService, IGenericRepository<DeliveryMethod> dmRepo, IGenericRepository<Core.Entities.Product> productRepo) : IPaymentService
{
    public async Task<ShoppingCart> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        var cart = await cartService.GetCartAsync(cartId);
        if (cart == null) throw new InvalidOperationException("Cart not found.");
        var shippingPrice = 0m;
        if (cart.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await dmRepo.GetByIdAsync((int)cart.DeliveryMethodId);
#pragma warning disable CS8603 // Possible null reference return.
            if (deliveryMethod == null) return null;
#pragma warning restore CS8603 // Possible null reference return.
            shippingPrice = deliveryMethod.Price;
        }
        foreach (var item in cart.Items)
        {
            var productItem = await productRepo.GetByIdAsync(item.ProductId);
#pragma warning disable CS8603 // Possible null reference return.
            if (productItem == null) return null;
#pragma warning restore CS8603 // Possible null reference return.
            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
            var service = new PaymentIntentService();
            PaymentIntent? intent = null;
            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "brl",
                    PaymentMethodTypes = ["card"]
                };
                intent = await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                };
                intent = await service.UpdateAsync(cart.PaymentIntentId, options);
            }
            await cartService.SetCartAsync(cart);
        }
        return cart;
    }
}
