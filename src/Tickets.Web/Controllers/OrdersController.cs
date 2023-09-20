using Tickets.Core.Aggregates.TicketOrderAggregate;
using Tickets.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tickets.Web.Api;

public class OrdersController : BaseApiController
{
    private readonly IRepository<TicketOrder> _repository;

    public OrdersController(IRepository<TicketOrder> repository)
    {
        _repository = repository;
    }

    // GET: api/Orders
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var Orders = (await _repository.ListAsync()).ToList();

        return Ok(Orders);
    }

    // GET: api/Orders
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var Order = await _repository.GetByIdAsync<Guid>(id);
        if (Order == null)
        {
            return NotFound();
        }

        return Ok(Order);
    }

    // POST: api/Orders
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderDTO orderDTO)
    {
        var createdOrder = await _repository.AddAsync(new TicketOrder(orderDTO.MovieSessionId, orderDTO.BuyerId, orderDTO.OrderDateAndTime, orderDTO.Tickets));

        return Ok(createdOrder.Id);
    }

    public record OrderDTO(Guid MovieSessionId, Guid BuyerId, DateTime OrderDateAndTime, List<Ticket> Tickets);
}
