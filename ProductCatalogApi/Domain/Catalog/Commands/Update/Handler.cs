using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Data;

namespace ProductCatalogApi.Domain.Catalog.Commands.Update
{
    public class Handler : IRequestHandler<Request, Result>
    {
        private readonly IMediator _mediator;
        
        private readonly CatalogContext _catalogContext;
        
        public Handler(IMediator mediator, CatalogContext catalogContext)
        {
            this._mediator = mediator;
            this._catalogContext = catalogContext;
        }
        
        public async Task<Result> Handle(Request request,  CancellationToken cancellationToken)
        {
            CatalogItem catalogItem = await _catalogContext.CatalogItems
                .SingleOrDefaultAsync(p => p.Id == request.Id);

            if (catalogItem == null)
            {
                var result = new Result();
                result.AddValidation($"Item with id {request.Id} not found.");
                return result;
            }

            catalogItem.Name = request.Name;
            catalogItem.Description = request.Description;
            catalogItem.Price = request.Price;
            catalogItem.PictureUrl = request.PictureUrl;
            catalogItem.PictureFileName = request.PictureFileName;
            catalogItem.CatalogBrandId = request.CatalogBrandId;
            catalogItem.CatalogTypeId = request.CatalogTypeId;
            _catalogContext.CatalogItems.Update(catalogItem);
            await _catalogContext.SaveChangesAsync(cancellationToken);
            /*
            await _mediator.Publish(new Notification
            {
                NomeFuncionario = solicitacao.Funcionario.Nome,
                ValorAntigo = solicitacao.Valor,
                Valor = request.Valor,
                DescricaoAntiga = request.Descricao,
                Descricao = solicitacao.Descricao
            }, cancellationToken);
            */
            
            return new Result<CatalogItem>(catalogItem);
        }
    }
}