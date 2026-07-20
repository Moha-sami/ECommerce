using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BasketController : BaseController
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
    {
        var basket = await _basketRepository.GetBasketAsync(id);
        return Ok(basket ?? new CustomerBasket(id));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basketDto)
    {
        var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);
        var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
        return Ok(updatedBasket);
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteBasket(string id)
    {
        return Ok(await _basketRepository.DeleteBasketAsync(id));
    }
}
