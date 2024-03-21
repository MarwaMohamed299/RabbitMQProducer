using Microsoft.AspNetCore.Mvc;

namespace RabbitMQProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IMessageProducer _messagePublisher;

        public OrdersController(OrderDbContext context, IMessageProducer messagePublisher)
        {
            _context = context;
            _messagePublisher = messagePublisher;
        }
        [HttpPost ("makeorder")]
        public async Task<IActionResult> CreateOrder(Order inputOrder)
        {
            var newOrder = new Order
            {
                ProductName = inputOrder.ProductName,
                Price = inputOrder.Price,
                Quantity = inputOrder.Quantity
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            _messagePublisher.SendMessages(newOrder);

            return Ok(new { id = newOrder.Id });
        }

    }
}
