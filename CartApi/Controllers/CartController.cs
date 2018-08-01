using System.Net;
using System.Threading.Tasks;
using CartApi.Domain.Models;
using CartApi.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CartApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class CartController : Controller
    {
        private ICartRepository _repository;

        private ILogger _logger;

        public CartController(ICartRepository repository, ILogger logger )
        {
            this._repository = repository;
            this._logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var basket = await _repository.GetCartAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] Cart value)
        {
            var basket = await _repository.UpdateCartAsync(value);
            return Ok(basket);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _logger.LogInformation("Delete method in Cart controller reached");
            _repository.DeleteCartAsync(id);
        }
    }
}