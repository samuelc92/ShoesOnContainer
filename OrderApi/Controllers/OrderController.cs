using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderApi.Domain.Data;
using OrderApi.Domain.Models;

namespace OrderApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderContext _ordersContext;
        private readonly IOptionsSnapshot<OrderSettings> _settings;
        private readonly ILogger<OrderController> _logger;
        
        public OrderController(OrderContext ordersContext, ILogger<OrderController> logger, IOptionsSnapshot<OrderSettings> settings)
        {
            _settings = settings;
            // _ordersContext = ordersContext;
            _ordersContext = ordersContext ?? throw new ArgumentNullException(nameof(ordersContext));
          
            ((DbContext)ordersContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;


            _logger = logger;
        }
        
                // POST api/Order
        [Route("new")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            //var envs = Environment.GetEnvironmentVariables();
            //var conString = _settings.Value.ConnectionString;
            //_logger.LogInformation($"{conString}");

            order.OrderStatus = OrderStatus.Preparing;
            order.OrderDate = DateTime.UtcNow;

            //_logger.LogInformation(" In Create Order");
            //_logger.LogInformation(" Order" +order.UserName);


            _ordersContext.Orders.Add(order);
            _ordersContext.OrderItems.AddRange(order.OrderItems);

            //_logger.LogInformation(" Order added to context");
            //_logger.LogInformation(" Saving........");

            await _ordersContext.SaveChangesAsync();
            return CreatedAtRoute("GetOrder", new { id = order.OrderId },order);

        }

        [HttpGet("{id}", Name = "GetOrder")]
        //  [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrder(int id)
        {
           
            var item = await _ordersContext.Orders
                .Include(x=>x.OrderItems)
                .SingleOrDefaultAsync(ci => ci.OrderId==id);
            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();
 
        }
        
        [Route("")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _ordersContext.Orders.ToListAsync();

           // var orders = await orderTask;

            return Ok(orders);
        }
    }
}