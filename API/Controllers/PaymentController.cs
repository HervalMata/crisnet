using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaymentController(IPaymentService paymentService, IGenericRepository<DeliveryMethod> dmRepo) : BaseApiController
{
    [Authorize]
    [HttpPost("{cartId}")]
    public async Task<ActionResult> CreateOrUpdatePaymentIntent(string cartId)
    {
        var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
        if (cart == null) return BadRequest("Problema com seu carrinho na API");
        return Ok(cart);
    }

    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await dmRepo.ListAllAsync());
    }
}
