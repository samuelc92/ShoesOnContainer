using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;
using ProductCatalogApi.ViewModels;

namespace ProductCatalogApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Catalog")]
    public class CatalogController : Controller
    {
        private readonly CatalogContext _catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;
        private readonly IMediator _mediator;
        
        public CatalogController(CatalogContext catalogContext, 
                                 IOptionsSnapshot<CatalogSettings> settings, 
                                 IMediator mediator)
        {
            this._catalogContext = catalogContext;
            this._settings = settings;
            ((DbContext) this._catalogContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this._mediator = mediator;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _catalogContext.CatalogTypes.ToListAsync();
            return Ok(items);
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _catalogContext.CatalogBrands    .ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced", _settings.Value.ExternalCatalogBaseUrl);
                return Ok(item);
            }

            return NotFound();
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize=6, [FromQuery] int pageIndex=0)
        {
            var totalItems = await _catalogContext.CatalogItems
                .LongCountAsync();
            var itemsOnPage = await _catalogContext.CatalogItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }
        
        [HttpGet]
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items(string name, [FromQuery] int pageSize=6, [FromQuery] int pageIndex=0)
        {
            var totalItems = await _catalogContext.CatalogItems
                .Where(c=>c.Name.StartsWith(name))
                .LongCountAsync();
            var itemsOnPage = await _catalogContext.CatalogItems
                .Where(c=>c.Name.StartsWith(name))
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }

        [HttpGet]
        [Route("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]
        public async Task<IActionResult> Items(int? catalogTypeId, int? catalogBrandId , [FromQuery] int pageSize=6, [FromQuery] int pageIndex=0)
        {
            var root = (IQueryable<CatalogItem>)this._catalogContext.CatalogItems;

            if (catalogTypeId.HasValue)
            {
                root = root.Where(c => c.CatalogTypeId == catalogTypeId.Value);
            }
            if (catalogBrandId.HasValue)
            {
                root = root.Where(c => c.CatalogBrandId == catalogBrandId.Value);
            }
            var totalItems = await root
                .LongCountAsync();
            var itemsOnPage = await root
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }
        
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct(Domain.Catalog.Commands.Create.Request request)
        {
            Domain.Result result = await _mediator.Send(request, CancellationToken.None);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateProduct(Domain.Catalog.Commands.Update.Request request)
        {
            Domain.Result result = await _mediator.Send(request, CancellationToken.None);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Domain.Result result = await _mediator.Send(new Domain.Catalog.Commands.Delete.Request {Id = id},
                CancellationToken.None);
            if (result.HasValidation)
                return BadRequest(result);
            return NoContent();
        }
        
        private List<CatalogItem> ChangeUrlPlaceHolder(List<CatalogItem> items)
        {
            items.ForEach(x=>
                x.PictureUrl = x.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced", 
                    _settings.Value.ExternalCatalogBaseUrl));
            return items;
        } 
    }
}